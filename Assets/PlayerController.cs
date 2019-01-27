using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IInteractionSource
{
    private const double AUTO_SORT_SECONDS = 0.3;

    public float Willingness
    {
        get { return m_willingness; }
        set { m_willingness = value; }
    }

    [SerializeField]
    private float m_moveSpeed = 1f;

    [SerializeField]
    private float m_rotationSpeed = 5f;

    [SerializeField]
    private Rigidbody m_rigidBody = null;

    private Vector2 m_inputLastFrame = Vector2.zero;
    private readonly List<InteractiveObject> m_nearbyInteractiveObjects = new List<InteractiveObject>();
    private InteractiveObject m_selectedInteractive = null;
    private InteractiveObject m_interactingInteractive = null;
    private DateTime? m_timeOfLastAutoSort = null;
    private float m_willingness = 0f;

    private Animator m_animator = null;

    [SerializeField]
    private Transform m_childTransformToFlip = null;
    private float m_rotationDegrees = 180.0f;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
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

    private void Update()
    {
        if (m_interactingInteractive == null)
        {
            var verticalMovement = Input.GetAxisRaw("Vertical");
            var horizontalMovement = Input.GetAxisRaw("Horizontal");
            m_inputLastFrame = new Vector2(horizontalMovement, verticalMovement);

            m_animator.SetFloat("MoveSpeed", m_inputLastFrame.magnitude);

            FlipCharacter(horizontalMovement);

            if (!m_timeOfLastAutoSort.HasValue || (DateTime.UtcNow - m_timeOfLastAutoSort.Value).TotalSeconds > AUTO_SORT_SECONDS)
            {
                SortInteractivesByDistance();
                m_timeOfLastAutoSort = DateTime.UtcNow;
            }

            if (Input.GetButtonDown("Submit"))
            {
                m_selectedInteractive?.BeginInteraction(this);
            }
        }
    }

    private void FixedUpdate()
    {
        var forwardDirection = transform.TransformDirection(new Vector3(m_inputLastFrame.x, 0f, m_inputLastFrame.y));
        m_rigidBody.MovePosition(transform.position + (forwardDirection * m_moveSpeed * Time.deltaTime));
    }

    public void SetPhysicsEnabled(bool enable)
    {
        m_rigidBody.isKinematic = !enable;
    }

    private void OnTriggerEnter(Collider other)
    {
        var foundInteractive = other.GetComponentInParent<InteractiveObject>();
        if(foundInteractive != null)
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
            (a,b)=> 
            {
                var firstDistance = Vector3.Distance(a.transform.position, transform.position);
                var secondDistance = Vector3.Distance(b.transform.position, transform.position);
                return firstDistance.CompareTo(secondDistance);
            }
        );

        if(m_nearbyInteractiveObjects.Count > 0)
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
}
