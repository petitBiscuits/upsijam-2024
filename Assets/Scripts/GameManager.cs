using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields
    private int score = 0;
    #endregion Fields
    
    #region Fields of Components
    [SerializeField] private Player player;
    #endregion Fields of Components

    #region Properties
    public int Score
    {
        get { return score; }
        set
        {
            OnScoreChange?.Invoke(this, score, value);
            score = value;
        }
    }
    #endregion Properties

    #region Event
    public event Action<GameManager, int, int> OnScoreChange;
    
    
    
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
        
        Player.OnPlayerLife += OnPlayerLifeChange;
        Player.OnFloeLife += OnFloeLifeChange;
    }

    private void OnFloeLifeChange(Player player, int before, int now)
    {
        print($"Floe life change from {before} to {now}");
    }

    private void OnPlayerLifeChange(Player player, int before, int now)
    {
        print($"Player life change from {before} to {now}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
