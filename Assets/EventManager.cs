using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventManager : MonoBehaviour
{

    [SerializeField]
    private float m_cellSize = 5f;

    [SerializeField]
    private int m_columns = 3;

    private void Start()
    {
        var childEvents = GetComponentsInChildren<EventBox>();
        RandomizeBoard(childEvents);

        childEvents.FirstOrDefault(e => e is PlayerStartEventBox)?.StartEvent();
        childEvents.FirstOrDefault(e => e is EntitySpawnerEventBox)?.StartEvent();
    }

    private void RandomizeBoard(EventBox[] childEvents)
    {
        var exhaustiveList = new List<EventBox>();
        var totalCapacity = m_columns * m_columns;
        for (int i = 0; i < totalCapacity; i++)
        {
            exhaustiveList.Add(null);
        }

        for (int i = 0; i < Mathf.Min(childEvents.Length, totalCapacity); i++)
        {
            exhaustiveList[i] = childEvents[i];
        }

        if (exhaustiveList.Count > 2)
        {
            var swapChances = Random.Range(20, 40);
            for (int i = 0; i < swapChances; i++)
            {
                var firstIndex = Random.Range(0, exhaustiveList.Count);
                var secondIndex = Random.Range(0, exhaustiveList.Count);
                while (firstIndex == secondIndex)
                {
                    secondIndex = Random.Range(0, exhaustiveList.Count);
                }

                var temp = exhaustiveList[secondIndex];
                exhaustiveList[secondIndex] = exhaustiveList[firstIndex];
                exhaustiveList[firstIndex] = temp;
            }
        }

        for (int row = 0; row < m_columns; row++)
        {
            for (int col = 0; col < m_columns; col++)
            {
                var index = (row * m_columns) + col;
                var current = exhaustiveList[index];
                if (current != null)
                {
                    current.transform.localPosition = new Vector3(col * m_cellSize, 0f, row * m_cellSize);
                    current.transform.GetComponent<BoxCollider>().size = Vector3.one * m_cellSize;
                }
            }
        }
    }

    private void OnValidate()
    {
        var childEvents = GetComponentsInChildren<EventBox>();
        RandomizeBoard(childEvents);
    }

}
