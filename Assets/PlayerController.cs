using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{

    private const double AUTO_SORT_SECONDS = 2.0;

    [SerializeField]
    private float m_moveSpeed = 1f;

    [SerializeField]
    private Rigidbody m_rigidBody = null;

    private Vector2 m_inputLastFrame = Vector2.zero;
    private readonly List<InteractiveObject> m_nearbyInteractiveObjects = new List<InteractiveObject>();
    private InteractiveObject m_selectedInteractive = null;
    private DateTime? m_timeOfLastAutoSort = null;

    private void Update()
    {
        var verticalMovement = Input.GetAxisRaw("Vertical");
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        m_inputLastFrame = new Vector2(horizontalMovement, verticalMovement);

        if(!m_timeOfLastAutoSort.HasValue || (DateTime.UtcNow - m_timeOfLastAutoSort.Value).TotalSeconds > AUTO_SORT_SECONDS)
        {
            SortInteractivesByDistance();
            m_timeOfLastAutoSort = DateTime.UtcNow;
        }

        if (Input.GetButtonDown("Submit"))
        {
            m_selectedInteractive?.Interact();
        }
    }

    private void FixedUpdate()
    {
        var forwardDirection = transform.TransformDirection(new Vector3(m_inputLastFrame.x, 0f, m_inputLastFrame.y));
        m_rigidBody.MovePosition(transform.position + (forwardDirection * m_moveSpeed * Time.deltaTime));
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
            if (m_nearbyInteractiveObjects[0] != m_selectedInteractive)
            {
                SelectInteractive(m_nearbyInteractiveObjects[0]);
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

}
