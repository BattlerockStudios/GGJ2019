using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

    [SerializeField]
    private CameraManager m_cameraManager = null;

    [SerializeField]
    private float m_moveSpeed = .1f;

    [SerializeField]
    private Transform m_defaultCameraPosition = null;

    [SerializeField]
    private Transform m_drivingCameraPosition = null;

    public void PushWheelAngle()
    {
        m_cameraManager.RegisterTarget("Wheel", m_drivingCameraPosition);
    }

    public void PopWheelAngle()
    {
        m_cameraManager.DeregisterTarget("Wheel");
    }

    private void Start()
    {
        m_cameraManager.RegisterTarget("Boat", m_defaultCameraPosition);
    }

    private void OnDestroy()
    {
        m_cameraManager.DeregisterTarget("Boat");
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime, Space.Self);
    }

}
