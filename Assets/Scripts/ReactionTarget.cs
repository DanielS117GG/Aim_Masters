using UnityEngine;

public class ReactionTarget : TargetBase
{
    public float maxLifeTime = 2f;

    float spawnTime;
    bool destroyed = false;
    ReactionSpawner ownerSpawner;

    public void Init(ReactionSpawner spawner)
    {
        ownerSpawner = spawner;
    }

    void OnEnable()
    {
        spawnTime = Time.time;
        destroyed = false;

        if (PracticeSessionManager.Instance != null)
            PracticeSessionManager.Instance.AddReactionSpawn();
    }

    void Update()
    {
        var m = PracticeSessionManager.Instance;
        if (m == null || !m.sessionActive) return;

        if (!destroyed && Time.time - spawnTime >= maxLifeTime)
        {
            m.AddReactionExpired();

            if (ownerSpawner != null)
                ownerSpawner.OnTargetRemoved(this);

            gameObject.SetActive(false);
        }
    }

    public override void OnHit(Vector3 hitPoint)
    {
        if (destroyed) return;

        float timeToHit = Time.time - spawnTime;
        bool firstShot = false;

        PracticeSessionManager.Instance.AddShotHit();
        PracticeSessionManager.Instance.AddReactionDestroyed(timeToHit, firstShot);

        destroyed = true;

        if (ownerSpawner != null)
            ownerSpawner.OnTargetRemoved(this);

        base.OnHit(hitPoint);
    }

    protected override void HandleHitLogic()
    {
    }
}