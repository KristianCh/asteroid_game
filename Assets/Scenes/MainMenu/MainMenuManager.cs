using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private bool AppliedInitialImpulse = false;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy active run data
        RunData rd = (RunData)FindObjectOfType(typeof(RunData));
        if (rd != null)
        {
            Destroy(rd.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!AppliedInitialImpulse)
        {
            BaseAsteroid[] asteroids = FindObjectsOfType<BaseAsteroid>();
            foreach (BaseAsteroid asteroid in asteroids)
            {
                Rigidbody rb = asteroid.GetComponent(typeof(Rigidbody)) as Rigidbody;
                if (rb != null)
                {
                    rb.AddForceAtPosition(new Vector3(Random.Range(-400, 400), Random.Range(-400, 400), 0),
                        asteroid.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)));
                }
            }
            AppliedInitialImpulse = true;
        }
    }

    // Go to difficulty select
    public void NewRun()
    {
        SceneManager.LoadScene("DifficultySelect", LoadSceneMode.Single);
    }

    // Quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
