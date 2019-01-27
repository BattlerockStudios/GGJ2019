using UnityEngine;

public abstract class EventBox : MonoBehaviour
{
    public abstract void StartEvent();
    public abstract void StopEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Boat")
        {
            StartEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Boat")
        {
            StopEvent();
        }
    }

}
