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
    Animator anim;

    // Used to stop animation from playing multiple times
    bool waiting;

    public bool Open
    {
        get => open;
        set => open = value;
    }

    public bool Locked
    {
        get => locked;
        set => locked = value;
    }

    public bool NeedsKey
    {
        get => needsKey;
        set => needsKey = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (open && !waiting)
        {
            anim.SetTrigger("OpenDoor");
            waiting = true;
        }
    }

    public void CloseDoor()
    {
        if (!open && waiting)
        {
            anim.SetTrigger("CloseDoor");
            waiting = false;
        }
    }

    public void Unlock()
    {
        locked = false;
        Debug.Log("Door Unlocked: " + locked);
    }

    public void Lock()
    {
        locked = true;
        Debug.Log("Door Locked: " + locked);
    }
}
