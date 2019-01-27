using System;
using System.Collections;
using UnityEngine;
using System.Linq;

public class HarpoonController : InteractiveObject
{

    private Vector2 m_angles = Vector2.one;

    [SerializeField]
    private float m_harpoonSpeed = .25f;

    [SerializeField]
    private float m_maxXAngle = 45f;

    [SerializeField]
    private float m_maxYAngle = 20f;

    [SerializeField]
    private float m_distance = 5f;

    [SerializeField]
    private CameraManager m_cameraManager = null;

    [SerializeField]
    private Transform m_cameraTarget = null;

    [SerializeField]
    private Transform m_cannonBarrel = null;

    [SerializeField]
    private Transform m_harpoon = null;

    [SerializeField]
    private LineRenderer m_lineRenderer = null;

    private float m_initialXAngle = 0f;
    private float m_initialYAngle = 0f;

    private Coroutine m_fireRoutineHandle = null;

    private void Start()
    {
        m_initialXAngle = transform.localRotation.eulerAngles.x;
        m_initialYAngle = transform.localRotation.eulerAngles.y;

        m_harpoon.gameObject.SetActive(false);
        m_lineRenderer.gameObject.SetActive(false);
    }

    public override void BeginInteraction(IInteractionSource interactionSource)
    {
        base.BeginInteraction(interactionSource);

        m_cameraManager.RegisterTarget(m_uniqueID.ToString(), m_cameraTarget);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void InteractionUpdate(bool isBeingInteractedWith)
    {
        base.InteractionUpdate(isBeingInteractedWith);

        if(isBeingInteractedWith)
        {
            var horizontal = Input.GetAxisRaw("Mouse X");
            var vertical = Input.GetAxisRaw("Mouse Y");
            m_angles.x = Mathf.Clamp(m_angles.x + horizontal * 2f, -m_maxXAngle, m_maxXAngle);
            m_angles.y = Mathf.Clamp(m_angles.y + vertical * 2f, -m_maxYAngle, m_maxYAngle);

            transform.localRotation = Quaternion.AngleAxis(m_angles.x + m_initialYAngle, Vector3.up) * Quaternion.AngleAxis(-m_angles.y + m_initialXAngle, Vector3.right);
        
            if(Input.GetButtonDown("Submit"))
            {
                if (m_fireRoutineHandle != null)
                {
                    StopCoroutine(m_fireRoutineHandle);
                }

                m_fireRoutineHandle = StartCoroutine(Fire());
            }

            if (Input.GetButtonDown("Cancel"))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                m_cameraManager.DeregisterTarget(m_uniqueID.ToString());
                ReleaseInteraction();
            }
        }
    }

    private void SyncChainToPositions()
    {
        m_lineRenderer.SetPosition(0, m_cannonBarrel.position);
        m_lineRenderer.SetPosition(1, m_harpoon.position);
    }

    private IEnumerator Fire()
    {
        m_harpoon.transform.position = m_cannonBarrel.position;
        m_harpoon.transform.rotation = m_cannonBarrel.rotation;
        SyncChainToPositions();
        m_harpoon.gameObject.SetActive(true);
        m_lineRenderer.gameObject.SetActive(true);

        var hitObject = default(Transform);

        var startTime = DateTime.UtcNow;
        var startPosition = m_harpoon.transform.position;
        var endPosition = m_cannonBarrel.position + (m_cannonBarrel.forward * m_distance);
        while (true)
        {
            var elapsedSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
            var percentCompleted = elapsedSeconds / m_harpoonSpeed;
            m_harpoon.transform.position = Vector3.Lerp(startPosition, endPosition, percentCompleted);
            SyncChainToPositions();

            DetectHits(out hitObject, percentCompleted * m_distance);
            if(hitObject != null)
            {
                break;
            }

            if (percentCompleted >= 1f)
            {
                break;
            }

            yield return null;
        }

        var hitEntity = default(CombatEntity);
        if(hitObject != null)
        {
            hitEntity = hitObject.GetComponentInParent<CombatEntity>();

            if (hitEntity == null)
            {
                // $$ play sounds
                hitObject.SetParent(m_harpoon, true);

                hitObject.GetComponentIfExists<BuoyancyController>(b => b.enabled = false);
                hitObject.GetComponentIfExists<Rigidbody>(r => r.isKinematic = true);
            }
        }

        startTime = DateTime.UtcNow;
        startPosition = m_harpoon.transform.position;
        endPosition = m_cannonBarrel.position;
        while (true)
        {
            var elapsedSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
            var percentCompleted = elapsedSeconds / m_harpoonSpeed;
            m_harpoon.transform.position = Vector3.Lerp(startPosition, endPosition, percentCompleted);
            SyncChainToPositions();
            if (percentCompleted >= 1f)
            {
                break;
            }

            yield return null;
        }

        if(hitEntity == null)
        {
            if (hitObject != null)
            {
                // $$ get result of item
                Destroy(hitObject.gameObject);
            }
        }
        else
        {
            hitEntity.ChangeHealth(20);
        }

        m_harpoon.gameObject.SetActive(false);
        m_lineRenderer.gameObject.SetActive(false);
    }

    private void DetectHits(out Transform hitTransform, float distance)
    {
        var hits = Physics.RaycastAll(m_cannonBarrel.position, m_cannonBarrel.forward, distance);
        hitTransform = hits
            .Where(h => h.transform != null && !h.transform.IsChildOf(m_harpoon) && h.transform.tag == "GrappleTarget")
            .OrderBy(h => h.distance)
            .Select(h => h.transform)
            .FirstOrDefault();
    }

}
