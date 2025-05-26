using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float offsetZ = 5f;
    public float smoothing = 2f; 
    // Player transform component
    Transform playerPos;

    void Start()
    {
        // Find the PlayerView in the scene and get its transform component
        playerPos = FindAnyObjectByType<PlayerView>().transform; 
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Position the camera should be in
        Vector3 targetPosition = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z - offsetZ); 

        // Set the position accordingly
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}