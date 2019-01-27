using System;
using UnityEngine;

public abstract class EventBox : MonoBehaviour
{
    public abstract void StartEvent();
    public abstract void StopEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Boat")
        {
            try
            {
                StartEvent();
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Boat")
        {
            try
            {
                StopEvent();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

}
