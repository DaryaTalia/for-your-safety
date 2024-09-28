using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get => _instance;
    }

    public enum GameStates { 
        // Story Beats
        MAIN_DECK_START,
        MAIN_DECK_INTERCOM_1_RINGING,
        MAIN_DECK_INTERCOM_1_ANSWERED,
        AIRLOCK_INTERCOM_1_RINGING,    
        AIRLOCK_INTERCOM_1_ANSWERED,  
        CREW_QUARTERS_INTERCOM_1_RINGING,
        CREW_QUARTERS_INTERCOM_1_ANSWERED,
        STORAGE_ROOM_INTERCOM_1_RINGING,
        STORAGE_ROOM_INTERCOM_1_ANSWERED,
        ENGINE_ROOM_ENTERED,
        GAME_COMPLETE,

        // Reset Beats
        PLAYER_KILLED    
    }

    public GameStates lastState;
    public GameStates currentState;

    public bool jettisonComplete;
    public bool keyFound;
    public bool gunFound;
    public bool storageRoomMinionKilled;
    public bool engineRoomWindowDestroyed;

    public List<Transform> savePoints;
    Transform lastSavePoint;

    [SerializeField]
    PlayerMovement playerMovement;


    public DoorScript MainDeckDoor;
    public DoorScript AirlockDoor;
    public DoorScript StorageDoor;

    public IntercomScript MainDeckIntercom;
    public IntercomScript AirlockIntercom;
    public IntercomScript CrewQuartersIntercom;
    public IntercomScript StorageIntercom;


    private void Awake()
    {
        _instance = this;

        if(playerMovement == null)
        {
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameStates.MAIN_DECK_START;
        lastState = GameStates.MAIN_DECK_START;
        if(savePoints.Count > 0)
        {
            lastSavePoint = savePoints[0];
        }
    }

    public void EnterNextState()
    {
        // Save the last game state
        lastState = currentState;
        ++currentState;
        // Save the next respawn point
        int index = savePoints.IndexOf(lastSavePoint);
        lastSavePoint = savePoints[++index];
        // Run next set of events
        StateMachine();
    }

    // Plays the next set of events based on current game state
    void StateMachine()
    {
        switch(currentState)
        {
            case GameStates.MAIN_DECK_START:
                HandleMainDeckStart();
                break;

            case GameStates.MAIN_DECK_INTERCOM_1_RINGING:
                HandleMainDeckIntercom1Ringing();
                break;

            case GameStates.MAIN_DECK_INTERCOM_1_ANSWERED:
                HandleMainDeckIntercom1Answered();
                break;

            case GameStates.AIRLOCK_INTERCOM_1_RINGING:
                HandleAirlockIntercom1Ringing();
                break;

            case GameStates.AIRLOCK_INTERCOM_1_ANSWERED:
                HandleAirlockIntercom1Answered();
                break;

            case GameStates.CREW_QUARTERS_INTERCOM_1_RINGING:
                HandleCrewQuartersIntercom1Ringing();
                break;

            case GameStates.CREW_QUARTERS_INTERCOM_1_ANSWERED:
                HandleCrewQuartersIntercom1Answered();
                break;

            case GameStates.STORAGE_ROOM_INTERCOM_1_RINGING:
                HandleStorageRoomIntercom1Ringing();
                break;

            case GameStates.STORAGE_ROOM_INTERCOM_1_ANSWERED:
                HandleStorageRoomIntercom1Answered();
                break;

            case GameStates.ENGINE_ROOM_ENTERED:
                HandleEngineRoomEntered();
                break;

            case GameStates.GAME_COMPLETE:
                HandleGameComplete();
                break;

            case GameStates.PLAYER_KILLED:
                HandlePlayerKilled();
                break;

            default:
                Debug.Log("Invalid State");
                break;                
        }
    }

    private void HandleMainDeckStart()
    {
        // Logic for handling MAIN_DECK_START
        Debug.Log("Main Deck Start state triggered.");
        // Initialize game
    }

    private void HandleMainDeckIntercom1Ringing()
    {
        // Logic for handling MAIN_DECK_INTERCOM_1_RINGING
        Debug.Log("Main Deck Intercom 1 is ringing.");
        MainDeckIntercom.Ring();
    }

    private void HandleMainDeckIntercom1Answered()
    {
        // Logic for handling MAIN_DECK_INTERCOM_1_ANSWERED
        Debug.Log("Main Deck Intercom 1 answered.");
        MainDeckDoor.Unlock();
    }

    private void HandleAirlockIntercom1Ringing()
    {
        // Logic for handling AIRLOCK_INTERCOM_1_RINGING
        Debug.Log("Airlock Intercom 1 is ringing.");
        AirlockIntercom.Ring();
    }

    private void HandleAirlockIntercom1Answered()
    {
        // Logic for handling AIRLOCK_INTERCOM_1_ANSWERED
        Debug.Log("Airlock Intercom 1 answered.");
    }

    public void HandleSuccessfulAirlock()
    {
        jettisonComplete = true;
        AirlockDoor.Unlock();
    }

    private void HandleCrewQuartersIntercom1Ringing()
    {
        // Logic for handling CREW_QUARTERS_INTERCOM_1_RINGING
        Debug.Log("Crew Quarters Intercom 1 is ringing.");
        CrewQuartersIntercom.Ring();
    }

    private void HandleCrewQuartersIntercom1Answered()
    {
        // Logic for handling CREW_QUARTERS_INTERCOM_1_ANSWERED
        Debug.Log("Crew Quarters Intercom 1 answered.");
    }

    private void HandleStorageRoomIntercom1Ringing()
    {
        // Logic for handling STORAGE_ROOM_INTERCOM_1_RINGING
        Debug.Log("Storage Room Intercom 1 is ringing.");
        StorageIntercom.Ring();
    }

    private void HandleStorageRoomIntercom1Answered()
    {
        // Logic for handling STORAGE_ROOM_INTERCOM_1_ANSWERED
        Debug.Log("Storage Room Intercom 1 answered.");
        StorageDoor.Unlock();
    }

    private void HandleEngineRoomEntered()
    {
        // Logic for handling ENGINE_ROOM_ENTERED
        Debug.Log("Engine Room entered.");
    }

    private void HandleGameComplete()
    {
        // Logic for handling GAME_COMPLETE
        Debug.Log("Game Complete!");
    }

    private void HandlePlayerKilled()
    {
        // Logic for handling PLAYER_KILLED
        Debug.Log("Player killed.");
        playerMovement.gameObject.transform.position = lastSavePoint.position;
        currentState = lastState;
        StateMachine();
    }

    private void UseKey()
    {

    }

}
