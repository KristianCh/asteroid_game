using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultySelectManager : MonoBehaviour
{
    public GameDifficulty SelectedDifficulty = GameDifficulty.Normal;

    void Start()
    {
        Button normalButton = GameObject.Find("Normal").GetComponent<Button>();
        normalButton.Select();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void SelectDifficultyNormal()
    {
        SelectedDifficulty = GameDifficulty.Normal;
    }

    public void SelectDifficultyHard()
    {
        SelectedDifficulty = GameDifficulty.Hard;
    }

    public void SelectDifficultyVeryHard()
    {
        SelectedDifficulty = GameDifficulty.VeryHard;
    }
}
