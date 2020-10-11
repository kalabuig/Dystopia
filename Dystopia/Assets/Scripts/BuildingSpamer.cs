using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpamer : MonoBehaviour
{
    public GameObject[] buildings;
    [SerializeField]
    private bool onlyFlip = false; //To swap between 90 degree rotations (false) and 180 degree rotations (true)

    void Start()
    {
        int[] rot;
        if(onlyFlip) rot = new int[2] {0,180};
        else rot = new int[4] {0,90,180,270};
        int randIndex = Random.Range(0, buildings.Length);
        int randRotIndex = Random.Range(0, rot.Length);
        GameObject go = Instantiate(buildings[randIndex], transform.position, Quaternion.identity);
        go.transform.Rotate(0,0,rot[randRotIndex]);
    }

}
