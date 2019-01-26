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
        var exhaustiveList = new List<EventBox>();
        var totalCapacity = m_columns * m_columns;
        for (int i = 0; i < totalCapacity; i++)
        {
            exhaustiveList.Add(null);
        }

        var childEvents = GetComponentsInChildren<EventBox>();
        for (int i = 0; i < childEvents.Length; i++)
        {
            //$$ this can overwrite old indexes!
            exhaustiveList[Random.Range(0, exhaustiveList.Count)] = childEvents[i];
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

        for (int x = 0; x < m_columns; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                var current = exhaustiveList[x + y];
                if (current != null)
                {
                    current.transform.position = new Vector3(x * m_cellSize, 0f, y * m_cellSize);
                }
            }
        }

        var startTile = childEvents.Where(e => e is PlayerStartEventBox).FirstOrDefault();
        startTile.StartEvent();
    }

    private void OnValidate()
    {
        var column = 0;
        var row = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            var childTransform = transform.GetChild(i);
            var colliderw = childTransform.GetComponent<BoxCollider>();
            colliderw.size = Vector3.one * m_cellSize;

            childTransform.localPosition = new Vector3(column * m_cellSize, 0f, row * m_cellSize);

            column++;

            if(column >= m_columns)
            {
                column = 0;
                row++;
            }
        }
    }

}
