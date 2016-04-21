using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    [Header("Menu Screens")]
    public GameObject startScreen;
    public GameObject levelSelectScreen;
    public GameObject posterScreen;
    public GameObject controlScreen;
    public GameObject creditScreen;

    [Header("First Buttons")]
    public GameObject startGameButton;
    public GameObject levelSelectFirstButton;
    public GameObject posterScreenButton;
    public GameObject posterScreenFirstButton;
    public GameObject controlsBackButton;
    public GameObject creditsBackButton;
	
    public void Start()
    {
        startScreen.SetActive(true);
        levelSelectScreen.SetActive(false);
        posterScreen.SetActive(false);
        controlScreen.SetActive(false);
        creditScreen.SetActive(false);
    }


	
	public void StartGame()
    {
        SceneManager.LoadScene(1);
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

    public void GoToPosters()
    {
        posterScreen.SetActive(true);
        startScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(posterScreenFirstButton);
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

        if (posterScreen.activeInHierarchy)
            posterScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(startGameButton);
    }

    public void LevelSelect(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }
}
