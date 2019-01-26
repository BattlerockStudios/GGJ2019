using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

    [SerializeField]
    private float m_moveSpeed = .1f;

    private void Update()
    {
        transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime, Space.Self);
    }

}
