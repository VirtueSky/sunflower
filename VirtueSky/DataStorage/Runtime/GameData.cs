using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VirtueSky.DataStorage
{
    public static class GameData
    {
        private static bool isInitialized;
        private static int profile;
        private static Dictionary<string, byte[]> datas = new();
        private const int INIT_SIZE = 64;

        public static event Action OnSaveEvent;

        #region Internal Stuff

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Init()
        {
            if (isInitialized) return;
            isInitialized = true;
            Load();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] Serialize<T>(T data)
        {
            return SerializeAdapter.ToBinary(data);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T Deserialize<T>(byte[] bytes)
        {
            return SerializeAdapter.FromBinary<T>(bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void RequireNullCheck()
        {
            if (datas == null) Load();
            if (datas == null) throw new NullReferenceException();
        }

        private static string GetPath => GetDataPath($"data_{profile}.sun");

        static string GetDataPath(string name)
        {
            var persistentDataPath = GetPersistentDataPath();
            if (!Directory.Exists(persistentDataPath))
            {
                Directory.CreateDirectory(persistentDataPath);
            }

            return Path.Combine(persistentDataPath, name);
        }

        private static string GetPersistentDataPath()
        {
#if UNITY_EDITOR
            return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "TempDataStorage");
#else
            return Application.persistentDataPath;
#endif
        }

        #endregion

        #region Public API

        public static bool IsInitialized => isInitialized;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeProfile(int profile)
        {
            if (GameData.profile == profile) return;

            Save();
            GameData.profile = profile;
            Load();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VerifyProfile(int profile)
        {
            return GameData.profile == profile;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save()
        {
            OnSaveEvent?.Invoke();

            byte[] bytes = Serialize(datas);
            File.WriteAllBytes(GetPath, bytes);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async void SaveAsync()
        {
            OnSaveEvent?.Invoke();

            byte[] bytes = Serialize(datas);
            await File.WriteAllBytesAsync(GetPath, bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Load()
        {
            if (!File.Exists(GetPath))
            {
                var stream = File.Create(GetPath);
                stream.Close();
            }

            byte[] bytes = File.ReadAllBytes(GetPath);
            if (bytes.Length == 0)
            {
                datas.Clear();
                return;
            }

            datas = Deserialize<Dictionary<string, byte[]>>(bytes) ?? new Dictionary<string, byte[]>(INIT_SIZE);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async void LoadAsync()
        {
            if (!File.Exists(GetPath))
            {
                var stream = File.Create(GetPath);
                stream.Close();
            }

            byte[] bytes = await File.ReadAllBytesAsync(GetPath);
            if (bytes.Length == 0)
            {
                datas.Clear();
                return;
            }

            datas = Deserialize<Dictionary<string, byte[]>>(bytes) ?? new Dictionary<string, byte[]>(INIT_SIZE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="default">If value of <paramref name="key"/> can not be found or empty! will return the default value of data type!</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(string key, T @default = default)
        {
            RequireNullCheck();

            datas.TryGetValue(key, out byte[] value);
            if (value == null || value.Length == 0) return @default;

            return Deserialize<T>(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>(string key, out T data)
        {
            RequireNullCheck();

            bool hasKey;
            if (datas.TryGetValue(key, out byte[] value))
            {
                data = Deserialize<T>(value);
                hasKey = true;
            }
            else
            {
                data = default;
                hasKey = false;
            }

            return hasKey;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set<T>(string key, T data)
        {
            RequireNullCheck();
            byte[] bytes = Serialize(data);
            if (datas.TryAdd(key, bytes)) return;
            datas[key] = bytes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasKey(string key) => datas.ContainsKey(key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteKey(string key) => datas.Remove(key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll() => datas.Clear();

        public static void DeleteFileData()
        {
            if (File.Exists(GetPath))
            {
                File.Delete(GetPath);
            }
        }

        /// <summary>
        /// Get raw byte[] of all data of profile
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Backup()
        {
            return SerializeAdapter.ToBinary(datas);
        }

        /// <summary>
        /// Load from byte[]
        /// </summary>
        /// <param name="bytes"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Restore(byte[] bytes)
        {
            datas = SerializeAdapter.FromBinary<Dictionary<string, byte[]>>(bytes);
        }

        #endregion
    }
}