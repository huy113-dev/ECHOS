using UnityEngine;

public class MysteriousFire : MonoBehaviour
{
    public Animator Fire;
    public int map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && map == 1)
        {
            Fire.SetTrigger("MapTut");
        }
        else if(collision.CompareTag("Player") && map == 0)
        {
            Fire.SetTrigger("MapIntro");
        }
        else if(collision.CompareTag("Player") && map == 2)
        {
            Fire.SetTrigger("MapChap1");
        }
    }
}
