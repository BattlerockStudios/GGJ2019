using System;
using UnityEngine;

public static class Extensions
{

    public static void GetComponentIfExists<T>(this GameObject objectReference, Action<T> accessFunction)
        where T : Component
    {
        var foundComponent = objectReference.GetComponent<T>();
        if (foundComponent != null)
        {
            accessFunction(foundComponent);
        }
    }

    public static void GetComponentIfExists<T>(this Transform objectReference, Action<T> accessFunction)
     where T : Component
    {
        objectReference.gameObject.GetComponentIfExists<T>(accessFunction);
    }

}
