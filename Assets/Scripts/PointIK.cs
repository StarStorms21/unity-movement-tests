using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(MultiAimConstraint))]
public class PointIK : BoneIK {
    public MultiAimConstraint ConstraintReference;

    //so the target knows what the 'default' position is just in case
   
    public Vector3 localRestPosition;

    private void Awake() {
        ConstraintReference = GetComponent<MultiAimConstraint>();

        if(ConstraintReference != null && ConstraintReference.data.sourceObjects[0].transform != null) {

            localRestPosition = GetTarget().localPosition;
        }

    }

    //add a primary target (for example facing direction)
    //and a secondary target (for example objects of interest)
    //and shift their weights depenting on the situation
    public override Transform GetTarget() {

        return ConstraintReference.data.sourceObjects[0].transform;

    }

    public override Vector3 getRestPosition() {
       return localRestPosition;
    }
}
