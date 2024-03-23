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
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("FloatingObject"))
        {
            FloatingObject floatingObject = collider.gameObject.GetComponent<FloatingObject>();
            FloatingObjectSO floatingObjectSo = floatingObject.floatingObjectSO;

            if (floatingObjectSo.score != 0)
            {
                gameManager.AddScore(floatingObjectSo.score);
            }

            Destroy(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag("EndFloe"))
        {
            gameManager.EndReached();
        }
    }
}
