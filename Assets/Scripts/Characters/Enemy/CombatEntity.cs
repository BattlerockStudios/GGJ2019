using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CombatEntity : MonoBehaviour
{

    private readonly List<ICombatEntityEventListener> m_eventListeners = new List<ICombatEntityEventListener>();

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
            if(!m_positionIndexToLockMap.TryGetValue(i, out object value))
            {
                m_positionIndexToLockMap[i] = @lock;
                return m_combatPosition[i];
            }
            else
            {
                if(value == null)
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
        if(position == null)
        {
            return;
        }

        for (int i = 0; i < m_combatPosition.Length; i++)
        {
            if(m_combatPosition[i] == position)
            {
                m_positionIndexToLockMap.Remove(i);
                return;
            }
        }
    }

    private readonly Dictionary<int, object> m_positionIndexToLockMap = new Dictionary<int, object>();

    [SerializeField]
    private Transform[] m_combatPosition = null;

    [SerializeField]
    private int m_maxHealth = 100;

    [SerializeField]
    private int m_health = 100;

    [SerializeField]
    private Renderer m_renderer = null;

    [SerializeField]
    [FormerlySerializedAs("m_audioSource")]
    private AudioSource m_damageAudioSource = null;

    [SerializeField]
    private AudioSource m_healAudioSource = null;

    private Coroutine m_damageRoutine = null;

    private Color m_startingColor;

    private void Start()
    {
        m_startingColor = m_renderer.material.color;
    }

    private IEnumerator FlashColor(bool isHeal)
    {
        var startTime = DateTime.UtcNow;
        while(true)
        {
            var progress = (DateTime.UtcNow - startTime).TotalSeconds / .125;
            var parabolicProgress = -4f * Mathf.Pow((float)progress - .5f, 2) + 1;
            m_renderer.material.color = Color.Lerp(m_startingColor, isHeal ? Color.green : Color.red, parabolicProgress);
            if (progress >= 1f)
            {
                break;
            }

            yield return null;
        }

        m_damageRoutine = null;
    }

    public void ChangeHealth(int damage)
    {
        var oldHealth = m_health;
        m_health -= damage;

        var isHeal = damage < 0;
        if(!isHeal)
        {
            if (m_damageAudioSource != null)
            {
                m_damageAudioSource.Play();
            }
        }
        else
        {
            if (m_healAudioSource != null)
            {
                m_healAudioSource.Play();
            }
        }

        for (int i = 0; i < m_eventListeners.Count; i++)
        {
            m_eventListeners[i]?.HealthChanged(this, m_health, oldHealth);
        }

        if(m_health <= 0)
        {
            Destroy(gameObject);
        }

        if(m_damageRoutine != null)
        {
            StopCoroutine(m_damageRoutine);
        }

        if (m_renderer != null)
        {
            m_damageRoutine = StartCoroutine(FlashColor(isHeal));
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

}

public interface ICombatEntityEventListener
{
    void HealthChanged(CombatEntity sender, int newHealth, int oldHealth);
}