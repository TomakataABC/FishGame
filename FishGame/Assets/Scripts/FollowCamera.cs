using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 newtrans;
    private Camera cameraCam;
    public float z;

    void Start()
    {
        newtrans = player.transform.position + new Vector3(0,0,-20f);
        cameraCam = GetComponent<Camera>();
    }
    void LateUpdate()
    {
        newtrans.x = player.transform.position.x;
        newtrans.y = player.transform.position.y;
        cameraCam.orthographicSize = z;
        transform.position = newtrans;
    }
}
