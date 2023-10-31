using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] GameObject Player;
    void Start()
    {
        
    }
    void Update()
    {
        transform.position = Player.transform.position + new Vector3(0, 11, -15);
    }
}
