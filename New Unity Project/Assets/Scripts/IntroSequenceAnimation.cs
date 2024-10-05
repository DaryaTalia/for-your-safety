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

    readonly float red = 0.2352941f;
    readonly float green = 0.3490196f;
    readonly float blue = 0.3372549f;

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
        IntroBackground.color = new Color(red, green, blue, 1);
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

    public void PlaySequence()
    {
        //IntroText.Insert(0, " ");
        foreach (string text in IntroText)
        {
            StartCoroutine(PlayText(text));
        }
        StartCoroutine(StartGame());
    }

    IEnumerator PlayText(string message)
    {
        yield return new WaitForSeconds(textSpeed * index++ + (textSpeedModifier / (message.Length + 1)));
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
