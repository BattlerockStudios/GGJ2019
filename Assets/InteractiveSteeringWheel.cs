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

        m_boat.OnWheelInteractBegin(this);
    }

    protected override void InteractionUpdate(bool isBeingInteractedWith)
    {
        base.InteractionUpdate(isBeingInteractedWith);

        if (isBeingInteractedWith)
        {
            if(!m_boat.OnWheelInteractionUpdate(this))
            {
                ReleaseInteraction();
            }
        }
    }

}