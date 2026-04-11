using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleResultsUI : MonoBehaviour
{
    public GameObject panel;
    public Text modeText;
    public Text precisionText;
    public Text streakText;
    public Text extra1Text;
    public Text extra2Text;
    public Text scoreText;
    public string menuSceneName = "MainMenu";

    void Start()
    {
        if (panel != null) panel.SetActive(false);
    }

    void Update()
    {
        if (PracticeSessionManager.Instance == null) return;

        if (!PracticeSessionManager.Instance.sessionActive && !PracticeSessionManager.Instance.resultsShown)
        {
            ShowResults();
            PracticeSessionManager.Instance.resultsShown = true;
        }
    }

    void ShowResults()
    {
        var m = PracticeSessionManager.Instance;

        if (panel == null) return;

        panel.SetActive(true);
        modeText.text = "Modo: " + m.currentMode;
        precisionText.text = "Precisión: " + (m.GetPrecision() * 100f).ToString("F1") + "%";
        streakText.text = "Racha máxima: " + m.maxStreak;

        if (m.currentMode == PracticeMode.Tracking)
        {
            extra1Text.text = "Tracking Hits: " + m.trackingHits;
            extra2Text.text = "Disparos: " + m.shotsFired;
            scoreText.text = "Aciertos: " + m.shotsHit;
        }
        else if (m.currentMode == PracticeMode.Reaction)
        {
            extra1Text.text = "Tiempo medio: " + m.GetReactionAverageTime().ToString("F3") + " s";
            extra2Text.text = "Destruidos: " + m.reactionDestroyed + "/" + m.reactionSpawned;
            scoreText.text = "Balas: " + m.shotsFired;
        }
        else if (m.currentMode == PracticeMode.LongRange)
        {
            extra1Text.text = "Balas: " + m.shotsFired;
            extra2Text.text = "C: " + m.closeRowDestroyed + " M: " + m.midRowDestroyed + " L: " + m.farRowDestroyed;
            scoreText.text = "Aciertos: " + m.shotsHit;
        }

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