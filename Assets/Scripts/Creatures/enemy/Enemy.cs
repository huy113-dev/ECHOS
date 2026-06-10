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
    public float RTimer1;
    public float RTimer2;
    private Character target;
    private float lastTurnTime = 0f;
    public Character Target => target;
    public bool isRight = true;
    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
        if (isDead)
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
    public override void OnDespawn()
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
            if (currentState is InvestigateState || Target != null)
            {
                target = null;
                StopMoving();

                EnemyRotation(!isRight);
                lastTurnTime = Time.time;


                ChangeState(new IdleState());

                return;
            }

            if (Time.time - lastTurnTime > 0.2f)
            {
                EnemyRotation(!isRight);
                lastTurnTime = Time.time;
            }
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
        rb.linearVelocity = new Vector2(transform.right.x * walkSpeed, rb.linearVelocity.y);

    }

    public void Chasing()
    {
        ChangeAnim("run");
        rb.linearVelocity = new Vector2(transform.right.x * runSpeed, rb.linearVelocity.y);
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
