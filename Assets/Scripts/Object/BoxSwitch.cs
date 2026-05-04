using UnityEngine;

public class BoxSwitch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Box box;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Stone")
        {
            gameObject.SetActive(false);
            box.BoxFall();
        }
    }
}
