using UnityEngine;

public class EntitySpawnerEventBox : EventBox
{
    [SerializeField]
    private EntitySpawnerData m_entitySpawner;
        
    private GameObject m_entity;

    public override void StartEvent()
    {
        // Randomly chooses from a predetermined list (chosen by a designer...so don't fuck it up )

        if(m_entitySpawner == null)
        {
            Debug.LogErrorFormat("{0}: m_entitySpawner not assigned.", name);
            return;
        }

        if(m_entitySpawner.entitiesToSpawn.Length <= 0)
        {
            Debug.LogErrorFormat("{0}: No entities in the array entitiesToSpawn in m_entitySpawner. Exiting out of StartEvent()", name);
            return;
        }

        var entitiesToSpawn = m_entitySpawner.entitiesToSpawn;
        m_entity = Instantiate(entitiesToSpawn[Random.Range(0, entitiesToSpawn.Length)], transform.position, Quaternion.identity);
        var spawnedChild = m_entity.AddComponent<SpawnedChild>();
        spawnedChild.parent = transform;

        Debug.Log(this.name + " Start");
        enabled = false;
    }

    public override void StopEvent()
    {
        if(m_entity != null)
        {
            m_entity.SetActive(false);
        }

        Debug.Log(this.name + " End");
    }
}
