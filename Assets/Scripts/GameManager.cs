using System.Collections;
using System.Collections.Generic;
using Combat.Asteroid;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int Stage = 1;
    public int Wave = 1;
    public int MaxWave = 2;
    public int SpawnsPerWave = 10;
    public int Spawns = 0;

    public float WaveCooldown = 4;
    public float WaveCooldownTimer = 4;
    public float SpawnCooldown = 5;
    public float SpawnCooldownTimer = 0;
    public bool IsWaveOn = false;

    public List<BaseAsteroid> ActiveAsteroids = new List<BaseAsteroid>();
    public static GameManager Instance;
    public TMP_Text WaveText;
    public Animator WaveTextAnimator;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Instance = this;
        MaxWave = 2 + Stage / 5;
        SpawnsPerWave = 5 + Stage / 2;

        WaveText.text = "Wave " + Wave.ToString() + "/" + MaxWave.ToString();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!IsWaveOn)
        {
            WaveCooldownTimer -= Time.deltaTime;

            if (WaveCooldownTimer <= 0)
            {
                IsWaveOn = true;
                WaveCooldownTimer = WaveCooldown;
                SetWaveText();
            }
        }

        else
        {
            SpawnCooldownTimer -= Time.deltaTime;

            if (SpawnCooldownTimer <= 0 && Spawns < SpawnsPerWave)
            {
                Spawns++;
                SpawnAsteroid();
                SpawnCooldownTimer = SpawnCooldown;
            }

            if (ActiveAsteroids.Count == 0 && Spawns == SpawnsPerWave)
            {
                IsWaveOn = false;
                Spawns = 0;
                Wave++;
                if (Wave <= MaxWave)
                {
                    SetWaveText();
                    WaveTextAnimator.Play("ShowWaveCounter", 0, 0);
                }
                else
                {
                    SceneManager.LoadScene("Store", LoadSceneMode.Single);
                }
            }
        }
    }

    // Pretty obvious
    private void SetWaveText()
    {
        WaveText.text = "Wave " + Wave.ToString() + "/" + MaxWave.ToString();
    }

    // Spawns an asteroid beyond the max range of the camera and applies a random force to it
    private void SpawnAsteroid()
    {
        Vector3 position = GenerateAsteroidPosition();
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        BaseAsteroid newAsteroid = null;
        
        if (Random.Range(0f, 1f) > 0.5f)
        {
            newAsteroid = AsteroidPrefabManager.InstantiateBaseAsteroid(3, position, Quaternion.identity);
        }
        else
        {
            newAsteroid = AsteroidPrefabManager.InstantiateMagneticAsteroid(3, position, Quaternion.identity);
        }
        
        newAsteroid.asteroidRigidbody.AddForceAtPosition(offset * 100, position + offset);
    }

    // Creates a Vector3 beyond the max range of the camera
    private Vector3 GenerateAsteroidPosition()
    {
        float xRange = Camera.main.orthographicSize * Camera.main.aspect * 1.5f;
        float yRange = Camera.main.orthographicSize * 1.5f;

        bool axis = Random.Range(-1f, 1f) > 0 ? true : false;
        int mult = Random.Range(-1f, 1f) > 0 ? 1 : -1;

        if (axis)
        {
            return new Vector3(Random.Range(-xRange, xRange), yRange * mult, 0);
        }
        else
        {
            return new Vector3(xRange * mult, Random.Range(-yRange, yRange), 0);

        }
    }
}
