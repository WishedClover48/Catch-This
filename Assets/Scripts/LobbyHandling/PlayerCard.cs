using Photon.Realtime;
using TMPro;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;

/// <summary>
/// Represents a single player's card in the lobby UI.
/// Displays the player's nickname and whether they are ready.
/// Updates automatically when the player's properties change.
/// </summary>
public class PlayerCard : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text nameText;   // UI text for the player's name
    [SerializeField] private TMP_Text readyText;  // UI text for the player's ready status

    public Player Player { get; private set; }    // The Photon Player this card represents

    /// <summary>
    /// Initializes this card with a specific player reference.
    /// </summary>
    public void SetPlayer(Player player)
    {
        Player = player;

        // Set the nickname in the UI
        nameText.text = player.NickName;

        // Update the ready text according to the player's properties
        UpdateReadyText();
    }

    /// <summary>
    /// Checks the "Ready" custom property and updates the ready text.
    /// </summary>
    private void UpdateReadyText()
    {
        // If the "Ready" property exists and is true, show "Ready"
        if (Player.CustomProperties.TryGetValue("Ready", out object readyValue) && (bool)readyValue)
        {
            readyText.text = "Ready";
        }
        else
        {
            readyText.text = "Not Ready";
        }
    }

    /// <summary>
    /// Callback from Photon whenever *any* player's properties update.
    /// If this card belongs to that player and "Ready" changed, refresh the UI.
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == Player && changedProps.ContainsKey("Ready"))
        {
            UpdateReadyText();
        }
    }
}
