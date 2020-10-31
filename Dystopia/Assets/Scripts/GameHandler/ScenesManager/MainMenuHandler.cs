using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
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
        Loader.Load(Loader.Scene.GameScene);
    }

    public void LoadGame() {

    }

    public void OpenSettings() {
        Loader.Load(Loader.Scene.SettingsScene);
    }

    public void OpenCredits() {

    }

    public void QuitToDesktop() {
        Application.Quit();
    }


}
