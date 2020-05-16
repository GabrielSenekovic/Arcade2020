using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum EnemyType
{
    CREGG = 0,
    SPNOG = 1,

    SCORPACA = 2
}

[System.Serializable]public struct EnemyScoreEntry
{
    public string name;
    public int value;
}
public class Score : MonoBehaviour
{
    Text scoreText;
    public int score = 0;

    void Awake() 
    {
        scoreText = GetComponentInChildren<Text>();
    }

    public EnemyScoreEntry[] enemyScores;
    public void GetScoreFromEnemy(EnemyType type)
    {
        score += enemyScores[(int)type].value;
        scoreText.text = "Score: " + score;
    }
}
