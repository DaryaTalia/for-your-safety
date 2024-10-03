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
    [SerializeField]
    string ObjectiveDialogue;

    List<Coroutine> activeCoroutines;

    int dialogueDelay = 4;
    bool ringing;
    bool ringCooldown;
    int ringTimer = 3;

    public bool Ringing
    {
        get => ringing;
        set => ringing = value;
    }

    private void Start()
    {
        activeCoroutines = new List<Coroutine>();
    }

    private void FixedUpdate()
    {
        if(ringing && !ringCooldown)
        {
            activeCoroutines.Add(StartCoroutine(Ring()));
            ringCooldown = true;

            if(!GameManager.Instance.audioManager.GetIntercomBeep().isPlaying)
            {
                GameManager.Instance.audioManager.PlayIntercomBeep();
            }
        }
    }

    IEnumerator Ring()
    {
        yield return new WaitForSeconds(ringTimer);
        ringCooldown = false;
    }

    public void Answer()
    {
        // Destroy Audio Device to end ringing
        ringing = false;
        ringCooldown = false;
        GameManager.Instance.audioManager.StopIntercomBeep();
        GameManager.Instance.uiManager.UpdateProtagText(" ");
        foreach(Coroutine cor in activeCoroutines)
        {
            StopCoroutine(cor);
        }
        StopAllCoroutines();
        //DisplayDialogue();
        PlayDialogue();
    }

    void PlayDialogue()
    {
        switch (GameManager.Instance.currentState)
        {
            case GameManager.GameStates.MAIN_DECK_INTERCOM_ANSWERED:
                GameManager.Instance.audioManager.PlayMainDeckScene();
                GameManager.Instance.audioManager.StopMainDeckSceneIntro();

                StartCoroutine(Continue(GameManager.Instance.audioManager.mainDeckSceneLength));
                StartCoroutine(ObjectiveEnumerator((int)GameManager.Instance.audioManager.mainDeckSceneLength + 2));
                break;
            case GameManager.GameStates.AIRLOCK_INTERCOM_ANSWERED:
                GameManager.Instance.audioManager.PlayAirlockScene();

                StartCoroutine(Continue(GameManager.Instance.audioManager.airlockSceneLength));
                StartCoroutine(ObjectiveEnumerator((int)GameManager.Instance.audioManager.airlockSceneLength + 2));
                break;
            case GameManager.GameStates.CREW_QUARTERS_INTERCOM_ANSWERED:
                GameManager.Instance.audioManager.PlayCrewQuartersScene();

                StartCoroutine(Continue(GameManager.Instance.audioManager.crewQuartersSceneLength));
                StartCoroutine(ObjectiveEnumerator((int)GameManager.Instance.audioManager.crewQuartersSceneLength + 2));
                break;
            case GameManager.GameStates.ENGINE_ROOM_ENTERED:
                GameManager.Instance.audioManager.PlayEngineeringScene();

                StartCoroutine(Continue(GameManager.Instance.audioManager.engineeringSceneLength));
                StartCoroutine(ObjectiveEnumerator((int)GameManager.Instance.audioManager.engineeringSceneLength + 2));
                break;

            default:
                break;
        }
    }

    IEnumerator Continue(float time)
    {
        yield return new WaitForSeconds(time);
    }

    void DisplayDialogue()
    {
        int order = 0;
        foreach(string text in IntercomDialogue)
        {
            StartCoroutine(DialogueIterator(text, dialogueDelay * order++));
        }
        StartCoroutine(ObjectiveEnumerator(dialogueDelay * order));
    }

    IEnumerator DialogueIterator(string message, int delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.uiManager.UpdateProtagText(message);
    }

    IEnumerator ObjectiveEnumerator(int delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.uiManager.UpdateObjectiveText(ObjectiveDialogue);

        switch (GameManager.Instance.currentState)
        {
            case GameManager.GameStates.MAIN_DECK_INTERCOM_ANSWERED:
                GameManager.Instance.audioManager.StopMainDeckScene();

                GameManager.Instance.SpawnKeycard();
                Destroy(this, 2f);
                break;
            case GameManager.GameStates.AIRLOCK_INTERCOM_ANSWERED:
                GameManager.Instance.audioManager.StopAirlockScene();

                GameManager.Instance.AirlockButton.GetComponent<ButtonScript>().enabled = true;
                Destroy(this, 2f);
                break;
            case GameManager.GameStates.CREW_QUARTERS_INTERCOM_ANSWERED:
                GameManager.Instance.audioManager.StopCrewQuartersScene();

                GameManager.Instance.crewQuartersDialogueComplete = true;
                Destroy(this, 2f);
                break;
            case GameManager.GameStates.ENGINE_ROOM_ENTERED:
                GameManager.Instance.audioManager.StopEngineeringScene();

                break;

            default:
                break;
        }

        GameManager.Instance.uiManager.UpdateProtagText(" ");
    }
}
