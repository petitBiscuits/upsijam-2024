using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    LevelStage,
    EndLevelStage,
    EndScreen,
    UnlimitedStage,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    #region Fields
    public GameState GameState = GameState.LevelStage;
    private bool isEndReached;
    private int score = 0;

    #endregion Fields

    #region Fields of Components
    [SerializeField] private Player player;
    #endregion Fields of Components

    #region Properties

    [SerializeField] private int _currentScore;
    [SerializeField] private int _score;
    private Multi _multi = new Multi();
    private float _timerUpdateScore;

    #endregion Properties

    #region Event
    // 1. [GameManager] is the game manager
    public event Action<bool, int> OnScoreChange;
    
    // 1. [float] is the distance traveled by the player
    public event Action<float> OnDistanceChange;
    
    #endregion Event
    
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
    }

    void FixedUpdate()
    {
    }

    private void OnFloeLifeChange(Player player, int before, int now)
    {
        print($"Floe life change from {before} to {now}");
    }

    private void OnBearCountChange(Player player, int before, int now)
    {
        print($"Player life change from {before} to {now}");
        if (now <= 0)
            print("LOSE !!!");

        if (now != before)
        {
            var op = MultiOperation.Decrease;
            if (now > before)
                op = MultiOperation.Increase;
            _multi.UpdateMulti(op);
        }

    }

    private void Start()
    {
        Player.OnBearCount += OnBearCountChange;
        Player.OnFloeLife += OnFloeLifeChange;
        
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
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            _multi.UpdateMulti(MultiOperation.Increase);
        }
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            _multi.UpdateMulti(MultiOperation.Decrease);
        } 
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            GameState = GameState.EndLevelStage;
        }
        
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
            print($"{_multi.Value}, {SettingsManager.Instance.MAX_MULTI},  {_multi.Value == SettingsManager.Instance.MAX_MULTI}");
            OnScoreChange?.Invoke(_multi.Value==SettingsManager.Instance.MAX_MULTI, _currentScore);
            yield return new WaitForSeconds(speed);
        }
    }

    public void AddScore(int floatingObjectScore)
    {
        _score += floatingObjectScore;
    }

    public void EndReached()
    {
        GameState = GameState.EndScreen;
    }

    public void InvokeDistanceChange(float distance)
    {
        OnDistanceChange?.Invoke(distance);
    }
}
