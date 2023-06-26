using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.Runtime
{
    public class Mouse : MonoBehaviour
    {
        public static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
            => new(EventSystem.current) { position = screenPos };
        
        public static bool IsMouseOverUI(Vector2 mousePosition)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ScreenPosToPointerData(mousePosition), results);
            return results.Count > 0;
        }
    }
}
