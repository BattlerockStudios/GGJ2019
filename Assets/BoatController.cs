using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

    [SerializeField]
    private CameraManager m_cameraManager = null;

    private SailSpeed m_currentSpeed = SailSpeed.Slow;

    public void ToggleSailSpeed()
    {
        var currentValue = (int)m_currentSpeed;
        currentValue++;
        if(currentValue > (int)SailSpeed.Fast)
        {
            currentValue = 0;
        }

        m_currentSpeed = (SailSpeed)currentValue;
    }

    private enum SailSpeed
    {
        Slow = 0,
        Medium,
        Fast
    }

    private const int MAX_HEALTH = 100;

    public bool IsDamaged
    {
        get { return m_health < MAX_HEALTH; }
    }

    [SerializeField]
    private Transform m_waterTransform = null;

    [SerializeField]
    private int m_health = MAX_HEALTH;

    private float m_sway = 0f;
    private float m_currentSpeedFloat = 0f;

    [SerializeField]
    private InteractiveSteeringWheel m_wheel = null;

    [SerializeField]
    private Transform m_defaultCameraPosition = null;

    [SerializeField]
    private Transform m_drivingCameraPosition = null;

    [SerializeField]
    private GameManager m_gameManager = null;

    public void OnWheelInteractBegin(InteractiveSteeringWheel interactiveSteeringWheel)
    {
        m_cameraManager.RegisterTarget("Wheel", m_drivingCameraPosition);
    }

    public void ChangeHealth(int damage)
    {
        m_health -= damage;
        if(m_health <= 0)
        {
            m_gameManager.EndSession(LevelOutcome.BoatDestroyed);
        }

        var percent = Mathf.Clamp(m_health / (float)MAX_HEALTH, 0f, MAX_HEALTH);
        var currentPosition = m_waterTransform.localPosition;
        currentPosition.y = Mathf.Lerp(.75f, .93f, 1f - percent);
        m_waterTransform.localPosition = currentPosition;
    }

    public bool OnWheelInteractionUpdate(InteractiveSteeringWheel interactiveSteeringWheel)
    {
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0f, horizontalMovement * GetSailSpeed() * Time.deltaTime * 10f, 0f);
        m_sway = Mathf.Clamp(m_sway + horizontalMovement, -10f,10f);

        if (Mathf.Abs(horizontalMovement) < float.Epsilon)
        {
            ReduceSway();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            m_cameraManager.DeregisterTarget("Wheel");
            return false;
        }
        else
        {
            return true;
        }
    }

    [SerializeField]
    private Transform m_visualParent = null;

    private void Start()
    {
        m_cameraManager.RegisterTarget("Boat", m_defaultCameraPosition);
    }

    private void OnDestroy()
    {
        m_cameraManager.DeregisterTarget("Boat");
    }

    private float GetSailSpeed()
    {
        switch (m_currentSpeed)
        {
            case SailSpeed.Slow: return .5f;
            case SailSpeed.Medium: return 2f;
            case SailSpeed.Fast: return 5f;
            default: return 0f;
        }
    }

    private void Update()
    {
        m_currentSpeedFloat = Mathf.Lerp(m_currentSpeedFloat, GetSailSpeed(), Time.deltaTime * 3f);
        transform.Translate(Vector3.forward * m_currentSpeedFloat * Time.deltaTime, Space.Self);
        m_visualParent.transform.localRotation = Quaternion.Euler(0f, 0f, -m_sway);

        if (!m_wheel.IsBeingInteractedWith)
        {
            ReduceSway();
        }
    }

    private void ReduceSway()
    {
        m_sway = Mathf.Lerp(m_sway, 0f, Time.deltaTime * 3f);
    }

}
