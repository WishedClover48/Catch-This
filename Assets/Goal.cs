using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviourPunCallbacks
{
    [SerializeField] private int TeamGoal;
    [SerializeField] private ScoreManager Score;
    [SerializeField] private GameObject ball;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            AddScore();
            ball.transform.position = Vector3.zero;
        }
    }
    [ContextMenu("Score")]
    private void AddScore()
    {
        if (TeamGoal == 0)
            Score.AddScore(1, 0);
        else Score.AddScore(0, 1);

    }
}
