using System.Linq;
using UnityEngine;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.GetComponent<T>();
                }
            }

            return null;
        }

        public static void SetLayerForAllChildObject(GameObject obj, int layerIndex)
        {
            obj.layer = layerIndex;
            obj.GetComponentsInChildren<Transform>().ToList().ForEach(x => { x.gameObject.layer = layerIndex; });
        }

        public static void SetTagForAllChildObject(GameObject obj, string tag)
        {
            obj.tag = tag;
            obj.GetComponentsInChildren<Transform>().ToList().ForEach(x => { x.gameObject.tag = tag; });
        }
    }
}