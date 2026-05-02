using UnityEditorInternal.VR;
using UnityEngine;

public class Player : MonoBehaviour
{
    //general
    private Rigidbody2D rb;
    private bool isFacingRight;

    //move
    [SerializeField] private float baseSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AnimationCurve jumpingGravityCurve;

    private float horizontal = 0f;
    public bool isGrounded;
    public bool isJumping;
    public float jumpTimer;

    //throwing
    [SerializeField] private ThrowingProjectile predictor;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform throwPoint;
    private bool isthrowing = false;
    public float currentForce = 0f;
    private bool isCharging = false;
    public float aimAngle;
    public Vector2 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded();
        isFacingRight = transform.right.x > 0;
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.F))
        {
            isthrowing = !isthrowing;
        }
        if (isthrowing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCharging = true;
                currentForce = 5f;

            }
            if (isCharging)
            {
                currentForce += 5f * Time.deltaTime;
                currentForce = Mathf.Clamp(currentForce, 5f, 10f);

                updateAngle();

                velocity = CalculateForce(currentForce);

                predictor.TrajectorySimulation(throwPoint.position, velocity);

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Shoot(velocity);
                    isthrowing = false;
                    isCharging = false;
                }

            }
            else
            {
                updateAngle();
            }

            return;
        }
        if (isJumping && !isGrounded)
        {
            OnJumping();
        }
        if (isGrounded)
        {
            isJumping = false;
            rb.gravityScale = 5f;
        }
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space) && !isJumping)
            {
                StartJumping();
            }
            if (Mathf.Abs(horizontal) < 0.1f)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                rb.linearVelocity = new Vector2(horizontal * baseSpeed, rb.linearVelocityY);
            }
        }

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.linearVelocity = new Vector2(horizontal * baseSpeed, rb.linearVelocityY);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }


    }
    private bool CheckGrounded()
    {
        Vector2 rayStartPosL = transform.position + new Vector3(0.3f, 1f, 0);
        Vector2 rayStartPosR = transform.position + new Vector3(-0.3f, 1f, 0);
        Debug.DrawLine(rayStartPosL, rayStartPosL + Vector2.down * 1.6f, Color.red);
        Debug.DrawLine(rayStartPosR, rayStartPosR + Vector2.down * 1.6f, Color.red);
        RaycastHit2D hitL = Physics2D.Raycast(rayStartPosL, Vector2.down, 1.6f, groundLayer);
        RaycastHit2D hitR = Physics2D.Raycast(rayStartPosR, Vector2.down, 1.6f, groundLayer);
        return hitL.collider != null || hitR.collider != null;
    }

    private void StartJumping()
    {
        isJumping = true;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpTimer = 0f;
    }
    
    private void OnJumping()
    {
        jumpTimer += Time.fixedDeltaTime;
        if(jumpTimer < 1f)
        {
            rb.gravityScale = jumpingGravityCurve.Evaluate(jumpTimer);
        }

    }

    private Vector2 CalculateForce(float force)
    {
        float radian = aimAngle * Mathf.Deg2Rad;
        float xDir = Mathf.Cos(radian) * (isFacingRight ? 1f : 1f);
        float yDir = Mathf.Sin(radian);
        return new Vector2(xDir, yDir).normalized * force;
    }

    private void Shoot(Vector2 velocity)
    {
        isCharging = false;
        predictor.hideTrajectory();
        GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = stone.GetComponent<Rigidbody2D>();
        rb.linearVelocity = velocity;
    }
    private void updateAngle()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            aimAngle += 10f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            aimAngle -= 10f * Time.deltaTime;
        }
        aimAngle = Mathf.Clamp(aimAngle, -20f, 90f);
    }
}
