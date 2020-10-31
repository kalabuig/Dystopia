using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //Camera
    private CameraBehavior cameraBehavior;
    
    //Player
    private Transform playerTransform;
    
    //Container
    private GameObject selectedContainer;

    //Panels
    private GameObject characterPanel;
    private GameObject craftingPanel;
    private GameObject scavengingPanel;
    public GameObject textPanel;
    private GameObject pausePanel;
    private GameObject gameOverPanel;

    //Zoom
    private float zoom=100f; //Zoom level
    private float zoomSpeed = 150f; //Zoom in and out speed
    private float minZoom = 40f; //More close zoom
    private float maxZoom = 200f; //More far zoom
    private float zoomWheelSensibility = 10f;
    
    //Pause control
    private static bool _gameIsPaused;
    public static bool gameIsPaused {
        get => _gameIsPaused;
    }

    private void Awake() {
        //Camera
        cameraBehavior = Camera.main.GetComponent<CameraBehavior>();
        //Player transform
        playerTransform = GameObject.Find("Player").transform;
        //CharacterPanel
        characterPanel = GameObject.Find("CharacterPanel");
        //Crafting Panel
        craftingPanel = GameObject.Find("CraftingPanel");
        //Scavenging Panel
        scavengingPanel = GameObject.Find("ScavengingPanel");
        //PausePanel
        pausePanel = GameObject.Find("PausePanel");
        //GameOverPanel
        gameOverPanel = GameObject.Find("GameOverPanel");
    }
    void Start()
    {
        TimeTickSystem.Create(); //Create the TimeTickSystem game object
        cameraBehavior.setFocus(() => playerTransform.position); //Camera will follow the player
        cameraBehavior.setZoom(() => zoom); //Set the zoom to its default value
        CloseInventoryPanel();
        CloseCraftingPanel();
        CloseScavengingPanel();
        gameOverPanel.SetActive(false);
        ResumeGame(); //inside it is --> pausePanel.SetActive(false);
        selectedContainer = null;
    }

    private void Update() {
        //Pause or resume game
        if(Input.GetKeyDown(KeyCode.Escape)) { //On ESC pressed
            if(characterPanel.activeSelf) { //if inventory is open, close inventory
                OpenCloseCharacterPanel(); 
            } else { //otherwise, pause or resume game
                PauseResumeGame();
            }
        }
        //Input control
        if(gameIsPaused == false) { //if not paused
            HandleZoom();
            HandleKeyboardInputs();
        } else { //if paused
            HandlePausedKeyboardInputs();
        }
    }

    public void SetContainer(GameObject container) {
        if(container.layer == LayerMask.NameToLayer("Containers")) {
            selectedContainer = container;
            scavengingPanel.GetComponent<ScavengingInventory>()?.loadItems(container.GetComponent<Container>());
            textPanel.SetActive(true);
        }
    }

    public void UnSetContainer(GameObject container) {
        if(container.layer == LayerMask.NameToLayer("Containers")) {
            scavengingPanel.GetComponent<ScavengingInventory>()?.storeItems(container.GetComponent<Container>());
            selectedContainer = null;
            textPanel.SetActive(false);
            CloseScavengingPanel();
        }
    }

    private void HandlePausedKeyboardInputs() {
        //Open or close the character panel
        if(Input.GetKeyDown(KeyCode.R)){ //resume game
            PauseResumeGame();
        } else if(Input.GetKeyDown(KeyCode.S)) { //save game
            SaveGame();
        } else if(Input.GetKeyDown(KeyCode.L)) { //load game
            LoadGame();
        } else if(Input.GetKeyDown(KeyCode.E)) { //exit game
            ExitGame();
        }
    }

    public void SaveGame() {

    }

    public void LoadGame() {
    }

    public void ExitGame() {
        ResumeGame();
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    private void HandleKeyboardInputs() {
        //Open or close the character panel
        if(Input.GetKeyDown(KeyCode.I)){
            OpenCloseCharacterPanel();
        }
        //Open crafting panel
        if(Input.GetKeyDown(KeyCode.C)) {
            characterPanel.SetActive(true);
            OpenCraftingPanel();
        }
        //Interact
        if(Input.GetKeyDown(KeyCode.E)) {
            if(selectedContainer!=null) {
                textPanel.SetActive(false);
                scavengingPanel.SetActive(true);
            }
            
        }
    }

    private void OpenCloseCharacterPanel() {
        characterPanel.SetActive(!characterPanel.activeSelf);
    }

    public void PauseResumeGame() {
        if(_gameIsPaused == false) {
            PauseGame();
        } else {
            ResumeGame();
        }
    }

    private void PauseGame() {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        pausePanel.SetActive(true);
        _gameIsPaused = true;
    }

    private void ResumeGame() {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        pausePanel.SetActive(false);
        _gameIsPaused = false;
    }

    private void HandleZoom() {
        //Keyboard zoom control
        if(Input.GetKey(KeyCode.KeypadPlus)) { //Zoom in
            zoom -= zoomSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.KeypadMinus)) { //Zoom out
            zoom += zoomSpeed * Time.deltaTime;
        };
        //Mouse wheel zoom control
        if(Input.mouseScrollDelta.y > 0) {
            zoom -= zoomSpeed * Time.deltaTime * zoomWheelSensibility;
        }
        if(Input.mouseScrollDelta.y < 0) {
            zoom += zoomSpeed * Time.deltaTime * zoomWheelSensibility;
        }
        //Zoom limits
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
    }

    public void CloseInventoryPanel() {
        characterPanel.SetActive(false);
    }

    public void CloseCraftingPanel() {
        craftingPanel.SetActive(false);
    }

    public void CloseScavengingPanel() {
        scavengingPanel.GetComponent<ScavengingInventory>()?.StopScavenge();
        scavengingPanel.SetActive(false);
    }

    public void OpenCraftingPanel() {
        craftingPanel.SetActive(true);
    }

}
