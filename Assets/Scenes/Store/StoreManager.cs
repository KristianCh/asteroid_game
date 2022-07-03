using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public RunData ActiveRunData;
    public TMP_Text ActiveFleetCount;
    public TMP_Text Credits;

    public 

    // Start is called before the first frame update
    void Start()
    {
        ActiveRunData = (RunData)FindObjectOfType(typeof(RunData));

        if (ActiveRunData != null)
        {
            ActiveFleetCount.text = "Active Fleet: " + ActiveRunData.Ships.Count.ToString() + "/" + ActiveRunData.FleetLimit.ToString();
            Credits.text = "Credits: " + ActiveRunData.Credits.ToString();

            for (int i = 0; i < 6; i++)
            {
                ShipDisplay sd = GameObject.Find("ShipDisplay" + i.ToString()).GetComponent<ShipDisplay>();
                if (i >= ActiveRunData.FleetLimit)
                {
                    sd.SDP.SetActive(false);
                }
                else
                {
                    if (i < ActiveRunData.Ships.Count)
                    {
                        sd.Setup(ActiveRunData.Ships[i]);
                    }
                    else
                    {
                        sd.Setup(null);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
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
