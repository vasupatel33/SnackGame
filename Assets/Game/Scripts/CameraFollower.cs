using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 distance;
    void Start()
    {
        distance = transform.position - player.transform.position;
    }
    void Update()
    {
        transform.position = player.transform.position + distance;
    }

}
