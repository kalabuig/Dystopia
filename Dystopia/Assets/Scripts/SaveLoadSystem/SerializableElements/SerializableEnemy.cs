using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableEnemy
{
    public SerializableTransform transform;
    public int currentHealth;
    //Enemy attributes
    public int enemyType; //Enemy.EnemyType
    public string enemyName;

    public SerializableEnemy(GameObject enemy) {
        DoSerialization(enemy);
    }

    public void DoSerialization(GameObject enemy) {
        //Transform
        transform = new SerializableTransform(enemy.transform);
        //Current Health
        Hittable hittableScript = enemy.GetComponent<Hittable>();
        currentHealth = hittableScript!=null ? hittableScript.currentHealth : 1; 
        //Enemy attributes:
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if(enemyComponent!=null) {
            //Enemy name
            enemyName = enemyComponent.GetEnemyName();
            //Enemy type
            enemyType = (int) enemyComponent.enemyType;
        }
    }

}
