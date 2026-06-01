using UnityEngine;

public class Enemy : Character
{
    [Header("References")]
    public Animator animator;
    public Transform player;

    [Header("Stats")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float patrolDistance = 4f;
    public float sightRange = 6f;
    public float attackRange = 1.5f;

    // Các biến ẩn khỏi Inspector nhưng cần thiết cho các State khác truy cập
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Vector3 targetPosition;
    [HideInInspector] public bool movingRight = true;
    [HideInInspector] public bool facingRight = true;

    // Lưu trữ trạng thái hiện tại
    private IState currentState;

    void Start()
    {
        //startPosition = transform.position;
        //targetPosition = startPosition + new Vector3(patrolDistance, 0, 0);

        // Bắt đầu bằng trạng thái đứng im
        ChangeState(new IdleState());
    }

    void Update()
    {
        // Liên tục thực thi logic của trạng thái hiện tại
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    // Hàm dùng để đổi trạng thái (Ví dụ: đang Idle chuyển sang Patrol)
    public void ChangeState(IState newState)
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

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Vẽ vòng tròn tầm nhìn
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}