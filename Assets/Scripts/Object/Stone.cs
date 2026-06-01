using System;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public float noiseRadius = 5f;
    public LayerMask Enemy;

    private bool hasHitGround = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(deSpawn), 3f);
    }

    // Update is called once per frame
    private void deSpawn()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!hasHitGround && collision.gameObject.CompareTag("Ground"))
        {
            hasHitGround = true;
            CreateNoise();
            gameObject.SetActive(false);
        }
    }

    private void CreateNoise()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, noiseRadius, Enemy);
        foreach (Collider2D hit in hitEnemies)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Alert(transform.position);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }
}
