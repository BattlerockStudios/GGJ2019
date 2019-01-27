using System;
using UnityEngine;

public class WaterBucket : InteractiveObject
{

    [SerializeField]
    private BoatController m_boatController = null;

    public override void BeginInteraction(IInteractionSource interactionSource)
    {
        base.BeginInteraction(interactionSource);

        if(!m_boatController.IsDamaged)
        {
            ReleaseInteraction();
        }
    }

    protected override void InteractionUpdate(bool isBeingInteractedWith)
    {
        base.InteractionUpdate(isBeingInteractedWith);

        if (isBeingInteractedWith)
        {
            if (Input.GetButtonDown("Submit"))
            {
                m_boatController.ChangeHealth(-1);
            }

            if(!m_boatController.IsDamaged)
            {
                ReleaseInteraction();
            }
        }
    }

}
