using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public GameObject[] sectorPrefabs; //the different type of prefabs for sectors
    private Transform player;
    
    private Dictionary<Vector2Int, GameObject> sectors; //sectors in the map
    private const int SIZE = 3; // SIZE x SIZE array of islands
    private const int DISTANCE_BETWEEN_SECTORS = 3000;
    private const int ACTIVATION_DISTANCE = DISTANCE_BETWEEN_SECTORS - 500;
    private const int DEACTIVATION_DISTANCE = DISTANCE_BETWEEN_SECTORS + 500;
    private const int SECONDS_TO_NEXT_REFRESH = 5;

    private void Awake() {
        sectors = new Dictionary<Vector2Int, GameObject>();
        player = GameObject.Find("Player").transform;
        startingSectors();
        }

    private void Start() {

        StartCoroutine(RefreshSetActive());
    }

    //Create the starting sectors of the map
    private void startingSectors() {
        int halfSIZE = (int)(SIZE/2);
        for(int x = -halfSIZE; x <= halfSIZE; x++) {
            for(int y = -halfSIZE; y <= halfSIZE; y++) {
                CreateSector(x, y);
            }
        }
        GameObject startingSector = sectors[new Vector2Int(0,0)];
        if(startingSector!=null)
            startingSector.SetActive(true); //Activate the starting sector (0,0)
    }

    public GameObject CreateSector(int x, int y) {
        int randIndex = Random.Range(0, sectorPrefabs.Length); //Select a random building
                GameObject go = Instantiate(sectorPrefabs[randIndex], Vector3.zero, Quaternion.identity); //instantiate island
                Vector3 relativePosition = new Vector3(x*DISTANCE_BETWEEN_SECTORS, y*DISTANCE_BETWEEN_SECTORS, 0);
                go.transform.SetParent(this.gameObject.transform, false); //Set parent (Map object)
                go.transform.localPosition += relativePosition; //set relative position
                go.SetActive(false); //by default the sectors are disabled
                sectors.Add(new Vector2Int(x, y), go);
                return go;
    }

    public void SectorSetActive(int x, int y, bool active) {
        if(sectors.TryGetValue(new Vector2Int(x, y), out GameObject sector)) {
            sector.SetActive(active);
        }
    }

    //Enable or disable any sector (relative to the distance to the player)
    IEnumerator RefreshSetActive() {
        while(true) {
            Vector3 playerPos = player.position;
            //Actual sector (the player is in this sector now)
            int xSec = (int)(playerPos.x / DISTANCE_BETWEEN_SECTORS);
            int ySec = (int)(playerPos.y / DISTANCE_BETWEEN_SECTORS);
            //Sectors to check if need to be activated or deactivated
            for(int x = xSec-1; x <= xSec+1; x++) {
                for(int y = ySec-1; y <= ySec+1; y++) {
                    if(sectors.TryGetValue(new Vector2Int(x, y), out GameObject sector)) { //if the sector exist
                        CheckToActivateOrDeactivate(sector, playerPos);
                    } else { //if the sector doesn't exist
                        GameObject newSector = CreateSector(x, y); //Create sector and activate if needed
                        if(newSector!=null) {
                            CheckToActivateOrDeactivate(newSector, playerPos);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(SECONDS_TO_NEXT_REFRESH); //wait some time before the next refresh
        }

    }

    private void CheckToActivateOrDeactivate(GameObject sector, Vector3 playerPos) {
        float d = (sector.transform.position - playerPos).magnitude;
        if( d < ACTIVATION_DISTANCE) {
            sector.SetActive(true); //activate sector
        }
        if( d > DEACTIVATION_DISTANCE) {
            sector.SetActive(false); //deactivate sector
        }
    }

}
