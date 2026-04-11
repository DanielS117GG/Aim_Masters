using UnityEngine;
using System.Collections.Generic;

public class ArenaSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public ArenaMovingEnemy movingPrefab;
    public ArenaDiskEnemy diskPrefab;

    [Header("Arena Bounds")]
    public Vector3 arenaCenter = Vector3.zero;
    public Vector3 arenaSize = new Vector3(40f, 6f, 40f);

    [Header("Moving Enemy Variants")]
    public Vector2 movingSpeedRange = new Vector2(2f, 6f);
    public Vector3[] movingScales = new Vector3[]
    {
        Vector3.one,
        new Vector3(0.75f, 0.75f, 0.75f),
        new Vector3(0.5f, 0.5f, 0.5f)
    };

    [Header("Spawn Limits")]
    public int maxMovingAlive = 3;
    public int maxDiskAlive = 4;

    [Header("Spawn Intervals")]
    public float movingSpawnInterval = 3f;
    public float diskSpawnInterval = 2.5f;

    readonly List<ArenaEnemyBase> activeEnemies = new List<ArenaEnemyBase>();

    float nextMovingSpawn;
    float nextDiskSpawn;

    void Start()
    {
        nextMovingSpawn = Time.time + 1f;
        nextDiskSpawn = Time.time + 1.5f;
    }

    void Update()
    {
        var m = PracticeSessionManager.Instance;
        if (m == null || !m.sessionActive) return;
        if (m.currentMode != PracticeMode.Arena) return;

        activeEnemies.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
        m.SetArenaVisibleTargets(CountVisibleArenaTargets());

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
        foreach (var e in activeEnemies)
        {
            if (e != null && e.gameObject.activeInHierarchy && e.enemyType == type)
                count++;
        }
        return count;
    }

    int CountVisibleArenaTargets()
    {
        int count = 0;

        foreach (var e in activeEnemies)
        {
            if (e != null && e.gameObject.activeInHierarchy)
                count++;
        }

        ArenaSniperEnemy[] snipers = FindObjectsOfType<ArenaSniperEnemy>();
        foreach (var s in snipers)
        {
            if (s != null && s.gameObject.activeInHierarchy)
                count++;
        }

        return count;
    }

    void SpawnMovingEnemy()
    {
        if (movingPrefab == null) return;

        Vector3 pos = GetRandomArenaPosition();
        ArenaMovingEnemy enemy = Instantiate(movingPrefab, pos, Quaternion.identity);
        enemy.Init(this);
        enemy.moveSpeed = Random.Range(movingSpeedRange.x, movingSpeedRange.y);
        enemy.transform.localScale = movingScales[Random.Range(0, movingScales.Length)];
        enemy.arenaMinBounds = arenaCenter - arenaSize * 0.5f;
        enemy.arenaMaxBounds = arenaCenter + arenaSize * 0.5f;
        activeEnemies.Add(enemy);
    }

    void SpawnDiskEnemy()
    {
        if (diskPrefab == null) return;

        Vector3 pos = GetRandomArenaPosition();
        ArenaDiskEnemy enemy = Instantiate(diskPrefab, pos, Quaternion.identity);
        enemy.Init(this);
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