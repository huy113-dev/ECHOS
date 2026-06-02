using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Enemy enemy;
    public Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !player.isHiding || collision.tag == "Player" && player.isthrowing)
        {
            Debug.Log("i see you bitch");
            enemy.SetTarget(collision.GetComponent<Character>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemy.SetTarget(null);
        }
    }
}
