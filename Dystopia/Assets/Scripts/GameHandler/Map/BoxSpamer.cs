using UnityEngine;

public class BoxSpamer : MonoBehaviour
{
    public GameObject[] boxes;

    void Start()
    {
        int[] rot = new int[4] {0,90,180,270};
        int randIndex = Random.Range(0, boxes.Length); //Select a random box
        int randRotIndex = Random.Range(0, rot.Length); //select orientation
        GameObject go = Instantiate(boxes[randIndex], Vector3.zero, Quaternion.identity); //instantiate box
        go.transform.SetParent(this.gameObject.transform, false); //set box position to the spam point (parent)
        go.transform.Rotate(0,0,rot[randRotIndex]); //rotate to orientation selected
    }
}
