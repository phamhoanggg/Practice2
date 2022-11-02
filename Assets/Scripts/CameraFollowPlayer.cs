using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Vector3 dis;

    private void FixedUpdate()
    {
        Vector3 dPos = target.position + dis;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, speed * Time.fixedDeltaTime);
        transform.position = sPos;
    }
}
