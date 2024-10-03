using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
#region Variables

    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get => _instance;
    }

    public enum GameStates
    {
        // Reset Beats
        MAIN_MENU,

        // Story Beats
        INTRO_SEQUENCE,
        MAIN_DECK_START,
        MAIN_DECK_INTERCOM_RINGING,
        MAIN_DECK_INTERCOM_ANSWERED,
        MAIN_DECK_KEYCARD_COLLECTED,
        AIRLOCK_INTERCOM_RINGING,    
        AIRLOCK_INTERCOM_ANSWERED,  
        AIRLOCK_JETTISON_COMPLETE,
        CREW_QUARTERS_INTERCOM_RINGING,
        CREW_QUARTERS_INTERCOM_ANSWERED,
        CREW_QUARTERS_CHASE,
        CREW_QUARTERS_KEY_OBTAINED,
        STORAGE_ROOM_ENTERED,
        STORAGE_ROOM_CHASE,
        STORAGE_ROOM_ACCESS_CARD_OBTAINED,
        ENGINE_ROOM_ENTERED,

        // Endings
        ENGINE_ROOM_PORTAL_OPENED,
        ENGINE_ROOM_WINDOW_SHOT,

        GAME_COMPLETE
    }

    public GameStates lastState;
    public GameStates currentState;

    public enum Locations
    {
        START,
        MAIN_DECK,
        AIRLOCK,
        CREW_QUARTERS,
        STORAGE_ROOM,
        ENGINE_ROOM,
        END
    }

    public Locations currentLocation;

    bool gamePaused;
    public bool GamePaused
    {
        get => gamePaused;
        set => gamePaused = value;
    }

    [SerializeField]
    public AudioManager audioManager;

    [SerializeField]
    List<GameObject> playerObjects;
    [SerializeField]
    List<Behaviour> playerBehaviours;


    [SerializeField]
    public GameObject KeycardPrefab;
    [SerializeField]
    Transform MDKeycardLocation;


    public bool keyFound;
    public bool gunFound;

    public bool crewQuartersDialogueComplete;

    // Engine Room Buttons
    public bool Button1Pushed;
    public bool Button2Pushed;

    // Engine Room Window
    public bool WindowShot;

    public List<Transform> savePoints;
    Transform lastSavePoint;

    [SerializeField]
    FirstPersonController playerMovement;

    public FirstPersonController Player
    {
        get => playerMovement;
    }

    public GameObject PlayerArm;
    public Transform GunshotPoint;

    [SerializeField]
    int health = 3;

    public int PlayerHealth
    {
        get => health;
        set => health = value;
    }

    [SerializeField]
    GameObject AirlockBody;
    int jettisonTimer = 5;

    [SerializeField]
    public GameObject AirlockButton;

    [SerializeField]
    AudioClip doorBanging;

    [SerializeField]
    GameObject minion;
    [SerializeField]
    GameObject minionSpawnCQ;
    [SerializeField]
    GameObject minionSpawnSR;

    [SerializeField]
    GameObject entity;

    [SerializeField]
    Transform portalLocation;
    [SerializeField]
    GameObject portal;

    [SerializeField]
    GameObject engineRoomButton1;
    [SerializeField]
    GameObject engineRoomButton2;
    [SerializeField]
    GameObject engineRoomWindow;
    [SerializeField]
    Mesh brokenWindowMesh;

    [Header("Doors")]
    public DoorScript MainDeckDoor;
    public DoorScript AirlockDoor;
    public DoorScript ExternalAirlockDoor;
    public DoorScript CrewQuartersDoor;
    public DoorScript StorageDoor;

    [Header("Intercoms")]
    public IntercomScript MainDeckIntercom;
    public IntercomScript AirlockIntercom;
    public IntercomScript CrewQuartersIntercom;
    public IntercomScript EngineRoomIntercom;

    public UIManager uiManager;
    CrewMemberRandomizer crewMemberRandomizer;

    #endregion

    #region Helper Functions

    private void Awake()
    {
        if(_instance != null & _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        HandleGameStart();
    }

    private void Update()
    {
        HandleEventListeners();
    }

    public void EnterNextState()
    {
        // Save the last game state
        lastState = currentState;
        ++currentState;
        // Run next set of events
        StateMachine();
    }

    public void EnterState(GameStates state)
    {
        lastState = currentState;
        currentState = state;
        StateMachine();
    }

    void NextRespawnPoint()
    {
        if(savePoints.Count < 1)
        {
            Debug.Log("Save Points Not Initialized, Exiting NextRespawnPoint().");
            return;
        }

        // Save the next respawn point
        int index = savePoints.IndexOf(lastSavePoint);
        lastSavePoint = savePoints[++index];
    }

    // Plays the next set of events based on current game state
    void StateMachine()
    {
        switch(currentState)
        {
            case GameStates.MAIN_DECK_START:
                HandleMainDeckStart();
                break;
            case GameStates.INTRO_SEQUENCE:
                HandleIntroSequence();
                break;

            case GameStates.MAIN_DECK_INTERCOM_RINGING:
                HandleMainDeckIntercomRinging();
                break;

            case GameStates.MAIN_DECK_INTERCOM_ANSWERED:
                HandleMainDeckIntercomAnswered();
                break;

            case GameStates.MAIN_DECK_KEYCARD_COLLECTED:
                HandleMainDeckKeyCardCollected();
                break;

            case GameStates.AIRLOCK_INTERCOM_RINGING:
                HandleAirlockIntercomRinging();
                break;

            case GameStates.AIRLOCK_INTERCOM_ANSWERED:
                HandleAirlockIntercomAnswered();
                break;

            case GameStates.AIRLOCK_JETTISON_COMPLETE:
                HandleAirlockJettisonComplete();
                break;

            case GameStates.CREW_QUARTERS_INTERCOM_RINGING:
                HandleCrewQuartersIntercomRinging();
                break;

            case GameStates.CREW_QUARTERS_INTERCOM_ANSWERED:
                HandleCrewQuartersIntercomAnswered();
                break;

            case GameStates.CREW_QUARTERS_CHASE:
                HandleCrewQuartersChase();
                break;

            case GameStates.CREW_QUARTERS_KEY_OBTAINED:
                HandleCrewQuartersKeyObtained();
                break;

            case GameStates.STORAGE_ROOM_ENTERED:
                HandleStorageRoomEntered();
                break;

            case GameStates.STORAGE_ROOM_CHASE:
                HandleStorageRoomChase();
                break;

            case GameStates.STORAGE_ROOM_ACCESS_CARD_OBTAINED:
                HandleStorageRoomAccessCardObtained();
                break;

            case GameStates.ENGINE_ROOM_ENTERED:
                HandleEngineRoomEntered();
                break;

            case GameStates.GAME_COMPLETE:
                HandleGameComplete();
                break;

            case GameStates.ENGINE_ROOM_PORTAL_OPENED:
                HandleEngineRoomPortalOpened();
                break;

            case GameStates.ENGINE_ROOM_WINDOW_SHOT:
                HandleEngineRoomWindowShot();
                break;

            default:
                Debug.Log("Invalid State");
                break;                
        }
    }

    string ConvertLocation()
    {
        switch(currentLocation)
        {
            case Locations.MAIN_DECK:
                return "Main Deck";
            case Locations.AIRLOCK:
                return "Airlock";
            case Locations.CREW_QUARTERS:
                return "Crew Quarters";
            case Locations.STORAGE_ROOM:
                return "Storage Room";
            case Locations.ENGINE_ROOM:
                return "Engine Room";

            default:
                return "";
        }
    }

    public void UpdateLocationUI()
    {
        uiManager.UpdateLocationText(ConvertLocation());
    }

    public void TakeDamage()
    {
        --health;
        CheckDeath();
    }

    void CheckDeath()
    {
        if (health <= 0)
        {
            Debug.Log("Player is Dead");
            ResetGame();
        }
    }

    void ResetGame()
    {
        // Reset Player
        currentState = GameStates.MAIN_DECK_START;
        lastState = GameStates.MAIN_DECK_START;
        if (savePoints.Count > 0)
        {
            lastSavePoint = savePoints[0];
        }

        playerMovement.gameObject.transform.position = lastSavePoint.position;
    }

#endregion

    #region Handlers

    private void HandleGameStart()
    {
        Debug.Log("Game Start");

        // Game State
        currentState = GameStates.MAIN_MENU;
        lastState = GameStates.MAIN_MENU;

        // UI & Player
        uiManager.StartMainMenuUI();
        foreach (GameObject go in playerObjects)
        {
            go.SetActive(false);
        }
        foreach (Behaviour be in playerBehaviours)
        {
            be.enabled = false;
        }

        // Crew Randomizer
        crewMemberRandomizer = GetComponent<CrewMemberRandomizer>();

        // Enemy
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            go.SetActive(false);
        }

        // Save States
        if (savePoints.Count < 1)
        {
            return;
        }
        lastSavePoint = savePoints[0];

        Player.gameObject.transform.position = lastSavePoint.position;
    }

    private void HandleEventListeners()
    {
        // Event Listeners
        if (keyFound &&
            currentLocation == Locations.MAIN_DECK)
        {
            EnterNextState();
        }
        else
        if (currentLocation == Locations.AIRLOCK &&
            currentState == GameStates.MAIN_DECK_KEYCARD_COLLECTED)
        {
            EnterNextState();
        }
        else
        if (currentLocation == Locations.CREW_QUARTERS &&
            currentState == GameStates.AIRLOCK_JETTISON_COMPLETE)
        {
            EnterNextState();
        }
        else
        if (currentLocation == Locations.CREW_QUARTERS &&
            currentState == GameStates.CREW_QUARTERS_INTERCOM_ANSWERED &&
            crewQuartersDialogueComplete)
        {
            EnterNextState();
        }
        else
        if (currentLocation == Locations.CREW_QUARTERS &&
            currentState == GameStates.CREW_QUARTERS_CHASE &&
            keyFound)
        {
            EnterNextState();
        }
        else
        if (currentLocation == Locations.STORAGE_ROOM &&
            currentState == GameStates.CREW_QUARTERS_KEY_OBTAINED)
        {
            EnterNextState();
        }
        else
        if (currentLocation == Locations.STORAGE_ROOM &&
            currentState == GameStates.STORAGE_ROOM_ENTERED
            && gunFound)
        {
            EnterNextState();
        }
        else
        if (currentLocation == Locations.STORAGE_ROOM &&
            currentState == GameStates.STORAGE_ROOM_CHASE
            && keyFound)
        {
            EnterNextState();
        }
        else
        if (currentLocation == Locations.ENGINE_ROOM &&
            currentState == GameStates.STORAGE_ROOM_ACCESS_CARD_OBTAINED)
        {
            EnterNextState();
        }
        else

        // Alternate Endings
        if (currentState == GameStates.ENGINE_ROOM_ENTERED &&
            Button1Pushed && Button2Pushed)
        {
            EnterState(GameStates.ENGINE_ROOM_PORTAL_OPENED);
        }
        if (currentState == GameStates.ENGINE_ROOM_ENTERED &&
            WindowShot)
        {
            EnterState(GameStates.ENGINE_ROOM_WINDOW_SHOT);
        }
    }

    private void HandleIntroSequence()
    {
        Debug.Log("Intro Sequence State Triggered.");

        uiManager.StartIntroSequence();
    }

    private void HandleMainDeckStart()
    {
        Debug.Log("Main Deck Start State Triggered.");

        // Initialize game
        if (savePoints.Count > 0)
        {
            lastSavePoint = savePoints[0];
        }

        // Enable Player
        foreach (GameObject go in playerObjects)
        {
            go.SetActive(true);
        }
        foreach (Behaviour be in playerBehaviours)
        {
            be.enabled = true;
        }

        // Initialize Crew Members
        crewMemberRandomizer.enabled = true;

        // Update Gameplay UI
        uiManager.StartGameUI();
        UpdateLocationUI();

        // Start First Task
        Player.gameObject.GetComponentInChildren<PlayerInteraction>().PlayMainDeckDialogue();

        audioManager.PlayMainDeckSceneIntro();
    }

    private void HandleMainDeckIntercomRinging()
    {  
        Debug.Log("Main Deck Intercom Ringing.");

        // Start First Task
        MainDeckIntercom.Ringing = true;

        MainDeckIntercom.GetComponentInChildren<ItemGlow>().SetActive();
    }

    private void HandleMainDeckIntercomAnswered()
    {
        Debug.Log("Main Deck Intercom Answered.");

        // Start First Task
        Player.GetComponentInChildren<PlayerInteraction>().StopMainDeckDialogue();
        MainDeckIntercom.Answer();

        MainDeckIntercom.GetComponentInChildren<ItemGlow>().SetInactive();
    }

    public void SpawnKeycard()
    {
        GameObject keycard = Instantiate(KeycardPrefab);
        keycard.transform.localPosition = MDKeycardLocation.position;
    }

    private void HandleMainDeckKeyCardCollected()
    {
        Debug.Log("Main Deck Keycard Collected.");

        // Start First Task
        MainDeckDoor.Unlock();
        MainDeckDoor.NeedsKey = false;
        keyFound = false;
    }

    private void HandleAirlockIntercomRinging()
    {
        Debug.Log("Airlock Intercom Ringing.");

        // Start First Task
        AirlockIntercom.Ringing = true;

        // Start Second Task
        MainDeckDoor.Lock();

        // Start Third Task
        Player.gameObject.GetComponentInChildren<PlayerInteraction>().PlayAirlockDialogue();

        uiManager.GetInventoryPanelController().RemoveItem("Key");

        AirlockIntercom.GetComponentInChildren<ItemGlow>().SetActive();
    }

    private void HandleAirlockIntercomAnswered()
    {
        Debug.Log("Airlock Intercom Answered.");

        // Start First Task
        //Player.GetComponentInChildren<PlayerInteraction>().StopAirlockDialogue();
        AirlockIntercom.Answer();

        // Start Second Task
        NextRespawnPoint();

        AirlockIntercom.GetComponent<ItemGlow>().SetInactive();
    }

    private void HandleAirlockJettisonComplete()
    {
        Debug.Log("Airlock Jettison Complete.");

        // Start First Task
        StartCoroutine(HandleAirlockJettisonSequence());
    }

    IEnumerator HandleAirlockJettisonSequence()
    {
        ExternalAirlockDoor.Open = true;
        ExternalAirlockDoor.GetComponent<Animator>().SetTrigger("OpenDoor");
        AirlockBody.AddComponent<Rigidbody>();
        AirlockBody.GetComponent<Rigidbody>().useGravity = false;
        AirlockBody.GetComponent<Rigidbody>().AddForce(Vector3.left * 15, ForceMode.Impulse);

        yield return new WaitForSeconds(jettisonTimer);

        Destroy(AirlockBody, jettisonTimer);

        // Start Second Task
        ExternalAirlockDoor.Open = false;
        ExternalAirlockDoor.GetComponent<Animator>().SetTrigger("CloseDoor");
        AirlockDoor.Unlock();
    }

    private void HandleCrewQuartersIntercomRinging()
    {
        Debug.Log("Crew Quarters Intercom is Ringing.");

        // Start First Task
        CrewQuartersIntercom.Ringing = true;

        Player.gameObject.GetComponentInChildren<PlayerInteraction>().PlayCrewQuartersDialogue();

        // Start Second Task
        AirlockDoor.Lock();

        CrewQuartersIntercom.GetComponentInChildren<ItemGlow>().SetActive();
    }

    private void HandleCrewQuartersIntercomAnswered()
    {
        Debug.Log("Crew Quarters Intercom Answered.");

        // Start First Task
        HandleFreezePlayerMovement();

        // Start Second Task
        CrewQuartersIntercom.Answer();

        // Start Third Task
        NextRespawnPoint();

        CrewQuartersIntercom.GetComponentInChildren<ItemGlow>().SetInactive();
    }

    private void HandleFreezePlayerMovement()
    {
        Player.enabled = false;
    }

    private void HandleCrewQuartersChase()
    {
        Debug.Log("Crew Quarters Chase Begins.");

        // Start First Task
        HandleUnfreezePlayerMovement();

        //Start Second Task
        Instantiate(minion, minionSpawnCQ.transform.position, Quaternion.identity);

        audioManager.PlayChaseMusic();
        audioManager.StopAmbientMusic();
    }

    private void HandleUnfreezePlayerMovement()
    {
        Player.enabled = true;
    }

    private void HandleCrewQuartersKeyObtained()
    {
        Debug.Log("Crew Quarters Key Obtained.");

        // Start First Task
        CrewQuartersDoor.Unlock();
    }
    
    private void HandleStorageRoomEntered()
    {
        Debug.Log("Storage Room Entered.");

        // Start First Task
        Destroy(GameObject.FindGameObjectWithTag("Enemy"));

        // Start Second Task
        CrewQuartersDoor.gameObject.AddComponent<AudioSource>();
        //CrewQuartersDoor.gameObject.GetComponent<AudioSource>().clip = doorBanging;
        //CrewQuartersDoor.gameObject.GetComponent<AudioSource>().Play();

        uiManager.GetInventoryPanelController().RemoveItem("Key");

        Player.gameObject.GetComponentInChildren<PlayerInteraction>().PlayStorageRoomDialogue();

        audioManager.PlayAmbientMusic();
        audioManager.PlayDoorCrash();
        audioManager.StopChaseMusic();
    }
    
    private void HandleStorageRoomChase()
    {
        Debug.Log("Storage Room Chase.");

        // Start First Task
        Instantiate(minion, minionSpawnSR.transform);

        audioManager.PlayChaseMusic();
        audioManager.StopDoorCrash();
        audioManager.StopAmbientMusic();
    }
    
    private void HandleStorageRoomAccessCardObtained()
    {
        Debug.Log("Storage Room Access Card Obtained.");

        // Start First Task
        StorageDoor.Unlock();

        audioManager.PlayAmbientMusic();
        audioManager.StopChaseMusic();
    }

    private void HandleEngineRoomEntered()
    {
        Debug.Log("Engine Room Entered.");

        // Start First Task
        GameObject newPortal = Instantiate(portal, portalLocation.position, Quaternion.identity);
        Instantiate(entity, newPortal.transform.position, Quaternion.identity);

        // Start Second Task
        EngineRoomIntercom.Answer();
        keyFound = false;

        uiManager.GetInventoryPanelController().RemoveItem("Key");

        // Start Third Task
        NextRespawnPoint();

        engineRoomWindow.GetComponentInChildren<ItemGlow>().SetActive();
        engineRoomButton1.GetComponentInChildren<ItemGlow>().SetActive();
        engineRoomButton2.GetComponentInChildren<ItemGlow>().SetActive();

        Player.gameObject.GetComponentInChildren<PlayerInteraction>().PlayEngineRoomDialogue();

        audioManager.PlayChaseMusic();
        audioManager.StopAmbientMusic();
    }

    private void HandleEngineRoomPortalOpened()
    {
        Debug.Log("Engine Room Portal Opened.");

        // Start First Task
        Instantiate(entity, portal.transform);
        Instantiate(entity, portal.transform);

        // End Game
        EnterNextState();
        EnterState(GameStates.GAME_COMPLETE);
    }
    
    private void HandleEngineRoomWindowShot()
    {
        Debug.Log("Engine Room Window Shot.");

        // Start First Task
        engineRoomWindow.GetComponentInChildren<MeshFilter>().mesh = brokenWindowMesh;
        engineRoomWindow.GetComponentInChildren<MeshCollider>().enabled = false;

        // Start Second Task

        // Calculate Directions to Window
        Vector3 directionPlayerToWindow = engineRoomWindow.transform.position - Player.gameObject.transform.position;
        Vector3 directionEntityToWindow = engineRoomWindow.transform.position - GameObject.FindGameObjectWithTag("Enemy").transform.position;

        // Throw Player and Entity out the window
        Player.gameObject.GetComponent<Rigidbody>().AddForce(directionPlayerToWindow, ForceMode.Force);
        GameObject.FindGameObjectWithTag("Entity").GetComponent<Rigidbody>().AddForce(directionEntityToWindow, ForceMode.Force);

        // End Game
        EnterState(GameStates.GAME_COMPLETE);
    }

    private void HandleGameComplete()
    {
        Debug.Log("Game Complete!");

        if(lastState == GameStates.ENGINE_ROOM_WINDOW_SHOT)
        {
            uiManager.StartNeutralEndingSequence();
        }
        else

        if (lastState == GameStates.ENGINE_ROOM_PORTAL_OPENED)
        {
            uiManager.StartBadEndingSequence();
        }

        StartCoroutine(HandleReturnToMainMenuCredits());
    }

    IEnumerator HandleReturnToMainMenuCredits()
    {
        yield return new WaitForSeconds(30);

        HandleGameStart();
        uiManager.DisableMainMenuPanel();
        uiManager.EnableCreditsPanel();
        uiManager.EnableCreditsMMButton();
    }

#endregion

}
