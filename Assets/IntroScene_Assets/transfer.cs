using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện bắt buộc để chuyển Map

public class transfer : MonoBehaviour
{
    [Header("Tên Map muốn chuyển tới")]
    public string nextSceneName = "Map_Chapter1 tut";

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu người chạm vào hang là Player
        if (other.CompareTag("Player"))
        {
            // Bùm! Chuyển thẳng sang Map mới ngay lập tức
            SceneManager.LoadScene(nextSceneName);
        }
    }
}