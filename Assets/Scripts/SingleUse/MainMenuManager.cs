using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    [Header("Menu Screens")]
    public GameObject startScreen;
    public GameObject levelSelectScreen;
    public GameObject controlScreen;
    public GameObject creditScreen;

    [Header("First Buttons")]
    public GameObject startGameButton;
    public GameObject levelSelectFirstButton;
    public GameObject controlsBackButton;
    public GameObject creditsBackButton;
	
    public void Start()
    {
        startScreen.SetActive(true);
        levelSelectScreen.SetActive(false);
        controlScreen.SetActive(false);
        creditScreen.SetActive(false);
    }


	
	public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToControls()
    {
        controlScreen.SetActive(true);
        startScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(controlsBackButton);
    }

    public void GoToLevelSelect()
    {
        levelSelectScreen.SetActive(true);
        startScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(levelSelectFirstButton);
    }

    public void GoToCredits()
    {
         creditScreen.SetActive(true);
         startScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(creditsBackButton);
    }

    public void BackFromControls()
    {
        startScreen.SetActive(true);
        controlScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(startGameButton);
    }

    public void BackFromCredits()
    {
        startScreen.SetActive(true);
        creditScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(startGameButton);
    }

    public void BackFromLevelSelect()
    {
        startScreen.SetActive(true);
        levelSelectScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(startGameButton);
    }

    public void LevelSelect(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }
}
