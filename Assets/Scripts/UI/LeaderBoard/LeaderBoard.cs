using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    private readonly Dictionary<Player,PlayerScoreUI> _playerScores=new Dictionary<Player,PlayerScoreUI>();
    [SerializeField]private PlayerScoreUI UITemplate;
    [SerializeField] private Color firstPlaceColor;
    [SerializeField] private Color secondPlaceColor;
    [SerializeField] private Color thirdPlaceColor;
    [SerializeField] private Color normalPlaceColor;
    
    
    private void Start()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            AddPlayerScore(player);
        }

        foreach (var score in _playerScores)
        {
            score.Value.SetPlayerScore(1,score.Key.NickName,score.Key.GetScore());
        }
        InvokeRepeating("UpdateLeaderBoard",1,1);
    }

    public Dictionary<Player,PlayerScoreUI> GetLeaderBoard()
    {
        return new Dictionary<Player, PlayerScoreUI>(_playerScores.OrderBy(x => x.Key.GetScore()));
    }
    public void AddPlayerScore(Player player)
    {
        _playerScores.Add(player,Instantiate(UITemplate,transform));
    }
    public void RemovePlayerScore(Player player)
    {
        _playerScores.Remove(player);
    }
    public void ClearLeaderBoard()
    {
        _playerScores.Clear();
    }

    public void UpdateLeaderBoard()
    {
        var sortedPlayers = _playerScores.Keys.OrderByDescending(p => p.GetScore()).ToList();
    
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            var player = sortedPlayers[i];
            int position = i + 1; // Positions start from 1
            int score = player.GetScore();
            _playerScores[player].UpdateScoreAndPosition(position, score);
            _playerScores[player].UpdateColor(GetColor(position));
            _playerScores[player].transform.SetSiblingIndex(position-1);
        }
        

    }

    private Color GetColor(int position)
    {
        switch (position)
        {
            case 1:return firstPlaceColor;
            case 2:return secondPlaceColor;
            case 3:return thirdPlaceColor;
            default:return normalPlaceColor;
        }
    }
    private int GetPLayerPosition(Player player)
    {
        var sortedPlayers = _playerScores.Keys.OrderByDescending(p => p.GetScore()).ToList();
        int position = sortedPlayers.IndexOf(player);
        return position >= 0 ? position + 1 : -1; // Returns -1 if player not found
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int num = 10;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                player.AddScore(num);
                num++;
            }
            UpdateLeaderBoard();
        }
    }
}