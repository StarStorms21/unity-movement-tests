using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RedoMovement : MonoBehaviour{

    public Camera PlayerCamera;
    public Rigidbody rb;
    [Space]
    public bool CanMove;
    public bool CanTurn;
    [Space]
    public float maxMovementSpeed;
    public float maxWalkSpeed;
    [Space]
    public float acceleration = 0.1f;
    public float deceleration = 0.3f;

    [SerializeField]
    private float _curSpeed;

    float _strafeInput;
    float _forwardInput;

    void Awake(){
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    private void Update(){
        _strafeInput = Input.GetAxis("Horizontal");
        _forwardInput = Input.GetAxis("Vertical");

    }

    private void FixedUpdate() {
        if (CanMove){
            calculateMovementSpeed();
            HandleInput();
        }
    }


    void HandleInput(){
        Vector3 adjustedForward = _forwardInput * new Vector3(PlayerCamera.transform.forward.x, 0, PlayerCamera.transform.forward.z);
        Vector3 adjustedStrafe = _strafeInput * new Vector3(PlayerCamera.transform.right.x, 0, PlayerCamera.transform.right.z);



        if (adjustedForward.magnitude > 1) adjustedForward /= adjustedForward.magnitude;
        if (adjustedStrafe.magnitude > 1) adjustedStrafe /= adjustedStrafe.magnitude;



        Vector3 finalMovementVector = new Vector3(adjustedStrafe.x + adjustedForward.x, 0, adjustedStrafe.z + adjustedForward.z);

     
        Move(finalMovementVector * _curSpeed);
    }


    void Move(Vector3 move){
        rb.MovePosition(transform.position+move*Time.deltaTime);
    }

    void calculateMovementSpeed(){
        float forwardInput = _forwardInput;
        float strafeInput = _strafeInput;
        bool shouldAccelerate = true;



        #region Acceleration and movement
        //w key down
        if ((forwardInput != 0 || strafeInput != 0) )//&& !isBlockedByWall())
        {

            if (shouldAccelerate)
            {
                //shift key down
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    //make it so it interpolates to reach max running speed!
                    _curSpeed = Mathf.Lerp(_curSpeed, maxMovementSpeed, Time.deltaTime * acceleration);
                }
                else
                {
                    //make it so it interpolates to reach the medium speed
                    _curSpeed = Mathf.Lerp(_curSpeed, maxWalkSpeed, Time.deltaTime * acceleration);
                }
            }
            else
            {
                _curSpeed = Mathf.Lerp(_curSpeed, 0, Time.deltaTime * acceleration);

            }
        }
        else
        {

            _curSpeed = Mathf.Lerp(_curSpeed, 0, Time.deltaTime * deceleration);

        }
        #endregion







        //animator.SetFloat("Speed", finalSpeedVel, 0.1f, Time.deltaTime);
        //lastPosition = transform.position;
    }
}
