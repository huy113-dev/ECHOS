using UnityEngine;

public class BoxLevelSwitch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Box box;
    [SerializeField] private Player player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("hit player");
            if (player.switchLevel())
            {
                box.BoxFall();
            }
        }
    }
}
