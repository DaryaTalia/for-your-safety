using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    bool locked;
    [SerializeField]
    bool needsKey;      // If true, validate if player has key
    [SerializeField]
    bool open;
    [SerializeField]
    float openSpeed = .5f;
    [SerializeField]
    float closeSpeed = 3f;

    [SerializeField]
    GameObject door;
    Vector3 closedPosition;
    float acceptableDistance = .001f;


    float doorHeight = 24f;

    // Start is called before the first frame update
    void Start()
    {
        closedPosition = door.transform.position;
        //Lock();
    }

    private void Update()
    {
        if(open && (closedPosition.y + doorHeight) - door.transform.position.y >= acceptableDistance)
        {
            door.transform.position = Vector3.Lerp(
                door.transform.position, 
                new Vector3(closedPosition.x, closedPosition.y + doorHeight, closedPosition.z),
                openSpeed * Time.deltaTime);
        }
        else if (!open && Vector3.Distance(door.transform.position, closedPosition) >= acceptableDistance)
        {
            door.transform.position = Vector3.Lerp(
                door.transform.position, 
                closedPosition,
                closeSpeed * Time.deltaTime);
        }
    }

    public void Unlock()
    {
        locked = false;
    }

    public void Lock()
    {
        locked = true;
    }

    private void OnTriggerStay(Collider other)
    {
        // Validate State
        if(open)
        {
            Debug.Log("Door is already open.");
            return;
        }

        // Validate Player
        if (!other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collider is not Player.");
            return;
        }

        // Validate if Door is locked
        if(locked)
        {
            // Validate if key is needed
            if (!needsKey)
            {
                Debug.Log("If locked but no key is needed, player must unlock door through Story Event.");
                return;
            }

            // Validate if Key is Needed & if Player has the Key
            if (needsKey && !GameManager.Instance.keyFound)
            {
                Debug.Log("The Player does not have a key.");
                return;
            }
            else if (needsKey && GameManager.Instance.keyFound)
            {
                Unlock();
                GameManager.Instance.keyFound = false;
            }
        }

        open = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Validate State
        if (!open)
        {
            Debug.Log("Door is already closed.");
            return;
        }

        open = false;
    }
}
