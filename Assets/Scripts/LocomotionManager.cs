using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveState {
    Idle,
    Walk,
    Run,
    Crouch
}


[RequireComponent(typeof(Rigidbody))]
public class LocomotionManager : MonoBehaviour {

    [Header("Data")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject camera;
    [SerializeField] private Transform model;
    [SerializeField] private Animator anim;

    [Header("Movement")]
    [SerializeField]private Vector3 _inp_movementDirection;
    [SerializeField] private Vector3 _movementInput;
    [SerializeField] private float _currentForwardSpeed;

    private MoveState movementState = MoveState.Idle;
    [Space]
    public float AccelerationSpeed = 1000;


    #region Public Getters
    public Vector3 VelocityDirection { get { return rb.velocity; } private set { } }
    public Vector3 InputMovementDirection { get { return _inp_movementDirection; } private set { } }

    public Vector3 InputDirection { get { return _movementInput; } private set { } }

    public float ForwardSpeed { get { return _currentForwardSpeed; } private set { } }
    public MoveState MovementState { get { return movementState; } private set { } }

    public Rigidbody Rigidbody { get { return rb; } private set { } }


    #endregion


    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    private void Update() {
        getUserInput();
        DetermineMovementState();

       
    }

    private void FixedUpdate() {
       

        Move(transform.forward*_inp_movementDirection.magnitude);
       
    }


    void getUserInput() {

        _movementInput = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));



        //rotate the camera forward vector by the -x rotation to remove the tiltng effect!
        Vector3 rotatedCamForward = Quaternion.AngleAxis(camera.transform.eulerAngles.y, Vector3.up) * Vector3.forward;


        Vector3 camForw = new Vector3(rotatedCamForward.x, 0, rotatedCamForward.z);
        Vector3 camRight = new Vector3(camera.transform.right.x, 0, camera.transform.right.z);

        


        camForw*= _movementInput.y;
        camRight*= _movementInput.x;


        if(camForw.magnitude > 1) camForw /= camForw.magnitude;
        if(camRight.magnitude > 1) camRight /= camRight.magnitude;

        _inp_movementDirection = camForw+camRight;

        if(_inp_movementDirection.magnitude > 1) _inp_movementDirection /= _inp_movementDirection.magnitude;

        _currentForwardSpeed = rb.velocity.magnitude;

    }

 

    public bool ShouldAccelerate() {
        switch(movementState) {
            case MoveState.Idle:
                return true;

            case MoveState.Walk:
                return rb.velocity.magnitude < 2.5f;

            case MoveState.Run:
                return rb.velocity.magnitude < 7;

            case MoveState.Crouch:
                return rb.velocity.magnitude < 1.5f;

        }
       
            return false;
 
    }

    void DetermineMovementState() {
        if(_inp_movementDirection.magnitude > 0) {
            if(Input.GetKey(KeyCode.LeftShift)) {
                movementState = MoveState.Run;
            }else
            if(Input.GetKey(KeyCode.LeftControl)) {
                movementState = MoveState.Crouch;
            }
            else {
                movementState = MoveState.Walk;
            }
            
        }
        else {
            movementState = MoveState.Idle;
        }
    }
 

    private void Move(Vector3 dir) {

       if (ShouldAccelerate())rb.AddForce(dir * AccelerationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_inp_movementDirection+transform.position+transform.up*1.5f,0.08f);
    }
}
