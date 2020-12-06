using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameHandler gameHandler;
    private Character character;
    private StatsModifiers statsModifiers;
    private Skills skills;

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
        character = GetComponent<Character>();
        statsModifiers = GetComponent<StatsModifiers>();
        skills = GetComponent<Skills>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse1) && GameHandler.IsMouseOverUI()==false) {
            Attack();
        }
    }

    //Check the weapon to set the damage amount
    public int GetDamageAmount() {
        EquippableItem weapon = gameHandler.GetWeapon();
        int baseDamage = weapon==null ? attackBaseDamage : weapon.damage;
        return baseDamage + skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.damage);
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

    private void SetScaleAttackTrail(float scale) {
        scale = ApplyAttackRangeSpecialModifiers(scale);
        srAttackTrail.gameObject.transform.localScale = new Vector3(scale, scale, 1f);
    }

    private void Attack() {
        if(canAttack==false) return;
        canAttack = false;
        float scale = AttackRangeToScale(GetWeaponRange());
        Collider2D[] hittedObjects = Physics2D.OverlapCircleAll(attackPoint.position, ApplyAttackRangeSpecialModifiers(attackRange * scale), hittableLayers);
        foreach(Collider2D obj in hittedObjects) {
            if(obj.isTrigger) continue; //We don't want to hit trigger colliders
            bool isCriticalHit = UnityEngine.Random.Range(0,100) < character.criticalChance 
                                                                    + statsModifiers.GetIntStatMod(StatsModifiers.Modifier.criticalChance) 
                                                                    + skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.criticalChance);
            int damage = isCriticalHit ? GetDamageAmount() * 2 : GetDamageAmount();
            Debug.Log("Damage: " + damage);
            obj.GetComponent<Hittable>()?.TakeDamage(damage, isCriticalHit);
        }
        SetScaleAttackTrail(scale);
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

    private float ApplyAttackRangeSpecialModifiers(float range) {
        List<PassiveSkillData> results = skills.GetSkillsWithSpecialModifier(SpecialModifier.AttackRange);
        float multiplyFactor = 1f;
        if(results!=null) {
            foreach(PassiveSkillData r in results) {
                multiplyFactor += r.valueAtThisLevel / 100f;
            }
        }
        return range * multiplyFactor;
    }

}