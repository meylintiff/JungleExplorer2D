using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour
{
    public float kecepatan = 2f;
    public float jarakGerak = 1f;

    [Header("Damage Settings")]
    public int damage = 20;
    public float knockbackForce = 5f;

    private Vector3 posisiAwal;
    private bool keAtas = true;

    void Start()
    {
        posisiAwal = transform.position;
    }

    void Update()
    {
        float gerak = kecepatan * Time.deltaTime;

        if (keAtas)
            transform.position += new Vector3(0, gerak, 0);
        else
            transform.position -= new Vector3(0, gerak, 0);

        if (Vector3.Distance(transform.position, posisiAwal) >= jarakGerak)
            keAtas = !keAtas;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            player.TakeDamage(damage, knockbackDir);
        }
    }
}
