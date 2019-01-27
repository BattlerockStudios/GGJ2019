using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BarrelDude : MonoBehaviour
{
    private const float TRANSITIONAL_STATE_WAIT_TIME = 2.0f;
    public BarrelDudeState state = BarrelDudeState.HIDING;
    public GameObject[] objectStates;

    private SphereCollider m_collider;

    private void Start()
    {
        InitializeCollider();
    }

    private void InitializeCollider()
    {
        m_collider = GetComponent<SphereCollider>();
        m_collider.radius = 8.0f;
        m_collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        ChangeStateIfTriggered(other, BarrelDudeState.PEEKING);
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(PlayTransitionalStateThenPlayFinalState(other));
    }

    private void ChangeStateIfTriggered(Collider other, BarrelDudeState state)
    {
        if (other.transform.tag == "Boat")
        {
            objectStates[(int)state].SetActive(true);

            for (var i = 0; i < objectStates.Length; i++)
            {
                objectStates[i].SetActive(i == (int)state);
            }
        }
    }

    private IEnumerator PlayTransitionalStateThenPlayFinalState(Collider other)
    {
        ChangeStateIfTriggered(other, BarrelDudeState.WAVING);
        yield return new WaitForSeconds(TRANSITIONAL_STATE_WAIT_TIME);
        ChangeStateIfTriggered(other, BarrelDudeState.HIDING);
    }
}

public enum BarrelDudeState
{
    HIDING,
    WAVING,
    PEEKING
}
