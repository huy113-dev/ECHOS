using System.Runtime.InteropServices;
using UnityEngine;

public class HidingBarrel : MonoBehaviour
{
    public Player player;
    private bool canHide = false;
    private float timer = 0f;
    void Update()
    {
        if (player.isHiding)
        {
            timer += Time.fixedDeltaTime;
        }
        
        if (canHide)
        {
            if (player.StartHiding() && !player.isHiding)
            {
                player.MovePLayerTo(transform.position);
                player.isHiding = true;
                player.sprite.enabled = false;
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                rb.linearVelocity = Vector2.zero;
                Physics2D.IgnoreLayerCollision(7, 11, true);
            }

            else if (player.isHiding && player.StartHiding() && timer > 0.3f)
            {
                player.isHiding = false;
                player.sprite.enabled = true;
                Physics2D.IgnoreLayerCollision(7, 11, false);

            }
        }

        SetHidingState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canHide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canHide = false;
            timer = 0f;
        }
    }
    private void SetHidingState()
    {
        if (player.isHiding && player.isthrowing)
        {
            player.sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            player.sprite.enabled = true;
            player.sprite.sortingOrder = -2;
        }
        else if(!player.isHiding)
        {
            player.sprite.maskInteraction = SpriteMaskInteraction.None;
            player.sprite.sortingOrder = 0;
        }
        else
        {
            player.sprite.enabled = false;
            player.sprite.maskInteraction = SpriteMaskInteraction.None;
        }
    }
}
