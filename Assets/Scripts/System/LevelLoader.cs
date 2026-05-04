using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;


    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(levelIndex);
    }
}
