using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardService : MonoBehaviour
{
    public static void SubmitScore(int score, string leaderboardKey, System.Action<bool> onDone = null)
    {
        LootLockerSDKManager.SetPlayerName("nacho98", response =>
        {
            if (!response.success)
            {
                Debug.LogError("Fallo al setear el nombre");
                Debug.Log(response.errorData.message);
            }
        });
        LootLockerSDKManager.SubmitScore("", score, leaderboardKey, response =>
        {
            if (!response.success)
            {
                Debug.LogError("Fallo el score");
                onDone?.Invoke(false);

                return;
            }
            Debug.Log("Se envio el score");
            onDone?.Invoke(true);
        });
    }
    [ContextMenu("test submit")]
    private void TestSubmit()
    {
        SubmitScore(134, "gloabalmaxpoints", success =>
        {
            Debug.Log("Submit done: " + success);
        });
    }
}
