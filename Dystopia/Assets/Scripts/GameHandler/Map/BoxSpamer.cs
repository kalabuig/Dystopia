﻿using UnityEngine;

public class BoxSpamer : MonoBehaviour
{
    [SerializeField] 
    [Range(1,100)]
    private int chanceToSpawn = 100;
    [Space]
    [Header("Containers")]
    public GameObject[] boxes;

    private void Awake() {
        int randNum = Random.Range(0, 101);
        if(randNum<chanceToSpawn) {
            Spam();
        }
        Destroy(this.gameObject); //Destroy the spam object from the scene
    }

    private void Spam()
    {
        int[] rot = new int[4] {0,90,180,270};
        int randIndex = Random.Range(0, boxes.Length); //Select a random box
        int randRotIndex = Random.Range(0, rot.Length); //select orientation
        GameObject go = Instantiate(boxes[randIndex], Vector3.zero, Quaternion.identity); //instantiate box
        //go.transform.SetParent(this.gameObject.transform, false); //set box position to the spam point (parent)
        go.transform.SetParent(transform.parent, true); //Set the parent
        go.transform.SetPositionAndRotation(transform.position, Quaternion.identity); //Set the position
        go.transform.Rotate(0,0,rot[randRotIndex]); //rotate to orientation selected
    }
}
