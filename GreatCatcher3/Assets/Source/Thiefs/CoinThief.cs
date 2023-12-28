using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinThief : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    private void Start()
    {
        StartCoroutine(Theft());
    }

    private IEnumerator Theft()
    {
        const int secondsInOneMinute = 60;
        const int secondsBetweenSpawn = secondsInOneMinute * 3;
        var waitForSecondsBetweenSpawns = new WaitForSeconds(secondsBetweenSpawn);

        while (true)
        {
            yield return waitForSecondsBetweenSpawns;

            const int minPercent = 0;
            const int maxPercent = 100;
            int successfulTheft = 40;
            int probabilityOfStolenMoney = Random.Range(minPercent,maxPercent);
            
            if (probabilityOfStolenMoney <= successfulTheft)
            {
                const float minStolenMoney = 0.15f;
                const float maxStolenMoney = 0.25f;

                float stolenMoneyModifier = Random.Range(minStolenMoney, maxStolenMoney);
                var stolenMoney = (int)(_wallet.Money * stolenMoneyModifier);
                
                _wallet.ChangeMoney(-stolenMoney);
            }
        }
    }
}
