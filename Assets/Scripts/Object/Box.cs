using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void BoxFall()
    {
        anim.SetBool("Level", true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
