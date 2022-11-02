using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;

    public float HitRadius;
    float moveX, moveZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        
        
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(moveX, 0, moveZ) * moveSpeed;
    }

    

  
}
