using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [SerializeField] private GameObject _oceanTilePrefab;
    [SerializeField] private List<GameObject> _floatingObjects = new List<GameObject>(); 
    
    [SerializeField] private float _oceanSpeed;
    
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
            oceanTile.GetComponent<SpriteRenderer>().material.color = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1), 1);
            
            AddObjectsToTile(oceanTile);
            
            _oceanTiles.Add(oceanTile);
        }
    }

    private void AddObjectsToTile(GameObject oceanTile)
    {
        foreach (var floatingObject in _floatingObjects)
        {
            var floatingObjectInstance = Instantiate(floatingObject, oceanTile.transform);
            floatingObjectInstance.transform.localPosition = new Vector3(0, Random.Range(-heightTile / 2, heightTile / 2), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveEverything();
        
        RemoveAndCreateNewTiles();
    }

    private void MoveEverything()
    {
        foreach (var oceanTile in _oceanTiles)
        {
            oceanTile.transform.position += new Vector3(_oceanSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void RemoveAndCreateNewTiles()
    {
        
        float startX = -screenWidth / 2 - widthTile;
        
        if (_oceanTiles[0].transform.position.x < startX - widthTile)
        {
            Destroy(_oceanTiles[0]);
            _oceanTiles.RemoveAt(0);
            
            var oceanTile = Instantiate(_oceanTilePrefab, transform);
            oceanTile.transform.position = new Vector3(_oceanTiles[_oceanTiles.Count - 1].transform.position.x + widthTile, 0, 0);
            oceanTile.GetComponent<SpriteRenderer>().material.color = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1), 1);
            AddObjectsToTile(oceanTile);
            _oceanTiles.Add(oceanTile);
        }
    }
}
