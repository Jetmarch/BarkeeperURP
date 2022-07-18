using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Difficulty", menuName = "Game difficulty preset")]
public class GameDifficultyPreset : ScriptableObject
{
    public string presetName;
    public Vector3 spawnMinCoords;
    public Vector3 spawnMaxCoords;
    public float spawnRate;
    [Range(0f, 1f)]
    public float chanceOfDoubleSpawn;
    public float minTossForce;
    public float maxTossForce;
    public float minTorqueForce;
    public float maxTorqueForce;
}
