using System;
using UnityEngine;

public class WaterBucket : InteractiveObject
{

    [SerializeField]
    private CombatEntity m_boatController = null;

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
                m_boatController.ChangeHealth(-5);
            }

            if (Input.GetButtonDown("Cancel") || !m_boatController.IsDamaged)
            {
                ReleaseInteraction();
            }
        }
    }

}
