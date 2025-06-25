using UnityEngine;

public class Treasure : MonoBehaviour
{
    public GameObject winPanel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cek apakah semua koin sudah dikumpulkan
            if (PoinController.instance.SemuaKoinTerkumpul())
            {
                winPanel.SetActive(true); // Tampilkan panel menang
                Time.timeScale = 0f;      // Pause game

                // 🔊 Mainkan suara kemenangan
                other.GetComponent<PlayerMovement>()?.PlayVictorySound();
            }
            else
            {
                Debug.Log("Kumpulkan semua koin dulu!");
            }
        }
    }
}
