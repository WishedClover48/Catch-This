public class ChatFlaggedEvent : Unity.Services.Analytics.Event
{
    public ChatFlaggedEvent() : base("Chat_Flagged")
    {
        
    }
    public int PlayerID { set { SetParameter("PlayerID", value); } }
    public int RoomID { set { SetParameter("RoomID", value); } }
    public string Slur { set { SetParameter("Slur", value); } }
} //N
public class GodAbilityKillsEvent : Unity.Services.Analytics.Event
{
    public GodAbilityKillsEvent() : base("God_Ability_Kills")
    {
        
    }
    public int MatchID { set { SetParameter("MatchID", value); } }
    public int LaserKillCount { set { SetParameter("LaserKillCount", value); } }
    public int MeteorKillCount { set { SetParameter("MeteorKillCount", value); } }
} //Z
public class GodAbilityUsedEvent : Unity.Services.Analytics.Event
{
    public GodAbilityUsedEvent() : base("God_Ability_Used") { }

    public int MatchID { set { SetParameter("MatchID", value); } }
    public int MeteorCount { set { SetParameter("MeteorCount", value); } }
    public int LaserCount { set { SetParameter("LaserCount", value); } }
} //Z
public class MatchEndedEvent : Unity.Services.Analytics.Event
{
    public MatchEndedEvent() : base("Match_Ended") { }

    public int MatchID { set { SetParameter("MatchID", value); } }
    public string WinnerRole { set { SetParameter("WinnerRole", value); } }
} //M
public class NaughtyRoomEvent : Unity.Services.Analytics.Event
{
    public NaughtyRoomEvent() : base("Naughty_Room") { }

    public int RoomID { set { SetParameter("RoomID", value); } }
    public bool Naughty { set { SetParameter("Naughty", value); } }
} //M
public class PlayerKilledEvent : Unity.Services.Analytics.Event
{
    public PlayerKilledEvent() : base("Player_Killed") { }

    public int MatchID { set { SetParameter("MatchID", value); } }
    public int PlayerID { set { SetParameter("PlayerID", value); } }
    public string PowerUpType { set { SetParameter("PowerUpType", value); } }
} //N
public class PlayerScoreRecordedEvent : Unity.Services.Analytics.Event
{
    public PlayerScoreRecordedEvent() : base("Player_Score_Recorded") { }

    public int MatchID { set { SetParameter("MatchID", value); } }
    public int PlayerID { set { SetParameter("PlayerID", value); } }
    public string Role { set { SetParameter("Role", value); } }
    public int Score { set { SetParameter("Score", value); } }
} //N
public class PowerUpPickedEvent : Unity.Services.Analytics.Event
{
    public PowerUpPickedEvent() : base("PowerUp_Picked") { }

    public int MatchID { set { SetParameter("MatchID", value); } }
    public string PowerUpType { set { SetParameter("PowerUpType", value); } }
    public float LifeTime { set { SetParameter("LifeTime", value); } }
    public int PlayerID { set { SetParameter("PlayerID", value); } }
} //M
public class RoomCreatedSuccessEvent : Unity.Services.Analytics.Event
{
    public RoomCreatedSuccessEvent() : base("Room_Created_Success") { }
} //Z
public class RoomCreationAttemptEvent : Unity.Services.Analytics.Event
{
    public RoomCreationAttemptEvent() : base("Room_Creation_Attemp") { }
} //Z
public class RoomJoinedEvent : Unity.Services.Analytics.Event
{
    public RoomJoinedEvent() : base("Room_Joined") { }

    public int MatchID { set { SetParameter("MatchID", value); } }
    public int PlayerCount { set { SetParameter("PlayerCount", value); } }
} //Z