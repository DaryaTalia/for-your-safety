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

    int dialogueDelay = 3;

    private void OnTriggerStay(Collider other)
    {
        // Crew
        if (other.gameObject.CompareTag("CrewMember"))
        {
            if (isColliding) return;
            isColliding = true;

            controller.SetNewTarget(other, "CrewMember");

            StartCoroutine(Reset());
            return;
        }


        // Key
        if (other.gameObject.CompareTag("Key"))
        {
            if (isColliding) return;
            isColliding = true;

            controller.SetNewTarget(other, "Key");

            StartCoroutine(Reset());
            return;
        }

        // Gun
        if (other.gameObject.CompareTag("Gun"))
        {
            if (isColliding) return;
            isColliding = true;

            controller.SetNewTarget(other, "Gun");

            StartCoroutine(Reset());
            return;
        }

        // Buttons
        if (other.gameObject.CompareTag("Button"))
        {
            if (isColliding) return;
            isColliding = true;

            controller.SetNewTarget(other, "Button");

            StartCoroutine(Reset());
            return;
        }

        // Intercom
        if (other.gameObject.CompareTag("Intercom"))
        {
            if (isColliding) return;
            isColliding = true;

            controller.SetNewTarget(other, "Intercom");

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

    private void OnTriggerExit(Collider other)
    {
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

}
