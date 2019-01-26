using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractiveObject : InteractiveObject
{

    public override void Interact()
    {
        base.Interact();

        Debug.Log("I farted on the meat!");
    }

}
