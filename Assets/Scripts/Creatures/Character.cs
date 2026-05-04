using System.Xml.Serialization;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected float hp;
    protected string CurrAnimName;
    protected bool invicible;
    public bool IsDead => hp <= 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        CurrAnimName = "Idle";
    }
    protected virtual void OnDespawn()
    {
        ChangeAnim("Die");
        Invoke(nameof(Dead), 2f);

    }

    public virtual void OnHit(float damage)
    {
        if (invicible)
        {
            return;
        }

        else
        {
            if (!IsDead)
            {
                hp -= damage;
                if (IsDead)
                {
                    OnDespawn();
                }
            }
        }
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