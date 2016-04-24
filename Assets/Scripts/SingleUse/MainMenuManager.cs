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

    public GameObject greyBox;
    public Image expandedPoster;

    private bool _displayingImage;

    public void Start()
    {
        startScreen.SetActive(true);
        levelSelectScreen.SetActive(false);
        posterScreen.SetActive(false);
        controlScreen.SetActive(false);
        creditScreen.SetActive(false);
        greyBox.SetActive(false);

        _displayingImage = false;
    }

    public void Update()
    {
        if (_displayingImage && Input.GetButtonDown("Cancel"))
            CollapsePoster();
    }

    #region MenuButtons
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

        PosterTracker.Instance.UpdatePosters();

        EventSystem.current.SetSelectedGameObject(posterScreenFirstButton);
    }

    public void GoToCredits()
    {
        creditScreen.SetActive(true);
        startScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(creditsBackButton);
    }
    #endregion

    #region BackButtons

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

    #endregion

    public void LevelSelect(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void ResetPosters()
    {
        PlayerPrefs.DeleteAll();
        PosterTracker.Instance.UpdatePosters();
    }

    public void ExpandPoster(Image poster)
    {
        expandedPoster.sprite = poster.sprite;
        greyBox.SetActive(true);
        _displayingImage = true;
    }

    public void CollapsePoster()
    {
        greyBox.SetActive(false);
        _displayingImage = false;
    }
}
