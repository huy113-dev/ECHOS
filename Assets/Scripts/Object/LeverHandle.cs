using UnityEngine;

public class LeverHandle : MonoBehaviour
{
    private bool canPickup = false;
    public Player player;
    private SpriteRenderer sprite;
    public Torch torch;
    public LeverBase Base;
    private void Update()
    {
        if (canPickup)
        {
            if (player.startInteract())
            {
                
                torch.EnableCollider();
                Base.haveKey = true;
                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = false;
        }
    }
}
