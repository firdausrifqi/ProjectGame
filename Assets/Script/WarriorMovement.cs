using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{
    [Header("SLIDING STATE")]
    [SerializeField] bool isTouchingFront;
    [SerializeField] Transform frontCheck;
    public float frontRadius;
    bool wallSliding;
    public float wallSlidingSpeed;
    bool walljumping;
    public float xwallforce;
    public float ywallforce;
    public float walljumptime;
    public bool jumping = false;

    [Header("MOVEMENTS")]
    public float speed;
    public float jumpForce;
    private float input;
    private bool facingRight = true;
    private int extraJump;
    public int extraJumpValue;
    public bool canRecharge = false;

    [Header("GROUND HEAD")]
    [SerializeField] Transform HeadCheck;
    public LayerMask HGround;

    [Header("OTHER")]
    [SerializeField] bool IsGrounded;
    public Transform GroundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    private Rigidbody2D rb;
    public Animator animator;
    private bool CombatIdle = false;
    private IEnumerator Coroutine;
    Collider2D _Col;


    // Start is called before the first frame update
    void Start()
    {
        extraJump = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        input = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(input * speed, rb.velocity.y);
        if (Input.GetButton("HoldX"))
        {
            // CombatIdle = !CombatIdle;
            rb.position = this.rb.position;
            canRecharge = true;
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            canRecharge = false;
            animator.SetInteger("AnimState", 0);
        }
        //BATAS
        if (!facingRight && input > 0)
        {
            flip();
        }
        else if (facingRight && input < 0)
        {
            flip();
        }
        else if ((Mathf.Abs(input) > Mathf.Epsilon) && IsGrounded)
        {
            canRecharge = false;
            animator.SetInteger("AnimState", 1);
        }
        // else if (CombatIdle)
        // {
        //     canRecharge = true;
        //     animator.SetInteger("AnimState", 1);
        // }
        // else if (!CombatIdle)
        // {
        //     canRecharge = false;
        //     animator.SetInteger("AnimState", 0);
        // }
    }

    void Update()
    {
        DoAction();
        rb.velocity = new Vector2(input * speed, rb.velocity.y);
        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, whatIsGround);
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, frontRadius, whatIsWall);
        //Debug.Log(isTouchingFront);
        animator.SetFloat("AirSpeed", rb.velocity.y);
        //Debug.Log("AIRSPEED: " + rb.velocity.y);
        // Check Ground
        if (IsGrounded)
        {
            jumping = false;
            extraJump = extraJumpValue;
        }
        else jumping = true;
        //JUMP STATE
        if ((Input.GetKeyDown("space") || Input.GetKeyDown("up") || Input.GetKeyDown("w")) && IsGrounded)
        {
            IsGrounded = false;
            jumping = true;
            // animator.SetTrigger("Jump");
            // animator.SetBool("Grounded", IsGrounded);
            rb.velocity = Vector2.up * jumpForce;
        }


        // if ((Input.GetKeyDown("space") || Input.GetKeyDown("up")) && extraJump > 0)
        // {
        //     //Debug.Log("FIRST JUMP");
        //     animator.SetTrigger("Jump");
        //     IsGrounded = false;
        //     animator.SetBool("Grounded", IsGrounded);
        //     rb.velocity = Vector2.up * jumpForce;
        //     extraJump--;
        // }
        // else if ((Input.GetKeyDown("space") || Input.GetKeyDown("up")) && extraJump == 0 && IsGrounded)
        // {
        //     //Debug.Log("LAST JUMP");
        //     animator.SetTrigger("Jump");
        //     IsGrounded = false;
        //     animator.SetBool("Grounded", IsGrounded);
        //     rb.velocity = Vector2.up * jumpForce;
        // }
        //SLIDE STATE
        if (isTouchingFront && !IsGrounded && input != 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }


        if ((Input.GetKeyDown("space") || Input.GetKeyDown("up")) && wallSliding == true)
        {
            animator.SetBool("Grounded", IsGrounded);
            animator.SetTrigger("Jump");
            walljumping = true;
            jumping = true;
            Invoke("SetWallJumpingTogalse", walljumptime);
        }
        if (walljumping)
        {
            rb.velocity = new Vector2(xwallforce * -input, ywallforce);
        }
        //HEAD CHECK


    }
    private IEnumerator SetEnabled(float Time)
    {
        yield return new WaitForSeconds(Time);

    }
    void DoAction()
    {
        if (jumping)
        {
            canRecharge = false;
            animator.SetTrigger("Jump");
            animator.SetBool("Jumping", jumping);
        }
        else animator.SetBool("Jumping", jumping);
    }

    void SetWallJumpingTogalse()
    {
        walljumping = false;
    }
    void flip()
    {
        facingRight = !facingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
    public float getXpos
    {
        get
        {
            return transform.position.x;
        }
    }
    public float getYpos
    {
        get
        {
            return transform.position.y;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(HeadCheck.position, checkRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(frontCheck.position, frontRadius);

    }

}
