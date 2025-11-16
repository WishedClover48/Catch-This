using LootLocker.Requests;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreLeatherboard : MonoBehaviour
{
    [SerializeField] private PlayerScoreUI UITemplate;
    [SerializeField] private Color firstPlaceColor;
    [SerializeField] private Color secondPlaceColor;
    [SerializeField] private Color thirdPlaceColor;
    [SerializeField] private Color normalPlaceColor;

    [SerializeField] string leaderboardKey = "gloabalmaxpoints";
    [SerializeField] int count = 10;
    [SerializeField] LootLockerLeaderboardMember[] members;
    private List<PlayerScoreUI> Scores=new List<PlayerScoreUI>();
    bool FinishedLoading = false;
    private void Start()
    {
        StartCoroutine(SetLeatherBoard());
    }
    private void UpdateLeatherBoard()
    {
        
        LootLockerSDKManager.GetScoreList(leaderboardKey, count, 0, response =>
        {
            if (!response.success)
            {
                Debug.LogError("Error al cargar el leaderboard");
            }

            var items = response.items;

            if (items != null && items.Length != 0)
                 members = response.items;
        });
        if (members == null) return;
        for (int i = 0; i < members.Length; i++)
        {
            if(Scores.Count<i+1)
                AddPlayerScore(members[i]);
            else
            {
                Scores[i].SetPlayerScore(members[i].rank, members[i].player.name, members[i].score);
            }
        }
        FinishedLoading=true;
    }
    public void AddPlayerScore(LootLockerLeaderboardMember score)
    {
        var test =Instantiate(UITemplate, transform);
        test.SetPlayerScore(score.rank, score.player.name, score.score);
        Scores.Add(test);

    }
    IEnumerator SetLeatherBoard()
    {
        while (!FinishedLoading)
        {
            UpdateLeatherBoard();
            yield return new WaitForSeconds(1f);
        }
        while (true)
        {
            UpdateLeatherBoard();
            yield return new WaitForSeconds(2f);
        }
    }
}
