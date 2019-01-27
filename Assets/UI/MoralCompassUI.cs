using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MoralCompassUI : MonoBehaviour
{

    public float CoverageAmount
    {
        get { return m_coverageAmount; }
        set { m_coverageAmount = value; }
    }

    [SerializeField]
    private Image m_leftHalf = null;

    [SerializeField]
    private Image m_rightHalf = null;

    [SerializeField]
    private Transform m_coverTransform = null;

    [Range(0f, 1f)]
    [FormerlySerializedAs("coverageAmount")]
    private float m_coverageAmount = 1f;

    [SerializeField]
    private Transform m_cameraTransform = null;

    [FormerlySerializedAs("WobbleSpeed")]
    [SerializeField]
    private float m_wobbleSpeed = 5f;

    private float m_angle = 0f;

    [FormerlySerializedAs("twerkAmount")]
    [SerializeField]
    private float m_wobbleAmount = 20f;

    [FormerlySerializedAs("originTransform")]
    [SerializeField]
    private Transform m_originTransform = null;

    public void Update()
    {
        m_leftHalf.fillAmount = Mathf.Lerp(0f, .5f, m_coverageAmount);
        m_rightHalf.fillAmount = Mathf.Lerp(0f, .5f, m_coverageAmount);

        var rotationTowardsCamera = Quaternion.Euler(0f, m_cameraTransform.rotation.eulerAngles.y, 0f) * Vector3.forward;

        var directionToOrigin = (m_originTransform.position - m_cameraTransform.position).normalized;
        m_angle = -Vector3.SignedAngle(rotationTowardsCamera, directionToOrigin, Vector3.up);

        transform.rotation = Quaternion.Euler(0f, 0f, m_angle);
        m_coverTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * m_wobbleSpeed) * m_wobbleAmount);
    }

}
