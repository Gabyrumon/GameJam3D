using DG.Tweening;
using UnityEngine;

public class WindMillRotation : MonoBehaviour
{
    public float m_rotationSpeed;
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -m_rotationSpeed * Time.deltaTime));
    }
}