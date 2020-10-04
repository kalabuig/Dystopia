using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpamer : MonoBehaviour
{
    public GameObject[] buildings;

    void Start()
    {
        int[] rot = new int[4] {0,90,180,270};
        int randIndex = Random.Range(0, buildings.Length);
        int randRotIndex = Random.Range(0, 4);
        GameObject go = Instantiate(buildings[randIndex], transform.position, Quaternion.identity);
        go.transform.Rotate(0,0,rot[randRotIndex]);
    }

}
