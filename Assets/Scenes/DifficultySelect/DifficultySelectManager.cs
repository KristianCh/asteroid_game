using Data;
using Run;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.DifficultySelect
{
    public class DifficultySelectManager : MonoBehaviour
    {
        [SerializeField]
        private DifficultySettingsData _SelectedDifficulty;
        public DifficultySettingsData SelectedDifficulty
        {
            get => _SelectedDifficulty;
            set => _SelectedDifficulty = value;
        }

        public static DifficultySelectManager Instance;

        void Start()
        {
            Instance = this;
            Button normalButton = GameObject.Find("Normal").GetComponent<Button>();
            normalButton.Select();
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void StartGame()
        {
            RunManager.Instance.RunData.Difficulty = _SelectedDifficulty;
            SceneManager.LoadScene("Store", LoadSceneMode.Single);
        }
    }
}
