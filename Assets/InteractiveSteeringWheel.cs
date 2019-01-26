using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSteeringWheel : InteractiveObject
{

    [SerializeField]
    private BoatController m_boat = null;

    private State m_currentState = State.Invalid;
    private Coroutine m_runningTransition = null;

    private void Start()
    {
        SetState(State.Inactive);
    }

    private void Update()
    {
        switch (m_currentState)
        {
            case State.Inactive:
                break;
            case State.Interacting:
                var horizontalMovement = Input.GetAxisRaw("Horizontal");
                m_boat.transform.Rotate(0f, horizontalMovement, 0f);

                if (Input.GetButtonDown("Cancel"))
                {
                    SetState(State.Inactive);
                }
                break;
            case State.Invalid:
            default:
                Debug.LogError($"Invalid {nameof(State)} {m_currentState}");
                break;
        }
    }

    public override void BeginInteraction(IInteractionSource interactionSource)
    {
        base.BeginInteraction(interactionSource);

        SetState(State.Interacting);
    }

    private void SetState(State newState)
    {
        if(newState != m_currentState)
        {
            switch (newState)
            {
                case State.Inactive:
                    m_boat.PopWheelAngle();
                    ReleaseInteraction();
                    break;
                case State.Interacting:
                    m_boat.PushWheelAngle();
                    break;
            }

            Debug.Log($"State changed from {m_currentState} to {newState}");
            m_currentState = newState;
        }
    }

    private enum State
    {
        Invalid = 0,
        Inactive,
        Interacting
    }

}