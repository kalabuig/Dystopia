using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType {
        Animal,
        Humanoid,
        Robot,
    }

    [SerializeField] protected EnemyType _enemyType;
    public EnemyType enemyType { get => _enemyType;}
    [SerializeField] protected string _enemyName = "Enemy";
    public string enemyName{ get => _enemyName;}
    
    [Space]
    [SerializeField] protected int _level = 1;
    public int level { get => _level; }

    [Space]
    [SerializeField] protected int _damage = 5;
    public int damage { get => _damage;}
    [SerializeField] protected float _attackRange = 5f;
    public float attackRange { get => _attackRange;}
    [SerializeField] protected float _attackDelay = 0.5f;
    public float attackDelay { get => _attackDelay;}
    [SerializeField] protected float _timeBetweenAttacks = 1f;
    public float timeBetweenAttacks { get => _timeBetweenAttacks;}
    [SerializeField] protected LayerMask _hittableLayers;
    public LayerMask hittableLayers { get => _hittableLayers; }

    [Space]
    [SerializeField] protected int _experience = 25; //experience that gives when killed
    public int experience { get => _experience;}

    public string GetEnemyName() {
        return _enemyName + " (" + _enemyType.ToString() + ")";
    }

    private void Awake() {
        //The level of the enemy is proportional to the distance to the initial point
        float distanceLevel = Vector3.Distance(transform.position, Vector3.zero) / 4000f;
        _level = 1 + (int)distanceLevel;
        //the damage change with the level
        _damage = _damage + Mathf.FloorToInt(_level * 0.5f); 
    }
}
