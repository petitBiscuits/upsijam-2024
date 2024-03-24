using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerColliderController : MonoBehaviour
{
    #region Fields of GameObjects
    private GameObject playerGO;
    #endregion Fields of GameObjects
    
    #region Fields of Components
    private GameManager gameManager;
    private Player player;
    [SerializeField]private List<GameObject> bears;
    [SerializeField]private List<GameObject> floes;
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
        ChangeBear(player.BearCount);
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
                var currentCount = player.BearCount;
                currentCount -= floatingObjectSo.playerDamage;
                currentCount = Mathf.Clamp(currentCount, 0, SettingsManager.Instance.MAX_BEAR);
                player.BearCount = currentCount;
                ChangeBear(player.BearCount);
            }
            if (floatingObjectSo.floeDamage != 0)
            {
                //player.FloeLife -= floatingObjectSo.floeDamage;
                //var maxBear = player.FloeLife * SettingsManager.Instance.MAX_BEAR_PER_FLOE;
                //player.BearCount = Mathf.Clamp(player.BearCount, 0, maxBear);
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
            SceneManager.LoadScene("HappyEndingScene");
        }
    }
    
    void ChangeBear(int count)
    {
        // get all recursive children name bear*
        var bearCount = bears.OrderBy(b => b.name).ToArray();
        
        if (count / (float)3 > 1)
        {
            floes[0].SetActive(true);
            //Change Bounds of boxCollider
            var boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.offset = new Vector2(-0.3f, 0);
            boxCollider.size = new Vector2(1.2f, 0.64f);
        }
        else
        {
            floes[0].SetActive(false);
            var boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.offset = new Vector2(0, 0);
            boxCollider.size = new Vector2(0.64f, 0.64f);
        };
        
        if (count / (float)3 > 2)
        {
            floes[1].SetActive(true);
            //Change Bounds of boxCollider
            var boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.offset = new Vector2(0f, 0);
            boxCollider.size = new Vector2(1.8f, 0.64f);
        }
        else
        {
            floes[1].SetActive(false);
            var boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.offset = new Vector2(0, 0);
            boxCollider.size = new Vector2(0.64f, 0.64f);
        };
        
        // hide all bear
        foreach (var bear in bearCount)
        {
            bear.SetActive(false);
        }

        for (int i = 0; i < count; i++)
        {
            bearCount[i].SetActive(true);
        }
    }
}
