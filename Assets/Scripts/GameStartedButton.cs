using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartedButton : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        
        SceneManager.LoadScene("MainMenu");
    }
}
