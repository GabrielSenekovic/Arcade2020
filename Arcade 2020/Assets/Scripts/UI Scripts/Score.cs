using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyType
{
    Cregg = 0,
    Spnog = 1,

    Skorpacka = 2
}

[System.Serializable]public struct EnemyScoreEntry
{
    public string name;
    public int value;
}
public class Score : MonoBehaviour
{
    int score = 0;

    public EnemyScoreEntry[] enemyScores;
    public void GetScoreFromEnemy(EnemyType type)
    {
        score += enemyScores[(int)type].value;
    }
}
