using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterAnimator characterAnimator;

    private void Awake()
    {
        characterAnimator = gameObject.GetComponent<CharacterAnimator>();
    }

    private void Update()
    {
        HandleMovement();
        
    }

    private void HandleMovement() {
        float speed = 40f;
        float moveX = 0f;
        float moveY = 0f;

        //Input detection
        if(Input.GetKey(KeyCode.W))
        {
            moveY = 1f; //UP
        }
        if(Input.GetKey(KeyCode.S))
        {
            moveY = -1f; //DOWN
        }
        if(Input.GetKey(KeyCode.A))
        {
            moveX = -1f; //LEFT
        }
        if(Input.GetKey(KeyCode.D))
        {
            moveX = 1f; //RIGHT
        }

        //Animation control and movement
        bool isIdle = moveX == 0 &&  moveY == 0; //Checking movement
        if(isIdle) {
            characterAnimator.PlayIdleAnimation(Vector3.zero); //PLay idle animation
        }
        else {
            //Moving character
            Vector3 moveDir = new Vector3(moveX, moveY).normalized; //Direction
            characterAnimator.PlayWalkAnimation(moveDir); //Play walk animation
            transform.position += moveDir * speed * Time.deltaTime; //Move
        }

        
    }
}
