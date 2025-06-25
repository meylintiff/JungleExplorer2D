using UnityEngine;
using TMPro;

public class PoinController : MonoBehaviour
{
    public static PoinController instance;
    public TMP_Text scoreText;

    private int poin = 0;
    private int totalKoin = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        // Hitung semua coin di scene (yang punya tag "Coin")
        totalKoin = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdateUI();
    }

    public void TambahPoin(int jumlah)
    {
        poin += jumlah;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = poin + " / " + totalKoin;
    }

    public bool SemuaKoinTerkumpul()
    {
        return poin >= totalKoin;
    }
}
