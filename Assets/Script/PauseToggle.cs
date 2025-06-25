using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseToggle : MonoBehaviour
{
    public Sprite pauseIcon;
    public Sprite resumeIcon;
    public Image buttonImage;

    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;

        if (buttonImage != null)
        {
            buttonImage.sprite = isPaused ? resumeIcon : pauseIcon;
        }
    }
}


