using System.Collections;
using UnityEngine;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    private Animator anim;

    [Header("Cài đặt thời gian Cutscene")]
    public float freezeDuration = 5f;
    public float getUpAnimDuration = 1.5f;

    [Header("Cài đặt Chữ chạy")]
    public TMP_Text playerText;
    public string wakeUpMessage = "Ưm... đau đầu quá... Mình đang ở đâu đây?";

    // THÊM: Câu nói thứ 2 và thời gian chờ
    public string secondMessage = "Cái gì vậy?";
    public float delayBetweenMessages = 2.5f; // Thời gian chờ giữa 2 câu
    public float typingSpeed = 0.05f;
    public float readTime = 2f; // Thời gian chữ dừng lại trên đầu để người chơi kịp đọc

    void Start()
    {
        anim = GetComponent<Animator>();

        if (playerText != null)
        {
            playerText.text = "";
        }

        Time.timeScale = 0f;

        if (GetComponent<Player>() != null)
        {
            GetComponent<Player>().enabled = false;
        }

        anim.Play("Intro_LieDown");
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("Walk");
        anim.ResetTrigger("Run");

        StartCoroutine(IntroCutsceneProcess());
    }

    IEnumerator IntroCutsceneProcess()
    {
        yield return new WaitForSecondsRealtime(freezeDuration);
        Time.timeScale = 1f;

        anim.Play("Intro_LieDown");
        yield return new WaitForSeconds(getUpAnimDuration);

        anim.Play("Idle 1");

        // GỌI HIỆU ỨNG CHẠY CHỮ
        if (playerText != null)
        {
            StartCoroutine(TypeMultipleLines());
        }

        if (GetComponent<Player>() != null)
        {
            GetComponent<Player>().enabled = true;
        }
    }

    // HIỆU ỨNG GÕ NHIỀU CÂU THOẠI
    IEnumerator TypeMultipleLines()
    {
        // === CÂU THỨ 1 ===
        playerText.text = "";
        foreach (char c in wakeUpMessage.ToCharArray())
        {
            playerText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Dừng lại 2 giây cho người chơi đọc câu 1
        yield return new WaitForSeconds(readTime);

        // Xóa câu 1 đi (Mất chữ đầu tiên)
        playerText.text = "";

        // === NGHỈ GIỮA HIỆP ===
        // Chờ đúng 2.5 giây theo ý bạn trước khi hiện câu tiếp theo
        yield return new WaitForSeconds(delayBetweenMessages);

        // === CÂU THỨ 2 ===
        foreach (char c in secondMessage.ToCharArray())
        {
            playerText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Dừng lại 2 giây cho người chơi đọc câu 2
        yield return new WaitForSeconds(readTime);

        // Xóa sạch chữ hoàn toàn để trả lại màn hình trống
        playerText.text = "";
    }
}