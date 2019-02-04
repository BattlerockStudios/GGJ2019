using UnityEngine;

public class DeathBox : MonoBehaviour
{

    [SerializeField]
    private GameManager m_gameManager = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Boat")
        {
            m_gameManager.EndSession(LevelOutcome.DriveOffLevel);
        }
    }

}
