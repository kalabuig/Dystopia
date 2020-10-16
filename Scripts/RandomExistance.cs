using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomExistance : MonoBehaviour
{
    [Range(0,100)]
    public int probabilityToBeDeleted;
    void Start()
    {
        int rand = Random.Range(0, 100);
        if(rand<probabilityToBeDeleted) {
            //Destroy myself
            Destroy(this.gameObject);
        }
    }
}
