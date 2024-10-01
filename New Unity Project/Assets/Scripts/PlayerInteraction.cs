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
    [TextArea(2, 5)]
    List<string> AirlockDialogue;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> CrewQuartersDialogue;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> StorageRoomDialogue;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> EngineRoomDialogue;

    int dialogueDelay = 3;

    private void OnTriggerEnter(Collider other)
    {
        // Key
        if(other.gameObject.CompareTag("Key"))
        {
            if (isColliding) return;
            isColliding = true;

            controller.SetNewTarget(other, "Key");

            StartCoroutine(Reset());
            return;
        }

        // Keycard
        if(other.gameObject.CompareTag("Keycard"))
        {
            if (isColliding) return;
            isColliding = true;

            other.gameObject.GetComponentInChildren<ItemCanvasUI>().gameObject.SetActive(true);

            controller.SetNewTarget(other, "Keycard");

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

        // Wrench
        if (other.gameObject.CompareTag("Wrench"))
        {
            if (isColliding) return;
            isColliding = true;

            controller.SetNewTarget(other, "Wrench");

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
        controller.ClearTarget();

        // Door
        if (other.gameObject.CompareTag("Door"))
        {
            // Validate State
            if (!other.gameObject.GetComponent<DoorScript>().Open)
            {
                Debug.Log("Door is already closed.");
                return;
            }

            // Validate Animation State
            if (!other.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DoorOpen"))
            {
                Debug.Log("Incorrect Animation State to Proceed.");
                return;
            }

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

    public void PlayMainDeckDialogue()
    {
        int order = 0;
        MainDeckDialogue.Insert(0, " ");
        MainDeckDialogue.Add(" ");
        foreach (string text in MainDeckDialogue)
        {
            StartCoroutine(Speak(text, dialogueDelay * order++));
        }
        StartCoroutine(ContinueGameMainDeck1());
    }

    public void StopMainDeckDialogue()
    {
        StopAllCoroutines();
    }

    IEnumerator Speak(string message, int delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.uiManager.UpdateProtagText(message);
    }

    IEnumerator ContinueGameMainDeck1()
    {
        yield return new WaitForSeconds(MainDeckDialogue.Count * dialogueDelay - dialogueDelay * 2);
        GameManager.Instance.EnterNextState();
    }

}
