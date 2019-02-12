using System.Collections.Generic;
using UnityEngine;

public class CombatEntity : MonoBehaviour
{

    public int MaxHealth
    {
        get { return m_maxHealth; }
    }

    public bool IsDamaged
    {
        get { return m_health < m_maxHealth; }
    }

    public Transform GetCombatPosition(object @lock)
    {
        for (int i = 0; i < m_combatPosition.Length; i++)
        {
            if (!m_positionIndexToLockMap.TryGetValue(i, out object value))
            {
                m_positionIndexToLockMap[i] = @lock;
                return m_combatPosition[i];
            }
            else
            {
                if (value == null)
                {
                    m_positionIndexToLockMap[i] = @lock;
                    return m_combatPosition[i];
                }
            }
        }

        return null;
    }

    public void ReleaseCombatPosition(Transform position)
    {
        if (position == null)
        {
            return;
        }

        for (int i = 0; i < m_combatPosition.Length; i++)
        {
            if (m_combatPosition[i] == position)
            {
                m_positionIndexToLockMap.Remove(i);
                return;
            }
        }
    }

    public void ChangeHealth(int damage)
    {
        var oldHealth = m_health;
        m_health -= damage;

        var isHeal = damage < 0;
        if (!isHeal)
        {
            // $$ZAS: hurt noise
        }
        else
        {
            // $$ZAS: heal noise
        }

        for (int i = 0; i < m_eventListeners.Count; i++)
        {
            m_eventListeners[i]?.HealthChanged(this, m_health, oldHealth);
        }

        if (m_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEventListener(BoatController boatController)
    {
        m_eventListeners.Add(boatController);
    }

    public void DeregisterEventListener(BoatController boatController)
    {
        m_eventListeners.Remove(boatController);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_combatPosition != null)
        {
            for (int i = 0; i < m_combatPosition.Length; i++)
            {
                Gizmos.DrawSphere(m_combatPosition[i].position, .5f);
            }
        }
    }

    [SerializeField]
    private Transform[] m_combatPosition = null;

    [SerializeField]
    private int m_maxHealth = 100;

    [SerializeField]
    private int m_health = 100;

    private readonly List<ICombatEntityEventListener> m_eventListeners = new List<ICombatEntityEventListener>();
    private readonly Dictionary<int, object> m_positionIndexToLockMap = new Dictionary<int, object>();

}

public interface ICombatEntityEventListener
{
    void HealthChanged(CombatEntity sender, int newHealth, int oldHealth);
}