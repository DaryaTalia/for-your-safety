using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntercomScript : MonoBehaviour
{
    public UnityEvent ActionDelegate;

    [SerializeField]
    [TextArea(5,7)]
    List<string> IntercomDialogue;
    int index;

    public void Ring()
    {
        // Play Intercom Ringing Sound on Loop
    }

    public void Answer()
    {
        // Destroy Audio Device to end ringing
        DisplayDialogue();
    }

    void DisplayDialogue()
    {

    }
}
