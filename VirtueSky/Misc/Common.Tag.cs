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

        public static GameObject SetLayerForAllChildObject(this GameObject obj, int layerIndex)
        {
            obj.layer = layerIndex;
            obj.GetComponentsInChildren<Transform>().ToList().ForEach(x => { x.gameObject.layer = layerIndex; });
            return obj;
        }

        public static GameObject SetLayer(this GameObject obj, int layerIndex)
        {
            obj.layer = layerIndex;
            return obj;
        }

        public static GameObject SetTagForAllChildObject(this GameObject obj, string tag)
        {
            obj.tag = tag;
            obj.GetComponentsInChildren<Transform>().ToList().ForEach(x => { x.gameObject.tag = tag; });
            return obj;
        }

        public static GameObject SetTag(this GameObject obj, string tag)
        {
            obj.gameObject.tag = tag;
            return obj;
        }
    }
}