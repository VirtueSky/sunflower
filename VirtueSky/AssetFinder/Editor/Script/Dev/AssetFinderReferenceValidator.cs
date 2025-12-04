using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    #if AssetFinderDEV
    /// <summary>
    /// Comprehensive reference validation system comparing FR2 vs Unity's GetDependencies
    /// </summary>
    internal class AssetFinderReferenceValidator
    {
        private struct ValidationResult
        {
            public int totalAssets;
            public int assetsWithDifferences;
            public int missingInFR2;
            public int extraInFR2;
            public List<AssetDifference> differences;
            public Dictionary<string, int> missingByExtension;
            public Dictionary<string, int> extraByExtension;
            public Dictionary<string, int> assetTypeIssues;
        }

        private struct AssetDifference
        {
            public string assetPath;
            public string guid;
            public string assetType;
            public List<string> missingInFR2;   // In Unity but not in FR2
            public List<string> extraInFR2;    // In FR2 but not in Unity
            public string summary;
        }

        private readonly Dictionary<string, string[]> unityDependencyCache = new Dictionary<string, string[]>();
        private readonly System.Text.StringBuilder reportBuilder = new System.Text.StringBuilder();

        public void ValidateAllReferences(bool exportToFile = false)
        {
            var startTime = System.DateTime.Now;
            var result = new ValidationResult
            {
                differences = new List<AssetDifference>(),
                missingByExtension = new Dictionary<string, int>(),
                extraByExtension = new Dictionary<string, int>(),
                assetTypeIssues = new Dictionary<string, int>()
            };

            // Clear caches
            unityDependencyCache.Clear();
            reportBuilder.Clear();

            // Get all critical assets from FR2 cache
            var criticalAssets = AssetFinderCache.Api.AssetList
                .Where(asset => asset != null && !asset.IsMissing && asset.IsCriticalAsset())
                .ToList();

            result.totalAssets = criticalAssets.Count;
            AppendToReport($"=== FR2 REFERENCE VALIDATION REPORT ===");
            AppendToReport($"Generated: {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            AppendToReport($"Total critical assets to analyze: {result.totalAssets}");
            AppendToReport("");

            AssetFinderLOG.Log($"[AssetFinderVALIDATION] Analyzing {result.totalAssets} critical assets...");

            var processedCount = 0;
            var lastProgressReport = 0;

            foreach (var asset in criticalAssets)
            {
                processedCount++;
                
                // Progress reporting (every 10%)
                var progressPercent = (processedCount * 100) / result.totalAssets;
                if (progressPercent >= lastProgressReport + 10)
                {
                    lastProgressReport = progressPercent;
                    AssetFinderLOG.Log($"[AssetFinderVALIDATION] Progress: {progressPercent}% ({processedCount}/{result.totalAssets})");
                }

                var difference = CompareAssetReferences(asset);
                if (difference.missingInFR2.Count > 0 || difference.extraInFR2.Count > 0)
                {
                    result.differences.Add(difference);
                    result.assetsWithDifferences++;
                    result.missingInFR2 += difference.missingInFR2.Count;
                    result.extraInFR2 += difference.extraInFR2.Count;

                    // Track by extension and asset type
                    string assetExt = System.IO.Path.GetExtension(asset.assetPath).ToLower();
                    result.assetTypeIssues[assetExt] = result.assetTypeIssues.GetValueOrDefault(assetExt, 0) + 1;

                    foreach (var missing in difference.missingInFR2)
                    {
                        string depPath = ExtractPathFromDependencyString(missing);
                        string depExt = System.IO.Path.GetExtension(depPath).ToLower();
                        result.missingByExtension[depExt] = result.missingByExtension.GetValueOrDefault(depExt, 0) + 1;
                    }

                    foreach (var extra in difference.extraInFR2)
                    {
                        string depPath = ExtractPathFromDependencyString(extra);
                        string depExt = System.IO.Path.GetExtension(depPath).ToLower();
                        result.extraByExtension[depExt] = result.extraByExtension.GetValueOrDefault(depExt, 0) + 1;
                    }
                }
            }

            var duration = System.DateTime.Now - startTime;
            LogValidationResults(result, duration, exportToFile);
        }

        private AssetDifference CompareAssetReferences(AssetFinderAsset asset)
        {
            var difference = new AssetDifference
            {
                assetPath = asset.assetPath,
                guid = asset.guid,
                assetType = asset.type.ToString(),
                missingInFR2 = new List<string>(),
                extraInFR2 = new List<string>()
            };

            try
            {
                // Get Unity's dependencies (with caching)
                string[] unityDeps;
                if (!unityDependencyCache.TryGetValue(asset.assetPath, out unityDeps))
                {
                    unityDeps = AssetDatabase.GetDependencies(asset.assetPath, false); // Direct dependencies only
                    unityDependencyCache[asset.assetPath] = unityDeps;
                }

                var unityGuids = new HashSet<string>();
                foreach (string depPath in unityDeps)
                {
                    if (depPath == asset.assetPath) continue; // Skip self-reference
                    string depGuid = AssetDatabase.AssetPathToGUID(depPath);
                    if (!string.IsNullOrEmpty(depGuid))
                    {
                        unityGuids.Add(depGuid);
                    }
                }

                // Get FR2's dependencies
                var fr2Guids = new HashSet<string>();
                if (asset.UseGUIDs != null)
                {
                    fr2Guids.UnionWith(asset.UseGUIDs.Keys);
                }

                // Find differences
                foreach (string unityGuid in unityGuids)
                {
                    if (!fr2Guids.Contains(unityGuid))
                    {
                        string depPath = AssetDatabase.GUIDToAssetPath(unityGuid);
                        difference.missingInFR2.Add($"{unityGuid} ({depPath})");
                    }
                }

                foreach (string fr2Guid in fr2Guids)
                {
                    if (!unityGuids.Contains(fr2Guid))
                    {
                        string depPath = AssetDatabase.GUIDToAssetPath(fr2Guid);
                        difference.extraInFR2.Add($"{fr2Guid} ({depPath})");
                    }
                }

                // Create summary
                if (difference.missingInFR2.Count > 0 || difference.extraInFR2.Count > 0)
                {
                    difference.summary = $"Missing: {difference.missingInFR2.Count}, Extra: {difference.extraInFR2.Count}";
                }
            }
            catch (System.Exception e)
            {
                difference.summary = $"Error during comparison: {e.Message}";
            }

            return difference;
        }

        private void LogValidationResults(ValidationResult result, System.TimeSpan duration, bool exportToFile)
        {
            AppendToReport($"Analysis completed in {duration.TotalSeconds:F2} seconds");
            AppendToReport($"Assets with differences: {result.assetsWithDifferences}");
            AppendToReport($"Total missing references in FR2: {result.missingInFR2}");
            AppendToReport($"Total extra references in FR2: {result.extraInFR2}");
            AppendToReport("");

            if (result.assetsWithDifferences == 0)
            {
                AppendToReport("🎉 Perfect match! FR2 and Unity have identical reference detection.");
                AssetFinderLOG.Log("🎉 Perfect match! FR2 and Unity have identical reference detection.");
            }
            else
            {
                float accuracy = ((result.totalAssets - result.assetsWithDifferences) * 100f / result.totalAssets);
                AppendToReport($"Accuracy: {accuracy:F1}%");
                AssetFinderLOG.Log($"[AssetFinderVALIDATION] Accuracy: {accuracy:F1}%");

                GenerateDetailedReport(result);
            }

            if (exportToFile)
            {
                string filePath = System.IO.Path.Combine(Application.dataPath, "../AssetFinderValidation_Report.txt");
                System.IO.File.WriteAllText(filePath, reportBuilder.ToString());
                AssetFinderLOG.Log($"[AssetFinderVALIDATION] Detailed report exported to: {filePath}");
            }
            else
            {
                // Console output (limited)
                AssetFinderLOG.Log("=== FR2 REFERENCE VALIDATION RESULTS ===");
                AssetFinderLOG.Log($"Analysis completed in {duration.TotalSeconds:F2} seconds");
                AssetFinderLOG.Log($"Total assets analyzed: {result.totalAssets}");
                AssetFinderLOG.Log($"Assets with differences: {result.assetsWithDifferences}");
                if (result.assetsWithDifferences > 0)
                {
                    AssetFinderLOG.Log($"Accuracy: {((result.totalAssets - result.assetsWithDifferences) * 100f / result.totalAssets):F1}%");
                    AssetFinderLOG.Log("Use 'Validate References (Export to File)' for detailed analysis.");
                }
            }
        }

        private void GenerateDetailedReport(ValidationResult result)
        {
            AppendToReport("=== TOP PROBLEMATIC ASSET TYPES ===");
            foreach (var kvp in result.assetTypeIssues.OrderByDescending(x => x.Value).Take(10))
            {
                AppendToReport($"{kvp.Key}: {kvp.Value} assets with differences");
            }
            AppendToReport("");

            AppendToReport("=== MOST COMMONLY MISSED DEPENDENCY TYPES ===");
            foreach (var kvp in result.missingByExtension.OrderByDescending(x => x.Value).Take(10))
            {
                AppendToReport($"{kvp.Key}: {kvp.Value} instances");
            }
            AppendToReport("");

            AppendToReport("=== MOST COMMONLY OVER-DETECTED TYPES ===");
            foreach (var kvp in result.extraByExtension.OrderByDescending(x => x.Value).Take(10))
            {
                AppendToReport($"{kvp.Key}: {kvp.Value} instances");
            }
            AppendToReport("");

            AppendToReport("=== DETAILED DIFFERENCES ===");
            var maxDetailsToShow = Mathf.Min(50, result.differences.Count);
            if (result.differences.Count > maxDetailsToShow)
            {
                AppendToReport($"Showing first {maxDetailsToShow} differences (out of {result.differences.Count} total):");
            }

            for (int i = 0; i < maxDetailsToShow; i++)
            {
                var diff = result.differences[i];
                AppendToReport($"\n[{i + 1}] {diff.assetPath}");
                AppendToReport($"    Type: {diff.assetType} | GUID: {diff.guid}");
                AppendToReport($"    Summary: {diff.summary}");

                if (diff.missingInFR2.Count > 0)
                {
                    AppendToReport($"    Missing in FR2 ({diff.missingInFR2.Count}):");
                    foreach (var missing in diff.missingInFR2.Take(5))
                    {
                        AppendToReport($"      - {missing}");
                    }
                    if (diff.missingInFR2.Count > 5)
                    {
                        AppendToReport($"      ... and {diff.missingInFR2.Count - 5} more");
                    }
                }

                if (diff.extraInFR2.Count > 0)
                {
                    AppendToReport($"    Extra in FR2 ({diff.extraInFR2.Count}):");
                    foreach (var extra in diff.extraInFR2.Take(5))
                    {
                        AppendToReport($"      - {extra}");
                    }
                    if (diff.extraInFR2.Count > 5)
                    {
                        AppendToReport($"      ... and {diff.extraInFR2.Count - 5} more");
                    }
                }
            }
        }

        private void AppendToReport(string line)
        {
            reportBuilder.AppendLine(line);
        }

        private string ExtractPathFromDependencyString(string depString)
        {
            // Extract path from format "guid (path)"
            int startParen = depString.IndexOf('(');
            int endParen = depString.IndexOf(')', startParen);
            if (startParen >= 0 && endParen > startParen)
            {
                return depString.Substring(startParen + 1, endParen - startParen - 1);
            }
            return depString;
        }

        private void AnalyzeValidationPatterns(List<AssetDifference> differences)
        {
            // This method is now integrated into GenerateDetailedReport
        }
    }
    #endif
}