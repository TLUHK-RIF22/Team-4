using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 7f;
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    private float minYPosition;
    private float maxYPosition;
    private float minXPosition;
    private float maxXPosition;
    private float cameraSize;

    void Start()
    {
        cameraSize = Camera.main.orthographicSize;
        minYPosition = minY + cameraSize;
        maxYPosition = maxY - cameraSize;
        minXPosition = minX + cameraSize * Camera.main.aspect;
        maxXPosition = maxX - cameraSize * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.ResetAspect();
        minXPosition = minX + cameraSize * Camera.main.aspect;
        maxXPosition = maxX - cameraSize * Camera.main.aspect;
        float clampedY = Mathf.Clamp(player.position.y, minYPosition, maxYPosition);
        float clampedX = Mathf.Clamp(player.position.x, minXPosition, maxXPosition);
        this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY, 0));
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(minX, maxY, 0));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(minX, minY, 0));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY, 0));
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(minX, maxY, 0));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(minX, minY, 0));
    }
}
