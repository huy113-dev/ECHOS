using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartArea : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
        Debug.Log("Restart Clicked");
    }

}