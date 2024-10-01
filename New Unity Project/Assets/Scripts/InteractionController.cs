using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    Collider target;
    string targetKey;

    // Press "E" to call 
    void OnInteract()
    {
        Debug.Log("1. Interacting with ... ");

        switch(targetKey)
        {
            case "Key":

                GameManager.Instance.keyFound = true;
                GameManager.Instance.uiManager.GetInventoryPanelController().AddNewItem(targetKey);
                Destroy(target.gameObject);

                ClearTarget();

                break;

            case "Gun":

                GameManager.Instance.gunFound = true;
                Destroy(target.gameObject);

                ClearTarget();

                break;

            case "Button":

                //target.GetComponent<ButtonScript>().ActionDelegate?.Invoke();
                GameManager.Instance.EnterNextState();

                ClearTarget();

                break;

            case "Intercom":


                if (target.GetComponent<IntercomScript>() == null)
                {
                    Debug.Log("Intercom Sctipt Not Found");
                    Debug.Log("Other = " + target.gameObject.name);
                    return;
                }

                if (target.GetComponent<IntercomScript>().ActionDelegate == null)
                {
                    Debug.Log("Action Delegate Not Found");
                    Debug.Log("Other = " + target.gameObject.name);
                    return;
                }

                //target.GetComponent<IntercomScript>().ActionDelegate?.Invoke();
                Debug.Log("Invoking Intercom Action Delegate");
                GameManager.Instance.EnterNextState();
                break;


            default:
                ClearTarget();
                break;
        }
    }

    public void OnPause()
    {
        if(!GameManager.Instance.GamePaused)
        {
            GameManager.Instance.uiManager.PauseGame();
            GameManager.Instance.GamePaused = true;
            GameManager.Instance.Player.gameObject.SetActive(false);
        } else
        {
            GameManager.Instance.uiManager.ResumeGame();
            GameManager.Instance.GamePaused = false;
            GameManager.Instance.Player.gameObject.SetActive(true);

        }
    }

    public void SetNewTarget(Collider _target, string _targetKey)
    {
        target = _target;
        targetKey = _targetKey;
    }

    public void ClearTarget()
    {
        Debug.Log("2. " + targetKey);

        target = null;
        targetKey = " ";
    }
}
