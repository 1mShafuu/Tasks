using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private const float ScoreStartMultiplier = 20f;
    
    private float _elapsedTime;
    private float _multiplier;
    
    public int Score { get; private set; }

    private void Awake()
    {
        _elapsedTime = 0;
        _multiplier = ScoreStartMultiplier;
    }

    private void Update()
    {
        const float scoreMultiplierDecrease = 0.0000001f;

        _elapsedTime += Time.deltaTime;
        _multiplier -= _elapsedTime * scoreMultiplierDecrease;
        _multiplier = Math.Max(1f, _multiplier);
        Score = Convert.ToInt32(Game.MaxPoints * _multiplier);
    }

    public void ResetScore()
    {
        _elapsedTime = 0;
        _multiplier = ScoreStartMultiplier;
    }
}
