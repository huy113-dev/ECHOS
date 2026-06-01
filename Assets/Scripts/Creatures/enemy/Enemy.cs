using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float attackRange;
    public IState currentState;
    public Vector3 investigateTarget;

    private Character target;
    public Character Target => target;
    public bool isRight = true;
    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
        if (IsDead)
        {
            return;
        }
    }
    private void changeAnimToIdle()
    {
        ChangeAnim("idle");
    }
    protected override void OnInit()
    {
        hp = 200f;
        base.OnInit();
        //CurrAnimName = "idle";
        ChangeState(new IdleState());
    }
    protected override void OnDespawn()
    {
        ChangeState(null);
        base.OnDespawn();
    }

    public virtual void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

        }
        else if (collision.tag == "EnemyWall")
        {
            EnemyRotation(!isRight);
        }
    }


    public void SetTarget(Character character)
    {
        this.target = character;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }
    public bool IsTargetInRange()
    {
        if (Target != null && Vector2.Distance(Target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Moving()
    {
        ChangeAnim("walk");
        rb.linearVelocity = transform.right * walkSpeed;

    }

    public void Chasing()
    {
        ChangeAnim("run");
        rb.linearVelocity = transform.right * runSpeed;
    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.linearVelocity = Vector2.zero;
    }
    public void EAttack()
    {
        ChangeAnim("attack");
    }

    public void EnemyRotation(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector2.up * 180);

    }

    public void Alert(Vector3 noisePosition)
    {
        if(currentState is AttackState || Target != null)
        {
            return;
        }
        investigateTarget = noisePosition;

        ChangeState(new InvestigateState());
    }

    public void MoveToPoint(Vector3 targetPos)
    {
        ChangeAnim("walk");
        EnemyRotation(targetPos.x > transform.position.x);
        rb.linearVelocity = new Vector2(transform.right.x * walkSpeed, rb.linearVelocity.y);
    }
}
