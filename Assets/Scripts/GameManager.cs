using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Combat.Asteroid;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField]
    private int _Stage = 1;
    
    [SerializeField]
    private int _Wave = 1;
    
    [SerializeField]
    private int _MaxWave = 2;
    
    [SerializeField]
    private int _SpawnsPerWave = 10;
    
    [SerializeField]
    private int _Spawns = 0;
    
    [SerializeField]
    private TMP_Text _WaveText;
    
    [SerializeField]
    private Animator _WaveTextAnimator;
    
    [SerializeField]
    private float _waveCooldown = 4;
    
    [SerializeField]
    private float _spawnCooldown = 5;
    
    private float _waveCooldownTimer = 4;
    private float _spawnCooldownTimer = 0;
    private bool _isWaveOn = false;

    private List<BaseAsteroid> ActiveAsteroids = new List<BaseAsteroid>();

    // Start is called before the first frame update
    public virtual void Start()
    {
        Instance = this;
        _MaxWave = 2 + _Stage / 5;
        _SpawnsPerWave = 5 + _Stage / 2;

        _WaveText.text = "Wave " + _Wave.ToString() + "/" + _MaxWave.ToString();
    }

    // Update is called once per frame
    // TODO: Refactor with coroutines
    public virtual void Update()
    {
        if (!_isWaveOn)
        {
            _waveCooldownTimer -= Time.deltaTime;

            if (_waveCooldownTimer <= 0)
            {
                _isWaveOn = true;
                _waveCooldownTimer = _waveCooldown;
                SetWaveText();
            }
        }

        else
        {
            _spawnCooldownTimer -= Time.deltaTime;

            if (_spawnCooldownTimer <= 0 && _Spawns < _SpawnsPerWave)
            {
                _Spawns++;
                SpawnAsteroid();
                _spawnCooldownTimer = _spawnCooldown;
            }

            if (ActiveAsteroids.Count == 0 && _Spawns == _SpawnsPerWave)
            {
                _isWaveOn = false;
                _Spawns = 0;
                _Wave++;
                if (_Wave <= _MaxWave)
                {
                    SetWaveText();
                    _WaveTextAnimator.Play("ShowWaveCounter", 0, 0);
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
        _WaveText.text = "Wave " + _Wave.ToString() + "/" + _MaxWave.ToString();
    }

    // Spawns an asteroid beyond the max range of the camera and applies a random force to it
    // TODO: Pooling
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
        var xRange = Camera.main.orthographicSize * Camera.main.aspect * 1.5f;
        var yRange = Camera.main.orthographicSize * 1.5f;

        var axis = Random.Range(-1f, 1f) > 0 ? true : false;
        var mult = Random.Range(-1f, 1f) > 0 ? 1 : -1;

        if (axis)
        {
            return new Vector3(Random.Range(-xRange, xRange), yRange * mult, 0);
        }
        return new Vector3(xRange * mult, Random.Range(-yRange, yRange), 0);

    }
    
    public static BaseAsteroid GetClosestAsteroid(Vector3 position)
    {
        BaseAsteroid closest = null;
        foreach (var asteroid in Instance.ActiveAsteroids.Where(a => !a.IsDead))
        {
            if ((closest.transform.position - position).sqrMagnitude > (asteroid.transform.position - position).sqrMagnitude)
            {
                closest = asteroid;
            }
        }
        return closest;
    }

    public static void RemoveAsteroid(BaseAsteroid asteroid)
    {
        Instance.ActiveAsteroids.Remove(asteroid);
    }

    public static void AddAsteroid(BaseAsteroid asteroid)
    {
        Instance.ActiveAsteroids.Add(asteroid);
    }
}
