using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Put this on the start screen scene, on an empty object

public class StartScreen : MonoBehaviour
{
    [Header("UI Elements")]
    public Button SlowBullButton;
    public Button InfTwineButton;

    private bool isSlowBull = false;
    private bool isInfTwine = false;

    private void Start()
    {
        // Loads saved preferences
        isSlowBull = PlayerPrefs.GetInt("SlowBull", 0) == 1;
        isInfTwine = PlayerPrefs.GetInt("InfTwine", 0) == 1;

        UpdateButtonVisuals();

        SlowBullButton.onClick.AddListener(ToggleHardMode);
        InfTwineButton.onClick.AddListener(ToggleEasyMode);
    }

    private void ToggleHardMode()
    {
        isSlowBull = !isSlowBull;
        isInfTwine = false; // Ensure easy mode is disabled

        PlayerPrefs.SetInt("SlowBull", isSlowBull ? 1 : 0);
        PlayerPrefs.SetInt("InfTwine", 0);
        PlayerPrefs.Save();

        UpdateButtonVisuals();
    }

    private void ToggleEasyMode()
    {
        isInfTwine = !isInfTwine;
        isSlowBull = false; // Ensure hard mode is disabled

        PlayerPrefs.SetInt("InfTwine", isInfTwine ? 1 : 0);
        PlayerPrefs.SetInt("SlowBull", 0);
        PlayerPrefs.Save();

        UpdateButtonVisuals();
    }

    private void UpdateButtonVisuals()
    {
        SlowBullButton.GetComponent<Image>().color = isSlowBull ? Color.red : Color.white;
        InfTwineButton.GetComponent<Image>().color = isInfTwine ? Color.green : Color.white;
    }
}
  