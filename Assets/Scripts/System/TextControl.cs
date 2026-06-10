using UnityEngine;

public class TextControl : MonoBehaviour
{
    public GameObject Text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(EnableText), 5f);
    }

    private void EnableText()
    {
        Text.SetActive(true);
    }

}
