using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameArea : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("intro");
    }
}