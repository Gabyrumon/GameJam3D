using UnityEngine;

namespace HUD.Runtime
{
    public class StayAtWorldRotation : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.Euler(0, -transform.parent.transform.rotation.y, 0);
        }
    }
}