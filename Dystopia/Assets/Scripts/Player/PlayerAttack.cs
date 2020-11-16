using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameHandler gameHandler;

    public Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask hittableLayers;

    private int attackDamage = 40;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse1) && GameHandler.IsMouseOverUI()==false) {
            Attack();
        }
    }

    private void Attack() {
        Collider2D[] hittedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, hittableLayers);
        foreach(Collider2D obj in hittedObjects) {
            if(obj.isTrigger) continue; //We don't want to hit trigger colliders
            obj.GetComponent<Hittable>()?.TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint!=null) {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
