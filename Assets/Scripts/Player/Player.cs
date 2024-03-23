using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Fields
    private Vector2 movementInput;
    #endregion Fields

    #region Fields of Configuration
    [SerializeField] private float speed = 10f;
    [SerializeField] private int life = 1;
    [SerializeField] private int floeLife = 1;
    #endregion Fields of Configuration

    #region Fields of Components
    private GameControls controls;
    private Rigidbody2D rb;
    #endregion Fields of Components

    #region Properties
    public int Life
    {
        get { return life; }
        set
        {
            OnPlayerLife?.Invoke(this, life, value);
            life = value;
        }
    }

    public int FloeLife
    {
        get { return floeLife; }
        set
        {
            OnFloeLife?.Invoke(this, floeLife, value);
            floeLife = value;
        }
    }
    #endregion Properties

    #region Event
    public static event Action<Player, int, int> OnPlayerLife;
    public static event Action<Player, int, int> OnFloeLife;
    #endregion Event

    void Awake()
    {
        controls = new GameControls();
        rb = GetComponent<Rigidbody2D>();
    }
    
    
    void FixedUpdate()
    {
        movementInput = controls.Player.Movement.ReadValue<Vector2>();
        rb.velocity = movementInput * speed;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
