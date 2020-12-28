using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{
    //Camera
    private CameraBehavior cameraBehavior;
    
    //Player
    private Transform _playerTransform;
    public Transform playerTransform { get => _playerTransform;}
    
    //Selected Container
    private static GameObject selectedContainer;

    //Panels
    private GameObject characterPanel;
    private EquipmentPanel equipmentPanel;
    private SkillsPanel skillsPanel;
    private StatsPanel statsPanel;
    private GameObject craftingPanel;
    private GameObject craftingBookPanel;
    private GameObject scavengingPanel;
    private GameObject waterFillerPanel;
    private GameObject fireSourcePanel;
    public GameObject textPanel;
    public WorldObjectTooltip worldObjectTooltip;
    private GameObject skillSelectionPanel;
    private GameObject pausePanel;
    private GameObject gameOverPanel;
    private MessagePanel messagePanel;
    private ProgressPanel progressPanel;
    private GameObject sleepingPanel;

    //Weapon
    [SerializeField] private EquipmentSlot weaponSlot; 

    //Batteries Slot and lantern
    [SerializeField] private EquipmentSlot batterySlot;
    private Lantern lantern;

    //Player Level and experience (UI)
    [SerializeField] private ExperienceProgressBar experienceProgressBar;
    private LevelSystem _levelSystem;
    public LevelSystem levelSystem { get => _levelSystem;}

    //Zoom
    private float zoom=100f; //Zoom level
    private float zoomSpeed = 100f; //Zoom in and out speed
    private float minZoom = 70f; //More close zoom
    private float maxZoom = 100f; //More far zoom
    private float zoomWheelSensibility = 5f;
    
    //Pause control
    private static bool _gameIsPaused;
    public static bool gameIsPaused {
        get => _gameIsPaused;
    }

    //Sleeping
    private bool sleeping = false;
    private int sleepingMinutes = 0;

    //Save Load Game Handler
    private SaveLoadGame saveLoadGame;

    private void Awake() {
        //Camera
        cameraBehavior = Camera.main.GetComponent<CameraBehavior>();
        //Player transform and lantern
        _playerTransform = GameObject.Find("Player").transform;
        if(_playerTransform!=null) lantern = _playerTransform.GetComponent<Lantern>();
        if(lantern!=null) lantern.SetBatterySlot(batterySlot);
        //CharacterPanel
        characterPanel = GameObject.Find("CharacterPanel");
        //EquipmentPanel
        equipmentPanel = GameObject.Find("EquipmentPanel")?.GetComponent<EquipmentPanel>();
        //Skills Panel
        skillsPanel = GameObject.Find("SkillsPanel")?.GetComponent<SkillsPanel>();
        //Stats Panel
        statsPanel = GameObject.Find("StatsPanel")?.GetComponent<StatsPanel>();
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
        //Skill Selection Panel
        skillSelectionPanel = GameObject.Find("SkillSelectionPanel");
        //Pause Panel
        pausePanel = GameObject.Find("PausePanel");
        //Game Over Panel
        gameOverPanel = GameObject.Find("GameOverPanel");
        //Message Panel
        messagePanel = GameObject.Find("MessagePanel")?.GetComponent<MessagePanel>();
        //Progress Panel
        progressPanel = GameObject.Find("ProgressPanel")?.GetComponent<ProgressPanel>();
        //Sleeping Panel
        sleepingPanel = GameObject.Find("SleepingPanel");
        //Save Load Game
        saveLoadGame = GameObject.Find("SaveLoadGameHandler")?.GetComponent<SaveLoadGame>();
        //Level System
        CreateLevelSystem();
        //Time Tick System
        TimeTickSystem.Create();
        //Unselect any container
        selectedContainer = null; 

    }

    void Start()
    {
        GetComponent<WeatherHandler>().SetWeather(WeatherHandler.WeatherType.None); //No weather effects
        SuscribeToPlayerEvents();
        cameraBehavior.setFocus(() => _playerTransform.position); //Camera will follow the player
        cameraBehavior.setZoom(() => zoom); //Set the zoom to its default value
        
        //Deactivate Panels
        CloseInventoryPanel();
        CloseCraftingPanel();
        CloseCraftingBookPanel();
        CloseScavengingPanel();
        CloseWaterFillerPanel();
        CloseFireSourcePanel();
        CloseSkillSelectionPanel();
        CloseSleepingPanel();
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        messagePanel.gameObject.SetActive(false);
        progressPanel.gameObject.SetActive(false);
        
        ResumeGame(); //Resume Game (needed in case we are createig a new game)

        if(PersistentData.instance.newGame) {
            //if it's a new game, activate the auto refresh funcionality of the map to create the first island
            GameObject.Find("Map")?.GetComponent<MapWithoutSectorsHandler>()?.ActivateRefresh();
            //New games start in fog weather
            WeatherHandler.Instance.SetWeather(WeatherHandler.WeatherType.Fog);
        }
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
        //Sleeping management
        SleepControl();
    }

    public static bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void SuscribeToPlayerEvents() {
        Character character = _playerTransform.gameObject.GetComponent<Character>();
        if(character!=null) {
            character.OnHealthZero += Character_OnHealthZero;
            character.OnHealthChange += Character_OnHealthChange;
            character.OnHungryChange += Character_OnHungryChange;
            character.OnThirstChange += Character_OnThirstChange;
            character.OnVigorChange += Character_OnVigorChange;
        }
        _playerTransform.gameObject.GetComponent<PlayerAttack>().OnAttackHitSomething += Player_OnAttackHitSomething;
    }

    private void Character_OnHealthChange(object sender, Character.AmountEventArgs e) {
        Vector3 pos = new Vector3(_playerTransform.position.x + 3, _playerTransform.position.y + 3, 0);
        if(e.amount == 1 || e.amount == -1) return;
        string sign = e.amount >= 0 ? "+" : "";
        TextPopup.Create(pos, sign + e.amount.ToString() + " Health", new Color(1,1,1,1));
    }

    private void Character_OnHungryChange(object sender, Character.AmountEventArgs e) {
        Vector3 pos = new Vector3(_playerTransform.position.x + 6, _playerTransform.position.y, 0);
        if(e.amount == 1 || e.amount == -1) return;
        string sign = e.amount >= 0 ? "+" : "";
        TextPopup.Create(pos, sign + e.amount.ToString() + " Food", new Color(1,1,1,1));
    }

    private void Character_OnThirstChange(object sender, Character.AmountEventArgs e) {
        Vector3 pos = new Vector3(_playerTransform.position.x - 3, _playerTransform.position.y + 3, 0);
        if(e.amount == 1 || e.amount == -1) return;
        string sign = e.amount >= 0 ? "+" : "";
        TextPopup.Create(pos, sign + e.amount.ToString() + " Water", new Color(1,1,1,1));
    }

    private void Character_OnVigorChange(object sender, Character.AmountEventArgs e) {
        Vector3 pos = new Vector3(_playerTransform.position.x - 6, _playerTransform.position.y, 0);
        if(e.amount == 1 || e.amount == -1) return;
        string sign = e.amount >= 0 ? "+" : "";
        TextPopup.Create(pos, sign + e.amount.ToString() + " Vigor", new Color(1,1,1,1));
    }

    private void Player_OnAttackHitSomething(object sender, PlayerAttack.AmountEventArgs e)
    {
        weaponSlot.amount -= e.amount; //reduce the usage of the weapon
    }

    public EquippableItem GetWeapon() {
        return weaponSlot.item!=null? weaponSlot.item as EquippableItem : null;
    }

    public List<EquippableItem> GetEquippedItems() {
        return equipmentPanel.GetEquippedItems();
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
        if(container==null) return;
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

    public static Sprite GetSprite(GameObject container) {
        if(container==null) return null;
        if(selectedContainer.layer == LayerMask.NameToLayer("Containers")) {
            return selectedContainer.GetComponent<Container>().GetSprite();
        } else if(selectedContainer.layer == LayerMask.NameToLayer("WaterFillers")) {
            return selectedContainer.GetComponent<WaterResource>().GetSprite();
        } else if(selectedContainer.layer == LayerMask.NameToLayer("FireSources")) {
            return selectedContainer.GetComponent<FireSource>().GetSprite();
        } else return null;
    }

    private void HandlePausedKeyboardInputs() {
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
        //Unset container to force containere slots to be saved into its container inventory
        UnSetContainer(selectedContainer);
        //SaveGame
        saveLoadGame.Save();
        //Disable panel and show message
        messagePanel.gameObject.SetActive(true);
        messagePanel.ShowPanel("Game Saved.", MessagePanel.MessageIcon.Celebration, SoundManager.Sound.ItemFound, 0.5f, 0.1f);
    }

    public void LoadGame() {
        //Load Game
        saveLoadGame.Load();
        //Disable panel and resume game (the loaded game)
        ResumeGame();
        pausePanel.SetActive(false);
    }

    public void ExitGame() {
        ResumeGame();
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    private void HandleKeyboardInputs() {
        //Open or close the character/inventory panel
        if(Input.GetKeyDown(KeyCode.I)){
            OpenCloseCharacterPanel();
        }
        //Open crafting panel
        if(Input.GetKeyDown(KeyCode.C)) {
            characterPanel.SetActive(true);
            OpenCraftingPanel();
        }
        //Switch on or switc off the player lantern light
        if(Input.GetKeyDown(KeyCode.Space)) {
            SwitchOnOffPlayerLight();
        }
        //Interact
        if(Input.GetKeyDown(KeyCode.E)) {
            if(selectedContainer!=null) { //Open the interactive panel (it depends of the type of object which the player is interacting)
                textPanel.SetActive(false);
                if(selectedContainer.gameObject.layer == LayerMask.NameToLayer("Containers")) {
                    if(scavengingPanel.activeSelf) CloseScavengingPanel();
                    else OpenScavengingPanel();
                }
                if(selectedContainer.gameObject.layer == LayerMask.NameToLayer("WaterFillers")) {
                    if(waterFillerPanel.activeSelf) CloseWaterFillerPanel();
                    else OpenWaterFillerPanel();
                }
                if(selectedContainer.gameObject.layer == LayerMask.NameToLayer("FireSources")) {
                    if(fireSourcePanel.activeSelf) CloseFireSourcePanel();
                    else OpenFireSourcePanel();
                }
            }
        }
        //Sleep
        if(Input.GetKeyDown(KeyCode.Z)) {
            Sleep();
        }
    }

    public void OpenCloseCharacterPanel() {
        characterPanel.SetActive(!characterPanel.activeSelf);
    }

    public void PauseResumeGame() {
        if(_gameIsPaused == false) {
            PauseGame();
            pausePanel.SetActive(true);
        } else {
            ResumeGame();
            pausePanel.SetActive(false);
        }
    }

    private void PauseGame() {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        _gameIsPaused = true;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        AudioListener.pause = false;
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

    public void OpenSkillSelectionPanel() {
        if(skillSelectionPanel!=null) {
            skillSelectionPanel.SetActive(true);
        }
    }

    public void CloseSkillSelectionPanel() {
        if(skillSelectionPanel!=null) {
            skillSelectionPanel.SetActive(false);
            skillsPanel.RefreshSkillsList();
            statsPanel.RefreshStats();
        }
    }

    private void OpenSleepingPanel() {
        sleepingPanel.SetActive(true);
    }

    private void CloseSleepingPanel() {
        sleepingPanel.SetActive(false);
    }

    public void Sleep() {
        Character character = playerTransform.GetComponent<Character>();
        if(character!=null && character.vigor < character.maxVigor) {
            sleeping = true;
            sleepingMinutes = 0;
            GetComponent<GameDateTimeHandler>().OnTimeChange += SleepControl; //suscribe to ingame time events
            Time.timeScale = 10f;
            OpenSleepingPanel();
        }
    }

    public void WakeUp() {
        sleeping = false;
        GetComponent<GameDateTimeHandler>().OnTimeChange -= SleepControl; //unsuscribe to ingame time events
        Time.timeScale = 1f;
        CloseSleepingPanel();
    }

    public void SleepControl() {
        if(sleeping) {
            sleepingMinutes++;
            if(sleepingMinutes>=15) { //in 7 hours of ingame time it refills 100 vigor points (100%)
            sleepingMinutes = 0;
            Character character = playerTransform.GetComponent<Character>();
            if(character!=null) {
                character.ModifyVigor(1);
                if(character.vigor == character.maxVigor) {
                    WakeUp();
                }
            }
            }
        }
    }

    private void CreateLevelSystem() {
        _levelSystem = new LevelSystem(); //Create level system
        experienceProgressBar.SetLevelSystem(_levelSystem); //Set level system to the experience progress bar
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged; //suscribe to event
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged; //suscribe to event
    }

    //Experience changed
    private void LevelSystem_OnExperienceChanged(object sender, LevelSystem.AmountEventArgs e)
    {
        XPPopup.Create(playerTransform.position, e.amount);
    }

    //Level changed
    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        PauseGame();
        OpenSkillSelectionPanel();
        skillSelectionPanel?.GetComponent<SkillSelectionPanel>()?.SetRandomSkills();
    }

    public void ShowMessage(string textToShow, MessagePanel.MessageIcon messageIcon = MessagePanel.MessageIcon.None, SoundManager.Sound messageSound = SoundManager.Sound.ItemFound, float showTime = 1.5f, float fadeTime = 2f) {
        messagePanel.gameObject.SetActive(true);
        messagePanel.ShowPanel(textToShow, messageIcon, messageSound, showTime, fadeTime);
    }

    public void SwitchOnOffPlayerLight() {
        if(lantern!=null) {
            lantern.SwitchOnOffLantern();
            lantern.ForceUIUpdate();
        }
    }
}
