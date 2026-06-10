using Unity.Cinemachine;
using UnityEngine;

public class Gateway : MonoBehaviour
{
    private Collider2D doorCollider;
    private bool canEnter = false;
    public Player player;
    public Animator faded;
    public Transform targetPosition;

    private void Start()
    {
        doorCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (canEnter)
        {
            if (player.startInteract())
            {
                Invoke(nameof(movePlayer), 1.5f);
                faded.SetTrigger("Start");
                Invoke(nameof(End), 2f);
            }
        }
    }

    public void EnableDoorCollider()
    {
        doorCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canEnter = false;
        }
    }
    private void End()
    {
        faded.SetTrigger("End");
    }

    private void movePlayer()
    {
        player.MovePLayerTo(targetPosition.position);
    }
}
