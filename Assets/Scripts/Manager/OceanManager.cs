using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [SerializeField] private GameObject _oceanTilePrefab;
    [SerializeField] private GameObject _oceanFloePrefab;
    [SerializeField] private GameObject _floePrefab;
    private bool _isFirstLayer = true;
    private int _countDown = 15;
    [SerializeField] private List<GameObject> _floatingObjects = new List<GameObject>();
    [SerializeField] private List<int> _floatingObjectsWeight = new();

    [SerializeField] private float _oceanSpeed;
    [SerializeField] private float _maxToSpawn;

    private List<GameObject> _oceanTiles = new List<GameObject>();

    private float widthTile;
    private float heightTile;
    private float screenWidth;

    // Start is called before the first frame update
    void Start()
    {
        widthTile = _oceanTilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        heightTile = _oceanTilePrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

        InitOcean();

        GameManager.Instance.OnScoreChange += Instance_OnScoreChange;
    }

    private void Instance_OnScoreChange(GameManager sender, bool isMultiMax, int score)
    {
        var speedMultiplier = ScoreManager.CalcOceanSpeedMultiplier(score);
        _oceanSpeed = SettingsManager.Instance.DEFAULT_OCEAN_SPEED * speedMultiplier;
        _maxToSpawn = ScoreManager.CalcMaxToSpawn(score); ;
    }

    private void InitOcean()
    {

        int tiles = Mathf.CeilToInt(screenWidth / widthTile) + 5;

        float startX = -screenWidth / 2 - widthTile;

        for (int i = 0; i < tiles; i++)
        {
            // spawn inside this GameObject
            var oceanTile = Instantiate(_oceanTilePrefab, transform);
            oceanTile.transform.position = new Vector3(startX + i * widthTile, 0, 0);

            _oceanTiles.Add(oceanTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Add default weight for missing ones
        if (_floatingObjects.Count > _floatingObjectsWeight.Count)
        {
            for (int i = 0; i < _floatingObjects.Count - _floatingObjectsWeight.Count; i++)
            {
                _floatingObjectsWeight.Add(SettingsManager.Instance.DEFAULT_SPAWN_WEIGHT);
            }
        }

        MoveEverything();

        RemoveAndCreateNewTiles();

    }

    private void MoveEverything()
    {
        foreach (var oceanTile in _oceanTiles)
        {
            oceanTile.transform.position -= new Vector3(_oceanSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void RemoveAndCreateNewTiles()
    {

        float startX = -screenWidth / 2 - widthTile;

        if (_oceanTiles[0].transform.position.x < startX - widthTile)
        {
            Destroy(_oceanTiles[0]);
            _oceanTiles.RemoveAt(0);

            if (GameManager.Instance.GameState == GameState.EndLevelStage && _countDown <= 0)
            {
                if (_isFirstLayer)
                {
                    _isFirstLayer = false;
                    AddNewLayer(_oceanFloePrefab);
                }
                AddNewLayer(_floePrefab);
                return;
            }
            else if (GameManager.Instance.GameState == GameState.EndLevelStage)
            {
                _countDown--;
            }
            AddNewLayer(_oceanTilePrefab);
        }
    }

    private void AddNewLayer(GameObject oceanTilePrefab)
    {
        var oceanTile = Instantiate(oceanTilePrefab, transform);
        oceanTile.transform.position = new Vector3(_oceanTiles[_oceanTiles.Count - 1].transform.position.x + widthTile, 0, 0);
        AddObjectsToTile(oceanTile);
        _oceanTiles.Add(oceanTile);
    }

    private void AddObjectsToTile(GameObject oceanTile)
    {
        if (GameManager.Instance.GameState != GameState.LevelStage) return;

        int nbToSpawn = 0;
        for (int i = 0; i < _maxToSpawn; i++)
        {
            float spawnProba = SettingsManager.Instance.DEFAULT_SPAWN_PROBA;
            var rnd = Random.Range(0, 100);
            print($"rnd: {rnd} spawnProba: {spawnProba}");
            if (rnd < spawnProba)
            {
                nbToSpawn++;
            }
        }

        var totalWeight = _floatingObjectsWeight.Sum(e => e);

        for (int spawnIdx = 0; spawnIdx < nbToSpawn; spawnIdx++)
        {
            var remainingWeight = Random.Range(0, totalWeight) + 1;

            var objectToSpawn = _floatingObjects[0];
            for (var i = 0; i < _floatingObjects.Count; i++)
            {
                var removedWeight = _floatingObjectsWeight[i];

                remainingWeight -= removedWeight;
                if (remainingWeight <= 0)
                {
                    objectToSpawn = _floatingObjects[i];
                    break;
                }
            }

            var floatingObjectInstance = Instantiate(objectToSpawn, oceanTile.transform);
            // the position need to be pixel perfect with the tile

            floatingObjectInstance.transform.localPosition = new Vector3(0, widthTile * (int)Random.Range(-heightTile / 2, heightTile / 2), 0);
        }
    }
}
