using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour, ICombatEntityEventListener
{

    [SerializeField]
    private CameraManager m_cameraManager = null;

    [SerializeField]
    private CombatEntity m_combatEntity = null;

    private IslandController m_dockedIsland = null;

    public void DockToIsland(IslandController islandController)
    {
        m_dockedIsland = islandController;
        StartCoroutine(AnimateDockToIsland(islandController));
    }

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



    [SerializeField]
    private Transform m_waterTransform = null;

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
        m_combatEntity.RegisterEventListener(this);
    }

    private void OnDestroy()
    {
        m_combatEntity.DeregisterEventListener(this);
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

    private IEnumerator AnimateDockToIsland(IslandController island)
    {
        var startTime = DateTime.UtcNow;
        var players = transform.GetComponentsInChildren<PlayerController>();
        var starts = new Vector3[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            starts[i] = players[i].transform.position;
            players[i].transform.parent = null;
            players[i].SetPhysicsEnabled(false);
        }

        var firstDirection = (new Vector3(island.transform.position.x, 0f, island.transform.position.z) - new Vector3(players[0].transform.position.x, 0f, players[0].transform.position.z)).normalized;
        var ends = new Vector3[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            ends[i] = players[i].transform.position + (firstDirection * 1f);
        }

        while (true)
        {
            var progress = (DateTime.UtcNow - startTime).TotalSeconds / .125;
            var parabolicProgress = -4f * Mathf.Pow((float)progress - .5f, 2) + 1;

            for (int i = 0; i < players.Length; i++)
            {
                players[i].transform.position = Vector3.Lerp(starts[i], ends[i], (float)progress) + new Vector3(0f, parabolicProgress * 1f, 0f);
            }

            if (progress >= 1f)
            {
                break;
            }

            yield return null;
        }

        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetPhysicsEnabled(true);
        }
    }

    private void Update()
    {
        if (m_dockedIsland == null)
        {
            m_currentSpeedFloat = Mathf.Lerp(m_currentSpeedFloat, GetSailSpeed(), Time.deltaTime * 3f);
            transform.Translate(Vector3.forward * m_currentSpeedFloat * Time.deltaTime, Space.Self);

            m_visualParent.transform.localRotation = Quaternion.Euler(0f, 0f, -m_sway);

            if (!m_wheel.IsBeingInteractedWith)
            {
                ReduceSway();
            }
        }
    }

    private void ReduceSway()
    {
        m_sway = Mathf.Lerp(m_sway, 0f, Time.deltaTime * 3f);
    }

    void ICombatEntityEventListener.HealthChanged(CombatEntity sender, int newHealth, int oldHealth)
    {
        if (newHealth <= 0)
        {
            m_gameManager.EndSession(LevelOutcome.BoatDestroyed);
        }

        var percent = Mathf.Clamp(newHealth / (float)sender.MaxHealth, 0f, sender.MaxHealth);
        var currentPosition = m_waterTransform.localPosition;
        currentPosition.y = Mathf.Lerp(.75f, .93f, 1f - percent);
        m_waterTransform.localPosition = currentPosition;
    }
}
