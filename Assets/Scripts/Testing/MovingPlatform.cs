using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour{
    public Rigidbody rb;
    public float speed;
   
    public Transform start,end;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
     
    }

    void FixedUpdate(){
        float time = Mathf.PingPong(Time.time , speed)/ speed;
        rb.MovePosition(Vector3.Lerp(start.position, end.position, time));
        

    }
}
