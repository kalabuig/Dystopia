using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject creditsPanel;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)) {
            NewGame();
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            LoadGame();
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            OpenSettings();
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            OpenCredits();
        }
        if(Input.GetKeyDown(KeyCode.Q)) {
            QuitToDesktop();
        }
    }

    public void NewGame() {
        PersistentData.instance.newGame = true;
        Loader.Load(Loader.Scene.GameScene);
    }

    public void LoadGame() {
        PersistentData.instance.newGame = false;
        Loader.Load(Loader.Scene.GameScene);
    }

    public void OpenSettings() {
        Loader.Load(Loader.Scene.SettingsScene);
    }

    public void OpenCredits() {
        creditsPanel.gameObject.SetActive(true);
    }

    public void QuitToDesktop() {
        Application.Quit();
    }


}
