using UnityEngine;

public abstract class EventBox : MonoBehaviour
{
    public abstract void StartEvent();
    public abstract void StopEvent();

    private void OnTriggerEnter(Collider other)
    {
        StartEvent();   
    }

    private void OnTriggerExit(Collider other)
    {
        StopEvent();
    }

}
