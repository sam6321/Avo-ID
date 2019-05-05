using UnityEngine;

public class AvoSpawnerTag : MonoBehaviour
{
    public AvoSpawner Spawner { set; get; }

    void OnDestroy()
    {
        Spawner.OnSpawnedItemDestroyed(this);
    }
}
