using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IInteractionSource
{
    #region Private Constants

    private const double AUTO_SORT_SECONDS = 0.3;

    #endregion   

    #region Private Variables

    private Battlerock.CharacterAction action;

    private IInputService m_inputService;

    [SerializeField]
    private Transform m_cameraParent = null;

    [SerializeField]
    private float m_moveSpeed = 3f;

    [SerializeField]
    private float m_rotationSpeed = 5f;

    [SerializeField]
    private Rigidbody m_rigidBody = null;
    private Animator m_animator = null;

    private Vector2 m_inputLastFrame = Vector2.zero;
    private readonly List<InteractiveObject> m_nearbyInteractiveObjects = new List<InteractiveObject>();
    private InteractiveObject m_selectedInteractive = null;
    private InteractiveObject m_interactingInteractive = null;
    private DateTime? m_timeOfLastAutoSort = null;

    [SerializeField]
    private Transform m_childTransformToFlip = null;
    private float m_rotationDegrees = 180.0f;

    private BoatController m_boatController = null;

    #endregion

    #region Public Properties

    public Transform CameraParent
    {
        get { return m_cameraParent; }
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        InitializeRigidbody();
        InitializeDependencies();
    }

    private void Update()
    {
        if (m_interactingInteractive == null)
        {
            var verticalMovement = m_inputService.GetVerticalMovementDirection();
            var horizontalMovement = m_inputService.GetHorizontalMovementDirection();

            m_inputLastFrame = new Vector2(horizontalMovement, verticalMovement);

            m_animator.SetFloat("MoveSpeed", m_inputLastFrame.magnitude);

            FlipCharacter(horizontalMovement);

            if (!m_timeOfLastAutoSort.HasValue || (DateTime.UtcNow - m_timeOfLastAutoSort.Value).TotalSeconds > AUTO_SORT_SECONDS)
            {
                SortInteractivesByDistance();
                m_timeOfLastAutoSort = DateTime.UtcNow;
            }

            if (m_inputService.GetInteractButtonPressed() == true)
            {
                m_selectedInteractive?.BeginInteraction(this);
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_interactingInteractive == null)
        {
            var forwardDirection = transform.TransformDirection(new Vector3(m_inputLastFrame.x, 0f, m_inputLastFrame.y));
            m_rigidBody.MovePosition(transform.position + (forwardDirection * m_moveSpeed * Time.deltaTime));
        }
    }

    #endregion

    #region Private Methods

    private void InitializeRigidbody()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void InitializeDependencies()
    {
        m_inputService = new Battlerock.InputService();
    }

    private void FlipCharacter(float inputDirection)
    {
        Vector3 direction = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;

        if (inputDirection == 0)
        {
            direction = new Vector3(m_childTransformToFlip.localRotation.eulerAngles.x, m_rotationDegrees, m_childTransformToFlip.localRotation.eulerAngles.z);
            targetRotation = Quaternion.Euler(direction);
            this.m_childTransformToFlip.localRotation = Quaternion.Lerp(m_childTransformToFlip.localRotation, targetRotation, Time.deltaTime * m_rotationSpeed);
            return;
        }

        m_rotationDegrees = inputDirection > 0 ? 180.0f : 0.0f;

        direction = new Vector3(m_childTransformToFlip.localRotation.eulerAngles.x, m_rotationDegrees, m_childTransformToFlip.localRotation.eulerAngles.z);
        targetRotation = Quaternion.Euler(direction);
        this.m_childTransformToFlip.localRotation = Quaternion.Lerp(m_childTransformToFlip.localRotation, targetRotation, Time.deltaTime * m_rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        var foundInteractive = other.GetComponentInParent<InteractiveObject>();
        if (foundInteractive != null)
        {
            m_nearbyInteractiveObjects.Add(foundInteractive);
            SortInteractivesByDistance();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var foundInteractive = other.GetComponentInParent<InteractiveObject>();
        if (foundInteractive != null)
        {
            m_nearbyInteractiveObjects.Remove(foundInteractive);
            SortInteractivesByDistance();
        }
    }

    private void SortInteractivesByDistance()
    {
        m_nearbyInteractiveObjects.Sort(
            (a, b) =>
            {
                var firstDistance = Vector3.Distance(a.transform.position, transform.position);
                var secondDistance = Vector3.Distance(b.transform.position, transform.position);
                return firstDistance.CompareTo(secondDistance);
            }
        );

        if (m_nearbyInteractiveObjects.Count > 0)
        {
            var availableInteractive = m_nearbyInteractiveObjects.FirstOrDefault(n => !n.IsBeingInteractedWith);

            if (availableInteractive != m_selectedInteractive)
            {
                SelectInteractive(availableInteractive);
            }
        }
        else
        {
            SelectInteractive(null);
        }
    }

    private void SelectInteractive(InteractiveObject selection)
    {
        m_selectedInteractive?.OnDeselect();

        m_selectedInteractive = selection;

        m_selectedInteractive?.OnSelect();
    }

    #endregion

    #region Public Methods

    public void AbortInteraction()
    {
        m_interactingInteractive?.ReleaseInteraction();
    }

    public void SetPhysicsEnabled(bool enable)
    {
        m_rigidBody.isKinematic = !enable;
    }

    public void RenderEditorGUI()
    {
        foreach (var item in m_nearbyInteractiveObjects)
        {
            GUILayout.Label($"Interactive found! {item.name}");
        }
    }

    public void OnInteractionBegin(InteractiveObject interactive)
    {
        m_interactingInteractive = interactive;
        m_rigidBody.isKinematic = true;
    }

    public void OnInteractionEnd(InteractiveObject interactive)
    {
        if (m_interactingInteractive == interactive)
        {
            m_rigidBody.isKinematic = false;
            m_interactingInteractive = null;
        }
    }

    #endregion
}
