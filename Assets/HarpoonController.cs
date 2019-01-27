using System;
using UnityEngine;

public class HarpoonController : InteractiveObject
{

    private Guid m_uniqueID = Guid.NewGuid();
    private Vector2 m_angles = Vector2.one;

    [SerializeField]
    private float m_maxXAngle = 45f;

    [SerializeField]
    private float m_maxYAngle = 20f;

    [SerializeField]
    private CameraManager m_cameraManager = null;

    [SerializeField]
    private Transform m_cameraTarget = null;

    private float m_initialXAngle = 0f;
    private float m_initialYAngle = 0f;

    private void Start()
    {
        m_initialXAngle = transform.localRotation.eulerAngles.x;
        m_initialYAngle = transform.localRotation.eulerAngles.y;
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
        
            if(Input.GetButtonDown("Cancel"))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                m_cameraManager.DeregisterTarget(m_uniqueID.ToString());
                ReleaseInteraction();
            }
        }
    }

}
