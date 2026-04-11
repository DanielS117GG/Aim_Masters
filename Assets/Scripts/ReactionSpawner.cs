using UnityEngine;
using System.Collections.Generic;

public class ReactionSpawner : MonoBehaviour
{
    public ReactionTarget prefab;

    [Header("Fase normal")]
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 5f;

    [Header("Fase rapida")]
    public float fastPhaseStartTime = 20f;
    public float fastSpawnInterval = 1f;

    [Header("Limites")]
    public int maxActive = 3;
    public Vector3 areaSize = new Vector3(10, 5, 10);

    List<ReactionTarget> activeTargets = new List<ReactionTarget>();
    float nextSpawnTime = 0f;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        var m = PracticeSessionManager.Instance;
        if (m == null || !m.sessionActive) return;
        if (m.currentMode != PracticeMode.Reaction) return;

        activeTargets.RemoveAll(t => t == null || !t.gameObject.activeInHierarchy);

        if (Time.time >= nextSpawnTime)
        {
            if (activeTargets.Count < maxActive)
            {
                SpawnOne();
            }

            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        float elapsedSessionTime = PracticeSessionManager.Instance.sessionDuration - PracticeSessionManager.Instance.timeLeft;

        if (elapsedSessionTime >= fastPhaseStartTime)
        {
            nextSpawnTime = Time.time + fastSpawnInterval;
        }
        else
        {
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnOne()
    {
        Vector3 pos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(0f, areaSize.y),
            Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
        );

        ReactionTarget newTarget = Instantiate(prefab, pos, Quaternion.identity);
        newTarget.Init(this);
        activeTargets.Add(newTarget);
    }

    public void OnTargetRemoved(ReactionTarget target)
    {
        activeTargets.Remove(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            transform.position + new Vector3(0, areaSize.y / 2f, 0),
            new Vector3(areaSize.x, areaSize.y, areaSize.z)
        );
    }
}