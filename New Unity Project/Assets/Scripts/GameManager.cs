using System;
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

    public bool keyFound;
    public bool gunFound;

    // Engine Room Buttons
    public bool Button1Pushed;
    public bool Button2Pushed;

    // Engine Room Window
    public bool WindowShot;

    public List<Transform> savePoints;
    Transform lastSavePoint;

    [SerializeField]
    PlayerMovement playerMovement;

    public PlayerMovement Player
    {
        get => playerMovement;
    }

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
    AudioClip doorBanging;

    [SerializeField]
    EnemyBehavior minion;

    [SerializeField]
    EnemyBehavior entity;

    [SerializeField]
    GameObject portal;

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

        if(playerMovement == null)
        {
            playerMovement = PlayerMovement.Instance;
        }
    }

    void Start()
    {
        currentState = GameStates.MAIN_MENU;
        lastState = GameStates.MAIN_MENU;
        Debug.Log("Game Start");
        uiManager.StartMainMenuUI();
        Player.gameObject.SetActive(false);
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            go.SetActive(false);
        }
    }

    private void Update()
    {
        // Event Listeners
        if(keyFound && currentLocation == Locations.MAIN_DECK)
        {
            EnterNextState();
        }
        if( !MainDeckDoor.Open && currentLocation == Locations.AIRLOCK )
        {
            EnterNextState();
        }
        if( !AirlockDoor.Open && currentLocation == Locations.CREW_QUARTERS )
        {
            EnterNextState();
        }
        if (keyFound && currentLocation == Locations.CREW_QUARTERS)
        {
            EnterNextState();
        }
        if ( !CrewQuartersDoor.Open && currentLocation == Locations.STORAGE_ROOM )
        {
            EnterNextState();
        }
        if (keyFound && currentLocation == Locations.STORAGE_ROOM)
        {
            EnterNextState();
        }
        if ( !StorageDoor.Open && currentLocation == Locations.ENGINE_ROOM )
        {
            EnterNextState();
        }

        // Alternate Endings
        if(Button1Pushed && Button2Pushed)
        {
            EnterState(GameStates.ENGINE_ROOM_PORTAL_OPENED);
        }
        if(WindowShot)
        {
            EnterState(GameStates.ENGINE_ROOM_WINDOW_SHOT);
        }
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

    private void HandleMainDeckStart()
    {
        // Logic for handling MAIN_DECK_START
        Debug.Log("Main Deck Start State Triggered.");

        // Initialize game
        if (savePoints.Count > 0)
        {
            lastSavePoint = savePoints[0];
        }

        // Enable Player
        Player.gameObject.SetActive(true);

        // Update Gameplay UI
        uiManager.StartGameUI();
        UpdateLocationUI();

        // Start First Task
        Player.gameObject.GetComponentInChildren<PlayerInteraction>().PlayMainDeckDialogue();
    }

    private void HandleMainDeckIntercomRinging()
    {  
        Debug.Log("Main Deck Intercom Ringing.");

        // Start First Task
        MainDeckIntercom.Ringing = true;
    }

    private void HandleMainDeckIntercomAnswered()
    {
        Debug.Log("Main Deck Intercom Answered.");

        // Start First Task
        Player.GetComponentInChildren<PlayerInteraction>().StopMainDeckDialogue();
        MainDeckIntercom.Answer();

        // Start Second Task
        NextRespawnPoint();
    }

    private void HandleMainDeckKeyCardCollected()
    {
        Debug.Log("Main Deck Keycard Collected.");

        // Start First Task
        Player.GetComponentInChildren<PlayerInteraction>().StopMainDeckDialogue();
        MainDeckDoor.Unlock();
    }

    private void HandleAirlockIntercomRinging()
    {
        Debug.Log("Airlock Intercom Ringing.");

        // Start First Task
        AirlockIntercom.Ringing = true;

        // Start Second Task
        MainDeckDoor.Lock();
    }

    private void HandleAirlockIntercomAnswered()
    {
        Debug.Log("Airlock Intercom Answered.");

        // Start First Task
        //Player.GetComponentInChildren<PlayerInteraction>().StopAirlockDialogue();
        AirlockIntercom.Answer();

        // Start Second Task
        NextRespawnPoint();
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
        AirlockBody.AddComponent<Rigidbody>();
        AirlockBody.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Force);

        yield return new WaitForSeconds(jettisonTimer);

        Destroy(AirlockBody, jettisonTimer);

        // Start Second Task
        ExternalAirlockDoor.Open = false;
        AirlockDoor.Unlock();
    }

    private void HandleCrewQuartersIntercomRinging()
    {
        Debug.Log("Crew Quarters Intercom is Ringing.");

        // Start First Task
        CrewQuartersIntercom.Ringing = true;

        // Start Second Task
        AirlockDoor.Lock();
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
        CrewQuartersDoor.gameObject.GetComponent<AudioSource>().clip = doorBanging;
        CrewQuartersDoor.gameObject.GetComponent<AudioSource>().Play();

        // Start Third Task
        NextRespawnPoint();
    }
    
    private void HandleStorageRoomChase()
    {
        Debug.Log("Storage Room Chase.");

        // Start First Task
        Instantiate(minion, CrewQuartersDoor.gameObject.transform);
    }
    
    private void HandleStorageRoomAccessCardObtained()
    {
        Debug.Log("Storage Room Access Card Obtained.");

        // Start First Task
        StorageDoor.Unlock();
    }

    private void HandleEngineRoomEntered()
    {
        Debug.Log("Engine Room Entered.");

        // Start First Task
        Instantiate(entity, portal.transform);

        // Start Second Task
        EngineRoomIntercom.Answer();

        // Start Third Task
        NextRespawnPoint();
    }

    private void HandleEngineRoomPortalOpened()
    {
        Debug.Log("Engine Room Portal Opened.");

        // Start First Task
        Instantiate(entity, portal.transform);
        Instantiate(entity, portal.transform);

        // End Game
        EnterNextState();
    }
    
    private void HandleEngineRoomWindowShot()
    {
        Debug.Log("Engine Room Window Shot.");

        // Start First Task
        engineRoomWindow.GetComponent<MeshFilter>().mesh = brokenWindowMesh;

        // Start Second Task

        // Calculate Directions to Window
        Vector3 directionPlayerToWindow = engineRoomWindow.transform.position - Player.gameObject.transform.position;
        Vector3 directionEntityToWindow = engineRoomWindow.transform.position - GameObject.FindGameObjectWithTag("EnemyBehavior").transform.position;

        // Throw Player and Entity out the window
        Player.gameObject.GetComponent<Rigidbody>().AddForce(directionPlayerToWindow, ForceMode.Force);
        GameObject.FindGameObjectWithTag("EnemyBehavior").GetComponent<Rigidbody>().AddForce(directionEntityToWindow, ForceMode.Force);

        // End Game
        EnterNextState();
    }

    private void HandleGameComplete()
    {
        Debug.Log("Game Complete!");
    }

#endregion

}
