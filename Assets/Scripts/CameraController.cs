using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = pos;
    }
}
