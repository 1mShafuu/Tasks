using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using TMPro;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
   [SerializeField] private GameObject _leaderboardElementPrefab;
   [SerializeField] private Transform _parentObjectTransform;
   
   private List<GameObject> _spawnedElements = new List<GameObject>();
   private int _playerPlace;
   
   public void ConstructLeaderBoard(List<PlayerLeaderboardInfo> playersInfo)
   {
      ClearLeaderboard();

      foreach (var info in playersInfo)
      {
         GameObject leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _parentObjectTransform);

         leaderboardElementInstance.TryGetComponent(out PlayerLeaderboardComponent leaderboardElement);
         leaderboardElement.Initialize(_playerPlace.ToString(), info.Name, info.Score);

         _spawnedElements.Add(leaderboardElementInstance);
         _playerPlace++;
      }
   }
   
   private void ClearLeaderboard()
   {
      foreach (var element in _spawnedElements)
      {
         Destroy(element);
      }

      _spawnedElements = new List<GameObject>();
      _playerPlace = 1;
   }
}
