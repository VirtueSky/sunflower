#define AssetFinderDEBUG

using System;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    public class AssetFinderTimeSlice
    {
        private readonly Action onCompleteCallback;
        private readonly Action<int> processingAction;
        private readonly Func<int> targetCountFunc;

        private int currentIndex;
        public readonly float timeSlice = 1 / 100f;

        public AssetFinderTimeSlice(Func<int> countFunc, Action<int> action, Action onComplete = null)
        {
            targetCountFunc = countFunc;
            processingAction = action;
            onCompleteCallback = onComplete;
        }

        public void Start()
        {
            currentIndex = 0;
            EditorApplication.update += ProcessQueue;
        }

        public void Stop()
        {
            EditorApplication.update -= ProcessQueue;
        }

        private void ProcessQueue()
        {
            float startTime = Time.realtimeSinceStartup;
            int targetCount = targetCountFunc.Invoke();

            // Process items within the time slice
            while (currentIndex < targetCount)
            {
                processingAction.Invoke(currentIndex);
                currentIndex++;

                if (Time.realtimeSinceStartup - startTime >= timeSlice)
                {
                    #if AssetFinderDEBUG
                    float pct = currentIndex * 100f / targetCount;
                    AssetFinderLOG.Log($"Progress: {pct:0.00}% -  {currentIndex}/{targetCount}");
                    #endif
                    return;
                }
            }

            targetCount = targetCountFunc.Invoke();

            // Stop processing if we've reached the target count
            if (currentIndex < targetCount) return;

            EditorApplication.update -= ProcessQueue;
            onCompleteCallback?.Invoke();
        }
    }
}
