using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public static Transform player;
    public static List<Transform> pointsOfInterest = new List<Transform>();
    public static CinemachineVirtualCamera vCam;
    // Start is called before the first frame update
    void Start()
    {
        pointsOfInterest = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
