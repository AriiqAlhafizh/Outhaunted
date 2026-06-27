using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StartAnimation()
    {
        animator.SetTrigger("Flip");
    }
}