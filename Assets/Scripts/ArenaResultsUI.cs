using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaResultsUI : MonoBehaviour
{
    public GameObject panel;
    public Text survivalTimeText;
    public Text hitsText;
    public Text shotsText;
    public Text precisionText;
    public Text killedByTypeText;
    public Text expiredByTypeText;

    public string menuSceneName = "MainMenu";

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        var m = PracticeSessionManager.Instance;
        if (m == null) return;
        if (m.currentMode != PracticeMode.Arena) return;

        if (!m.sessionActive && !m.resultsShown)
        {
            ShowResults();
            m.resultsShown = true;
        }
    }

    void ShowResults()
    {
        var m = PracticeSessionManager.Instance;

        if (panel == null)
        {
            Debug.LogError("No asignaste el panel de ArenaResultsUI.");
            return;
        }

        panel.SetActive(true);

        if (survivalTimeText != null)
            survivalTimeText.text = "Tiempo sobrevivido: " + m.arenaSurvivalTime.ToString("F1") + "s";

        if (hitsText != null)
            hitsText.text = "Aciertos: " + m.shotsHit;

        if (shotsText != null)
            shotsText.text = "Disparos: " + m.shotsFired;

        if (precisionText != null)
            precisionText.text = "Precision: " + (m.GetPrecision() * 100f).ToString("F1") + "%";

        if (killedByTypeText != null)
            killedByTypeText.text =
                "Eliminados -> Movil: " + m.arenaMovingKilled +
                " Disco: " + m.arenaDiskKilled +
                " Sniper: " + m.arenaSniperKilled;

        if (expiredByTypeText != null)
            expiredByTypeText.text =
                "Expirados -> Movil: " + m.arenaMovingExpired +
                " Disco: " + m.arenaDiskExpired +
                " Sniper: " + m.arenaSniperExpired;

        Time.timeScale = 0f;
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
