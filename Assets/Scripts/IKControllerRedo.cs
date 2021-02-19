using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControllerRedo : MonoBehaviour {
    private Animator animator;
    private Vector3 rightFootPosition, leftFootPosition, rightFootIKPosition, leftFootIKPosition;
    private Quaternion leftFootIkRot, rightFootIkRot;

    private float pelvisPositionY, lastRightFootPosition, lastLeftFootPosition;

    [Header("Feet")]
    public bool enableFeetIK = true;
    [SerializeField][Range(0,2)]private float heightFromGroundRaycast = 1.14f;
    [SerializeField][Range(0, 2)]private float rayCastDownDist = 1.5f;

    [SerializeField] LayerMask enviromentMask;
    [SerializeField] private float pelvisOffset = 0f;
    [SerializeField] [Range(0, 1)] private float prelvisUpAndDownSpeed = 0.28f;
    [SerializeField] [Range(0, 1)] private float feetToIkPosition = 0.5f;

    public string leftFootAnimVarName = "IKLeftFootWeight";
    public string rightFootAnimVarName = "IKRightFootWeight";

    public bool showSolverDebug;

}
