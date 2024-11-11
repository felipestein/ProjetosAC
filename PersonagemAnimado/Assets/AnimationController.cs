using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("PlayAnim1");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("PlayAnim2");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("PlayAnim3");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}