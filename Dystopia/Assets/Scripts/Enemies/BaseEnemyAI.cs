using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAI : Enemy
{
    public enum State {
        Idleing,
        Moving,
        Chasing,
        Attacking,
        KnockBack,
        Searching,
        Returning,
    }

    [Space] 
    [SerializeField] private SpriteRenderer srAttackTrail;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask blockingLayerMasks; //Layers that can block the vision of the enemy
    [Space]
    [SerializeField] private float viewDistance = 100f; //View Distance
    [SerializeField] private float fovAngle = 60f; //Field Of View Angle
    [Space]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float rotationSpeed = 180f;
    [Space]
    [SerializeField] private float secondsSearching = 3f; //Seconds searching the player after we lost sight of it
    [SerializeField] private int amountOfSearchingsToDo = 3;
    [Space]
    [SerializeField] private float maxDistanceFromStartingPoint = 400f; //max distance from starting point to lose aggro and return no the starting point

    private GameHandler gameHandler;
    private Transform player;
    private Vector3 lastPlayerPosition;
    private State myState; //State machine of the enemy
    private Rigidbody2D myRigidbody2D;
    private Vector3 moveDir = Vector3.zero;
    private float timePlayerHasBeenLost = 0; //Time.DeltaTime when player is lost while chasing and enemy reach the last known player position
    private int searchingCounter = 0; //Number of searchings done
    private bool attacking = false; //true while attacking, otherwise false

    private float timeAuxiliar = 0f; //auxiliar time variable used to calculate changes in behaviour when idleing or moving

    private float distanceToAttackPoint;

    private Vector3 startingPosition;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
        player = gameHandler.playerTransform;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        distanceToAttackPoint = 2f * Vector3.Distance(attackPoint.position,transform.position); //Distance calculated from the center, so we need to multiply by 2
        timeAuxiliar = 0f;
        startingPosition = transform.position; //set the starting position
    }

    private void FixedUpdate() {
        DoAction();
    }

    private Vector3 FindTargetPlayer() {
        if(Vector3.Distance(transform.position, player.position) < viewDistance) { //Player inside view distance
            Vector3 dirToPlayer = (player.position - transform.position).normalized; //Direction to player
            if(Vector3.Angle(myDirection(), dirToPlayer) < fovAngle / 2f) { //Player inside the view cone (field of view)
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance, blockingLayerMasks);
                if(raycastHit2D.collider != null) { //if the raycast hits something
                    if(raycastHit2D.collider.gameObject.layer == LayerMask.NameToLayer("Player")) { //Hitting player
                        return player.position;
                    } else { //Hitting something that it is not the player
                        //Nothing
                    }
                }
            }
        }
        return Vector3.zero;
    }

    private bool CheckPlayerInView() { //if player in view, save its position as last player position and return true
        Vector3 positionPlayerInView = FindTargetPlayer();
        if(positionPlayerInView != Vector3.zero) {
            lastPlayerPosition = player.position;
            return true;
        }
        return false;
    }

    private void DoAction() {
        if(Vector3.Distance(startingPosition, transform.position) > maxDistanceFromStartingPoint) {
            myState = State.Returning;
        }
        switch(myState) {
            case State.Idleing:
                if(CheckPlayerInView()) {
                    myState = State.Chasing;
                    Debug.Log("Chasing");
                } else {
                    if(timeAuxiliar==0f) {
                        timeAuxiliar = Time.time;
                    }
                    Ideling();
                }
                break;
            case State.Moving:
                if(CheckPlayerInView()) {
                    myState = State.Chasing;
                    Debug.Log("Chasing");
                } else {
                    if(timeAuxiliar==0f) {
                        timeAuxiliar = Time.time;
                    }
                    Moving();
                }
                break;
            case State.Chasing:
                CheckPlayerInView();
                ChasePlayer();
                break;
            case State.Attacking:
                if(CheckPlayerInView()) {
                    AttackPlayer();
                } else {
                    timePlayerHasBeenLost = Time.time;
                    myState = State.Searching;
                    Debug.Log("Searching");
                }
                break;
            case State.KnockBack:
                break;
            case State.Searching:
                if(CheckPlayerInView()) {
                    myState = State.Chasing;
                    Debug.Log("Chasing");
                } else {
                    SearchPlayer();
                }
                break;
            case State.Returning:
                Returning();
                break;
            default:
                break;
        }
    }

    private Vector3 myDirection() {
        if(attackPoint == null) return Vector3.zero;
        return (attackPoint.position - transform.position).normalized;
    }

    private void AimAtPlayer(Vector3 aimDir) {
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0, angle);
    }

    private void AttackPlayer() {
        if(attacking == false) { //if we are not attacking yet
            if(InRange(player.position)) {
                StartCoroutine(DoAttack()); //Attack
         } else {
                myState = State.Chasing; //Chase to get in range
                Debug.Log("Chasing");
            }
        }
    }

    private IEnumerator DoAttack() {
        attacking = true;
        yield return new WaitForSeconds(attackDelay); //attack delay
        //Check if player is in range when attacking
        Collider2D[] hittedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, hittableLayers);
        foreach(Collider2D obj in hittedObjects) {
            if(obj.isTrigger) continue; //We don't want to hit trigger colliders
            //Check for player to get damage
            Character character = obj.GetComponent<Character>();
            if(character!=null) {
                character.GetDamage(damage);
                Debug.Log("Damage done to player: " + damage);
            }
            //Check for objectes than can be hitted by the enemy
            Hittable hittableObj = obj.GetComponent<Hittable>(); 
            if(hittableObj != null)  {
                hittableObj.TakeDamage(damage, false);
            }
        }
        StartCoroutine("ShowAttackTrail");
        Debug.Log("Attack Done");
        yield return new WaitForSeconds(timeBetweenAttacks); //time until the next attack
        attacking = false;
    }

    IEnumerator ShowAttackTrail() {
        SoundManager.PlaySound(SoundManager.Sound.AttackMele);
        srAttackTrail.enabled = true;
        yield return new WaitForSeconds(timeBetweenAttacks);
        srAttackTrail.enabled = false;
    }

    private void ChasePlayer() {
        moveDir = (lastPlayerPosition - transform.position).normalized; //Direction to player
        AimAtPlayer(moveDir);
        myRigidbody2D?.MovePosition(transform.position + moveDir * speed * Time.deltaTime);
        if(InRange(lastPlayerPosition)) { //if we are very close to the target position
            myState = State.Attacking;
            Debug.Log("Attacking");
        }
    }

    private bool InRange(Vector3 positionToCheck) {
        return Vector3.Distance(positionToCheck, transform.position) < distanceToAttackPoint + 0.5f;
    }

    private void SearchPlayer() {
        if(Time.time < timePlayerHasBeenLost + (secondsSearching * searchingCounter) ) {
            transform.Rotate ( new Vector3(0,0,1) * ( rotationSpeed * Time.deltaTime ) );
            //moveDir = myDirection();
            //myRigidbody2D?.MovePosition(transform.position + moveDir * speed * Time.deltaTime);
        } else {
            float randAngle = Random.Range(0, 360); //random angle in degrees
            transform.eulerAngles = new Vector3(0,0, randAngle); //Set a random direction
            searchingCounter++;
            if(searchingCounter > amountOfSearchingsToDo) {
                searchingCounter = 0;
                myState = State.Idleing;
                Debug.Log("Iddleing");
            }
        }
    }

    private void Ideling() {
        if(Time.time < timeAuxiliar + 2.5f) { 
            transform.Rotate ( new Vector3(0,0,1) * ( rotationSpeed * Time.deltaTime ) ); //rotate for a while
        }
        if(Time.time >= timeAuxiliar + 5f) { //Wait for a while and change to move
            myState = State.Moving;
            timeAuxiliar = 0f;
        }
    }

    private void Moving() {
        if(Time.time < timeAuxiliar + 2f) { //move for a while
            moveDir = myDirection();
            myRigidbody2D?.MovePosition(transform.position + moveDir * speed * Time.deltaTime);
        } 
        if(Time.time >= timeAuxiliar + 4f) { //Wait for a while and change to idle
            myState = State.Idleing;
            timeAuxiliar = 0f;
        }
    }

    private void Returning() {
        moveDir = (startingPosition - transform.position).normalized; //direction to the starting point
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0, angle); //rotate to the walking direction (starting point)
        myRigidbody2D?.MovePosition(transform.position + moveDir * speed * Time.deltaTime); //move
        if(Vector3.Distance(startingPosition, transform.position) < 1f) { //until it is close to the starting point
            myState = State.Idleing;
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(myState != State.Attacking && other.gameObject.CompareTag("Player")) {
            lastPlayerPosition = player.position;
            myState = State.Chasing;
            Debug.Log("Chasing");
        }
    }

    private void OnDestroy() {
            gameHandler?.levelSystem?.AddExperience(experience);
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint!=null) {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
