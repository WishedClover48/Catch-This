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
    [SerializeField] private Button readyButton;          // Reference to the UI button
    [SerializeField] private TMPro.TMP_Text buttonText;   // Reference to the text displayed on the button

    private void Start()
    {
        // Make sure button text reflects the player's current ready state
        UpdateButtonText();
    }

    /// <summary>
    /// Called when the Ready button is clicked.
    /// Toggles the "Ready" state for the local player.
    /// </summary>
    public void OnReadyClicked()
    {
        // Default state: not ready
        bool isReady = false;

        // If the player already has a "Ready" property, read its value
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out object readyValue))
        {
            isReady = (bool)readyValue;
        }

        // Create a new properties table with the opposite value (toggle)
        Hashtable props = new Hashtable { { "Ready", !isReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        // Update the button label immediately for feedback
        UpdateButtonText();
    }

    /// <summary>
    /// Updates the button label text depending on whether the local player is ready.
    /// </summary>
    private void UpdateButtonText()
    {
        bool isReady = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out object readyValue)
                       && (bool)readyValue;

        buttonText.text = isReady ? "Unready" : "Ready";
    }

    /// <summary>
    /// Callback from Photon when any player's properties change.
    /// Used here to refresh the button if *this* player's ready state changes.
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // If the updated player is the local one, update the button text
        if (targetPlayer == PhotonNetwork.LocalPlayer && changedProps.ContainsKey("Ready"))
        {
            UpdateButtonText();
        }
    }
}
