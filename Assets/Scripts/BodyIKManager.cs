using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles smouth rotation of the body as segments, also has a reference to all of them
/// and takes over the Head control if there is no ImportanceTarget
/// </summary>
/// 
public class BodyIKManager : MonoBehaviour {
    [Header("Data")]
    [SerializeField] Transform Root;
    [SerializeField] Rigidbody rb;
    [SerializeField] LocomotionManager lm;

    [Header("Head")]
    [SerializeField] PointIK Head;


    [Header("Body")]
    [SerializeField] PointIK Chest;


    [Header("Legs")]
    [SerializeField] TwoBoneIK RightLeg;
    [SerializeField] TwoBoneIK LeftLeg;


    private void Awake() {
        Root = transform.parent;
        rb = lm.Rigidbody;
    }

    private void Update() {
        RotateToDirOfMovement();
        AccelerationTilt();
    }


    //head + chest should only moved when they arent focusin on sth else or when sprinting
    void RotateToDirOfMovement() {
        float bodySpeed = 250;
        float torsoSpeed = bodySpeed/4;
        float headSpeed = bodySpeed/2;

        Vector3 CharacterLookDirection = lm.InputMovementDirection;
        CharacterLookDirection.Normalize();
        Vector3 headPos = CharacterLookDirection + transform.position;

        if(CharacterLookDirection != Vector3.zero) {
            CharacterLookDirection.Scale(new Vector3(1, 0, 1));
            Quaternion CharacterLookRot = Quaternion.LookRotation(CharacterLookDirection);


            Head.GetTarget().parent.rotation = Quaternion.RotateTowards(Head.GetTarget().parent.rotation, CharacterLookRot, Time.deltaTime * headSpeed);
            Chest.GetTarget().parent.rotation = Quaternion.RotateTowards(Chest.GetTarget().parent.rotation, CharacterLookRot, Time.deltaTime * torsoSpeed);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, CharacterLookRot, Time.deltaTime * bodySpeed);

        }
        else {
            Quaternion CharacterLookRot = Quaternion.LookRotation(rb.transform.forward);

            Head.GetTarget().parent.rotation = Quaternion.RotateTowards(Head.GetTarget().parent.rotation, CharacterLookRot, Time.deltaTime * headSpeed*2);
            Chest.GetTarget().parent.rotation = Quaternion.RotateTowards(Chest.GetTarget().parent.rotation, CharacterLookRot, Time.deltaTime * torsoSpeed*3);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, CharacterLookRot, Time.deltaTime * bodySpeed);

        }

    }


   
    void AccelerationTilt() {

        float x = Mathf.Lerp(transform.localRotation.eulerAngles.x, Mathf.Abs(rb.velocity.magnitude * 2), Time.deltaTime * 5);


        transform.localRotation = Quaternion.Euler(new Vector3(x, 0, 0));

    }
}
