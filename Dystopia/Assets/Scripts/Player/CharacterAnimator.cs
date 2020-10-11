using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        animator = gameObject.GetComponent<Animator>();
    }
 
     public void PlayIdleAnimation() {
         animator.Play("Idle");
    }

    public void PlayWalkAnimation() {
        animator.Play("Move");
    }
}
