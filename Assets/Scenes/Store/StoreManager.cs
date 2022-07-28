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
        public RunData ActiveRunData;
        [SerializeField]
        public TMP_Text ActiveFleetCount;
        [SerializeField]
        public TMP_Text Credits;

        [SerializeField] 
        public List<ShipDisplay> ShipDisplays = new List<ShipDisplay>();
        [SerializeField] 
        public List<ShipDisplay> StoreDisplays = new List<ShipDisplay>();

        public void Start()
        {
            ActiveRunData = (RunData)FindObjectOfType(typeof(RunData));

            SetupShipDisplays();
        }

        private void SetupShipDisplays()
        {
            if (ActiveRunData == null) return;
            
            ActiveFleetCount.text = $"Active Fleet: {ActiveRunData.Ships.Count}/{ActiveRunData.FleetLimit}";
            Credits.text = $"Credits: {ActiveRunData.Credits}";

            for (var i = 0; i < 6; i++)
            {
                if (i >= ActiveRunData.FleetLimit)
                {
                    ShipDisplays[i].SDP.SetActive(false);
                }
                else
                {
                    ShipDisplays[i].Setup(i < ActiveRunData.Ships.Count ? ActiveRunData.Ships[i] : null);
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
