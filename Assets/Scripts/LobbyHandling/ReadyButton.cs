using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

/// <summary>
/// Handles the "Ready" button logic for the local player.
/// Toggles a custom property ("Ready") on PhotonNetwork.LocalPlayer.
/// Updates the button text accordingly.
/// </summary>
public class ReadyButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button readyButton;          
    [SerializeField] private TMPro.TMP_Text buttonText;   

    private void Start()
    {
        UpdateButtonText();
    }

    public void OnReadyClicked()
    {
        bool isReady = false;

        // If the player already has a "Ready" property, read its value
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out object readyValue))
        {
            isReady = (bool)readyValue;
        }

        // Create a new properties table with the opposite value (toggle)
        Hashtable props = new Hashtable { { "Ready", !isReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        bool isReady = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out object readyValue)
                       && (bool)readyValue;

        buttonText.text = isReady ? "Unready" : "Ready";
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // If the updated player is the local one, update the button text
        if (targetPlayer == PhotonNetwork.LocalPlayer && changedProps.ContainsKey("Ready"))
        {
            UpdateButtonText();
        }
    }
}
