using UnityEngine;

public static class PracticeScoring
{
    public static float GetTrackingScore()
    {
        var m = PracticeSessionManager.Instance;

        float precisionScore = m.GetPrecision() * 100f;

        float idealHits = 60f;
        float hitRatio = (float)m.trackingHits / idealHits;
        float volumeScore = Mathf.Clamp01(hitRatio) * 100f;

        return 0.7f * precisionScore + 0.3f * volumeScore;
    }

    public static float GetReactionScore()
    {
        var m = PracticeSessionManager.Instance;

        float precisionScore = m.GetPrecision() * 100f;
        float destroyedPercentScore = m.GetReactionDestroyedPercent() * 100f;
        float firstShotScore = m.GetReactionFirstShotPercent() * 100f;

        float avgTime = m.GetReactionAverageTime();
        float timeScore = 0f;

        if (avgTime > 0f)
        {
            float normalized = Mathf.InverseLerp(1.5f, 0.2f, avgTime);
            timeScore = normalized * 100f;
        }

        return precisionScore * 0.25f +
               destroyedPercentScore * 0.35f +
               timeScore * 0.25f +
               firstShotScore * 0.15f;
    }

    public static float GetLongRangeScore()
    {
        var m = PracticeSessionManager.Instance;
        return m.shotsHit;
    }
}