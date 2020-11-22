using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType {
        Wolf,
        Humanoid,
    }

    [SerializeField] protected EnemyType _enemyType;
    public EnemyType enemyType { get => _enemyType;}
    [SerializeField] protected string _enemyName = "Enemy";
    public string enemyName{ get => _enemyName;}
    [Space]
    [SerializeField] protected int _damage = 10;
    public int damage { get => _damage;}
    [SerializeField] protected float _attackDelay = 0.5f;
    public float attackDelay { get => _attackDelay;}
    [SerializeField] protected float _timeBetweenAttacks = 1f;
    public float timeBetweenAttacks { get => _timeBetweenAttacks;}

    public string GetEnemyName() {
        return _enemyName + " (" + _enemyType.ToString() + ")";
    }
}
