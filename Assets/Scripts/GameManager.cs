using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Fields
    private int score = 0;
    [SerializeField] private List<FloatingObjectSO> availableFloatingObjectSO = new();
    /// <summary>
    /// Same index as <see cref="availableFloatingObjectSO"/>
    /// </summary>
    [SerializeField] private List<int> availableFloatingObjectSpawnWeight = new();
    /// <summary>
    /// If <see cref="availableFloatingObjectSpawnWeight"/> lenght is not enough, it will take this value
    /// </summary>
    [SerializeField] private int defaultSpawnWeight = 1;
    [SerializeField] private float travelSpeed = 2;
    [SerializeField] private float traveledDistance = 0;
    [SerializeField] private float nextSpawnInDistance = 10;

    [SerializeField] private int minSpawnInDistance = 5;
    [SerializeField] private int maxSpawnInDistance = 15;

    [SerializeField] private int spawnPositionOffset = 15;

    #endregion Fields

    #region Fields of Components
    [SerializeField] private Player player;
    #endregion Fields of Components

    #region Fields of GameObjects
    [SerializeField] private GameObject oceanGO;
    #endregion Fields of GameObjects

    #region Properties

    private int _currentScore;
    private int _score;
    private Multi _multi = new Multi();
    private float _timerUpdateScore;

    public List<FloatingObjectSO> AvailableFloatingObjectSO
    {
        get { return availableFloatingObjectSO; }
        set { availableFloatingObjectSO = value; }
    }
    public Dictionary<GameObject, FloatingObjectSO> FloatingObjects { get; set; } = new();

    #endregion Properties

    #region Event
    // 1. [GameManager] is the game manager
    public event Action<GameManager, bool, int> OnScoreChange;
    
    // 1. [float] is the distance traveled by the player
    public event Action<float> OnDistanceChange;
    
    #endregion Event
    
    private float distance = 0;
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        Player.OnPlayerLife += OnPlayerLifeChange;
        Player.OnFloeLife += OnFloeLifeChange;
    }

    void FixedUpdate()
    {
        // Add default weight for missing ones
        if (availableFloatingObjectSO.Count > availableFloatingObjectSpawnWeight.Count)
        {
            for (int i = 0; i < availableFloatingObjectSO.Count - availableFloatingObjectSpawnWeight.Count; i++)
            { 
                availableFloatingObjectSpawnWeight.Add(defaultSpawnWeight);
            }
        }

        // Distance traveled
        var traveled = travelSpeed * Time.fixedDeltaTime;
        oceanGO.transform.Translate(Vector3.left * traveled);
        traveledDistance += traveled;
        nextSpawnInDistance -= traveled;
        
        // Floating spawn
        var leftScreenBorder = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        var minSpawnY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1)).y;
        var maxSpawnY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).y;

        if (nextSpawnInDistance <= 0)
        {
            nextSpawnInDistance += UnityEngine.Random.Range(minSpawnInDistance, maxSpawnInDistance);

            var totalWeight = availableFloatingObjectSpawnWeight.Sum(e => e);
            var remainingWeight = UnityEngine.Random.Range(0, totalWeight) + 1;
            
            FloatingObjectSO objectToSpawn = availableFloatingObjectSO[0];
            for (var i = 0; i < availableFloatingObjectSO.Count; i++)
            {
                var removedWeight = availableFloatingObjectSpawnWeight[i];

                remainingWeight -= removedWeight;
                if (remainingWeight <= 0)
                {
                    objectToSpawn = availableFloatingObjectSO[i];
                    break;
                }
            }
            

            var position = new Vector3(leftScreenBorder + spawnPositionOffset, UnityEngine.Random.Range(minSpawnY, maxSpawnY), 0);
            var floatingObject = Instantiate(objectToSpawn.prefab, position, Quaternion.identity);
            floatingObject.transform.SetParent(oceanGO.transform);

            FloatingObjects.Add(floatingObject, objectToSpawn);
        }
    }

    private void OnFloeLifeChange(Player player, int before, int now)
    {
        print($"Floe life change from {before} to {now}");
    }

    private void OnPlayerLifeChange(Player player, int before, int now)
    {
        print($"Player life change from {before} to {now}");
    }

    private void Start()
    {
        StartCoroutine(UpdateScore());
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _score += 100*_multi.Value;
            print(_score);
        }
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            _multi.UpdateMulti(MultiOperation.Increase);
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            _multi.UpdateMulti(MultiOperation.Decrease);
        }
        
        distance += Time.deltaTime;
        OnDistanceChange?.Invoke(distance);
    }
    
    IEnumerator UpdateScore()
    {
        while (true)
        {
            float speed = 1 / (float)_multi.Value;
            if (Mathf.Abs(_currentScore - _score) < 50)
            {
                _currentScore = _score;
            }
            else
            {
                _currentScore = (int)Mathf.Lerp(_currentScore, _score, 0.2f);
            }
            OnScoreChange?.Invoke(this, _multi.Value==SettingsManager.Instance.MAX_MULTI, _currentScore);
            yield return new WaitForSeconds(speed);
        }
    }

    public void AddScore(int floatingObjectScore)
    {
        _score += floatingObjectScore;
    }
}
