using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    bool isColliding;
    [SerializeField]
    InteractionController controller;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> MainDeckDialogue;
    [SerializeField]
    string MainDeckObjective;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> AirlockDialogue;
    [SerializeField]
    string AirlockObjective;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> CrewQuartersDialogue;
    [SerializeField]
    string CrewQuartersObjective;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> StorageRoomDialogue;
    [SerializeField]
    string StorageRoomObjective;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> EngineRoomDialogue;
    [SerializeField]
    string EngineRoomObjective;

    int dialogueDelay = 2;

    private void OnTriggerEnter(Collider other)
    {
        // Vents
        if(other.gameObject.CompareTag("Vent"))
        {
            gameObject.GetComponentInParent<FirstPersonController>().inVent = true;
        }

        // Crew
        if (other.gameObject.CompareTag("CrewMember"))
        {
            Colliding(other, "CrewMember");

            StartCoroutine(Reset());
            return;
        }

        // Key
        if (other.gameObject.CompareTag("Key"))
        {
            Colliding(other, "Key");

            StartCoroutine(Reset());
            return;
        }

        // Gun
        if (other.gameObject.CompareTag("Gun"))
        {
            Colliding(other, "Gun");

            StartCoroutine(Reset());
            return;
        }

        // Buttons
        if (other.gameObject.CompareTag("Button"))
        {
            Colliding(other, "Button");

            StartCoroutine(Reset());
            return;
        }

        // Button 1
        if (other.gameObject.CompareTag("EngineRoomButton1"))
        {
            Colliding(other, "EngineRoomButton1");

            StartCoroutine(Reset());
            return;
        }

        // Button 2
        if (other.gameObject.CompareTag("EngineRoomButton2"))
        {
            Colliding(other, "EngineRoomButton2");

            StartCoroutine(Reset());
            return;
        }

        // Intercom
        if (other.gameObject.CompareTag("Intercom"))
        {
            Colliding(other, "Intercom");

            StartCoroutine(Reset());
            return;
        }

        // Door
        if(other.gameObject.CompareTag("Door"))
        {
            if (isColliding) return;
            isColliding = true;

            // Validate State
            if (other.gameObject.GetComponent<DoorScript>().Open)
            {
                Debug.Log("Door is already open.");

                StartCoroutine(Reset());
                return;
            }

            // Validate if Door is locked
            if (other.gameObject.GetComponent<DoorScript>().Locked)
            {
                Debug.Log("Door is locked.");
            }

            // Validate if key is needed
            if (other.gameObject.GetComponent<DoorScript>().Locked && !other.gameObject.GetComponent<DoorScript>().NeedsKey)
            {
                Debug.Log("If locked but no key is needed, player must unlock door through Story Event.");

                StartCoroutine(Reset());
                return;
            }

            // Validate if Key is Needed & if Player has the Key
            if (other.gameObject.GetComponent<DoorScript>().NeedsKey && !GameManager.Instance.keyFound)
            {
                Debug.Log("The Player does not have a key.");

                StartCoroutine(Reset());
                return;
            }
            else if (other.gameObject.GetComponent<DoorScript>().NeedsKey && GameManager.Instance.keyFound)
            {
                other.gameObject.GetComponent<DoorScript>().Unlock();
                GameManager.Instance.keyFound = false;
            }

            other.gameObject.GetComponent<DoorScript>().Open = true;
            other.gameObject.GetComponent<DoorScript>().OpenDoor() ;
            Debug.Log("Door Is Open");

            StartCoroutine(Reset());
            return;
        }

    }

    void Colliding(Collider other, string target)
    {
        if (isColliding) return;
        isColliding = true;

        controller.SetNewTarget(other, target);
    }

    private void OnTriggerExit(Collider other)
    {
        // Vents
        if (other.gameObject.CompareTag("Vent"))
        {
            gameObject.GetComponentInParent<FirstPersonController>().inVent = false;
        }

        // Door
        if (other.gameObject.CompareTag("Door"))
        {
            // Validate State
            if (!other.gameObject.GetComponent<DoorScript>().Open)
            {
                Debug.Log("Door is already closed.");
                return;
            }

            controller.ClearTarget();

            other.gameObject.GetComponent<DoorScript>().Open = false;
            other.gameObject.GetComponent<DoorScript>().CloseDoor() ;
            Debug.Log("Door Is Closed");
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }

    IEnumerator Speak(string message, int delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.uiManager.UpdateProtagText(message);
    }

    public void PlayMainDeckDialogue()
    {
        int order = 0;
        MainDeckDialogue.Insert(0, " ");
        MainDeckDialogue.Add(" ");
        foreach (string text in MainDeckDialogue)
        {
            StartCoroutine(Speak(text, dialogueDelay * order++));
        }
        StartCoroutine(ContinueGameMainDeck());
    }

    public void StopMainDeckDialogue()
    {
        GameManager.Instance.uiManager.UpdateProtagText(" ");
        StopAllCoroutines();
    }

    IEnumerator ContinueGameMainDeck()
    {
        GameManager.Instance.uiManager.UpdateObjectiveText(MainDeckObjective);
        yield return new WaitForSeconds(MainDeckDialogue.Count * dialogueDelay - dialogueDelay * 2);
        GameManager.Instance.EnterNextState();
    }

    public void PlayAirlockDialogue()
    {
        GameManager.Instance.uiManager.UpdateObjectiveText(AirlockObjective);
    }

    public void PlayCrewQuartersDialogue()
    {
        GameManager.Instance.uiManager.UpdateObjectiveText(CrewQuartersObjective);
    }

    public void PlayStorageRoomDialogue()
    {
        int order = 0;
        StorageRoomDialogue.Insert(0, " ");
        StorageRoomDialogue.Add(" ");
        foreach (string text in StorageRoomDialogue)
        {
            StartCoroutine(Speak(text, dialogueDelay * order++));
        }

        GameManager.Instance.uiManager.UpdateObjectiveText(StorageRoomObjective);
    }

    public void PlayEngineRoomDialogue()
    {
        int order = 0;
        EngineRoomDialogue.Insert(0, " ");
        EngineRoomDialogue.Add(" ");
        foreach (string text in EngineRoomDialogue)
        {
            StartCoroutine(Speak(text, dialogueDelay * order++));
        }

        GameManager.Instance.uiManager.UpdateObjectiveText(EngineRoomObjective);
    }

}
