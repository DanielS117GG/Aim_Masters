using UnityEngine;

public enum PracticeMode
{
    Tracking,
    Reaction,
    LongRange,
    Arena
}

public enum LongRangeRow
{
    Close,
    Mid,
    Far
}

public enum ArenaEnemyType
{
    Moving,
    Disk,
    Sniper
}

public class PracticeSessionManager : MonoBehaviour
{
    public static PracticeSessionManager Instance;

    [Header("Config")]
    public PracticeMode currentMode;
    public float sessionDuration = 120f;

    [Header("Estado")]
    public float timeLeft;
    public bool sessionActive = true;
    public bool resultsShown = false;

    [Header("Comunes")]
    public int shotsFired;
    public int shotsHit;
    public int maxStreak;
    int currentStreak;

    [Header("Tracking")]
    public float trackingAliveTime;
    public int trackingHits;

    [Header("Reaccion")]
    public int reactionSpawned;
    public int reactionDestroyed;
    public int reactionExpired;
    public float reactionTotalTime;
    public int reactionFirstShotHits;

    [Header("LongRange General")]
    public int longRangeDestroyed;

    [Header("LongRange Filas")]
    public int closeRowDestroyed;
    public int midRowDestroyed;
    public int farRowDestroyed;

    [Header("Arena")]
    public float arenaSurvivalTime;
    public int arenaVisibleTargets;
    public int arenaExpiredLives;
    public int arenaMaxExpiredLives = 10;

    public int arenaMovingKilled;
    public int arenaDiskKilled;
    public int arenaSniperKilled;

    public int arenaMovingExpired;
    public int arenaDiskExpired;
    public int arenaSniperExpired;

    void Awake()
    {
        Instance = this;
        timeLeft = sessionDuration;
        sessionActive = true;
        resultsShown = false;
    }

    void Update()
    {
        if (!sessionActive) return;

        if (currentMode == PracticeMode.Arena)
        {
            arenaSurvivalTime += Time.deltaTime;
        }
        else
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                sessionActive = false;
            }
        }
    }

    public void AddShotFired()
    {
        if (!sessionActive) return;
        shotsFired++;
    }

    public void AddShotHit()
    {
        if (!sessionActive) return;

        shotsHit++;
        currentStreak++;

        if (currentStreak > maxStreak)
            maxStreak = currentStreak;
    }

    public void AddShotMiss()
    {
        if (!sessionActive) return;
        currentStreak = 0;
    }

    public float GetPrecision()
    {
        if (shotsFired <= 0) return 0f;
        return (float)shotsHit / shotsFired;
    }

    public void AddTrackingAliveTime(float delta)
    {
        if (!sessionActive) return;
        trackingAliveTime += delta;
    }

    public void AddTrackingHit()
    {
        if (!sessionActive) return;
        trackingHits++;
    }

    public float GetTrackingHitsPerSecond()
    {
        if (trackingAliveTime <= 0f) return 0f;
        return trackingHits / trackingAliveTime;
    }

    public void AddReactionSpawn()
    {
        if (!sessionActive) return;
        reactionSpawned++;
    }

    public void AddReactionDestroyed(float timeToHit, bool firstShot)
    {
        if (!sessionActive) return;

        reactionDestroyed++;
        reactionTotalTime += timeToHit;

        if (firstShot)
            reactionFirstShotHits++;
    }

    public void AddReactionExpired()
    {
        if (!sessionActive) return;

        reactionExpired++;
        currentStreak = 0;
    }

    public float GetReactionAverageTime()
    {
        if (reactionDestroyed <= 0) return 0f;
        return reactionTotalTime / reactionDestroyed;
    }

    public float GetReactionDestroyedPercent()
    {
        if (reactionSpawned <= 0) return 0f;
        return (float)reactionDestroyed / reactionSpawned;
    }

    public float GetReactionExpiredPercent()
    {
        if (reactionSpawned <= 0) return 0f;
        return (float)reactionExpired / reactionSpawned;
    }

    public float GetReactionFirstShotPercent()
    {
        if (reactionSpawned <= 0) return 0f;
        return (float)reactionFirstShotHits / reactionSpawned;
    }

    public int GetReactionMissedTargets()
    {
        return reactionExpired;
    }

    public void AddLongRangeDestroyed(LongRangeRow row)
    {
        if (!sessionActive) return;

        longRangeDestroyed++;

        switch (row)
        {
            case LongRangeRow.Close:
                closeRowDestroyed++;
                break;
            case LongRangeRow.Mid:
                midRowDestroyed++;
                break;
            case LongRangeRow.Far:
                farRowDestroyed++;
                break;
        }
    }

    public float GetBulletsPerLongRangeTarget()
    {
        if (longRangeDestroyed <= 0) return 0f;
        return (float)shotsFired / longRangeDestroyed;
    }

    public void SetArenaVisibleTargets(int count)
    {
        arenaVisibleTargets = count;
    }

    public void AddArenaKill(ArenaEnemyType type)
    {
        if (!sessionActive) return;

        switch (type)
        {
            case ArenaEnemyType.Moving:
                arenaMovingKilled++;
                break;
            case ArenaEnemyType.Disk:
                arenaDiskKilled++;
                break;
            case ArenaEnemyType.Sniper:
                arenaSniperKilled++;
                break;
        }
    }

    public void AddArenaExpired(ArenaEnemyType type)
    {
        if (!sessionActive) return;

        arenaExpiredLives++;

        switch (type)
        {
            case ArenaEnemyType.Moving:
                arenaMovingExpired++;
                break;
            case ArenaEnemyType.Disk:
                arenaDiskExpired++;
                break;
            case ArenaEnemyType.Sniper:
                arenaSniperExpired++;
                break;
        }

        currentStreak = 0;

        if (arenaExpiredLives >= arenaMaxExpiredLives)
        {
            sessionActive = false;
        }
    }

    public int GetArenaTotalKills()
    {
        return arenaMovingKilled + arenaDiskKilled + arenaSniperKilled;
    }

    public int GetArenaTotalExpired()
    {
        return arenaMovingExpired + arenaDiskExpired + arenaSniperExpired;
    }
}