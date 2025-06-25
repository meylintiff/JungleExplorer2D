using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanelController : MonoBehaviour
{
    public GameObject menuPanel;     // Ini MenuPanel
    public GameObject menuButton;    // Ini Menu (ikon di pojok)

    private bool isPaused = false;

    void Start()
    {
        menuPanel.SetActive(false); // Panel disembunyikan di awal
    }

    public void ToggleMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            menuPanel.SetActive(true); 
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        menuButton.SetActive(true); // Tampilkan tombol Menu lagi
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainmenu"); // Ubah sesuai nama scene main menu kamu
    }
}
