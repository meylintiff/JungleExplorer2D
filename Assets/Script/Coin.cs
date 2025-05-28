using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int nilaiPoin = 1; // poin yang didapat tiap koin

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Tambah poin lewat PoinController
            PoinController.instance.TambahPoin(nilaiPoin);

            // Hancurkan koin setelah poin bertambah
            Destroy(gameObject);
        }
    }
}
