using System;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.DataStorage;

namespace VirtueSky.AutoRuntimeInit
{
    public class AutoInitialize : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        #region Save Data Game When Pause Or Quit Game

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                GameData.Save();
            }
        }

        private void OnApplicationQuit()
        {
            GameData.Save();
        }

        #endregion
    }
}