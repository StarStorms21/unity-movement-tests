using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoneIK : MonoBehaviour {
   public abstract Transform GetTarget();
    public void SetTargetPosition(Vector3 position) {
        GetTarget().transform.position = position;
    }

    public abstract Vector3 getRestPosition();
    

    protected void ResetTargetPosition() {

    }
}
