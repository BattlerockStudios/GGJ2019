using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private MoralCompassUI m_compassUI = null;

    [SerializeField]
    private BoatController m_boat = null;

    [SerializeField]
    private PlayerController[] m_players = null;

    [SerializeField]
    private Transform m_winUI = null;

    [SerializeField]
    private Transform m_loseUI = null;

    public void EndSession(LevelOutcome outcome)
    {
        m_boat.gameObject.SetActive(false);

        switch (outcome)
        {
            case LevelOutcome.BoatDestroyed:
            case LevelOutcome.DriveOffLevel:
                m_winUI.gameObject.SetActive(false);
                m_loseUI.gameObject.SetActive(true);
                m_compassUI.gameObject.SetActive(false);
                break;
            case LevelOutcome.FoundHome:
                m_winUI.gameObject.SetActive(true);
                m_loseUI.gameObject.SetActive(false);
                m_compassUI.gameObject.SetActive(false);
                break;
        }

        Invoke(nameof(Reload), 5f);
    }

    private void Reload()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Update()
    {
        m_compassUI.CoverageAmount = 1f - m_players.Average(p => p.Willingness);
    }

}

public enum LevelOutcome
{
    BoatDestroyed,
    DriveOffLevel,
    FoundHome
}