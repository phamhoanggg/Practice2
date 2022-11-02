using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulling : MonoBehaviour
{
    public LineRenderer TrajectoryLineRenderer;
    public float ThrowSpeed;
    public Vector3 pullingPosition;

    private bool pulling;
    public GameObject ball;

    private GameObject curBall;
    private bool release;

    // Start is called before the first frame update
    void Start()
    {
        TrajectoryLineRenderer = FindObjectOfType<LineRenderer>();
        TrajectoryLineRenderer.transform.position = Vector3.zero;
        release = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if (pulling)
        {      
            DisplayTrajectoryLineRenderer();
        }
        curBall = GameObject.FindGameObjectWithTag("Ball");
        
    }

    int cnt = 0;
    private void FixedUpdate()
    {
        if (release)
        {
            if (cnt < 5)
            {
                transform.Rotate(0, -9, 0, Space.Self);
                cnt++;
            }
            else
            {
                cnt = 0;
                release = false;
            }
        }
    }

    private void OnMouseDown()
    {
        pulling = true;
        transform.Rotate(0, 45, 0, Space.Self);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z);

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        Vector3 newPos = GetMouseWorldPos();
        pullingPosition = new Vector3(newPos.x, newPos.y * 0.1f, transform.position.z + newPos.y * 2);
        

    }

    private void OnMouseUp()
    {
        pulling = false;
        TrajectoryLineRenderer.enabled = false;
        release = true;


        if (curBall == null) 
        {
            ball.GetComponent<Ball>().veloc = (transform.position - pullingPosition) * ThrowSpeed;
            if (curBall == null) Instantiate(ball, transform.position, Quaternion.identity);
            ball.GetComponent<Ball>().BeginPosition = ball.transform.position;
        }else if (Vector3.Distance(transform.position, curBall.transform.position) < GetComponent<Player>().HitRadius)
        {
            
            curBall.GetComponent<Ball>().veloc = (transform.position - pullingPosition) * ThrowSpeed;
            curBall.GetComponent<Ball>().TimeSinceStart = 0;
            curBall.GetComponent<Ball>().BeginPosition = curBall.transform.position;
            Destroy(curBall.GetComponent<Ball>().curAim);
        }
    }

    void DisplayTrajectoryLineRenderer()
    {
        TrajectoryLineRenderer.enabled = true;
        Vector3 veloc = transform.position - pullingPosition;
        int segmentCount = 15;
        Vector3[] segments = new Vector3[segmentCount];
        Vector3 segVelocity = veloc * ThrowSpeed;
        segments[0] = transform.position;

        
        for (int i = 1; i < segmentCount; i++)
        {
            float time = i * Time.fixedDeltaTime * 3;
            segments[i].x = segments[0].x + segVelocity.normalized.x * segVelocity.magnitude * time;
            segments[i].y = segments[0].y + segVelocity.normalized.y * segVelocity.magnitude * time - 0.5f * Physics.gravity.magnitude * Mathf.Pow(time, 2);
            segments[i].z = segments[0].z + segVelocity.normalized.z * segVelocity.magnitude * time;
        }

        TrajectoryLineRenderer.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
            TrajectoryLineRenderer.SetPosition(i, segments[i]);
    }
}
