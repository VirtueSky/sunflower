using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VirtueSky.Utils
{
    public class InputUtils
    {
        public static bool IsPointerOverUI(Vector2 pos)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(pos.x, pos.y)
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}