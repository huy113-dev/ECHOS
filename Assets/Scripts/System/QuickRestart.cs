using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện bắt buộc để chuyển cảnh

public class QuickRestart : MonoBehaviour
{
    // Hàm này sẽ chạy khi bạn click chuột vào nút Restart
    public void ClickToRestart()
    {
        Time.timeScale = 1f; // Trả thời gian game về bình thường (đề phòng lúc chết game bị đóng băng)

        // Tải lại chính Scene hiện tại từ đầu
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}