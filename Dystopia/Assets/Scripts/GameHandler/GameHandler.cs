using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameHandler : MonoBehaviour
{
    //Camera
    private CameraBehavior cameraBehavior;
    
    //Player
    private Transform playerTransform;
    
    //Container
    private static GameObject selectedContainer;

    //Panels
    private GameObject characterPanel;
    private GameObject craftingPanel;
    private GameObject craftingBookPanel;
    private GameObject scavengingPanel;
    private GameObject waterFillerPanel;
    private GameObject fireSourcePanel;
    public GameObject textPanel;
    public WorldObjectTooltip worldObjectTooltip;
    private GameObject pausePanel;
    private GameObject gameOverPanel;

    //Weapon
    [SerializeField] private EquipmentSlot weaponSlot; 

    //Zoom
    private float zoom=100f; //Zoom level
    private float zoomSpeed = 150f; //Zoom in and out speed
    private float minZoom = 40f; //More close zoom
    private float maxZoom = 100f; //More far zoom
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
        //Craftin Book Panel
        craftingBookPanel = GameObject.Find("CraftingBookPanel");
        //Scavenging Panel
        scavengingPanel = GameObject.Find("ScavengingPanel");
        //Water Filler Panel
        waterFillerPanel = GameObject.Find("WaterFillerPanel");
        //Fire Source Panel
        fireSourcePanel = GameObject.Find("FireSourcePanel");
        //PausePanel
        pausePanel = GameObject.Find("PausePanel");
        //GameOverPanel
        gameOverPanel = GameObject.Find("GameOverPanel");
    }
    void Start()
    {
        GetComponent<WeatherHandler>().SetWeather(WeatherHandler.WeatherType.None); //No weather effects
        SuscribeToCharacterEvents();
        TimeTickSystem.Create(); //Create the TimeTickSystem game object
        cameraBehavior.setFocus(() => playerTransform.position); //Camera will follow the player
        cameraBehavior.setZoom(() => zoom); //Set the zoom to its default value
        CloseInventoryPanel();
        CloseCraftingPanel();
        CloseCraftingBookPanel();
        CloseScavengingPanel();
        CloseWaterFillerPanel();
        CloseFireSourcePanel();
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

    public static bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void SuscribeToCharacterEvents() {
        playerTransform.gameObject.GetComponent<Character>().OnHealthZero += Character_OnHealthZero;
    }

    public EquippableItem GetWeapon() {
        return weaponSlot.item!=null? weaponSlot.item as EquippableItem : null;
    }

    private void Character_OnHealthZero(object sender, EventArgs e) {
        Invoke("PlayerDie", 0.5f); //Wait for sound and image effects to end
    }

    private void PlayerDie() {
        PauseGame();
        gameOverPanel.SetActive(true);
    }

    public void SetContainer(GameObject container) {
        if(container.layer == LayerMask.NameToLayer("Containers")) {
            selectedContainer = container;
            scavengingPanel.GetComponent<ScavengingInventory>()?.loadItems(container.GetComponent<Container>());
            textPanel.SetActive(true);
        }
        if(container.layer == LayerMask.NameToLayer("WaterFillers")) {
            selectedContainer = container;
            waterFillerPanel.GetComponent<WaterFillerInventory>()?.loadItems(container.GetComponent<WaterResource>());
            textPanel.SetActive(true);
        }
        if(container.layer == LayerMask.NameToLayer("FireSources")) {
            selectedContainer = container;
            fireSourcePanel.GetComponent<FireSourceInventory>()?.loadItems(container.GetComponent<FireSource>());
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
        if(container.layer == LayerMask.NameToLayer("WaterFillers")) {
            waterFillerPanel.GetComponent<WaterFillerInventory>()?.storeItems(container.GetComponent<WaterResource>());
            selectedContainer = null;
            textPanel.SetActive(false);
            CloseWaterFillerPanel();
        }
        if(container.layer == LayerMask.NameToLayer("FireSources")) {
            fireSourcePanel.GetComponent<FireSourceInventory>()?.storeItems(container.GetComponent<FireSource>());
            selectedContainer = null;
            textPanel.SetActive(false);
            CloseFireSourcePanel();
        }
    }

    public static GameObject GetSelectedContainer() {
        return selectedContainer;
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
                if(selectedContainer.gameObject.layer == LayerMask.NameToLayer("Containers")) {
                    OpenScavengingPanel();
                }
                if(selectedContainer.gameObject.layer == LayerMask.NameToLayer("WaterFillers")) {
                    OpenWaterFillerPanel();
                }
                if(selectedContainer.gameObject.layer == LayerMask.NameToLayer("FireSources")) {
                    OpenFireSourcePanel();
                }
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
        if(craftingBookPanel.activeSelf == false) {
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
    }

    public void CloseInventoryPanel() {
        characterPanel.SetActive(false);
    }

    public void OpenScavengingPanel() {
        scavengingPanel.GetComponent<ScavengingInventory>()?.AutoSetTitle();
        scavengingPanel.SetActive(true);
    }

    public void CloseScavengingPanel() {
        scavengingPanel.GetComponent<ScavengingInventory>()?.StopScavenge();
        scavengingPanel.SetActive(false);
    }

    public void OpenCraftingPanel() {
        craftingPanel.SetActive(true);
    }

    public void CloseCraftingPanel() {
        craftingPanel.SetActive(false);
    }

    public void OpenCraftingBookPanel() {
        craftingBookPanel.SetActive(true);
    }

    public void CloseCraftingBookPanel() {
        craftingBookPanel.SetActive(false);
    }

    public void OpenWaterFillerPanel() {
        waterFillerPanel.GetComponent<WaterFillerInventory>()?.AutoSetTitle();
        waterFillerPanel.SetActive(true);
    }

    public void CloseWaterFillerPanel() {
        waterFillerPanel.SetActive(false);
    }

    public void OpenFireSourcePanel() {
        fireSourcePanel.GetComponent<FireSourceInventory>()?.AutoSetTitle();
        fireSourcePanel.SetActive(true);
    }

    public void CloseFireSourcePanel() {
        fireSourcePanel.SetActive(false);
    }
}
