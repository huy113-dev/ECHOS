using UnityEngine;

public class Stone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(deSpawn), 3f);
    }

    // Update is called once per frame
    private void deSpawn()
    {
        gameObject.SetActive(false);
    }
}
