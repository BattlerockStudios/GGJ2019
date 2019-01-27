using System.Collections.Generic;
using UnityEngine;

public class EntitySpawnerEventBox : EventBox, ISpawnParent
{
    [SerializeField]
    private EntitySpawnerData m_entitySpawner;

    [SerializeField]
    private Transform[] m_spawnPoints;

    [SerializeField]
    private Transform m_waterTransform = null;

    private GameObject m_entity;

    public void OnChildDestroyed(SpawnedChild spawnedChild)
    {
        // Do nothing
    }

    private void Start()
    {
        
    }

    public override void StartEvent()
    {
        // Randomly chooses from a predetermined list (chosen by a designer...so don't fuck it up )

        if (m_entitySpawner == null)
        {
            Debug.LogErrorFormat("{0}: m_entitySpawner not assigned.", name);
            return;
        }

        if (m_entitySpawner.entitiesToSpawn.Length <= 0)
        {
            Debug.LogErrorFormat("{0}: No entities in the array entitiesToSpawn in m_entitySpawner. Exiting out of StartEvent()", name);
            return;
        }

        m_spawnPoints = GetComponentsInChildren<Transform>();

        var entitiesToSpawn = m_entitySpawner.entitiesToSpawn;
        m_entity = Instantiate(entitiesToSpawn[Random.Range(0, entitiesToSpawn.Length)], m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].position, Quaternion.identity);
        var spawnedChild = m_entity.AddComponent<SpawnedChild>();
        spawnedChild.parent = this;

        m_entity.GetComponentIfExists<BuoyancyController>(b => b.WaterTransform = m_waterTransform);

        Debug.Log(this.name + " Start");
        enabled = false;
    }

    public override void StopEvent()
    {
        if (m_entity != null)
        {
            m_entity.SetActive(false);
        }

        Debug.Log(this.name + " End");
    }
}
