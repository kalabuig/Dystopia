using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        animator = gameObject.GetComponent<Animator>();
    }
 
     public void PlayIdleAnimation(Vector3 p) {
         animator.Play("Idle");
    }

    public void PlayWalkAnimation(Vector3 p) {
        animator.Play("Move");
    }
}
