
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // Range of the distance with step of 1000
    
    public float DISTANCE_MAX = 10000;
    public int MAX_MULTI = 16;
    public int LEVEL_SCORE_DIVIDER = 500;
    public int DEFAULT_OCEAN_SPEED = 5;
    public int DEFAULT_SPAWN_WEIGHT = 1;
    public int DEFAULT_SPAWN_PROBA = 40; // 40%
    public int MAX_TO_SPAWN = 5;
    public int MAX_BEAR_PER_FLOE = 2;

    public static SettingsManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}