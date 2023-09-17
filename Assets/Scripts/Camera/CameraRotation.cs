using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个脚本用来管控玩家走到角落后视角旋转90度的问题
/// </summary>

public class CameraRotation : MonoBehaviour
{
    public Transform player;
    public Transform cameraFollow;
    public int rotationDegree;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Player")
        {
            cameraFollow.Rotate(0, rotationDegree, 0);
            other.SendMessage("Rotation", rotationDegree);
        }
    }
}
