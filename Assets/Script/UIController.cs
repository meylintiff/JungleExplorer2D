using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Lanjutkan waktu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart scene sekarang
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainmenu"); // Ganti "MainMenu" sesuai nama scene kamu
    }
}
