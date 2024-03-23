using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    #region Fields of GameObjects
    private GameObject playerGO;
    #endregion Fields of GameObjects
    
    #region Fields of Components
    private GameManager gameManager;
    private Player player;
    #endregion Fields of Components

    void Awake()
    {
        playerGO = GameObject.Find("Player");
        if (playerGO == null)
            throw new System.Exception("Player not found");

        player = playerGO.GetComponent<Player>();

        print($"Hello {player}");
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (gameManager.FloatingObjects.ContainsKey(collider.gameObject))
        {
            var floatingObject = gameManager.FloatingObjects[collider.gameObject];
            player.FloeLife -= floatingObject.floeDamage;
            player.Life -= floatingObject.playerDamage;
            gameManager.AddScore(floatingObject.score);
        }
        else if (collider.gameObject.CompareTag("EndFloe"))
        {
            gameManager.ReachedEnd();
        }

        print($"Collision !! {player} and {collider.gameObject.tag}");
    }
}
