using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float offsetZ = 5f;
    [SerializeField] private float smoothing = 2f;

    Transform playerPos;

    void Start() => playerPos = FindAnyObjectByType<PlayerView>().transform;

    void Update() => FollowPlayer();

    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z - offsetZ);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}