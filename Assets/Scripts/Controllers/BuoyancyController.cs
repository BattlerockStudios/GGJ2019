using System;
using UnityEngine;

public class BuoyancyController : MonoBehaviour
{

    public Transform WaterTransform
    {
        get { return m_waterTransform; }
        set { m_waterTransform = value; }
    }

    [SerializeField]
    private float m_bobSpeed = .1f;

    [SerializeField]
    private Vector3 m_rotateSpeed = new Vector3(1f, 0f, 1f);

    [SerializeField]
    private Transform m_waterTransform = null;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, m_waterTransform.position.y + (Mathf.Sin(Time.time * 2f) * m_bobSpeed), transform.position.z);

        transform.Rotate(
                GetRandomValue() * m_rotateSpeed.x,
                GetRandomValue() * m_rotateSpeed.y,
                GetRandomValue() * m_rotateSpeed.z,
            Space.World
        );
    }

    private float GetRandomValue()
    {
        return Mathf.Lerp(-1f, 1f, Mathf.PerlinNoise(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
    }

}
