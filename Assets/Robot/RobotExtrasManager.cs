using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class RobotExtrasManager : MonoBehaviour
{
    [SerializeField] GameObject center;
    public Light siren;
    public int randomColorStart = 0;
    private Color[] colors;
    public float sirenInterval = 0.5f;
    private float raySize = 1.0f;
    private Vector3 lastPosition;
    
    public RobotMovementManager RobotMovementManager;
    public void Init(int _randomColorStart, Vector3 _position, Quaternion _rotation){
        randomColorStart = _randomColorStart;
        transform.position = _position;
        transform.rotation = _rotation;
    }
    
    void Start()
    {
        RobotMovementManager = GetComponent<RobotMovementManager>();
        lastPosition = transform.position;
        colors = new Color[2];
        colors[0] = new Color(1,0,0);
        colors[1] = new Color(1,1,1);
        StartCoroutine(ChangeColorRoutine());
    }

    IEnumerator ChangeColorRoutine(){
        while(true){
            siren.color = colors[randomColorStart];
            randomColorStart = (randomColorStart + 1) % colors.Length;
            yield return new WaitForSeconds(sirenInterval);
        }
    }
    void Update(){
        float distanceMoved = Vector3.Distance(lastPosition, transform.position);

        if (distanceMoved >= 1.0f){
            RaycastCheck();
            lastPosition = transform.position;
        }
    }

    void RaycastCheck(){
        Vector3 p1 = center.transform.position;

        if (Physics.Raycast(p1, center.transform.forward, out RaycastHit hitForward, raySize)){
            if (hitForward.collider.gameObject != this.gameObject){
                RobotMovementManager.isHit = true;
                RobotMovementManager.whatTouched = hitForward.collider.gameObject.tag;
                RobotMovementManager.hitPoint = hitForward.point;
            }
        }

        if (Physics.Raycast(p1, -center.transform.forward, out RaycastHit hitBackward, raySize)){
            if (hitBackward.collider.gameObject != this.gameObject){
                RobotMovementManager.isHit = true;
                RobotMovementManager.whatTouched = hitBackward.collider.gameObject.tag;
                RobotMovementManager.hitPoint = hitBackward.point;
            }
        }

        if (Physics.Raycast(p1, -center.transform.right, out RaycastHit hitLeft, raySize)){
            if (hitLeft.collider.gameObject != this.gameObject){
                RobotMovementManager.isHit = true;
                RobotMovementManager.whatTouched = hitLeft.collider.gameObject.tag;
                RobotMovementManager.hitPoint = hitLeft.point;
            }
        }

        if (Physics.Raycast(p1, center.transform.right, out RaycastHit hitRight, raySize)){
            if (hitRight.collider.gameObject != this.gameObject){
                RobotMovementManager.isHit = true;
                RobotMovementManager.whatTouched = hitRight.collider.gameObject.tag;
                RobotMovementManager.hitPoint = hitRight.point;
            }
        }

        Debug.DrawRay(p1, center.transform.forward * raySize, Color.red);
        Debug.DrawRay(p1, -center.transform.forward * raySize, Color.blue);
        Debug.DrawRay(p1, -center.transform.right * raySize, Color.green);
        Debug.DrawRay(p1, center.transform.right * raySize, Color.yellow);
    }
}
