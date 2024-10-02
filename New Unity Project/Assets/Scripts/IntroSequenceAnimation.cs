using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class IntroSequenceAnimation : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI IntroTextBox;
    [SerializeField]
    Image IntroBackground;
    float fadeDuration = 3f;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> IntroText;

    int textSpeed = 5;
    int textSpeedModifier = 8;
    int index = 0;
    bool fade;

    public void Start()
    {
        IntroTextBox.text = "  ";
    }

    public void Update()
    {
        if(fade)
        {
            if(GetComponentInChildren<Camera>())
            {
                Destroy(GetComponentInChildren<Camera>());
            }
            IntroBackground.CrossFadeAlpha(0, fadeDuration, false);
        }
    }

    public void PlayIntroSequence()
    {
        //IntroText.Insert(0, " ");
        foreach (string text in IntroText)
        {
            StartCoroutine(PlayIntroText(text));
        }
        StartCoroutine(StartGame());
    }

    IEnumerator PlayIntroText(string message)
    {
        yield return new WaitForSeconds(textSpeed * index++ + (textSpeedModifier / message.Length));
        IntroTextBox.text = message;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(textSpeed * index);
        IntroTextBox.text = "  ";
        fade = true;
        yield return new WaitForSeconds(fadeDuration * fadeDuration);
        GameManager.Instance.EnterNextState();
    }
}
