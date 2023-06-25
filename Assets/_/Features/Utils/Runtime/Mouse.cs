using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.Runtime
{
    public class Mouse : MonoBehaviour
    {
        public static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
            => new(EventSystem.current) { position = screenPos };
    }
}
