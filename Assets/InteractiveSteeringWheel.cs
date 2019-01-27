using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSteeringWheel : InteractiveObject
{

    [SerializeField]
    private BoatController m_boat = null;

    public override void BeginInteraction(IInteractionSource interactionSource)
    {
        base.BeginInteraction(interactionSource);

        m_boat.PushWheelAngle();
    }

    protected override void InteractionUpdate(bool isBeingInteractedWith)
    {
        base.InteractionUpdate(isBeingInteractedWith);

        if (isBeingInteractedWith)
        {
            var horizontalMovement = Input.GetAxisRaw("Horizontal");
            m_boat.transform.Rotate(0f, horizontalMovement, 0f);

            if (Input.GetButtonDown("Cancel"))
            {
                m_boat.PopWheelAngle();
                ReleaseInteraction();
            }
        }
    }

}