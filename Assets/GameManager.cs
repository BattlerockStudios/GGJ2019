using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private MoralCompassUI m_compassUI = null;

    [SerializeField]
    private PlayerController[] m_players = null;

    private void Update()
    {
        m_compassUI.CoverageAmount = 1f - m_players.Average(p => p.Willingness);
    }

}
