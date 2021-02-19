using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour {
    [Header("Data")]
    public Animator animator;
    public Rigidbody rb;
    public GameObject camera;
    public Transform model;

    [Header("Basic Movement")]
    //if this character can move
    public bool canMove = true;
    //if this character can turn
    public bool canTurn = true;

    [Space]
    [SerializeField]
    private bool _isRunning;
    [SerializeField]

    private bool _isWalking;
   
    [Space]
    //movement speed
    public float rotationSpeed = 5f;

    [Space]
    public float movementSpeedMultiplier = 2.5f;

    [Space]
    //for animations
    public float runSpeed = 1;
    public float walkSpeed = 0.6f;

    [Space]
    public float acceleration = 0.1f;
    public float deceleration = 0.3f;
    [Space]
    [SerializeField]
    private float finalSpeedVel;

    [Space]
    public float velocityZ;
    public float velocityX;

    public Vector3 forwardMovementVector;
        

    [Header("Collision Detection")]

    Ray centerOfMassRay;
    Vector3 CenterOfMassOnGround;



    [Header("Wall Detection")]
    public float chestHeight;


    [Header("Animations")]
    Vector3 lastPosition;
    public float distanceFromLastPosition;


    private float strafeInput;
    private float forwardInput;

    //for smouthing
    float totalMovement;
    float prevTotalMovement;

    [Header("Tilting")]
    public float walkTilt;
    public float runTilt;

    [Header("Step Up")]
    public Transform FeetHeight;
    public Transform KneeHeight;

    public float stepHeight = 0.3f;
    public float stepSmouth = 0.1f;

    [Header("Velocity")]
    public Vector3 MovementVelocity;
    Vector3 lastPos;

    private void Awake() {
        rb = GetComponent<Rigidbody>();

        //KneeHeight.transform.position = new Vector3(KneeHeight.transform.position.x, transform.position.y+stepHeight, KneeHeight.transform.position.z);
    }
    void Start() {

    }
    private void Update()
    {
         strafeInput = Input.GetAxis("Horizontal");
         forwardInput = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }

    void FixedUpdate() {

        TiltModel();
        if ( canMove ) {
            calculateMovementPhysics();
            ReadInput();
        }
        
    }

    

    void ReadInput() {

        Vector3 adjustedForward = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z);
        Vector3 adjustedStrafe = new Vector3(camera.transform.right.x, 0, camera.transform.right.z);

        Vector3 forwardMovement = adjustedForward * forwardInput;
        Vector3 strafeMovement = adjustedStrafe * strafeInput;


        if (canTurn) rotateToCamera(rotationSpeed, forwardMovement, strafeMovement);

        totalMovement = Mathf.Max(Mathf.Abs(forwardInput), Mathf.Abs(strafeInput)) ;

        forwardMovementVector = transform.forward * Mathf.Lerp(prevTotalMovement,totalMovement,Time.deltaTime);
        if(forwardMovementVector.magnitude > 1) forwardMovementVector /= forwardMovementVector.magnitude;
        TryStepUp();


        Move((transform.forward * totalMovement/20) * finalSpeedVel);

        


        //  velocityZ = Vector3.Dot(forwardMovement.normalized, camera.transform.forward);
        //  velocityX = Vector3.Dot(strafeMovement.normalized, camera.transform.right);

        //animator.SetFloat("VelocityZ", velocityZ,0.1f, Time.deltaTime);
        // animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

        prevTotalMovement = totalMovement;
    }

    void calculateMovementPhysics() {
        MovementVelocity = (rb.position - lastPos) * 50;
        MovementVelocity.Scale(new Vector3(1, 0, 1));
        //MovementVelocity*=Time.deltaTime;
        if(MovementVelocity.magnitude > 1) MovementVelocity /= MovementVelocity.magnitude;
        lastPos = rb.position;
        bool shouldAccelerate = true;



        #region Acceleration and movement
        //w key down
        if((forwardInput != 0 || strafeInput != 0)){ //&& !isBlockedByWall()) {

            if ( shouldAccelerate ) {
                //shift key down
                if ( Input.GetKey(KeyCode.LeftShift) ) {
                    //make it so it interpolates to reach max running speed!
                    _isRunning = true;
                    _isWalking = false;
                    finalSpeedVel = Mathf.Lerp(finalSpeedVel, runSpeed, Time.deltaTime * acceleration + 0.1f);
                } else {
                    //make it so it interpolates to reach the medium speed
                    _isWalking = true;
                    _isRunning = false;
                    finalSpeedVel = Mathf.Lerp(finalSpeedVel, walkSpeed, Time.deltaTime * acceleration + 0.1f);
                }
            } else {
                finalSpeedVel = Mathf.Lerp(finalSpeedVel, 0, Time.deltaTime * acceleration + 0.1f);
                _isWalking = false;
                _isRunning = false;
            }
        } else {
            _isWalking = false;
            _isRunning = false;
            finalSpeedVel = Mathf.Lerp(finalSpeedVel, 0, Time.deltaTime * deceleration + 0.1f);
           
        }
        #endregion





        giveAnimatorSpeed();

        // animator.SetFloat("Speed", finalSpeedVel/runSpeed, 0.1f, Time.deltaTime);
        //lastPosition = transform.position;
    }

    void giveAnimatorSpeed() {
       
        if(_isRunning && runSpeed > 1) {
           
            setAnimatorSpeed(MovementVelocity.magnitude * (finalSpeedVel/runSpeed));
        }
        else if(_isWalking) {

          
            setAnimatorSpeed(MovementVelocity.magnitude * finalSpeedVel);
        }
        else {
            
            setAnimatorSpeed(MovementVelocity.magnitude);
        }
    }

    void setAnimatorSpeed(float speed) {
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    float getAnimatorSpeed() {
        return animator.GetFloat("Speed");
    }

    void rotateToCamera(float speed, Vector3 forwardMovement, Vector3 strafeMovement) {
        

        Vector3 CharacterLookDirection = forwardMovement+strafeMovement;
        CharacterLookDirection.Scale(new Vector3(1, 0, 1));

        //CharacterLookDirection += MovementVelocity;

        if (CharacterLookDirection != Vector3.zero)
        {
            Quaternion CharacterLookRot = Quaternion.LookRotation(CharacterLookDirection);

            rb.rotation = Quaternion.RotateTowards(transform.rotation, CharacterLookRot, Time.deltaTime * speed);
        }
    }

    bool isBlockedByWall() {
        float maxDistance = 2;
        RaycastHit hitInfo;
        Vector3 start = new Vector3(transform.position.x, transform.position.y+chestHeight, transform.position.z);

        if ( Physics.BoxCast(start, new Vector3(.3f, .3f, .3f), transform.forward, out hitInfo) ) {
            if (hitInfo.distance < 1f) {
                return true;
            }
        }

        return false;
    }

    private void TiltModel(){

        
       

        float value = Mathf.Lerp(model.localRotation.eulerAngles.x, Mathf.Abs(forwardMovementVector.magnitude * calculateTilt()), Time.deltaTime);


        model.localRotation = Quaternion.Euler(new Vector3( value, 0, 0));

      
    }

    private float calculateTilt(){
        return getAnimatorSpeed();
    }

    private void Move(Vector3 totalMovement)
    {

        rb.MovePosition(rb.position + totalMovement);

    }

    Vector3 hit;
    private void TryStepUp(){

        if(CanStepUp(.37f) && forwardMovementVector.magnitude>0) {
            
            //float step_up_speed = stepSmouth * Time.deltaTime;
           // Move(new Vector3(0, step_up_speed, 0));
            //cullSpeedOnStairs(runSpeed/1.8f);
        }
    }

    private bool CanStepUp(float lowHitLen) {
        float kneeHitLen = lowHitLen + 0.1f;

        RaycastHit lowHit;

        if(Physics.Raycast(FeetHeight.position, transform.TransformDirection(Vector3.forward), out lowHit, lowHitLen)) {
            RaycastHit upHit;
            if(!Physics.Raycast(KneeHeight.position, transform.TransformDirection(Vector3.forward), out upHit, kneeHitLen)) {
                return true;
            }
        }

        RaycastHit lowHit45;
        if(Physics.Raycast(FeetHeight.position, transform.TransformDirection(1.5f, 0, 1), out lowHit45, lowHitLen)) {
            RaycastHit upHit45;
            if(!Physics.Raycast(KneeHeight.position, transform.TransformDirection(1.5f, 0, 1), out upHit45, kneeHitLen)) {
                return true;
            }
        }

        RaycastHit lowHit45m;
        if(Physics.Raycast(FeetHeight.position, transform.TransformDirection(-1.5f, 0, 1), out lowHit45m, lowHitLen)) {
            RaycastHit upHit45m;           
            if(!Physics.Raycast(KneeHeight.position, transform.TransformDirection(-1.5f, 0, 1), out upHit45m, kneeHitLen)) {

                return true;
            }
        }

        return false;;
    }
    private void cullSpeedOnStairs(float speedToHit) {
        if(_isRunning) {
            finalSpeedVel = Mathf.Lerp(finalSpeedVel, speedToHit, Time.deltaTime * deceleration + 0.1f);
        }
    }


    void OnDrawGizmos() {
        //Gizmos.color = Color.black;
        //Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + chestHeight, transform.position.z), new Vector3(.3f, .3f, .3f));
        //Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + chestHeight /1.4f, transform.position.z), new Vector3(.3f, .3f, .3f));


         //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(new Vector3(transform.position.x+forwardMovementVector.x,
        //    transform.position.y + chestHeight,
        //    transform.position.z + forwardMovementVector.z), .1f);


        
        //Gizmos.DrawSphere(hit, .07f);

       // Gizmos.color = Color.black;
      //  Gizmos.DrawWireCube(KneeHeight.position, new Vector3(.3f, .05f, .3f));
       //// Gizmos.DrawWireCube(FeetHeight.position, new Vector3(.3f, .05f, .3f));
      //  Gizmos.DrawWireCube(transform.position+transform.up*stepHeight, new Vector3(.3f, .05f, .3f));



      //  Gizmos.DrawRay(new Ray(FeetHeight.position, transform.TransformDirection(Vector3.forward)));
      //  Gizmos.DrawRay(new Ray(KneeHeight.position, transform.TransformDirection(Vector3.forward)));
    }

}
