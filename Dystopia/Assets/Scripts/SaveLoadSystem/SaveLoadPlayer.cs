using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPlayer : MonoBehaviour
{
    private GameObject player;
    private Character character;

    private const string fileName = "player";

    private void Awake() {
        player = GameObject.Find("Player");
        if(player!=null) {
            character = player.GetComponent<Character>();
        }
    }

    public void Save(GameObject playerToSave) {
        if(playerToSave!=null) {
            //Character (replace for player when finished) !!!
            SerializableCharacter serPlayer = new SerializableCharacter(playerToSave.GetComponent<Character>());
            SaveLoadSystem.Save<SerializableCharacter>(serPlayer, fileName);
        }
    }

    public void Load() {
        //Get Data
        SerializableCharacter serLoadedCharacter = SaveLoadSystem.Load<SerializableCharacter>(fileName);
        
        //Position
        //Vector3 posPlayer = new Vector3(serLoadedCharacter.position.x, serLoadedCharacter.position.y, serLoadedCharacter.position.z);
        //player.transform.position = posPlayer;
        
        //Load Character values
        if(serLoadedCharacter!=null) {
            character.LoadSerializedData(serLoadedCharacter);
        }
    }
}
