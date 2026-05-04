using UnityEngine;

public class BrokenBridge : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Animator anim;
    public void Broking()
    {
        anim.SetBool("Broken", true);
    }
}
