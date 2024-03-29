using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    // Update is called once per frame
    void Update()
    {
        float clampedY = Mathf.Clamp(player.position.y, minY, maxY);
        this.transform.position = new Vector3(player.position.x, clampedY, this.transform.position.z);
    }
}
