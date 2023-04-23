using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Software-Architecture/WaveScriptableObject", order = 0)]
public class WaveScriptableObject : ScriptableObject
{
    public List<GameObject> EnemiesToSpawn = new List<GameObject>();
    public float TimeBetweenSpawns = 1f;
}
