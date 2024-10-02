using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationListener : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "MAIN_DECK":
                GameManager.Instance.currentLocation = GameManager.Locations.MAIN_DECK;
                GameManager.Instance.UpdateLocationUI();
                break;
            case "AIRLOCK":
                GameManager.Instance.currentLocation = GameManager.Locations.AIRLOCK;
                GameManager.Instance.UpdateLocationUI();
                break;
            case "CREW_QUARTERS":
                GameManager.Instance.currentLocation = GameManager.Locations.CREW_QUARTERS;
                GameManager.Instance.UpdateLocationUI();
                break;
            case "STORAGE_ROOM":
                GameManager.Instance.currentLocation = GameManager.Locations.STORAGE_ROOM;
                GameManager.Instance.UpdateLocationUI();
                break;
            case "ENGINE_ROOM":
                GameManager.Instance.currentLocation = GameManager.Locations.ENGINE_ROOM;
                GameManager.Instance.UpdateLocationUI();
                break;

            default:
                break;
        }
    }
}
