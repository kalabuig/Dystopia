using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton for persistent data
public class PersistentData : MonoBehaviour
{
    private static PersistentData _instance;
    public static PersistentData instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PersistentData> ();
                if (_instance == null) {
                    GameObject obj = new GameObject ();
                    obj.name = "PersistentData";
                    _instance = obj.AddComponent<PersistentData>();
                }
            }
            return _instance;
        }
    }
 
    public bool newGame = true; //True: creates a new game, False: loads a saved game

    public virtual void Awake ()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad (this.gameObject);
        } else {
            Destroy (gameObject);
        }
    }
}
