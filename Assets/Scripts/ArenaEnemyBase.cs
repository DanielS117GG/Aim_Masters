using UnityEngine;

public abstract class ArenaEnemyBase : TargetBase
{
    public ArenaEnemyType enemyType;
    public float lifeTime = 10f;

    protected float spawnTime;
    protected bool resolved = false;
    protected ArenaSpawner ownerSpawner;

    public void Init(ArenaSpawner spawner)
    {
        ownerSpawner = spawner;
    }

    protected virtual void OnEnable()
    {
        spawnTime = Time.time;
        resolved = false;
    }

    protected virtual void Update()
    {
        if (PracticeSessionManager.Instance == null) return;
        if (!PracticeSessionManager.Instance.sessionActive) return;
        if (resolved) return;

        if (Time.time - spawnTime >= lifeTime)
        {
            resolved = true;

            PracticeSessionManager.Instance.AddArenaExpired(enemyType);

            if (ownerSpawner != null)
                ownerSpawner.OnEnemyRemoved(this);

            gameObject.SetActive(false);
        }
    }

    public override void OnHit(Vector3 hitPoint)
    {
        if (resolved) return;
        resolved = true;

        PracticeSessionManager.Instance.AddShotHit();
        PracticeSessionManager.Instance.AddArenaKill(enemyType);

        if (ownerSpawner != null)
            ownerSpawner.OnEnemyRemoved(this);

        base.OnHit(hitPoint);
    }
}
