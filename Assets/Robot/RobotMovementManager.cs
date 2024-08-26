using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotMovementManager : MonoBehaviour
{
    Rigidbody botRigid;
    public float velocity;
    public float tempVelocity;
    public float fixedDistance = 1f;
    public float raySize = 1f;
    public Vector3 hitPoint;
    [SerializeField] GameObject center;
    public bool isHit = false;
    string whatTouched;

    public void Init(float _velocity){
        velocity = _velocity;
        tempVelocity = _velocity;
    }

    void Awake(){
        botRigid = GetComponent<Rigidbody>();
    }
    void Start(){
        StartCoroutine(CheckHit());
    }
    void FixedUpdate(){
        HitHandler();
        Move();
    }
    void Move(){
        botRigid.MovePosition(botRigid.position + transform.forward * Time.fixedDeltaTime * velocity);
    }

    void HitHandler(){
        if(isHit){
            velocity = 0;
            //ClampPosition();
        }
    }
    IEnumerator CheckHit(){
        while(true) {
            Vector3 p1 = center.transform.position;

            if (Physics.Raycast(p1, center.transform.forward, out RaycastHit hitForward, raySize)){
                if (hitForward.collider.gameObject != this.gameObject){
                    isHit = true;
                    whatTouched = hitForward.collider.gameObject.tag;
                    hitPoint = hitForward.point;
                }
            }

            if (Physics.Raycast(p1, -center.transform.forward, out RaycastHit hitBackward, raySize)){
                if (hitBackward.collider.gameObject != this.gameObject){
                    isHit = true;
                    whatTouched = hitBackward.collider.gameObject.tag;
                    hitPoint = hitBackward.point;
                }
            }

            if (Physics.Raycast(p1, -center.transform.right, out RaycastHit hitLeft, raySize)){
                if (hitLeft.collider.gameObject != this.gameObject){
                    isHit = true;
                    whatTouched = hitLeft.collider.gameObject.tag;
                    hitPoint = hitLeft.point;
                }
            }

            if (Physics.Raycast(p1, center.transform.right, out RaycastHit hitRight, raySize)){
                if (hitRight.collider.gameObject != this.gameObject){
                    isHit = true;
                    whatTouched = hitRight.collider.gameObject.tag;
                    hitPoint = hitRight.point;
                }
            }

            Debug.DrawRay(p1, center.transform.forward * raySize, Color.red);
            Debug.DrawRay(p1, -center.transform.forward * raySize, Color.blue);
            Debug.DrawRay(p1, -center.transform.right * raySize, Color.green);
            Debug.DrawRay(p1, center.transform.right * raySize, Color.yellow);

            yield return new WaitForSeconds(1f);
        }
    }
}
