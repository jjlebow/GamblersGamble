using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    public Vector3 offset;
    public float leftBound;
    public float rightBound;
    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.x > leftBound && player.position.x < rightBound)
            transform.position = new Vector3(player.position.x + offset.x, offset.y, offset.z);
    }
}
