using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(TwoBoneIKConstraint))]
public class TwoBoneIK : BoneIK {
    public TwoBoneIKConstraint ConstraintReference;

    //so the target knows what the 'default' position is just in case
    public Vector3 localRestPosition;

    private void Awake() {
        ConstraintReference = GetComponent<TwoBoneIKConstraint>();

        if(ConstraintReference != null && ConstraintReference.data.target != null) {

            localRestPosition = GetTarget().localPosition;
        }

    }

    public override Transform GetTarget() {

        return ConstraintReference.data.target;

    }
    public override Vector3 getRestPosition() {
        return localRestPosition;
    }


}
