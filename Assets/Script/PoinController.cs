using UnityEngine;
using TMPro;  // wajib import ini untuk TextMeshPro

public class PoinController : MonoBehaviour
{
    public static PoinController instance;
    public TMP_Text scoreText;  // tipe TMP_Text untuk TextMeshPro

    private int poin = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void TambahPoin(int jumlah)
    {
        poin += jumlah;
        scoreText.text = "Poin: " + poin;
    }
}
