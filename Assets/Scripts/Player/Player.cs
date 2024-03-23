using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    #region Fields
    private Vector2 movementInput;
    #endregion Fields

    #region Fields of Configuration
    [SerializeField] private float speed = 10f;
    [FormerlySerializedAs("life")] [SerializeField] private int bearCount = 1;
    [SerializeField] private int floeLife = 1;
    #endregion Fields of Configuration

    #region Fields of Components
    private GameControls controls;
    private Rigidbody2D rb;
    #endregion Fields of Components

    #region Properties
    public int BearCount
    {
        get { return bearCount; }
        set
        {
            if (value != bearCount) {
                OnBearCount?.Invoke(this, bearCount, value);
                bearCount = value;
            }
        }
    }

    public int FloeLife
    {
        get { return floeLife; }
        set
        {
            if (value != floeLife)
            {
                OnFloeLife?.Invoke(this, floeLife, value);
                floeLife = value;
            }
        }
    }
    #endregion Properties

    #region Event
    public static event Action<Player, int, int> OnBearCount;
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

        // Limit movement to the visible screen
        var topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        var bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        var position = transform.position;
        position.x = Mathf.Clamp(position.x, bottomLeft.x, topRight.x);
        position.y = Mathf.Clamp(position.y, bottomLeft.y, topRight.y);

        var velocity = movementInput * speed;
        if (position.x <= bottomLeft.x)
            velocity.x = Mathf.Max(0, velocity.x);
        if (position.x >= topRight.x)
            velocity.x = Mathf.Min(0, velocity.x);
        if (position.y <= bottomLeft.y)
            velocity.y = Mathf.Max(0, velocity.y);
        if (position.y >= topRight.y)
            velocity.y = Mathf.Min(0, velocity.y);

        // Clamp the velocity to a maximum value
        rb.velocity = Vector2.ClampMagnitude(velocity, speed);
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
