using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float radius;
    [SerializeField] private LayerMask WhatLayer;
    [SerializeField] private Player player;

    private bool LedgeCanDetect;
    private void Update()
    {
        if (LedgeCanDetect)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, WhatLayer);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            LedgeCanDetect = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            LedgeCanDetect = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
