using UnityEngine;
using System.Collections.Generic;

public class ArenaSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    public EnemyFactory enemyFactory;

    [Header("Arena Bounds")]
    public Vector3 arenaCenter = Vector3.zero;
    public Vector3 arenaSize = new Vector3(40f, 6f, 40f);

    [Header("Spawn Limits")]
    public int maxMovingAlive = 3;
    public int maxDiskAlive = 4;

    [Header("Spawn Intervals")]
    public float movingSpawnInterval = 3f;
    public float diskSpawnInterval = 2.5f;

    readonly List<ArenaEnemyBase> activeEnemies = new List<ArenaEnemyBase>();

    float nextMovingSpawn;
    float nextDiskSpawn;
    IArenaEnemyFactory cachedFactory;

    void Awake()
    {
        cachedFactory = enemyFactory;
    }

    void Start()
    {
        nextMovingSpawn = Time.time + 1f;
        nextDiskSpawn = Time.time + 1.5f;
    }

    void Update()
    {
        PracticeSessionManager manager = PracticeSessionManager.Instance;
        if (manager == null || !manager.sessionActive) return;
        if (manager.currentMode != PracticeMode.Arena) return;

        if (cachedFactory == null)
            cachedFactory = enemyFactory;

        activeEnemies.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeInHierarchy);
        manager.SetArenaVisibleTargets(CountVisibleArenaTargets());

        if (Time.time >= nextMovingSpawn && CountAlive(ArenaEnemyType.Moving) < maxMovingAlive)
        {
            SpawnMovingEnemy();
            nextMovingSpawn = Time.time + movingSpawnInterval;
        }

        if (Time.time >= nextDiskSpawn && CountAlive(ArenaEnemyType.Disk) < maxDiskAlive)
        {
            SpawnDiskEnemy();
            nextDiskSpawn = Time.time + diskSpawnInterval;
        }
    }

    int CountAlive(ArenaEnemyType type)
    {
        int count = 0;

        foreach (ArenaEnemyBase enemy in activeEnemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy && enemy.enemyType == type)
                count++;
        }

        return count;
    }

    int CountVisibleArenaTargets()
    {
        int count = 0;

        foreach (ArenaEnemyBase enemy in activeEnemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
                count++;
        }

        ArenaSniperEnemy[] snipers = FindObjectsOfType<ArenaSniperEnemy>();
        foreach (ArenaSniperEnemy sniper in snipers)
        {
            if (sniper != null && sniper.gameObject.activeInHierarchy)
                count++;
        }

        return count;
    }

    void SpawnMovingEnemy()
    {
        if (cachedFactory == null) return;

        Vector3 position = GetRandomArenaPosition();
        ArenaMovingEnemy enemy = cachedFactory.CreateMovingEnemy(
            position,
            this,
            GetArenaMinBounds(),
            GetArenaMaxBounds()
        );

        if (enemy != null)
            activeEnemies.Add(enemy);
    }

    void SpawnDiskEnemy()
    {
        if (cachedFactory == null) return;

        Vector3 position = GetRandomArenaPosition();
        ArenaDiskEnemy enemy = cachedFactory.CreateDiskEnemy(position, this);

        if (enemy != null)
            activeEnemies.Add(enemy);
    }

    Vector3 GetRandomArenaPosition()
    {
        return arenaCenter + new Vector3(
            Random.Range(-arenaSize.x * 0.5f, arenaSize.x * 0.5f),
            Random.Range(0.5f, arenaSize.y),
            Random.Range(-arenaSize.z * 0.5f, arenaSize.z * 0.5f)
        );
    }

    public Vector3 GetArenaMinBounds()
    {
        return arenaCenter - arenaSize * 0.5f;
    }

    public Vector3 GetArenaMaxBounds()
    {
        return arenaCenter + arenaSize * 0.5f;
    }

    public void OnEnemyRemoved(ArenaEnemyBase enemy)
    {
        activeEnemies.Remove(enemy);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(arenaCenter, arenaSize);
    }
}