using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IKController : MonoBehaviour{
    [Header("Data")]
    public Animator anim;
    public Rigidbody rb;

    [Header("Foot Placement")]
    public float FeetYOffset;
    public IKBone LeftLeg;
    public IKBone RightLeg;

    //Current movement
    public IKBone LowestLeg;
    public IKBone HighestLeg;

    public float footRayHeight;
    [Space]

    [Header("Hip Placement")]
    public Transform hipBone;
    public Transform spineBone;//need this so the hip rotation doesnt make the whole character sway

    bool hipAdjustments = true;
    [Range(0, 2f)]
    public float MaxGroundCheckDistance; // Distance from where the foot transform is to the lowest possible position of the foot.
    public LayerMask GroundMask; // Select all layers that foot placement applies to.
    public float adjustmentSpeed;


    [Range(1, 3f)]
    public float hipAngleExageration;


    Ray debug;
    RaycastHit hit2;


    float curveRightAmm;
    float curveLeftAmm;

    private void Awake() {
        
    }

    private void FixedUpdate() {
        curveRightAmm = anim.GetFloat("IKRightFootWeight");
        curveLeftAmm = anim.GetFloat("IKLeftFootWeight");

        if(LowestLeg && HighestLeg) {
            //if(LowestLeg.GetIKGoalWeight() > 0.5 && HighestLeg.GetIKGoalWeight() < 0.5) {
                float legDif = LowestLeg.GetIKGoalPosition().y - HighestLeg.GetIKGoalPosition().y;

                transform.localPosition = Vector3.Lerp(transform.localPosition, (transform.up * legDif), Time.deltaTime * 5);


                //print(legDif);
            //}
        }
    }
    public void OnAnimatorIK(int layerIndex) {

        if(anim && layerIndex == 0) {

            //calculate the leg ik
            castFootRay(LeftLeg);
            castFootRay(RightLeg);

            if(LowestLeg) {
              
               Quaternion rot = Quaternion.Euler(0,0,LowestLeg.transform.localRotation.eulerAngles.z* hipAngleExageration);
                   
               


                //anim.SetBoneLocalRotation(HumanBodyBones.Hips, rot);
               //anim.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Inverse(rot));
            }
            else {
                //anim.SetBoneLocalRotation(HumanBodyBones.Hips, anim.GetBoneTransform(HumanBodyBones.Hips).localRotation);
                //anim.SetBoneLocalRotation(HumanBodyBones.Spine, anim.GetBoneTransform(HumanBodyBones.Spine).localRotation);
            }
            //AdjustHipHeight();
        }

    }

  

    private void castFootRay(IKBone footBone) {
        RaycastHit hit;
        //changed this to a max step height instead, so the player doesnt do weird shit
        Ray ray = new Ray(new Vector3(footBone.transform.position.x, transform.parent.position.y+footRayHeight, footBone.transform.position.z), Vector3.down);

        if(Physics.Raycast(ray, out hit, MaxGroundCheckDistance + 1f, GroundMask)) {
            Vector3 footPosition = hit.point; // The target foot position is where the raycast hit a walkable object...

            footBone.SetIKGoalPosition(footPosition+Vector3.up* FeetYOffset);
            footBone.SetIKGoalRotation(Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation);
            footBone.SetIKGoalWeight(footBone.ikGoal == AvatarIKGoal.LeftFoot?curveLeftAmm:curveRightAmm);

            if (LowestLeg == null || LowestLeg.GetIKGoalPosition().y > footBone.GetIKGoalPosition().y) {
                LowestLeg = footBone;

            }
            else if (HighestLeg == null || HighestLeg.GetIKGoalPosition().y < footBone.GetIKGoalPosition().y) {
                HighestLeg = footBone;
            }
        }
        else {
            LowestLeg = null;
            HighestLeg = null;
            footBone.SetIKGoalWeight(0);
        }
    }


    Ray centerOfMassRay;
    Vector3 CenterOfMassOnGround; 
    private void AdjustHipHeight() {
        RaycastHit hit;
        centerOfMassRay = new Ray(rb.position+rb.centerOfMass , -transform.up);
       if(Physics.Raycast(centerOfMassRay, out hit, MaxGroundCheckDistance, GroundMask)) {
            CenterOfMassOnGround = hit.point;

            //anim.SetBoneLocalRotation(HumanBodyBones.Hips, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation);
       }
    }


    private void MoveModel(Vector3 destination) {
        transform.position = Vector3.Lerp(transform.position,destination, adjustmentSpeed*Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rb.position + rb.centerOfMass, -transform.up*MaxGroundCheckDistance);
        Gizmos.DrawSphere(rb.position + rb.centerOfMass, .07f);


        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(CenterOfMassOnGround, .07f);


        Gizmos.DrawSphere(transform.position+transform.up*footRayHeight, .07f);

    }


    private void DrawHelperAtCenter(
                       Vector3 direction, Color color, float scale) {
        Gizmos.color = color;
        Vector3 destination = transform.position + direction * scale;
        Gizmos.DrawLine(transform.position, destination);
    }


 
}


