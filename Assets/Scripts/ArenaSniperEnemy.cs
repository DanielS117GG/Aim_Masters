using UnityEngine;

public class ArenaSniperEnemy : ArenaEnemyBase
{
    private ArenaSniperSpawnPoint ownerPoint;

    public void Init(ArenaSniperSpawnPoint point)
    {
        ownerPoint = point;
    }

    protected override void OnEnable()
    {
        spawnTime = Time.time;
        resolved = false;
    }

    protected override void Update()
    {
        if (PracticeSessionManager.Instance == null) return;
        if (!PracticeSessionManager.Instance.sessionActive) return;
        if (resolved) return;

        if (Time.time - spawnTime >= lifeTime)
        {
            resolved = true;

            PracticeSessionManager.Instance.AddArenaExpired(enemyType);

            if (ownerPoint != null)
                ownerPoint.OnEnemyExpired();

            gameObject.SetActive(false);
        }
    }

    public override void OnHit(Vector3 hitPoint)
    {
        if (resolved) return;

        resolved = true;

        if (PracticeSessionManager.Instance != null)
        {
            PracticeSessionManager.Instance.AddShotHit();
            PracticeSessionManager.Instance.AddArenaKill(enemyType);
        }

        if (ownerPoint != null)
            ownerPoint.OnEnemyKilled();

        gameObject.SetActive(false);
    }

    protected override void HandleHitLogic()
    {
    }
}