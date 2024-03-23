using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    #region Fields of GameObjects
    private GameObject scriptsGO;
    private GameObject playerGO;
    #endregion Fields of GameObjects
    
    #region Fields of Components
    private GameManager gameManager;
    private Player player;
    #endregion Fields of Components

    void Awake()
    {
        scriptsGO = GameObject.Find("_scripts");
        if (scriptsGO == null)
            throw new System.Exception("Scripts générique not found");

        playerGO = GameObject.Find("Player");
        if (playerGO == null)
            throw new System.Exception("Player not found");

        gameManager = scriptsGO.GetComponent<GameManager>();
        player = playerGO.GetComponent<Player>();

        print($"Hello {player}");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // TODO Scriptable Object
        player.FloeLife += 1;
        player.Life += 1;
        gameManager.Score += 1;
        
        if (collider.gameObject.CompareTag("Waste"))
        {
            print("Well played");
        }

        print($"Collision !! {player} and {collider.gameObject.tag}");
    }
}
