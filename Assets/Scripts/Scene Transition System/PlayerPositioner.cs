using UnityEngine;

public class PlayerPositioner
{
    private IPlayerPositionStrategy positionStrategy;
    
    public PlayerPositioner(IPlayerPositionStrategy strategy)
    {
        positionStrategy = strategy;
    }
    
    public void PositionPlayer(Transform playerTransform, SceneTransitionManager.Location fromLocation)
    {
        Transform startPoint = positionStrategy.GetPlayerPosition(fromLocation);
        if (startPoint == null) return;
        
        CharacterController playerCharacter = playerTransform.GetComponent<CharacterController>();
        playerCharacter.enabled = false;
        playerTransform.position = startPoint.position;
        playerTransform.rotation = startPoint.rotation;
        playerCharacter.enabled = true;
    }
}