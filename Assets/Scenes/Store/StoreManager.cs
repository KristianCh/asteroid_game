using System.Collections.Generic;
using Run;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Store
{
    public class StoreManager : MonoBehaviour
    {
        [SerializeField]
        public TMP_Text ActiveFleetCount;
        [SerializeField]
        public TMP_Text Credits;

        [SerializeField] 
        public List<ShipDisplay> ShipDisplays = new List<ShipDisplay>();
        [SerializeField] 
        public List<ShipDisplay> StoreDisplays = new List<ShipDisplay>();

        private RunData _activeRunData => RunManager.Instance.RunData;

        public void Start()
        {
            SetupShipDisplays();
        }

        private void SetupShipDisplays()
        {
            if (_activeRunData == null) return;
            
            ActiveFleetCount.text = $"Active Fleet: {_activeRunData.Ships.Count}/{_activeRunData.FleetLimit}";
            Credits.text = $"Credits: {_activeRunData.Credits}";

            for (var i = 0; i < 6; i++)
            {
                if (i >= _activeRunData.FleetLimit)
                {
                    ShipDisplays[i].SDP.SetActive(false);
                }
                else
                {
                    ShipDisplays[i].Setup(i < _activeRunData.Ships.Count ? _activeRunData.Ships[i] : null);
                }
            }
        }

        private void GenerateOffer()
        {
            
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}
