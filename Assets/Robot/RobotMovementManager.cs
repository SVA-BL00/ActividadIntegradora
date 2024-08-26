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
    private Quaternion targetRotation;
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
            ClampPosition();
            RotateToObject();
        }
    }
    IEnumerator CheckHit(){
        while(true) {
            Vector3 p1 = center.transform.position;

            if (Physics.Raycast(p1, center.transform.forward, out RaycastHit hitForward, raySize)){
                if (hitForward.collider.gameObject != this.gameObject){
                    whatTouched = hitForward.collider.gameObject.tag;
                    hitPoint = hitForward.point;
                    isHit = true;
                    yield break;
                }
            }

            if (Physics.Raycast(p1, -center.transform.forward, out RaycastHit hitBackward, raySize)){
                if (hitBackward.collider.gameObject != this.gameObject){
                    whatTouched = hitBackward.collider.gameObject.tag;
                    hitPoint = hitBackward.point;
                    isHit = true;
                    yield break;
                }
            }

            if (Physics.Raycast(p1, -center.transform.right, out RaycastHit hitLeft, raySize)){
                if (hitLeft.collider.gameObject != this.gameObject){
                    whatTouched = hitLeft.collider.gameObject.tag;
                    hitPoint = hitLeft.point;
                    isHit = true;
                    yield break;
                }
            }

            if (Physics.Raycast(p1, center.transform.right, out RaycastHit hitRight, raySize)){
                if (hitRight.collider.gameObject != this.gameObject){
                    whatTouched = hitRight.collider.gameObject.tag;
                    hitPoint = hitRight.point;
                    isHit = true;
                    yield break;
                }
            }

            Debug.DrawRay(p1, center.transform.forward * raySize, Color.red);
            Debug.DrawRay(p1, -center.transform.forward * raySize, Color.blue);
            Debug.DrawRay(p1, -center.transform.right * raySize, Color.green);
            Debug.DrawRay(p1, center.transform.right * raySize, Color.yellow);

            yield return new WaitForSeconds(1f);
        }
    }
    void ClampPosition(){
        Vector3 position = transform.position;

        position.x = Mathf.Round(position.x * 2) / 2f;
        position.z = Mathf.Round(position.z * 2) / 2f;

        if (position.x % 1 == 0){
            position.x += 0.5f;
        }
        if (position.z % 1 == 0){
            position.z += 0.5f;
        }

        transform.position = position;
        botRigid.position = position;
    }
    void RotateToObject(){
        Vector3 direction = hitPoint - transform.position;
        direction.y = 0;

        targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f){
            transform.rotation = targetRotation;
        }
    }

}
