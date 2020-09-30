using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using CodeMonkey.Utils;

public class CharacterMovement : MonoBehaviour
{
    private CharacterAnimator characterAnimator;
    private Camera cam;
    private float speed = 60f; //character speed
    private Vector3 mousePos; //mouse position

    private void Awake()
    {
        characterAnimator = gameObject.GetComponent<CharacterAnimator>();
        cam = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
        AimAtMouse();
    }

    private void AimAtMouse() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0, angle);
    }

    private void HandleMovement() {
        float moveX = 0f;
        float moveY = 0f;

        //Input detection
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical"); //forward or backward

        //Animation control and movement
        bool isIdle = moveX == 0 &&  moveY == 0; //Checking movement
        if(isIdle) {
            characterAnimator.PlayIdleAnimation(); //PLay idle animation
        }
        else {
            characterAnimator.PlayWalkAnimation(); //Play walk animation
            Vector3 moveDir = new Vector3(moveX, moveY).normalized; //Direction
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition); 
            mousePos.z = 0;
            Vector3 aimDir = (mousePos - transform.position).normalized; //Pointing to the mouse
            Vector3 aimDirLateral = new Vector3(); //Lateral movement
            Vector3 aimDirFrontal = new Vector3(); //Forward or backward movement
            if(moveDir.x > 0 ) //to the right
                aimDirLateral = Quaternion.Euler(0, 0, -90) * aimDir;
            if(moveDir.x < 0 ) //to the left
                aimDirLateral = Quaternion.Euler(0, 0, 90) * aimDir;
            if(moveDir.y > 0 ) //forward
                aimDirFrontal = aimDir;
            if(moveDir.y < 0 ) //backward
                aimDirFrontal = Quaternion.Euler(0, 0, 180) * aimDir;
            aimDir = (aimDirFrontal + aimDirLateral).normalized;
            aimDir.z = 0;
            transform.position += aimDir * speed * Time.deltaTime; //Move           
        }
    }
}
