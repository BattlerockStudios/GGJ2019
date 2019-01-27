using System;
using UnityEngine;

public class IslandController : MonoBehaviour
{

    public Transform EntryPoint
    {
        get { return m_entryPoint; }
    }

    [SerializeField]
    private Transform m_entryPoint = null;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if(other.transform.tag == "Boat")
        {
            var boatControllers = other.transform.GetComponentsInParent<BoatController>();
            boatControllers[0].DockToIsland(this);
        }
    }
}
