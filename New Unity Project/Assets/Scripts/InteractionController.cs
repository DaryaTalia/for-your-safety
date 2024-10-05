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
                GameManager.Instance.gameObject.GetComponent<CrewMemberRandomizer>().ToggleNextText(target);

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
                GameManager.Instance.PlayerArm.SetActive(true);
                Destroy(target.gameObject);

                ClearTarget();

                break;

            case "Button":

                target.GetComponentInChildren<ButtonScript>().ActionDelegate?.Invoke();

                ClearTarget();

                break;

            case "EngineRoomButton1":

                GameManager.Instance.Button1Pushed = true;
                GameObject.FindGameObjectWithTag("EngineRoomButton1").GetComponent<ItemGlow>().SetInactive();
                GameObject.FindGameObjectWithTag("EngineRoomButton1").GetComponent<ItemGlow>().enabled = false;

                ClearTarget();

                break;

            case "EngineRoomButton2":

                GameManager.Instance.Button2Pushed = true;
                GameObject.FindGameObjectWithTag("EngineRoomButton2").GetComponent<ItemGlow>().SetInactive();
                GameObject.FindGameObjectWithTag("EngineRoomButton2").GetComponent<ItemGlow>().enabled = false;

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

                ClearTarget();

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
            GameManager.Instance.Player.enabled = false;
        } else
        {
            GameManager.Instance.uiManager.ResumeGame();
            GameManager.Instance.GamePaused = false;
            if (GameManager.Instance.currentState != GameManager.GameStates.CREW_QUARTERS_INTERCOM_ANSWERED)
            {
                GameManager.Instance.Player.enabled = true;
            }

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
