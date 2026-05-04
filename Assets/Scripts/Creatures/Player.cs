using System.Collections;
using Unity.VisualScripting;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    //general
    private Rigidbody2D rb;
    private bool isFacingRight;
    private bool isDead = false;
    private Vector3 savePoint;
    private bool isFreeze = false;

    [Header("Simple Move")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AnimationCurve jumpingGravityCurve;
    public float Speed;
    private float horizontal = 0f;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimer;
    public bool isCrouching;
    private bool HeadBlocked;


    [Header("Running")]
    [SerializeField] private AnimationCurve RunningSpeedCurve;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StaminaCost;
    [SerializeField] private float rechargeRate;
    public Image StaminaBar;
    public float stamina;
    private float speedTimer;
    private bool isRunning;
    private Coroutine recharge;


    [Header("Throwing")]
    [SerializeField] private ThrowingProjectile predictor;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce;
    private bool isthrowing = false;
    private float aimAngle;
    private Vector2 velocity;

    [Header("Ledge climb")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbStartPos;
    private Vector2 climbOverPos;

    private bool canGrabLedge = true;
    private bool canClimb;
    public bool ledgeDetected;
    void Start()
    {
        SavePoint();
        rb = GetComponent<Rigidbody2D>();
        CurrAnimName = "Idle";
    }
    void Update()
    {
        if (isFreeze)
        {
            return;
        }
        if (isDead)
        {
            return;
        }
        isGrounded = CheckGrounded();
        HeadBlocked = BlockingHead();
        isFacingRight = transform.right.x > 0;
        horizontal = Input.GetAxisRaw("Horizontal");
        CheckForLedge();

        //moving
        if (Mathf.Abs(horizontal) > 0.1f && !isRunning && !isJumping)
        {
            ChangeAnim("Walk");
            rb.linearVelocity = new Vector2(horizontal * baseSpeed, rb.linearVelocityY);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }

        //throwing
        if (Input.GetKeyDown(KeyCode.F))
        {
            isthrowing = !isthrowing;
            if (!isthrowing)
            {
                predictor.hideTrajectory();
            }
        }
        if (isthrowing)
        {

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
                isFacingRight = transform.right.x > 0;
            }

            updateAngle();
            velocity = CalculateForce(throwForce);

            predictor.TrajectorySimulation(throwPoint.position, velocity);

            if (Input.GetKey(KeyCode.J))
            {
                Shoot(velocity);
                isthrowing = false;
            }
            return;
        }
        if (isJumping && !isGrounded)
        {
            OnJumping();
        }
        if (isGrounded)
        {
            //jumping
            isJumping = false;
            rb.gravityScale = 5f;

            if (Input.GetKey(KeyCode.Space) && !isJumping && !HeadBlocked)
            {
                StartJumping();
            }
            if (Mathf.Abs(horizontal) < 0.1f)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            
            //running
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                speedTimer += Time.deltaTime;
                if(stamina > 0f && !isJumping)
                {
                    ChangeAnim("Run");
                }

                Speed = RunningSpeedCurve.Evaluate(speedTimer);
                if (speedTimer > 1f)
                {
                    Speed = RunningSpeedCurve.Evaluate(1);
                }
                stamina -= StaminaCost;

                StaminaBar.fillAmount = stamina / MaxStamina;

                if (stamina < 0f)
                {
                    stamina = 0f;
                    Speed = baseSpeed;
                    isRunning = false;
                }

                rb.linearVelocity = new Vector2(horizontal * Speed, rb.linearVelocityY);
                if (Mathf.Abs(horizontal) > 0.1f)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
                    isFacingRight = transform.right.x > 0;
                }

                if (recharge != null)
                {
                    StopCoroutine(recharge);
                }
                recharge = StartCoroutine(StaminaRecharge());
            }
            if (Mathf.Abs(horizontal) < 0.1f && !isJumping && !isCrouching)
            {
                ChangeAnim("Idle");
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }

            //crouch
            if(Input.GetKey(KeyCode.LeftControl) && !isRunning && !isJumping || HeadBlocked)
            {
                isCrouching = true;
                ChangeAnim("Crouch");
                float CrouchSpeed = baseSpeed / 2;
                if (Mathf.Abs(horizontal) > 0.1f)
                {
                    ChangeAnim("CrouchMove");
                    rb.linearVelocity = new Vector2(horizontal * CrouchSpeed, rb.linearVelocityY);
                    transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
                }

            }
            if (Input.GetKeyUp(KeyCode.LeftControl) || !isGrounded || isJumping)
            {
                isCrouching = false;
            }

        }

        if (!isGrounded && rb.linearVelocity.y < 0.1f && !canClimb)
        {
            isJumping = false;
            ChangeAnim("Fall");
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            speedTimer = 0f;
            Speed = baseSpeed;
        }




    }
    protected override void OnInit()
    {
       //hp = 100f;
        base.OnInit();
        //isAttacking = false;
        isDead = false;
        transform.position = savePoint;
        //ChangeAnim("idle");
    }

    public void Freeze()
    {
        isFreeze = true;
    }
    private void SavePoint()
    {
        savePoint = transform.position;
    }
    private bool CheckGrounded()
    {
        if (isFacingRight)
        {
            Vector2 rayStartPosR = transform.position + new Vector3(0.6f, 0, 0);
            Vector2 rayStartPosL = transform.position + new Vector3(0, 0, 0);
            Debug.DrawLine(rayStartPosL, rayStartPosL + Vector2.down * 1.23f, Color.red);
            Debug.DrawLine(rayStartPosR, rayStartPosR + Vector2.down * 1.23f, Color.red);
            RaycastHit2D hitL = Physics2D.Raycast(rayStartPosL, Vector2.down, 1.23f, groundLayer);
            RaycastHit2D hitR = Physics2D.Raycast(rayStartPosR, Vector2.down, 1.23f, groundLayer);
            return hitL.collider != null || hitR.collider != null;
        }
        else
        {
            Vector2 rayStartPosR = transform.position + new Vector3(0, 0, 0);
            Vector2 rayStartPosL = transform.position + new Vector3(-0.6f, 0, 0);
            Debug.DrawLine(rayStartPosL, rayStartPosL + Vector2.down * 1.23f, Color.red);
            Debug.DrawLine(rayStartPosR, rayStartPosR + Vector2.down * 1.23f, Color.red);
            RaycastHit2D hitL = Physics2D.Raycast(rayStartPosL, Vector2.down, 1.23f, groundLayer);
            RaycastHit2D hitR = Physics2D.Raycast(rayStartPosR, Vector2.down, 1.23f, groundLayer);
            return hitL.collider != null || hitR.collider != null;
        }

    }

    private void StartJumping()
    {
        isJumping = true;
        ChangeAnim("JumpIn");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpTimer = 0f;
    }

    private void OnJumping()
    {
        jumpTimer += Time.fixedDeltaTime;
        if (jumpTimer < 1f)
        {
            rb.gravityScale = jumpingGravityCurve.Evaluate(jumpTimer);
        }

    }

    private void CheckForLedge()
    {
        if(ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            Vector2 ledgePosition = GetComponentInChildren<LedgeCheck>().transform.position;
            climbStartPos = ledgePosition + offset1;
            climbOverPos = ledgePosition + offset2;
            canClimb = true;
        }
        if (canClimb)
        {
            ChangeAnim("ClimbLedge");
            transform.position = climbStartPos;
        }
    }

    private void LedgeClimbOver()
    {
        canClimb = false;
        transform.position = climbOverPos;
        canGrabLedge = true;
    }

    private bool BlockingHead()
    {
        Vector2 rayStartPos = transform.position + new Vector3(0.15f, 0, 0);
        Debug.DrawLine(rayStartPos, rayStartPos + Vector2.up * 1f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(rayStartPos, Vector2.up, 1f, groundLayer);
        return hit.collider != null;
    }

    private Vector2 CalculateForce(float force)
    {
        float radian = aimAngle * Mathf.Deg2Rad;
        float xDir = Mathf.Cos(radian) * (isFacingRight ? 1f : -1f);
        float yDir = Mathf.Sin(radian);
        return new Vector2(xDir, yDir).normalized * force;
    }

    private void Shoot(Vector2 velocity)
    {
        predictor.hideTrajectory();
        GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = stone.GetComponent<Rigidbody2D>();
        rb.linearVelocity = velocity;
    }
    private void updateAngle()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            aimAngle += 20f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            aimAngle -= 20f * Time.deltaTime;
        }
        aimAngle = Mathf.Clamp(aimAngle, -20f, 90f);
    }

    private IEnumerator StaminaRecharge()
    {
        yield return new WaitForSeconds(1f);
        while (stamina < MaxStamina)
        {
            stamina += rechargeRate;
            if (stamina > MaxStamina)
            {
                stamina = MaxStamina;
            }
            StaminaBar.fillAmount = stamina / MaxStamina;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool switchLevel()
    {
        if (Input.GetKey(KeyCode.E))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
        {
            //ChangeAnim("die");
            isDead = true;
            
            Invoke(nameof(OnInit), 1f);
        }
    }
}