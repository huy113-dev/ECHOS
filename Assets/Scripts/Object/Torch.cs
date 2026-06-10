using UnityEngine;

public class Torch : MonoBehaviour
{
    public Animator Burn;
    private Animator thisAnim;
    private Collider2D thisCollider;
    void Start()
    {
        thisAnim = GetComponent<Animator>(); 
        thisCollider = GetComponent<Collider2D>();
    }



    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            thisAnim.SetTrigger("Fall");

            Invoke(nameof(StartBurn), 0.3f);

            Invoke(nameof(PicBurn), 1.5f);
        }
    }

    private void StartBurn()
    {
        Burn.SetTrigger("Start");
    }
    private void PicBurn()
    {
        Burn.SetTrigger("Burn");
        enabled = false;
    }
    public void EnableCollider()
    {
        thisCollider.enabled = true;
    }
}
