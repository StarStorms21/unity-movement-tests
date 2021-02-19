using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKBone : MonoBehaviour {
    public Animator animator;
    public AvatarIKGoal ikGoal;
    



    //IK position
    public Vector3 GetIKGoalPosition() {
        return animator.GetIKPosition(ikGoal);
    }
    public void SetIKGoalPosition(Vector3 position) {
        animator.SetIKPosition(ikGoal,position);
    }

    public void InterpolateIKToPosition(Vector3 finalPos, float t) {
        //from current to new position
        SetIKGoalPosition(Vector3.Slerp(GetIKGoalPosition(),finalPos,t));
    }

    //IK Rotation
    public Quaternion GetIKGoalRotation() {
        return animator.GetIKRotation(ikGoal);
    }

    public void SetIKGoalRotation(Vector3 eulers) {
        animator.SetIKRotation(ikGoal, Quaternion.Euler(eulers));
    }
    public void SetIKGoalRotation(Quaternion rot) {
        animator.SetIKRotation(ikGoal, rot);
    }
    public void InterpolateIKRotation(Quaternion end, float t) {
        SetIKGoalRotation(Quaternion.Slerp(GetIKGoalRotation(),end,t));
    }

    //IK weight
    public float GetIKGoalWeight() {
        return animator.GetIKPositionWeight(ikGoal);
    }
    public void SetIKGoalWeight(float w) {
        animator.SetIKPositionWeight(ikGoal, w);
        animator.SetIKRotationWeight(ikGoal, w);
    }

    public void InterpWeight(float end, float t) {
        SetIKGoalWeight(Mathf.Lerp(GetIKGoalWeight(), end, t));
    }


}
