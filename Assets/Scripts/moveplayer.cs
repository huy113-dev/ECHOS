using UnityEngine;

public class moveplayer : MonoBehaviour
{
    // Biến public để bạn có thể chỉnh sửa tốc độ trực tiếp trong Unity Editor
    public float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Lấy giá trị đầu vào (sẽ trả về từ -1 đến 1)
        // -1: Trái (Phím A hoặc Mũi tên trái)
        //  1: Phải (Phím D hoặc Mũi tên phải)
        //  0: Không bấm gì
        float moveInput = Input.GetAxis("Horizontal");

        // Di chuyển nhân vật
        // Vector3.right tương đương với (1, 0, 0)
        // Nhân với Time.deltaTime để tốc độ di chuyển đều đặn trên mọi cấu hình máy tính
        transform.Translate(Vector3.right * moveInput * speed * Time.deltaTime);
    }
}
