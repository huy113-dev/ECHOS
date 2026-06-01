using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator Door;
    public Animator PressurePlate;
    public float time = 0.5f;
    public float timer = 0f;
    public float delayedTime = 3.5f;
    private bool isPressed;

    private void Update()
    {
        if (isPressed)
        {
            timer += Time.fixedDeltaTime;
            if(timer >= time)
            {
                    Door.SetFloat("DoorSpeed", 1f);
                    PressurePlate.SetFloat("PPSpeed", 1f);
            }
        }
        ClampAnimation(Door, "DoorSpeed");
        ClampAnimation(PressurePlate, "PPSpeed");
    }

    private void ClampAnimation(Animator anim, string speedParam)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float currentSpeed = anim.GetFloat(speedParam);

        if (currentSpeed < 0 && stateInfo.normalizedTime <= 0f)
        {
            anim.SetFloat(speedParam, 0f);
            anim.Play(stateInfo.shortNameHash, 0, 0f);
        }
        else if (currentSpeed > 0 && stateInfo.normalizedTime >= 1f)
        {
            anim.SetFloat(speedParam, 0f);
            anim.Play(stateInfo.shortNameHash, 0, 1f);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Enemy")
        {
            isPressed = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Enemy")
        {
            isPressed = false;
            
            if(timer > time + 4f)
            {
                timer = 0f;
                Invoke(nameof(ResetTrigger), delayedTime);
            }
            else
            {
                timer = 0f;
                Door.SetFloat("DoorSpeed", -1f);
                PressurePlate.SetFloat("PPSpeed", -1f);
            }
            
            
        }
    }
    private void ResetTrigger()
    {
        Door.SetFloat("DoorSpeed", -1f);
        PressurePlate.SetFloat("PPSpeed", -1f);
    }
}
