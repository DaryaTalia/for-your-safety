using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    public Collider target;
    public string targetKey;

    void OnInteract()
    {
        Debug.Log("1. Interacting with ... ");

        switch(targetKey)
        {
            case "Key":

                GameManager.Instance.keyFound = true;
                Destroy(target.gameObject);

                Debug.Log("2. " + targetKey);
                target = null;
                targetKey = " ";

                break;

            case "Gun":

                GameManager.Instance.gunFound = true;
                Destroy(target.gameObject);

                Debug.Log("2. " + targetKey);
                target = null;
                targetKey = " ";

                break;



            default:
                Debug.Log("2. " + targetKey);
                break;
        }
    }
}
