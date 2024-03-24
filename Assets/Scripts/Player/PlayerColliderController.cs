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
            if (floatingObjectSo.playerDamage != 0)
            {
                player.BearCount -= floatingObjectSo.playerDamage;
            }
            if (floatingObjectSo.floeDamage != 0)
            {
                player.FloeLife -= floatingObjectSo.floeDamage;
                var maxBear = player.FloeLife * SettingsManager.Instance.MAX_BEAR_PER_FLOE;
                player.BearCount = Mathf.Clamp(player.BearCount, 0, maxBear);
            }

            if (floatingObject.TryGetComponent<AudioSource>(out var audioSource))
            {
                var audioPlayer = GameObject.FindGameObjectWithTag("AudioPlayer").GetComponent<AudioSource>();
                audioPlayer.clip = audioSource.clip;
                audioPlayer.volume = audioSource.volume;
                audioPlayer.Play();
            }

            Destroy(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag("EndFloe"))
        {
            gameManager.EndReached();
        }
    }
}
