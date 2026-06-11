using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGameOver()
    {
        Debug.Log("Show Game Over");

        gameOverPanel.SetActive(true);
    }

    public void RestartScene()
    {
        Debug.Log("===> CHO TOI CO CHAY VAO HAM RESTART ROI NHE! <===");
        // Trả thời gian về bình thường
        Time.timeScale = 1f;

        // Tải lại Scene hiện tại luôn, không làm thêm gì khác ở dưới nữa
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
