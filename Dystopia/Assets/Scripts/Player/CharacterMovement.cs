using UnityEngine;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    enum CharacterControlType { LookingMouse=0, Independent=1}; //Type of movement control for the character

    private CharacterAnimator characterAnimator;
    private Rigidbody2D characterRigidbody2D;
    private Camera cam;
    private FieldOfView fieldOfView;
    private Character character;
    private StatsModifiers statsModifiers;

    private float speed; //character speed
    private Vector3 mousePos; //mouse position
    private Vector3 moveDir; //move direction
    
    private CharacterControlType characterControlType = CharacterControlType.Independent;

    private void Awake()
    {
        character = GetComponent<Character>();
        statsModifiers = GetComponent<StatsModifiers>();
        RefreshSpeedValue();
        characterAnimator = GetComponent<CharacterAnimator>();
        characterRigidbody2D = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        GameObject go_fieldOfView = GameObject.Find("FieldOfView");
        if(go_fieldOfView!=null)
            fieldOfView = go_fieldOfView.GetComponent<FieldOfView>();
    }

    public void RefreshSpeedValue() {
        speed = character.moveSpeed + statsModifiers.GetFloatStatMod(StatsModifiers.Modifier.moveSpeed);
    }

    private void Update()
    {
        if(GameHandler.gameIsPaused == false) {
            SwapCharacterControlType();
            HandleMovement();
            Vector3 aimDir = AimAtMouse();
            SetFieldOfView(aimDir);
        }
    }

    private void FixedUpdate() {
        //Move using rigid body to take care of physics (collisions)
        characterRigidbody2D.MovePosition(transform.position + moveDir * speed * Time.deltaTime);
    }

    private void SetFieldOfView(Vector3 aimDir) {
        if(fieldOfView!=null) {
            fieldOfView.SetAimDirection(aimDir);
            fieldOfView.SetOrigin(transform.position);
        }
    }

    private void SwapCharacterControlType() {
        if(Input.GetKeyDown(KeyCode.K)){
            characterControlType =  characterControlType == CharacterControlType.Independent ? CharacterControlType.LookingMouse : CharacterControlType.Independent;
        }
    }

    private Vector3 AimAtMouse() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0, angle);
        return aimDir;
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
            moveDir = Vector3.zero;
            characterAnimator.PlayIdleAnimation(); //PLay idle animation
        }
        else {
            characterAnimator.PlayWalkAnimation(); //Play walk animation
            moveDir = new Vector3(moveX, moveY).normalized; //Direction
            //check character control to use
            switch(characterControlType) {
                case CharacterControlType.Independent:
                    //Vector3 targetPosition = transform.position + (moveDir * speed * Time.deltaTime);
                    //if(checkCollision(moveDir)==false) //if no collision
                    //    transform.position = targetPosition; //Move
                    //transform.position += moveDir * speed * Time.deltaTime;
                    break;
                case CharacterControlType.LookingMouse: //same as default
                default:
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
                    
                    moveDir = aimDir;
                    //transform.position += aimDir * speed * Time.deltaTime; //Move
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
    }
}
