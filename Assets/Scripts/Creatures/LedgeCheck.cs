using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float radius;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private Player player;

    private bool LedgeCanDetect;
    private void Update()
    {
        if (LedgeCanDetect)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, Ground);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            LedgeCanDetect = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            LedgeCanDetect = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
