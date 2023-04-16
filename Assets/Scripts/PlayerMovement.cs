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

    float groundCheckDistance = 0.02f;
    float wallCheckDistance = 0.2f;

    bool isOnGround;
    bool isOnWall;

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

        if (!isOnWall)
        {
            rb.velocity = new Vector2(_inputVector.x * movementSpeed, rb.velocity.y);
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
        if (ctx.performed && isOnGround)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
}
