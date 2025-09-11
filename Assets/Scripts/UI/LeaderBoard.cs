using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    private Dictionary<int,PLayerScore> _playerScores=new Dictionary<int,PLayerScore>();

    public Dictionary<int,PLayerScore> GetLeaderBoard()
    {
       return new Dictionary<int, PLayerScore>(_playerScores.OrderBy(x=>x.Value.Score));
    }
    public void AddPlayerScore(int id,string name,int score)
    {
        _playerScores.Add(id,new PLayerScore() { Name = name, Score = score });
    }
    public void RemovePlayerScore(int id)
    {
        _playerScores.Remove(id);
    }
    public void ClearLeaderBoard()
    {
        _playerScores.Clear();
    }

    public void UpdateLeaderBoard(int id, string name, int score)
    {
        _playerScores[id] = new PLayerScore() { Name = name, Score = score };
    }
}

public class PLayerScore
{
    public string Name;
    public int Score;
}