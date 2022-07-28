using Combat.Asteroid;
using Run;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        private bool AppliedInitialImpulse = false;

        // Start is called before the first frame update
        public void Start()
        {
            // Destroy active run data
            var rd = (RunData)FindObjectOfType(typeof(RunData));
            if (rd != null)
            {
                Destroy(rd.gameObject);
            }
        }

        // Update is called once per frame
        public void Update()
        {
            if (!AppliedInitialImpulse)
            {
                var asteroids = FindObjectsOfType<BaseAsteroid>();
                foreach (var asteroid in asteroids)
                {
                    var rb = asteroid.GetComponent(typeof(Rigidbody)) as Rigidbody;
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
}
