using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThiefAutomation : MonoBehaviour
{
    [SerializeField] private AnimalsThiefMovement _movement;

    private void OnEnable()
    {
        _movement.EndPositionReached += OnEndPositionReached;
    }

    private void OnDisable()
    {
        _movement.EndPositionReached -= OnEndPositionReached;
    }

    private void Start()
    {
        _movement.enabled = false;
        StartCoroutine(ThiefCooldown());
    }

    private IEnumerator ThiefCooldown()
    {
        const int secondsInOneMinute = 60;
        const int secondsInThreeMinutes = secondsInOneMinute * 3;
        const int secondsInFourMinutes = secondsInOneMinute * 4;
        int secondsBetweenSpawn = Random.Range(secondsInThreeMinutes, secondsInFourMinutes);
        //const int secondsBetweenSpawn = 40;
        var waitForSecondsBetweenSpawns = new WaitForSeconds(secondsBetweenSpawn);

        while (true)
        {
            yield return waitForSecondsBetweenSpawns;
            _movement.enabled = true;
        }
    }

    private void OnEndPositionReached()
    {
        _movement.enabled = false;
    }
}
