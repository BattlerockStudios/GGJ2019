using System;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{

    public Material SelectedMaterial = null;
    public Material DeselectedMaterial = null;
    public Renderer Renderer = null;

    protected IInteractionSource m_interactionSource = null;

    public virtual void BeginInteraction(IInteractionSource interactionSource)
    {
        m_interactionSource = interactionSource;
        m_interactionSource.OnInteractionBegin(this);
    }

    public void OnDeselect()
    {
        Renderer.material = DeselectedMaterial;

        Debug.Log($"{name} is deselected!");
    }

    public void OnSelect()
    {
        Renderer.material = SelectedMaterial;

        Debug.Log($"{name} is selected!");
    }

    public void ReleaseInteraction()
    {
        m_interactionSource?.OnInteractionEnd(this);
        m_interactionSource = null;
    }

}

public interface IInteractionSource
{
    void OnInteractionBegin(InteractiveObject interactive);
    void OnInteractionEnd(InteractiveObject interactive);
}