using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCanvasUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI itemText;
    [SerializeField]
    string text;

    public void ShowText()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = text;
    }

    public void HideText()
    {
        itemText.gameObject.SetActive(false);
    }
}
