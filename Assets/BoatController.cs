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

    private PlayerController[] m_islandPlayers = new PlayerController[0];
    private Vector3[] m_islandEnds = new Vector3[0];

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
            players[i].SetIsland(island, this);
            players[i].AbortInteraction();
        }

        m_cameraManager.RegisterTarget("test", players[0].CameraParent);

        while (true)
        {
            var progress = (DateTime.UtcNow - startTime).TotalSeconds / 2f;
            var parabolicProgress = -4f * Mathf.Pow((float)progress - .5f, 2) + 1;

            for (int i = 0; i < players.Length; i++)
            {
                players[i].transform.position = Vector3.Lerp(starts[i], island.EntryPoint.position, (float)progress) + new Vector3(0f, parabolicProgress * 8f, 0f);
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

        m_islandEnds = starts;
        m_islandPlayers = players;
    }

    private IEnumerator AnimateFromIslandToBoat()
    {
        var startTime = DateTime.UtcNow;
        var starts = new Vector3[m_islandPlayers.Length];
        for (int i = 0; i < m_islandPlayers.Length; i++)
        {
            starts[i] = m_islandPlayers[i].transform.position;
            m_islandPlayers[i].SetPhysicsEnabled(false);
        }

        m_cameraManager.DeregisterTarget("test");

        while (true)
        {
            var progress = (DateTime.UtcNow - startTime).TotalSeconds / 2f;
            var parabolicProgress = -4f * Mathf.Pow((float)progress - .5f, 2) + 1;

            for (int i = 0; i < m_islandPlayers.Length; i++)
            {
                m_islandPlayers[i].transform.position = Vector3.Lerp(starts[i], m_islandEnds[i], (float)progress) + new Vector3(0f, parabolicProgress * 8f, 0f);
            }

            if (progress >= 1f)
            {
                break;
            }

            yield return null;
        }

        for (int i = 0; i < m_islandPlayers.Length; i++)
        {
            m_islandPlayers[i].transform.parent = transform;
            m_islandPlayers[i].SetPhysicsEnabled(true);
        }

        startTime = DateTime.UtcNow;
        var startRot = transform.rotation;
        var end = Quaternion.Inverse(startRot);
        while (true)
        {
            var progress = (DateTime.UtcNow - startTime).TotalSeconds / 1f;
            transform.rotation = Quaternion.Slerp(startRot, end, (float)progress);
            if (progress >= 1f)
            {
                break;
            }

            yield return null;
        }

        m_dockedIsland = null;
    }

    public void EndIsland(PlayerController playerController)
    {
        StartCoroutine(AnimateFromIslandToBoat());
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
