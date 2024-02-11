using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Software-Architecture/Level", order = 0)]
/// <summary>
/// This class is responsible for the level scriptable object.
/// It is used to store the waves and the time between waves for a level.
/// </summary>
public class LevelScriptableObject : ScriptableObject
{
    public List<WaveScriptableObject> waves = new List<WaveScriptableObject>();
    [Header("Time in between waves")] public int timeBetweenWaves = 5;
    [Header("How many times can you be hit before the level ends?")] public int health = 3;
}
