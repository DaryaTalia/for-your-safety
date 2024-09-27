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
            return;
        }

        // Gun
        if (other.gameObject.CompareTag("Gun"))
        {
            GameManager.Instance.gunFound = true;
            Destroy(other.gameObject);
            return;
        }

        // Buttons
        if (other.gameObject.CompareTag("Button"))
        {
            other.GetComponent<ButtonScript>().ActionDelegate?.Invoke();
            return;
        }

        // Intercom
        if (other.gameObject.CompareTag("Intercom"))
        {
            other.GetComponent<IntercomScript>().ActionDelegate?.Invoke();
            return;
        }
    }
}
