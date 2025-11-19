using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class DisconectionHandler : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _errorSign;
    [SerializeField] private GameObject _waitingSign;
    [SerializeField] private RoundsManager _roundsManager;

    private bool _playerReconnected = false;
    private bool _isWaiting = false;

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "has disconnected and its inactivity is detected as " + otherPlayer.IsInactive);
        HandlePlayerDisconnect(otherPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (_isWaiting) 
        { 
            _playerReconnected = true;
        }
    }

    private void HandlePlayerDisconnect(Player player)
    {
        if (_isWaiting) return;

        _isWaiting = true;
        PauseGame();
        StartCoroutine(PlayerLeft());
    }

    IEnumerator PlayerLeft()
    {
        _waitingSign.SetActive(true);
        _playerReconnected = false;

        yield return new WaitForSecondsRealtime(3);

        _waitingSign.SetActive(false);
        _isWaiting = false;

        ResumeGame();
        if (_playerReconnected)
        {
            yield break;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _roundsManager.EndRound();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
