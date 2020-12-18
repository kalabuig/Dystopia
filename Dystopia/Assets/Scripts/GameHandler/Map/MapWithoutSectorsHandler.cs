using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWithoutSectorsHandler : MonoBehaviour
{
    public GameObject[] islandPrefabs; //the different type of prefabs for islands
    private Transform player;
    
    private Dictionary<Vector2Int, GameObject> islands; //islands in the map
    private const int SIZE = 1; // SIZE x SIZE array of islands
    private const int DISTANCE_BETWEEN_ISLANDS = 1000;
    private const int ACTIVATION_DISTANCE = DISTANCE_BETWEEN_ISLANDS - 200; //=750
    private const int DEACTIVATION_DISTANCE = DISTANCE_BETWEEN_ISLANDS + 250; //=1250
    private const float SECONDS_TO_NEXT_REFRESH = 0.1f; //5f;

    private bool updateActive = false; //To control the RefreshSetActive method activity (set it to true after loading the map or at a new game)

    private void Awake() {
        islands = new Dictionary<Vector2Int, GameObject>();
        player = GameObject.Find("Player").transform;
        //startingIsland();
        }

    private void Start() {
        StartCoroutine(RefreshSetActive());
    }

    //THIS METHOD IS NOT NEEDED NOW, REFRESHSETACTIVE CREATES THE FIRST ISLAND
    //Create the starting island of the map
    public void startingIsland() {
        int halfSIZE = (int)(SIZE/2);
        for(int x = -halfSIZE; x <= halfSIZE; x++) {
            for(int y = -halfSIZE; y <= halfSIZE; y++) {
                CreateIsland(x, y);
            }
        }
        GameObject startingIsland = islands[new Vector2Int(0,0)];
        if(startingIsland!=null)
            startingIsland.SetActive(true); //Activate the starting sector (0,0)
    }

    //Method used to Load a map, it adds the Loaded island to the Islands Dictionary
    public void AddIslandToDictionary(GameObject goIsland) {
        int x = (int) (goIsland.transform.position.x / DISTANCE_BETWEEN_ISLANDS);
        int y = (int) (goIsland.transform.position.y / DISTANCE_BETWEEN_ISLANDS);
        goIsland.SetActive(false); //by default the islands are disabled
        islands.Add(new Vector2Int(x, y), goIsland);
    }

    public GameObject CreateIsland(int x, int y) {
        int randIndex = Random.Range(0, islandPrefabs.Length); //Select a random island
        GameObject go = Instantiate(islandPrefabs[randIndex], Vector3.zero, Quaternion.identity); //instantiate island
        Vector3 relativePosition = new Vector3(x*DISTANCE_BETWEEN_ISLANDS, y*DISTANCE_BETWEEN_ISLANDS, 0);
        go.transform.SetParent(this.gameObject.transform, false); //Set parent (Map object)
        go.transform.localPosition += relativePosition; //set relative position
        go.SetActive(false); //by default the islands are disabled
        islands.Add(new Vector2Int(x, y), go);
        return go;
    }

    public void IslandSetActive(int x, int y, bool active) {
        if(islands.TryGetValue(new Vector2Int(x, y), out GameObject island)) {
            island.SetActive(active);
        }
    }

    //Enable or disable any island (relative to the distance to the player)
    IEnumerator RefreshSetActive() {
        while(updateActive==false) {
            yield return null; //Waiting until activation
        }
        while(true) {
            Vector3 playerPos = player.position;
            //Actual island (the player is in this island now)
            int xIsl = (int)(playerPos.x / DISTANCE_BETWEEN_ISLANDS);
            int yIsl = (int)(playerPos.y / DISTANCE_BETWEEN_ISLANDS);
            //Islands to check if need to be activated or deactivated
            for(int x = xIsl-2; x <= xIsl+2; x++) {
                for(int y = yIsl-2; y <= yIsl+2; y++) {
                    if(islands.TryGetValue(new Vector2Int(x, y), out GameObject island)) { //if the island exist
                        ActivateOrDeactivateIsland(island, playerPos);
                    } else { //if the island doesn't exist
                        if(Vector3.Distance(new Vector3(x*DISTANCE_BETWEEN_ISLANDS, y*DISTANCE_BETWEEN_ISLANDS,0), playerPos) < ACTIVATION_DISTANCE ) {
                            GameObject newIsland = CreateIsland(x, y); //Create island and activate it
                            if(newIsland!=null) {
                                newIsland.SetActive(true);
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(SECONDS_TO_NEXT_REFRESH); //wait some time before the next refresh
        }
    }

    private void ActivateOrDeactivateIsland(GameObject island, Vector3 playerPos) {
        float d = Vector3.Distance(island.transform.position, playerPos);
        if( d < ACTIVATION_DISTANCE) {
            island.SetActive(true); //activate island
        }
        if( d > DEACTIVATION_DISTANCE) {
            island.SetActive(false); //deactivate island
        }
    }

    public void ActivateRefresh() {
        updateActive = true;
    }

}
