using UnityEngine;
using System.Collections;

public class LongRangeTarget : TargetBase
{
    [Header("LongRange")]
    public LongRangeRow rowType = LongRangeRow.Close;
    public float respawnDelay = 1.5f;

    Vector3 originalPosition;
    Quaternion originalRotation;
    bool hitProcessed = false;
    Collider col;
    Renderer[] rends;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        col = GetComponent<Collider>();
        rends = GetComponentsInChildren<Renderer>(true);
    }

    void OnEnable()
    {
        hitProcessed = false;
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        if (col != null)
            col.enabled = true;

        foreach (var r in rends)
            r.enabled = true;
    }

    public override void OnHit(Vector3 hitPoint)
    {
        if (hitProcessed) return;
        hitProcessed = true;

        var m = PracticeSessionManager.Instance;
        if (m != null)
        {
            m.AddShotHit();
            m.AddLongRangeDestroyed(rowType);
        }

        HandleHitLogic();
        StartCoroutine(RespawnRoutine());
    }

    protected override void HandleHitLogic()
    {
        if (col != null)
            col.enabled = false;

        foreach (var r in rends)
            r.enabled = false;
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (PracticeSessionManager.Instance != null &&
            PracticeSessionManager.Instance.sessionActive)
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            if (col != null)
                col.enabled = true;

            foreach (var r in rends)
                r.enabled = true;

            hitProcessed = false;
        }
    }
}