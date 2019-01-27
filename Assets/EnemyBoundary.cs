using System.Collections.Generic;
using UnityEngine;

public interface ISpawnParent
{
    void OnChildDestroyed(SpawnedChild spawnedChild);
}

public class EnemyBoundary : MonoBehaviour, ISpawnParent
{

    [SerializeField]
    private EnemyController m_prefab = null;

    [SerializeField]
    private int m_numberToSpawn = 1;

    [SerializeField]
    private bool m_repopulate = false;

    [SerializeField]
    private Collider m_collider = null;

    private CombatEntity m_boatController = null;

    private readonly List<EnemyController> m_spawnedChildren = new List<EnemyController>();

     void ISpawnParent.OnChildDestroyed(SpawnedChild spawnedChild)
    {
        spawnedChild.transform.GetComponentIfExists<EnemyController>(c => m_spawnedChildren.Remove(c));
    }

    private void Start()
    {
        if(!m_repopulate)
        {
            for (int i = 0; i < m_numberToSpawn; i++)
            {
                SpawnChild();
            }
        }
    }

    public bool IsWithin(Vector3 point)
    {
        Vector3 closest = m_collider.ClosestPoint(point);
        Vector3 origin = m_collider.transform.position + (m_collider.transform.rotation * m_collider.bounds.center);
        Vector3 originToContact = closest - origin;
        Vector3 pointToContact = closest - point;

        // Here we make the magic, originToContact points from the center to the closest point. So if the angle between it and the pointToContact is less than 90, pointToContact is also looking from the inside-out.
        // The angle will probably be 180 or 0, but it's bad to compare exact floats and the rigidbody centerOfMass calculation could add some potential wiggle to the angle, so we use "< 90" to account for any of that.
        return (Vector3.Angle(originToContact, pointToContact) < 90);
    }

    private void Update()
    {
        if (m_repopulate && m_spawnedChildren.Count < m_numberToSpawn)
        {
            SpawnChild();
        }
    }

    private void SpawnChild()
    {
        var newChild = Instantiate(m_prefab);
        newChild.transform.position = m_collider.bounds.center;
        var childScript = newChild.gameObject.AddComponent<SpawnedChild>();
        childScript.parent = this;

        m_spawnedChildren.Add(newChild);

        newChild.SetTarget(m_boatController);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"trigger enter {other.transform.name}");
        if(other.transform.tag == "Boat")
        {
            SetBoat(other.transform.GetComponentInParent<CombatEntity>());
        }
    }

    private void SetBoat(CombatEntity boatController)
    {
        for (int i = 0; i < m_spawnedChildren.Count; i++)
        {
            m_spawnedChildren[i].SetTarget(boatController);
        }

        m_boatController = boatController;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"trigger exit {other.transform.name}");
        if (other.transform.tag == "Boat")
        {
            SetBoat(null);
        }
    }

}
