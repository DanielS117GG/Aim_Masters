using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuInicio;
    public GameObject panelJuego;

    public GameObject panelPausa;

    private bool estaPausado = false;

    void Start()
    {
        menuInicio.SetActive(true);
        panelJuego.SetActive(false);
        panelPausa.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePausa();
        }
    }

    public void TogglePausa()
    {
        estaPausado = !estaPausado;

        panelPausa.SetActive(estaPausado);

        if (estaPausado)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void IrAJuego()
    {
        menuInicio.SetActive(false);
        panelJuego.SetActive(true);
    }


    public void RegresarMenu()
    {
        panelJuego.SetActive(false);
        menuInicio.SetActive(true);
    }

    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}
