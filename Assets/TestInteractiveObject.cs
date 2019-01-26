using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractiveObject : InteractiveObject
{

    public override void BeginInteraction(IInteractionSource interactionSource)
    {
        base.BeginInteraction(interactionSource);

        Debug.Log($"Interacted with {name}");

        ReleaseInteraction();
    }

}
