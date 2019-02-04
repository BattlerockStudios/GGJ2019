using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraManager : MonoBehaviour
{

    private readonly Dictionary<string, Transform> m_registeredTargets = new Dictionary<string, Transform>();

    // Update is called once per frame
    void Update()
    {
        var lastItem = m_registeredTargets.LastOrDefault();
        if(lastItem.Value != null)
        {
            transform.position = Vector3.Lerp(transform.position, lastItem.Value.position, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, lastItem.Value.rotation, Time.deltaTime * 10f);
        }
    }

    internal void RegisterTarget(string key, Transform transformTarget)
    {
        m_registeredTargets[key] = transformTarget;
    }

    internal void DeregisterTarget(string key)
    {
        m_registeredTargets.Remove(key);
    }
}
