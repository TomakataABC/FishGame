using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 newtrans;

    void Start()
    {
        newtrans = transform.position;

    }
    void LateUpdate()
    {
        newtrans.x = player.transform.position.x;
        newtrans.y = player.transform.position.y;
        transform.position = newtrans;
    }
}
