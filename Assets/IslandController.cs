using System;
using UnityEngine;

public class IslandController : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
        if(collision.transform.tag == "Boat")
        {
            var boatControllers = collision.transform.GetComponentsInParent<BoatController>();
            boatControllers[0].DockToIsland(this);
        }
    }

}
