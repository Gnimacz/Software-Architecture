using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Software-Architecture/Level", order = 0)]
public class LevelScriptableObject : ScriptableObject
{
    [SerializeField] public List<WaveScriptableObject> waves = new List<WaveScriptableObject>();
    [SerializeField][Header("Time in between waves")] protected int timeBetweenWaves = 5;
}
