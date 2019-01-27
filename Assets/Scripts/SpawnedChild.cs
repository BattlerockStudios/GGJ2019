using UnityEngine;

public class SpawnedChild : MonoBehaviour
{
    public ISpawnParent parent;

    private void OnDestroy()
    {
        parent.OnChildDestroyed(this);
    }
}
