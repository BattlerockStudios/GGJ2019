using System;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public virtual void Interact()
    {
        // Override me
    }

    public void OnDeselect()
    {
        Debug.Log($"{name} is selected!");
    }

    public void OnSelect()
    {
        Debug.Log($"{name} is deselected!");
    }
}
