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
    GameObject IntroSequencePanel;
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
    TextMeshProUGUI ObjectiveText;
    [SerializeField]
    TextMeshProUGUI ProtagText;

    [SerializeField]
    GameObject NeutralEndingPanel;
    [SerializeField]
    GameObject BadEndingPanel;

    public InventoryPanelUI GetInventoryPanelController()
    {
        return InventoryPanel.GetComponent<InventoryPanelUI>();
    }

    void OnAwake()
    {
        StartMainMenuUI();
    }

    public void StartMainMenuUI()
    {
        EnableMainMenuPanel();

        DisableCreditsPanel();
        DisableGameplayPanel();
        DisableIntroSequencePanel();
        DisablePausePanel();
    }

    public void StartIntroSequence()
    {
        DisableMainMenuPanel();
        EnableIntroSequencePanel();
        IntroSequencePanel.GetComponent<IntroSequenceAnimation>().PlaySequence();
    }

    public void StartNeutralEndingSequence()
    {
        DisableGameplayPanel();
        EnableNeutralEndingPanel();
        NeutralEndingPanel.GetComponent<IntroSequenceAnimation>().PlaySequence();
    }

    public void StartBadEndingSequence()
    {
        DisableGameplayPanel();
        EnableBadEndingPanel();
        BadEndingPanel.GetComponent<IntroSequenceAnimation>().PlaySequence();
    }

    public void StartGameUI()
    {
        DisableIntroSequencePanel();
        EnableGameplayPanel();
        UpdateLocationText(" ");
        UpdateProtagText(" ");
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

    public void EnableIntroSequencePanel()
    {
        IntroSequencePanel.SetActive(true);
    }

    public void DisableIntroSequencePanel()
    {
        IntroSequencePanel.SetActive(false);
    }

    public void EnableNeutralEndingPanel()
    {
        NeutralEndingPanel.SetActive(true);
    }

    public void DisableNeutralEndingPanel()
    {
        NeutralEndingPanel.SetActive(false);
    }

    public void EnableBadEndingPanel()
    {
        BadEndingPanel.SetActive(true);
    }

    public void DisableBadEndingPanel()
    {
        BadEndingPanel.SetActive(false);
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

    public void UpdateObjectiveText(string objectiveText)
    {
        ObjectiveText.text = objectiveText;
    }

    public void UpdateProtagText(string protagText)
    {
        ProtagText.text = protagText;
    }
}
