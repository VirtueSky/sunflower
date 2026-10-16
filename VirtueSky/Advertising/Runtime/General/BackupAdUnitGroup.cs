using System;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Utils;

namespace VirtueSky.Ads
{
    /// <summary>
    /// Pairs a primary ad unit with an optional backup ad unit of the same format.
    /// When the backup is enabled both units are loaded side by side, the primary is always
    /// preferred on show, and the backup is discarded and reloaded if it sits in cache
    /// longer than the configured expire time without being shown.
    /// When the backup is disabled this behaves exactly like loading/showing the primary alone.
    /// Primary, backup and options are configured in AdSetting and assigned by the ad client at runtime.
    /// </summary>
    [EditorIcon("icon_scriptable")]
    public class BackupAdUnitGroup : ScriptableObject
    {
        [NonSerialized] private AdUnitVariable primary;
        [NonSerialized] private AdUnitVariable backup;
        [NonSerialized] private Func<bool> useBackup;
        [NonSerialized] private Func<float> expireSeconds;

        [NonSerialized] private DateTime backupReadyTime;
        [NonSerialized] private bool backupWasReady;

        public AdUnitVariable Primary => primary;
        public AdUnitVariable Backup => backup;

        public bool IsBackupEnabled => useBackup != null && useBackup() && backup != null && !string.IsNullOrEmpty(backup.Id);

        /// <summary>
        /// Return true if the primary or the backup ad unit is ready to show.
        /// </summary>
        public bool IsReady()
        {
            var unit = Select();
            return unit != null && unit.IsReady();
        }

        public bool IsShowing => (primary != null && primary.IsShowing) || (IsBackupEnabled && backup.IsShowing);

        /// <summary>
        /// Show the selected ad unit (primary first, backup when the primary is not ready).
        /// Returns the ad unit actually shown so callbacks can be chained: group.Show().OnCompleted(...)
        /// </summary>
        public AdUnitVariable Show(string placement = "")
        {
            var unit = Select();
            if (unit == null)
            {
                VLog.LogWarning($"Advertising: {name} is not setup, make sure Advertising is initialized.");
                return null;
            }

            return unit.Show(placement);
        }

        /// <summary>
        /// Primary first, backup only when the primary has nothing to show.
        /// Falls back to the primary so callers keep the current no-op show behaviour when neither is ready.
        /// </summary>
        public AdUnitVariable Select()
        {
            if (!IsBackupEnabled) return primary;
            if (primary != null && primary.IsReady()) return primary;
            return backup.IsReady() ? backup : primary;
        }

        /// <summary>
        /// Assign primary/backup ad units and options. If groupAsset is null a runtime instance is created.
        /// </summary>
        internal static BackupAdUnitGroup Setup(BackupAdUnitGroup groupAsset, AdUnitVariable primary, AdUnitVariable backup,
            Func<bool> useBackup, Func<float> expireSeconds)
        {
            var group = groupAsset != null ? groupAsset : CreateInstance<BackupAdUnitGroup>();
            group.primary = primary;
            group.backup = backup;
            group.useBackup = useBackup;
            group.expireSeconds = expireSeconds;
            group.backupReadyTime = default;
            group.backupWasReady = false;
            return group;
        }

        internal void Init(AdSetting adSetting)
        {
            if (primary != null) primary.Init(adSetting);
            if (IsBackupEnabled) backup.Init(adSetting);
        }

        internal void Load()
        {
            LoadUnit(primary);
            if (!IsBackupEnabled) return;
            CheckBackupExpired();
            LoadUnit(backup);
        }

        private static void LoadUnit(AdUnitVariable unit)
        {
            if (unit == null || unit.IsShowing) return;
            if (!unit.IsReady() && !unit.IsLoading) unit.Load();
        }

        private void CheckBackupExpired()
        {
            var isReady = backup.IsReady();
            // Stamp the moment the backup finishes loading, so the expire window starts from a fresh ad.
            if (isReady && !backupWasReady) backupReadyTime = DateTime.UtcNow;
            backupWasReady = isReady;

            if (!isReady || backup.IsShowing) return;
            if ((DateTime.UtcNow - backupReadyTime).TotalSeconds < expireSeconds()) return;

            VLog.Log($"Advertising: backup ad unit expired, reloading: {backup.Id}");
            backup.Destroy();
            backup.Load();
            backupReadyTime = DateTime.UtcNow;
            backupWasReady = false;
        }
    }
}
