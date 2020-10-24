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
        int randIndex = Random.Range(0, buildings.Length); //Select a random building
        int randRotIndex = Random.Range(0, rot.Length); //select orientation
        GameObject go = Instantiate(buildings[randIndex], Vector3.zero, Quaternion.identity); //instantiate island
        go.transform.SetParent(this.gameObject.transform, false); //set building position to the spam point (parent)
        go.transform.Rotate(0,0,rot[randRotIndex]); //rotate to orientation selected
    }

}
