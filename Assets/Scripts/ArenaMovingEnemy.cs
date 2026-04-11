using UnityEngine;

public class ArenaMovingEnemy : ArenaEnemyBase
{
    public float moveSpeed = 3f;
    public float directionChangeInterval = 1.5f;
    public Vector3 arenaMinBounds = new Vector3(-20f, 0.5f, -20f);
    public Vector3 arenaMaxBounds = new Vector3(20f, 3f, 20f);

    Vector3 moveDirection;
    float nextDirectionChangeTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        PickNewDirection();
    }

    protected override void Update()
    {
        base.Update();

        if (resolved) return;
        if (PracticeSessionManager.Instance == null || !PracticeSessionManager.Instance.sessionActive) return;

        if (Time.time >= nextDirectionChangeTime)
            PickNewDirection();

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, arenaMinBounds.x, arenaMaxBounds.x);
        p.y = Mathf.Clamp(p.y, arenaMinBounds.y, arenaMaxBounds.y);
        p.z = Mathf.Clamp(p.z, arenaMinBounds.z, arenaMaxBounds.z);
        transform.position = p;
    }

    void PickNewDirection()
    {
        moveDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-0.3f, 0.3f),
            Random.Range(-1f, 1f)
        ).normalized;

        nextDirectionChangeTime = Time.time + directionChangeInterval;
    }

    protected override void HandleHitLogic()
    {
    }
}
