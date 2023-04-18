using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    Animator animator;

    Vector2 _inputVector;

    [SerializeField] float movementSpeed = 12;
    [SerializeField] float jumpForce = 100;
    [SerializeField] float jumpCutModifier = 0.05f;
    [SerializeField] float coyoteTime = 0.5f;

    float groundCheckDistance = 0.05f;
    float wallCheckDistance = 0.2f;

    float coyoteTimeCounter;

    bool isOnGround;
    bool isOnWall;

    bool isJumpPressed;

    RaycastHit2D[] groundHit = new RaycastHit2D[5];
    RaycastHit2D[] wallHit = new RaycastHit2D[5];

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        isOnGround = col.Cast(Vector2.down, groundHit, groundCheckDistance) > 0;
        isOnWall = col.Cast(transform.localScale.x > 0 ? Vector2.right : Vector2.left, wallHit, wallCheckDistance) > 0;
        animator.SetBool("isRunning", rb.velocity.x != 0);

        if (isOnGround && !isJumpPressed)
        {
            coyoteTimeCounter = coyoteTime;
        }

        if (!isJumpPressed && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutModifier);
        }

        if (!isOnWall)
        {
            rb.velocity = new Vector2(_inputVector.x * movementSpeed, rb.velocity.y);
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _inputVector = ctx.ReadValue<Vector2>();
        if (_inputVector != Vector2.zero)
        {
            transform.localScale = new Vector2(_inputVector.x > 0 ? 1 : -1, 1);
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        isJumpPressed = ctx.ReadValueAsButton();
        if (isJumpPressed && coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0;
        }
    }
}
