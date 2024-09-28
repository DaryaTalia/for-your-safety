using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    bool isColliding;
    [SerializeField]
    InteractionController controller;

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
            other.GetComponent<ButtonScript>().ActionDelegate?.Invoke();

            StartCoroutine(Reset());
            return;
        }

        // Intercom
        if (other.gameObject.CompareTag("Intercom"))
        {
            if (isColliding) return;
            isColliding = true;

            if (other.GetComponent<IntercomScript>() == null)
            {
                Debug.Log("Intercom Sctipt Not Found");
                Debug.Log("Other = " + other.gameObject.name);

                StartCoroutine(Reset());
                return;
            }

            if (other.GetComponent<IntercomScript>().ActionDelegate == null)
            {
                Debug.Log("Action Delegate Not Found");
                Debug.Log("Other = " + other.gameObject.name);

                StartCoroutine(Reset());
                return;
            }

            other.GetComponent<IntercomScript>().ActionDelegate?.Invoke();
            Debug.Log("Invoking Intercom Action Delegate");

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

}
