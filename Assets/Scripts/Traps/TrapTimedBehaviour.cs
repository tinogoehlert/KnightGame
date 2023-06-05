using System.Collections;
using UnityEngine;

public class TrapTimedBehaviour : MonoBehaviour
{
    const int PLAYER_LAYER = 6;
    Animator animator;
    [SerializeField] float delay_seconds;
    [SerializeField] float intial_delay_seconds;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        StartCoroutine(DelayedAttack(intial_delay_seconds));
    }

    public void Attack()
    {
        if (delay_seconds <= 0)
        {
            animator.SetTrigger("attack");
            return;
        }
        StartCoroutine(DelayedAttack(delay_seconds));
    }

    IEnumerator DelayedAttack(float ds)
    {
        yield return new WaitForSeconds(ds);
        animator.SetTrigger("attack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == PLAYER_LAYER)
        {
            var player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Killme();
            }
        }
    }

}
