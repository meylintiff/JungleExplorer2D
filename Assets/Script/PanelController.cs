using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject startPanel;
    public PlayerMovement playerMovementScript;  // drag player ke sini dari Inspector
    public float durasiPanel = 2f;

    void Start()
    {
        // Tampilkan panel
        startPanel.SetActive(true);

        // Matikan input player
        playerMovementScript.SetPlayerInput(false);

        // Tunggu lalu mulai game
        StartCoroutine(HilangkanPanel());
    }

    IEnumerator HilangkanPanel()
    {
        yield return new WaitForSeconds(durasiPanel);

        // Sembunyikan panel
        startPanel.SetActive(false);

        // Aktifkan input player
        playerMovementScript.SetPlayerInput(true);
    }
}



