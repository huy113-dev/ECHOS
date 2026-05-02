using System.Collections;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //general
    private Rigidbody2D rb;
    private bool isFacingRight;

    [Header ("Simple Move")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AnimationCurve jumpingGravityCurve;
    public float Speed;
    private float horizontal = 0f;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimer;


    [Header ("Running")]
    [SerializeField] private AnimationCurve RunningSpeedCurve;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StaminaCost;
    [SerializeField] private float rechargeRate;
    public Image StaminaBar;
    public float stamina;
    private float speedTimer;
    private bool isRunning;
    private Coroutine recharge;


    [Header ("Throwing")]
    [SerializeField] private ThrowingProjectile predictor;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform throwPoint;
    private bool isthrowing = false;
    private float currentForce = 0f;
    private bool isCharging = false;
    private float aimAngle;
    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = CheckGrounded();
        isFacingRight = transform.right.x > 0;
        horizontal = Input.GetAxisRaw("Horizontal");

        //throwing
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
            //jumping
            isJumping = false;
            rb.gravityScale = 5f;

            if (Input.GetKey(KeyCode.Space) && !isJumping)
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

                Speed = RunningSpeedCurve.Evaluate(speedTimer);
                if(speedTimer > 1f)
                {
                    Speed = RunningSpeedCurve.Evaluate(1);
                }
                stamina -= StaminaCost;

                StaminaBar.fillAmount = stamina / MaxStamina;

                if(stamina < 0f)
                {
                    stamina = 0f;
                }

                rb.linearVelocity = new Vector2(horizontal * Speed, rb.linearVelocityY);

                if(recharge != null)
                {
                    StopCoroutine(recharge);
                }
                recharge = StartCoroutine(StaminaRecharge());
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;
                speedTimer = 0f;
                Speed = baseSpeed;
            }
        }

        //moving
        if (Mathf.Abs(horizontal) > 0.1f && !isRunning)
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

    private IEnumerator StaminaRecharge()
    {
        yield return new WaitForSeconds(1f);
        while(stamina < MaxStamina)
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
}
