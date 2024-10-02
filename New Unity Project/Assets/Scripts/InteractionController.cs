using UnityEngine;

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
            case "CrewMember":
                GameManager.Instance.gameObject.GetComponent<CrewMemberRandomizer>().ToggleNextText();

                ClearTarget();

                break;

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

                target.GetComponentInChildren<ButtonScript>().ActionDelegate?.Invoke();

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

                Debug.Log("Invoking Intercom Action Delegate");
                target.GetComponent<IntercomScript>().ActionDelegate?.Invoke();
                target.GetComponent<IntercomScript>().ActionDelegate = null;
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
