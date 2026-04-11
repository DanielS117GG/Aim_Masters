using UnityEngine;

public class LongRangeSpawner : MonoBehaviour
{
    public LongRangeTarget prefab;
    public int count = 5;
    public Vector3 areaSize = new Vector3(20, 5, 5);

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        Vector3 pos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f),
            Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
        );

        LongRangeTarget t = Instantiate(prefab, pos, Quaternion.identity);
        //t.spawner = this;
    }

    public void OnTargetDestroyed(LongRangeTarget target)
    {
        SpawnOne();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}

