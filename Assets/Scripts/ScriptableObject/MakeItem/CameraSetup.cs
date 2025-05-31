using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{
    public Transform playerTransform;

    void Start()
    {
        CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam != null && playerTransform != null)
        {
            vcam.Follow = playerTransform;
            vcam.LookAt = playerTransform;
        }
    }
}
