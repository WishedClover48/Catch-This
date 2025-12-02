using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GodCounter
{
    private static int _meteorCastsCount;
    private static int _meteorKillsCount;
    private static int _laserCastCount;
    private static int _laserKillsCount;
    
    public static int GetMeteorCastsCount()  { return _meteorCastsCount; }
    public static int GetMeteorKillsCount()  { return _meteorKillsCount; }
    public static int GetLaserCastCount()  { return _laserCastCount; }
    public static int GetLaserKillsCount()  { return _laserKillsCount; }
    public static void MeteorUsed() { _meteorCastsCount++; }
    public static void MeteorKill() { _meteorKillsCount++; }
    public static void LaserUsed() { _laserCastCount++; }
    public static void LaserKill() { _laserKillsCount++; }

    public static void ResetValues()
    {
        _meteorCastsCount = 0;
        _meteorKillsCount = 0;
        
        _laserKillsCount = 0;
        _laserCastCount = 0;
    }
}
