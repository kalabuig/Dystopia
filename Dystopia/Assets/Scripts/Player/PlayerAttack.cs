using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameHandler gameHandler;

    public Transform attackPoint;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private LayerMask hittableLayers;

    [Space]
    [SerializeField] private SpriteRenderer srAttackTrail;

    private int attackBaseDamage = 1;
    private bool canAttack = true;
    private float timeBetweenAttacks = 0.2f;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse1) && GameHandler.IsMouseOverUI()==false) {
            Attack();
        }
    }

    //Check the weapon to set the damage amount
    private int GetDamageAmount() {
        EquippableItem weapon = gameHandler.GetWeapon();
        return weapon==null ? attackBaseDamage : weapon.damage;
    }

    private AttackRange GetWeaponRange() {
        EquippableItem weapon = gameHandler.GetWeapon();
        return weapon==null ? AttackRange.Short : weapon.attackRange;
    }


    private float AttackRangeToScale(AttackRange attackRange) {
        switch(attackRange) {
            default:
            case AttackRange.Short:
                return 0.75f;
            case AttackRange.Medium:
                return 1f;
            case AttackRange.Large:
                return 1.5f;
        }
    }

    private void ScaleAttackTrail(float scale) {
        srAttackTrail.gameObject.transform.localScale = new Vector3(scale, scale, 1f);
    }

    private void Attack() {
        if(canAttack==false) return;
        canAttack = false;
        float scale = AttackRangeToScale(GetWeaponRange());
        Collider2D[] hittedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange * scale, hittableLayers);
        foreach(Collider2D obj in hittedObjects) {
            if(obj.isTrigger) continue; //We don't want to hit trigger colliders
            obj.GetComponent<Hittable>()?.TakeDamage(GetDamageAmount());
        }
        ScaleAttackTrail(scale);
        StartCoroutine("ShowAttackTrail"); 
    }

    IEnumerator ShowAttackTrail() {
        SoundManager.PlaySound(SoundManager.Sound.AttackMele);
        srAttackTrail.enabled = true;
        yield return new WaitForSeconds(timeBetweenAttacks);
        srAttackTrail.enabled = false;
        canAttack = true;
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint!=null) {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}