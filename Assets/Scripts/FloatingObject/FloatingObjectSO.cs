using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatingObjectSO : ScriptableObject
{
    public GameObject prefab;
    public List<Sprite> sprites = new(); // if set, will take randomly from the list
    public int score = 0; // Negative for decreasing
    public int playerDamage = 0; // Negative for healing
    public int floeDamage = 0; // Negative for healing
    
    
}
