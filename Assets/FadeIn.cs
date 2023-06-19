using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("FadeIn");
    }


}
