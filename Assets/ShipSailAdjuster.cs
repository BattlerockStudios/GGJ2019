using UnityEngine;

public class ShipSailAdjuster : InteractiveObject
{

    [SerializeField]
    private BoatController m_boat = null;

    public override void BeginInteraction(IInteractionSource interactionSource)
    {
        base.BeginInteraction(interactionSource);

        m_boat.ToggleSailSpeed();

        ReleaseInteraction();
    }

}
