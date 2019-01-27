using UnityEngine;

public class EntitySpawnerEventBox : EventBox
{
    [SerializeField]
    private EntitySpawnerData m_entitySpawner;
        
    private GameObject m_entity;

    public override void StartEvent()
    {
        // Randomly chooses from a predetermined list (chosen by a designer...so don't fuck it up )
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
