using UnityEngine;

public class LeverBase : MonoBehaviour
{
    public Player player;

    private bool canrepair = false;
    public bool haveKey = false;
    public GameObject Lever;

    private void Update()
    {
        if (canrepair)
        {
            if (player.startInteract())
            {
                Lever.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && haveKey)
        {
            canrepair = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canrepair = false;
        }
    }
}
