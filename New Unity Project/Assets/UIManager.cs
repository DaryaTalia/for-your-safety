using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField]
    GameObject MainMenuPanel;
    [SerializeField]
    GameObject CreditsPanel;
    [SerializeField]
    GameObject CreditsBackButtonMM;

    [Header("Gameplay")]
    [SerializeField]
    GameObject GameplayPanel;
    [SerializeField]
    GameObject PausePanel;
    [SerializeField]
    GameObject CreditsBackButtonPM;
    [SerializeField]
    GameObject InventoryPanel;
    [SerializeField]
    TextMeshProUGUI LocationText;
    [SerializeField]
    TextMeshProUGUI ProtagText;
    [SerializeField]
    TextMeshProUGUI IntercomMessageText;

    public InventoryPanelUI GetInventoryPanelController()
    {
        return InventoryPanel.GetComponent<InventoryPanelUI>();
    }

    void OnAwake()
    {
        EnableMainMenuPanel();
        DisableCreditsPanel();
        DisableGameplayPanel();
    }

    public void StartGameUI()
    {
        DisableMainMenuPanel();
        EnableGameplayPanel();
        UpdateLocationText(" ");
        UpdateProtagText(" ");
        UpdateIntercomText(" ");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        EnablePausePanel();
        DisableGameplayPanel();
    }

    public void ResumeGame()
    {
        DisablePausePanel();
        EnableGameplayPanel();
    }

    public void EnableMainMenuPanel()
    {
        MainMenuPanel.SetActive(true);
    }

    public void DisableMainMenuPanel()
    {
        MainMenuPanel.SetActive(false);
    }
    public void EnableCreditsPanel()
    {
        CreditsPanel.SetActive(true);
    }

    public void DisableCreditsPanel()
    {
        CreditsPanel.SetActive(false);
    }
    
    public void EnableCreditsMMButton()
    {
        CreditsBackButtonMM.SetActive(true);
    }

    public void DisableCreditsMMButton()
    {
        CreditsBackButtonMM.SetActive(false);
    }
    public void EnableCreditsPMButton()
    {
        CreditsBackButtonPM.SetActive(true);
    }

    public void DisableCreditsPMButton()
    {
        CreditsBackButtonPM.SetActive(false);
    }
    
    public void EnableGameplayPanel()
    {
        GameplayPanel.SetActive(true);
    }

    public void DisableGameplayPanel()
    {
        GameplayPanel.SetActive(false);
    }
    
    public void EnablePausePanel()
    {
        PausePanel.SetActive(true);
    }

    public void DisablePausePanel()
    {
        PausePanel.SetActive(false);
    }

    public void UpdateLocationText(string locationText)
    {
        LocationText.text = locationText;
    }

    public void UpdateProtagText(string protagText)
    {
        ProtagText.text = protagText;
    }

    public void UpdateIntercomText(string intercomText)
    {
        IntercomMessageText.text = intercomText;
    }

}
