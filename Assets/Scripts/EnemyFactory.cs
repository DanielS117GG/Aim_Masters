using UnityEngine;

public class EnemyFactory : MonoBehaviour, IArenaEnemyFactory
{
    [Header("Prefabs")]
    public ArenaMovingEnemy movingPrefab;
    public ArenaDiskEnemy diskPrefab;

    [Header("Moving Enemy Variants")]
    public Vector2 movingSpeedRange = new Vector2(2f, 6f);
    public Vector3[] movingScales = new Vector3[]
    {
        Vector3.one,
        new Vector3(0.75f, 0.75f, 0.75f),
        new Vector3(0.5f, 0.5f, 0.5f)
    };

    public ArenaMovingEnemy CreateMovingEnemy(Vector3 position, ArenaSpawner ownerSpawner, Vector3 arenaMinBounds, Vector3 arenaMaxBounds)
    {
        if (movingPrefab == null) return null;

        ArenaMovingEnemy enemy = Instantiate(movingPrefab, position, Quaternion.identity);
        enemy.Init(ownerSpawner);
        enemy.moveSpeed = Random.Range(movingSpeedRange.x, movingSpeedRange.y);

        if (movingScales != null && movingScales.Length > 0)
            enemy.transform.localScale = movingScales[Random.Range(0, movingScales.Length)];

        enemy.arenaMinBounds = arenaMinBounds;
        enemy.arenaMaxBounds = arenaMaxBounds;

        return enemy;
    }

    public ArenaDiskEnemy CreateDiskEnemy(Vector3 position, ArenaSpawner ownerSpawner)
    {
        if (diskPrefab == null) return null;

        ArenaDiskEnemy enemy = Instantiate(diskPrefab, position, Quaternion.identity);
        enemy.Init(ownerSpawner);
        return enemy;
    }
}
