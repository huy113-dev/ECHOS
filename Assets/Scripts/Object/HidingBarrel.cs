using System.Runtime.InteropServices;
using UnityEngine;

public class HidingBarrel : MonoBehaviour
{
    public Player player;
    private bool canHide = false;
    void Update()
    {
        if (canHide)
        {
            if (player.StartHiding())
            {
                player.MovePLayerTo(transform.position);
                player.isHiding = true;
            }
        }
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
        }
    }
}
