using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    private float minYEdge;
    private float maxYEdge;
    private float minXEdge;
    private float maxXEdge;
    private float cameraSize;


    // Update is called once per frame
    void Update()
    {
        float clampedY = Mathf.Clamp(player.position.y, minY, maxY);
        float clampedX = Mathf.Clamp(player.position.x, minX, maxX);
        this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawLine(new Vector3(minXEdge, minYEdge, 0), new Vector3(maxXEdge, minYEdge, 0));
        Gizmos.DrawLine(new Vector3(maxXEdge, minYEdge, 0), new Vector3(maxXEdge, maxYEdge, 0));
        Gizmos.DrawLine(new Vector3(maxXEdge, maxYEdge, 0), new Vector3(minXEdge, maxYEdge, 0));
        Gizmos.DrawLine(new Vector3(minXEdge, maxYEdge, 0), new Vector3(minXEdge, minYEdge, 0));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawLine(new Vector3(minXEdge, minYEdge, 0), new Vector3(maxXEdge, minYEdge, 0));
        Gizmos.DrawLine(new Vector3(maxXEdge, minYEdge, 0), new Vector3(maxXEdge, maxYEdge, 0));
        Gizmos.DrawLine(new Vector3(maxXEdge, maxYEdge, 0), new Vector3(minXEdge, maxYEdge, 0));
        Gizmos.DrawLine(new Vector3(minXEdge, maxYEdge, 0), new Vector3(minXEdge, minYEdge, 0));
    }

    void OnValidate()
    {
        cameraSize = Camera.main.orthographicSize;
        minYEdge = minY - cameraSize;
        maxYEdge = maxY + cameraSize;
        minXEdge = minX - cameraSize * Camera.main.aspect;
        maxXEdge = maxX + cameraSize * Camera.main.aspect;
    }   
}
