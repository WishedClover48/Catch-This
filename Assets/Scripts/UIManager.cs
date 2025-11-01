using System;
using Photon.Pun;
using UnityEngine;

public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager Instance;
    [SerializeField] private SequenceActivator startingSequence;
    [SerializeField] private LeaderBoard leaderBoard;
    [SerializeField] private GameObject leaderBoardToggle;
    public event Action SequenceEnded;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        startingSequence.SequenceFinished += SequenceEnd;
        GameManager.Instance.RoundEnd += RoundEnd;
    }

    public void StartSequence()
    {
        startingSequence.StartSequence();
    }
    private void SequenceEnd()=> SequenceEnded?.Invoke();

    public void RoundEnd()
    {
        photonView.RPC("TurnOnLeaderBoard", RpcTarget.All);
    }
    [PunRPC]
    public void TurnOnLeaderBoard()=>leaderBoard.gameObject.SetActive(true);
    public void LeaderBoardToggleButton()=>leaderBoardToggle.SetActive(true);
}
