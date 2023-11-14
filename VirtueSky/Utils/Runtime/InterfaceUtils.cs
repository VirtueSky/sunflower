using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace VirtueSky.Utils
{
    public static class InterfaceUtils
    {
        public static List<T> GetAllInterfaces<T>()
        {
            var interfaces = new List<T>();
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                var childrenInterfaces = rootGameObject.GetComponentsInChildren<T>(true);
                foreach (var childInterface in childrenInterfaces)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }
    }
}