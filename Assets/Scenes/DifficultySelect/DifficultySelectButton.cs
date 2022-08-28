// Author: Kristián Chovančák
// Created: 28.08.2022
// Copyright: (c) Noxgames
// http://www.noxgames.com/

using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.DifficultySelect
{
    public class DifficultySelectButton : MonoBehaviour
    {
        [SerializeField]
        private Button _Button;

        [SerializeField]
        private DifficultySettingsData _Difficulty;

        [SerializeField]
        private TMP_Text _Title;
        
        [SerializeField]
        private TMP_Text _Description;
        
        public void Awake()
        {
            _Button.onClick.AddListener(OnClick);
            _Title.text = _Difficulty.Title;
            _Description.text = _Difficulty.Description;
        }

        private void OnClick()
        {
            DifficultySelectManager.Instance.SelectedDifficulty = _Difficulty;
        }
    }
}