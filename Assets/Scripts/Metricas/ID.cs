using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ID
{
    private static int _room;
    private static int _match;
    private static int _round;
    private static int _player;

    public static void Initialize(int roomID)
    {
        _room = roomID;
        _match = 0;
        _round = 0;
    }
    public static void Initialize(int roomID, int playerID)
    {
        _room = roomID;
        _match = 0;
        _round = 0;
        _player = playerID;
    }

    public static int GetPlayerID()
    {
        return _player;
    }
    public static int GetRoomID()
    {
        return _room;
    }
    public static int GetMatchID()
    {
        string room = _room.ToString("D4");
        string match = _match.ToString("D2");
        string round = _round.ToString("D2");

        string concatenated = room + match + round;

        return int.Parse(concatenated);
    }
    public static void IncrementMatch()
    {
        _match++;
    }
    public static void IncrementRound()
    {
        _round++;
    }
}
