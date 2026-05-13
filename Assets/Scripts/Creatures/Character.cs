using UnityEngine;

public class Character : MonoBehaviour
{
    // Thêm biến để điều khiển component Animator
    protected Animator anim;
    protected string CurrAnimName;

    // Unity sẽ tự động chạy Awake() khi game bắt đầu
    protected virtual void Awake()
    {
        // Tự động tìm và lấy bộ điều khiển hoạt ảnh (Animator) trên nhân vật
        anim = GetComponent<Animator>();
    }

    protected virtual void OnInit()
    {
        // Logic khởi tạo chung cho mọi nhân vật
    }

    // Hàm đổi animation đã được nâng cấp
    protected void ChangeAnim(string animName)
    {
        // Nếu animation muốn đổi khác với animation hiện tại thì mới chạy (tránh bị kẹt frame)
        if (CurrAnimName != animName)
        {
            CurrAnimName = animName;

            // Dòng lệnh CỐT LÕI giúp animation của bạn xuất hiện trở lại!
            if (anim != null)
            {
                anim.Play(CurrAnimName);
            }
        }
    }
}