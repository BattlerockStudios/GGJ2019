using UnityEngine;

public class PlayerStartEventBox : EventBox
{

    [SerializeField]
    private Transform m_playerTransform = null;

    public override void StartEvent()
    {
        if(!enabled)
        {
            return;
        }

        m_playerTransform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        m_playerTransform.rotation = transform.rotation;

        Debug.Log(this.name + " Start");

        enabled = false;
    }

    public override void StopEvent()
    {
        if(!enabled)
        {
            return;
        }

        Debug.Log(this.name + " Start");
    }

}
