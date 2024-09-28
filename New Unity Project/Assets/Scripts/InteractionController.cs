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
                Destroy(target.gameObject);

                ClearTarget();

                break;

            case "Gun":

                GameManager.Instance.gunFound = true;
                Destroy(target.gameObject);

                ClearTarget();

                break;

            case "Wrench":

                GameManager.Instance.wrenchFound = true;
                Destroy(target.gameObject);

                ClearTarget();

                break;



            default:
                ClearTarget();
                break;
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
