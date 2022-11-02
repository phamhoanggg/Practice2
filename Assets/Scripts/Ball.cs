using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 veloc;
    public float TimeSinceStart;
    public Vector3 BeginPosition;
    public GameObject aim;
    public LayerMask GroundLayer;

    private GameObject player;
    public GameObject curAim;

    Vector3 LastVeloc;
    // Start is called before the first frame update
    void Start()
    {
        BeginPosition = transform.position;
        TimeSinceStart = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10) Destroy(gameObject);

          
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < player.GetComponent<Player>().HitRadius && veloc.z < 0)
        {
            TimeSinceStart += Time.fixedDeltaTime * 0.05f;
        }
        else TimeSinceStart += Time.fixedDeltaTime;
        
        // Calculate new position
        float posX = BeginPosition.x + veloc.normalized.x * veloc.magnitude * TimeSinceStart;
        float posY = BeginPosition.y + veloc.normalized.y * veloc.magnitude * TimeSinceStart - 0.5f * Physics.gravity.magnitude * Mathf.Pow(TimeSinceStart, 2);
        float posZ = BeginPosition.z + veloc.normalized.z * veloc.magnitude * TimeSinceStart;

        transform.position = new Vector3(posX, posY, posZ);      // Update new position

        float lastVelocY = veloc.y + Physics.gravity.y * TimeSinceStart;   // Calculate current velocity
        LastVeloc = new Vector3(veloc.x, lastVelocY, veloc.z);   // Update last velocity

        curAim = GameObject.FindGameObjectWithTag("Aim");
        if (curAim == null)
        {
            DetectCollisionPos();
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        BeginPosition = transform.position;
        TimeSinceStart = 0;
        LastVeloc *= 0.9f;
        Destroy(curAim);

        if (other.gameObject.CompareTag("Wall"))
        {
            veloc = new Vector3(LastVeloc.x, LastVeloc.y, -LastVeloc.z);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            
            veloc = new Vector3(LastVeloc.x, -LastVeloc.y, LastVeloc.z);      
            if(transform.position.x < -24 || transform.position.x > 24 || transform.position.z < -42 || transform.position.z > 42)
            {
                Debug.Log("Out!");
                Destroy(gameObject);
               
            }else
            {
                Debug.Log("In!");
            }
        }
        else if (other.gameObject.CompareTag("Barrier"))
        {
            veloc = new Vector3(LastVeloc.x, LastVeloc.y, -LastVeloc.z);
            Destroy(gameObject);
            
        }
    }

    void DetectCollisionPos()
    {
        for (int i = 0; i < 1000; i++)
        {
            float TimeDetect = TimeSinceStart + Time.fixedDeltaTime * i;
            float posXDetect = BeginPosition.x + veloc.normalized.x * veloc.magnitude * TimeDetect;
            float posYDetect = BeginPosition.y + veloc.normalized.y * veloc.magnitude * TimeDetect - 0.5f * Physics.gravity.magnitude * Mathf.Pow(TimeDetect, 2);
            float posZDetect = BeginPosition.z + veloc.normalized.z * veloc.magnitude * TimeDetect;

            Collider[] col = Physics.OverlapSphere(new Vector3(posXDetect, posYDetect, posZDetect), 0.1f, GroundLayer);
            if (col.Length > 0)
            {
                Instantiate(aim, new Vector3(posXDetect, 0.1f, posZDetect), Quaternion.identity);
                break;
            }
        }
    }

}
