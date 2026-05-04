using UnityEngine;

public class BrokenBridgeTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private BrokenBridge Bridge;
    [SerializeField] private Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Bridge.Broking();
            player.Freeze();
        } 
    }
}
