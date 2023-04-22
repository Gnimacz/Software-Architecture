using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Software-Architecture/Level", order = 0)]
public class LevelScriptableObject : ScriptableObject
{
    [SerializeField] public List<WaveScriptableObject> waves = new List<WaveScriptableObject>();
    [SerializeField][Header("Time in between waves")] public int timeBetweenWaves = 5;
    [SerializeField][Header("How many times can you be hit before the level ends?")] public int health = 3;
}
