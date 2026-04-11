using UnityEngine;

public class ArenaSniperSpawnPoint : MonoBehaviour
{
    public ArenaSniperEnemy sniperPrefab;

    [Header("Respawn")]
    public float minRespawnTime = 10f;
    public float maxRespawnTime = 30f;

    private ArenaSniperEnemy currentEnemy;
    private float nextSpawnTime;
    private bool waitingRespawn;

    void Start()
    {
        SpawnNow();
    }

    void Update()
    {
        var manager = PracticeSessionManager.Instance;
        if (manager == null || !manager.sessionActive) return;
        if (manager.currentMode != PracticeMode.Arena) return;

        if (waitingRespawn && Time.time >= nextSpawnTime)
        {
            SpawnNow();
        }
    }

    void SpawnNow()
    {
        if (sniperPrefab == null) return;

        currentEnemy = Instantiate(sniperPrefab, transform.position, transform.rotation);
        currentEnemy.Init(this);
        waitingRespawn = false;
    }

    void ScheduleRespawn()
    {
        waitingRespawn = true;
        nextSpawnTime = Time.time + Random.Range(minRespawnTime, maxRespawnTime);
    }

    public void OnEnemyKilled()
    {
        currentEnemy = null;
        ScheduleRespawn();
    }

    public void OnEnemyExpired()
    {
        currentEnemy = null;
        ScheduleRespawn();
    }
}
