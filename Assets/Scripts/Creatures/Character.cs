using System.Xml.Serialization;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected float hp;
    protected string CurrAnimName;
    protected bool invicible;
    public bool isDead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        CurrAnimName = "Idle";
    }
    public virtual void OnDespawn()
    {
        ChangeAnim("Die");
        isDead = true;
        Invoke(nameof(OnInit), 2f);

    }

    protected void ChangeAnim(string AnimName)
    {
        Debug.Log(AnimName);
        if (CurrAnimName != AnimName)
        {
            anim.ResetTrigger(CurrAnimName);
            CurrAnimName = AnimName;
            anim.SetTrigger(CurrAnimName);
        }
    }
    public void Dead()
    {
        Destroy(gameObject);
    }

}