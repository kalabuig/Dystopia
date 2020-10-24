using UnityEngine;

public class IslandSpamer : MonoBehaviour
{
    public GameObject[] islands;

    void Start()
    {
        int[] rot = new int[4] {0,90,180,270};
        int randIndex = Random.Range(0, islands.Length); //Select a random island
        int randRotIndex = Random.Range(0, rot.Length); //select orientation
        GameObject go = Instantiate(islands[randIndex], Vector3.zero, Quaternion.identity); //instantiate island
        go.transform.SetParent(this.gameObject.transform, false); //set island position to the spam point (parent)
        go.transform.Rotate(0,0,rot[randRotIndex]); //rotate to orientation selected
    }
}
