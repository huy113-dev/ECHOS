using UnityEngine;

public class DoorLeverSwitch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator[] Door;
    public Animator Lever;
    public Player player;

    private bool isSwitched = false;
    private bool canSwitch = false;

    private void Update()
    {
        foreach(Animator anim in Door)
        {
            ClampAnimation(anim, "DoorSpeed");
        }
        if(canSwitch && player.startInteract())
        {
            isSwitched = !isSwitched;
            if(isSwitched)
            {
                Lever.SetBool("On", true);
                Lever.SetBool("Off", false);
                foreach (Animator anim in Door)
                {   
                    anim.SetFloat("DoorSpeed", 1f);
                }
            }
            else if (!isSwitched)
            {
                Lever.SetBool("Off", true);
                Lever.SetBool("On", false);
                foreach (Animator anim in Door)
                {
                    anim.SetFloat("DoorSpeed", -1f);
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        canSwitch = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canSwitch = false;
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
}
