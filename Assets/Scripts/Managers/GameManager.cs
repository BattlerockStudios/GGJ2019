using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [SerializeField]
    private BoatController m_boat = null;

    [SerializeField]
    private PlayerController[] m_players = null;

    public void EndSession(LevelOutcome outcome)
    {
        m_boat.gameObject.SetActive(false);

        switch (outcome)
        {
            case LevelOutcome.BoatDestroyed:
            case LevelOutcome.DriveOffLevel:
                break;
            case LevelOutcome.FoundHome:
                break;
        }

        Invoke(nameof(Reload), 5f);
    }

    private void Reload()
    {
        SceneManager.LoadScene("SampleScene");
    }

}

public enum LevelOutcome
{
    BoatDestroyed,
    DriveOffLevel,
    FoundHome
}