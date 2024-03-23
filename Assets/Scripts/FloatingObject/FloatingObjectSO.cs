using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatingObjectSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public int score = 0; // Negative for decreasing
    public int playerDamage = 0; // Negative for healing
    public int floeDamage = 0; // Negative for healing
}
