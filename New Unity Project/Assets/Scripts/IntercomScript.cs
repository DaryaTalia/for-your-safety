using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntercomScript : MonoBehaviour
{
    public UnityEvent ActionDelegate;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> IntercomDialogue;

    int dialogueDelay = 2;
    bool ringing;
    bool ringCooldown;
    int ringTimer = 3;

    public bool Ringing
    {
        get => ringing;
        set => ringing = value;
    }

    private void FixedUpdate()
    {
        if(ringing && !ringCooldown)
        {
            StartCoroutine(Ring());
            ringCooldown = true;
        }
    }

    IEnumerator Ring()
    {
        // Play Intercom Ringing Sound on Loop
        GameManager.Instance.uiManager.UpdateProtagText(" ");
        yield return new WaitForSeconds(ringTimer);
        GameManager.Instance.uiManager.UpdateProtagText("**Ringing**");
        yield return new WaitForSeconds(ringTimer);
        ringCooldown = false;
    }

    public void Answer()
    {
        // Destroy Audio Device to end ringing
        ringing = false;
        ringCooldown = false;
        GameManager.Instance.uiManager.UpdateProtagText(" ");
        StopAllCoroutines();
        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        int order = 0;
        IntercomDialogue.Add(" ");
        foreach(string text in IntercomDialogue)
        {
            StartCoroutine(DialogueIterator(text, dialogueDelay * order++));
        }
    }

    IEnumerator DialogueIterator(string message, int delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.uiManager.UpdateProtagText(message);
    }
}
