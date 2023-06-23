using UnityEngine;

public class ShieldGrowOnAwake : MonoBehaviour
{
    public int m_sizeSpeed;
    public float m_maxScale;

    private void Update()
    {
        if (transform.localScale.y > m_maxScale) return;

        transform.localScale += Vector3.one * m_sizeSpeed * Time.deltaTime;
    }
}