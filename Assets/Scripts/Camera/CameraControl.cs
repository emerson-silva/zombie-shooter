using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public Vector3 cameraDistance;

    void Update()
    {
        this.transform.position = player.transform.position + cameraDistance;
    }
}
