using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private CombatEntity m_target = null;

    [SerializeField]
    private float m_moveSpeed = 1f;

    [SerializeField]
    private float m_rotateSpeed = 1f;

    [SerializeField]
    private float m_slowRange = 20f;

    private EnemyBoundary m_boundary = null;

    private Transform m_targetPosition = null;

    private DateTime? m_timeOfLastAttack = null;

    private Coroutine m_attackRoutine = null;

    private double m_timeTillNextAttack = 0;

    private void Start()
    {
        m_timeTillNextAttack = UnityEngine.Random.Range(2, 6);
    }

    public void SetTarget(CombatEntity entity)
    {
        m_target = entity;
        if (entity == null)
        {
            Reset();
        }
    }

    private void Update()
    {
        if(m_target != null)
        {
            if (m_targetPosition == null)
            {
                m_targetPosition = m_target.GetCombatPosition(this);
            }

            var distanceToTarget = Vector3.Distance(transform.position, m_targetPosition.position);
            var currentSpeed = m_moveSpeed;
            if(distanceToTarget <= m_slowRange)
            {
                currentSpeed = Mathf.Lerp(m_slowRange, 1f, distanceToTarget / m_slowRange);
            }

            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

            if(distanceToTarget < .5f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((m_target.transform.position - transform.position).normalized), m_rotateSpeed * Time.deltaTime);
                AttemptAttack();
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((m_targetPosition.position - transform.position).normalized), m_rotateSpeed * Time.deltaTime);
            }

            if (m_boundary != null)
            {
                if (!m_boundary.IsWithin(transform.position))
                {
                    Reset();
                }
            }
        }
    }

    private void Reset()
    {
        if (m_attackRoutine != null)
        {
            StopCoroutine(m_attackRoutine);
            m_attackRoutine = null;
        }

        m_target = null;
        m_targetPosition = null;
    }

    private IEnumerator DoAttack()
    {
        var startTime = DateTime.UtcNow;
        var startPosition = transform.position;

        var directionToTarget = (m_target.transform.position - transform.position).normalized;
        while (true)
        {
            var elapsedSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
            var percentCompleted = elapsedSeconds / .125f;

            var endPosition = transform.position + (directionToTarget * .5f);
            startPosition.y = transform.position.y;
            endPosition.y = transform.position.y;

            transform.position = Vector3.Lerp(startPosition, endPosition, percentCompleted);
            if (percentCompleted >= 1f)
            {
                break;
            }

            yield return null;
        }

        //$$ sound
        m_target.ChangeHealth(2);

        startTime = DateTime.UtcNow;
        startPosition = transform.position;
        while (true)
        {
            var elapsedSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
            var percentCompleted = elapsedSeconds / .125f;

            startPosition.y = transform.position.y;
            transform.position = Vector3.Lerp(startPosition, m_targetPosition.position, percentCompleted);
            if (percentCompleted >= 1f)
            {
                break;
            }

            yield return null;
        }

        m_timeOfLastAttack = DateTime.UtcNow;
        m_attackRoutine = null;
        m_timeTillNextAttack = UnityEngine.Random.Range(2, 6);
    }

    private void AttemptAttack()
    {
        if(m_attackRoutine != null)
        {
            return;
        }

        if (!m_timeOfLastAttack.HasValue || (DateTime.UtcNow - m_timeOfLastAttack.Value).TotalSeconds > m_timeTillNextAttack)
        {
            m_attackRoutine = StartCoroutine(DoAttack());
        }
    }

    private void OnDestroy()
    {
        m_target?.ReleaseCombatPosition(m_targetPosition);
    }

}
