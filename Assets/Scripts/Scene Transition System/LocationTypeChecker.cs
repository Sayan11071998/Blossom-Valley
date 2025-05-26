using UnityEngine;
using System.Linq;

public static class LocationTypeChecker
{
    static readonly SceneTransitionManager.Location[] indoor = { 
        SceneTransitionManager.Location.PlayerHome, 
        SceneTransitionManager.Location.ChickenCoop 
    };
    
    public static bool IsIndoor(SceneTransitionManager.Location location)
    {
        return indoor.Contains(location);
    }
}