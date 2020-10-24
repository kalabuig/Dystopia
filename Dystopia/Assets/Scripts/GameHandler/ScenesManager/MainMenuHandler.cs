using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)) { //New game test
            NewGame();
        }
    }

    public void NewGame() {
        Loader.Load(Loader.Scene.GameScene);
    }

    public void LoadGame() {

    }

    public void OpenSettings() {

    }

    public void OpenCredits() {

    }

    public void QuitToDesktop() {
        Application.Quit();
    }


}
