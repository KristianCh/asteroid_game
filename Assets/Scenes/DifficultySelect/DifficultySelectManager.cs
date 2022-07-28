using Run;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.DifficultySelect
{
    public class DifficultySelectManager : MonoBehaviour
    {
        public GameDifficulty SelectedDifficulty = GameDifficulty.Normal;
        public RunData RunDataPrefab;

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
            RunData rd = Instantiate(RunDataPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<RunData>();
            rd.Difficulty = SelectedDifficulty;
            SceneManager.LoadScene("Store", LoadSceneMode.Single);
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
}
