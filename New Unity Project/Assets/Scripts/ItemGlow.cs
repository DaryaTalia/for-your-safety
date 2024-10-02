using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGlow : MonoBehaviour
{
    [SerializeField]
    Material normal;
    [SerializeField]
    Material glow;

    bool active;
    bool glowing;
    float timer;
    float duration = 60f;

    void FixedUpdate()
    {
        if(active)
        {
            if(timer < duration)
            {
                timer += 1;
            } 
            else
            {
                ToggleGlow();
                timer = 0;
            }
            Debug.Log(timer);
        }
    }

    public void SetActive()
    {
        active = true;
    }

    public void SetInactive()
    {
        active = false;
        GetComponent<MeshRenderer>().material = normal;
    }

    public void ToggleGlow()
    {
        if(glowing)
        {
            GetComponent<MeshRenderer>().material = normal;
            glowing = false;
        } 
        else
        {
            GetComponent<MeshRenderer>().material = glow;
            glowing = true;
        }
    }
}
