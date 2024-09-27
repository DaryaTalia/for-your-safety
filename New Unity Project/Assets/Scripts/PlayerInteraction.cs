using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Key
        if(other.gameObject.CompareTag("Key"))
        {
            GameManager.Instance.keyFound = true;
            Destroy(other.gameObject);
        }

        // Gun
        if (other.gameObject.CompareTag("Gun"))
        {
            GameManager.Instance.gunFound = true;
            Destroy(other.gameObject);
        }
    }
}
