using UnityEngine;
using UnityEngine.UI;

public class ArenaHUD : MonoBehaviour
{
    public Text survivalTimeText;
    public Text visibleTargetsText;
    public Text livesLostText;

    void Update()
    {
        var m = PracticeSessionManager.Instance;
        if (m == null) return;
        if (m.currentMode != PracticeMode.Arena) return;

        if (survivalTimeText != null)
            survivalTimeText.text = "Tiempo: " + m.arenaSurvivalTime.ToString("F1") + "s";

        if (visibleTargetsText != null)
            visibleTargetsText.text = "Objetivos visibles: " + m.arenaVisibleTargets;

        if (livesLostText != null)
            livesLostText.text = "Expirados: " + m.arenaExpiredLives + "/" + m.arenaMaxExpiredLives;
    }
}