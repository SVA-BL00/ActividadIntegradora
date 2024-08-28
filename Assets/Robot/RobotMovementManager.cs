using System;
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
    public string whatTouched;
    private float waitSecond;
    public string objectRecognized = "Unknown";
    private PickUpController PUC;


    Vector3[] directions = new Vector3[] 
    {
        Vector3.right,    // N
        -Vector3.right,   // S
        Vector3.forward, // W
        -Vector3.forward   // E
    };
    string[] directionLabels = new string[] { "N", "S", "W", "E" };

    public (string direction, string objectLabel)[] resultTuple;
    public bool hasObject = false;
    public string ID;
    public void Init(float _velocity){
        velocity = _velocity;
        tempVelocity = _velocity;
    }

    void Awake(){
        botRigid = GetComponent<Rigidbody>();
        PUC = GetComponent<PickUpController>();
    }
    void Start(){
        ID = GetID(gameObject.name);
        waitSecond = 1 / velocity;
        StartCoroutine(CheckHit());
    }
    void FixedUpdate(){
        HitHandler();
    }
    void Move(){
        if(velocity != tempVelocity){
            velocity = tempVelocity;
        }
        botRigid.MovePosition(botRigid.position + transform.forward * Time.fixedDeltaTime * velocity);
    }

    void HitHandler(){
        // DEPENDE QUÉ RETORNE PYTHON!!!!!
        // string function = pythonscript.choice()
        // switch(function){
        //     case "MoveForward":
        //         Move();
        //         break;
        //     case "Grab":
        //     case "Drop":
        //     case "Turn":
        //         velocity = 0;
        //         ClampPosition();
        //         if(function == "Turn")){
        //            Turn();
        //         }else if(function == "Grab"){
        //             Grab();
        //         }else if(function == "Drop"){
        //             Drop();
        //         }
        //         break;
        // }
    }
    IEnumerator CheckHit(){
        while(true){
            PerformRaycast();
            yield return new WaitForSeconds(waitSecond);
        }
    }
    void PerformRaycast(){
        Vector3 p1 = center.transform.position;
        var hitResults = new List<(string direction, string objectLabel)>();

        string rotationDirection = GetRotation();
        Debug.Log(rotationDirection);

        for(int i = 0; i < directions.Length; i++){
            Vector3 adjustedDirection = GetAdjustedDirection(directions[i], rotationDirection);
            
            if(Physics.Raycast(p1, center.transform.TransformDirection(adjustedDirection), out RaycastHit hit, raySize)){
                if(hit.collider.gameObject != this.gameObject){
                    whatTouched = hit.collider.gameObject.tag;
                    hitPoint = hit.point;
                    objectRecognized = hit.collider.gameObject.name;
                    hitResults.Add((directionLabels[i], whatTouched));
                }
            }else{
                hitResults.Add((directionLabels[i], "0"));
            }
        }

        resultTuple = hitResults.ToArray();
        JSONpy toPython = new JSONpy(ID, transform.position, GetRotation(), resultTuple);

        // Debugging raycasts
        Debug.DrawRay(p1, center.transform.TransformDirection(Vector3.forward) * raySize, Color.red);
        Debug.DrawRay(p1, center.transform.TransformDirection(Vector3.back) * raySize, Color.blue);
        Debug.DrawRay(p1, center.transform.TransformDirection(Vector3.right) * raySize, Color.green);
        Debug.DrawRay(p1, center.transform.TransformDirection(Vector3.left) * raySize, Color.yellow);

        for (int i = 0; i < resultTuple.Length; i++) {
            Debug.Log($"{resultTuple[i].direction},{resultTuple[i].objectLabel}");
        }
    }
    Vector3 GetAdjustedDirection(Vector3 direction, string rotationDirection){
        switch(rotationDirection){
            case "E": //
                return new Vector3(-direction.x, direction.y, -direction.z);
            case "N": //
                return new Vector3(-direction.z, direction.y, direction.x);
            case "W":
                return direction;
            case "S": //
                return new Vector3(direction.z, direction.y, -direction.x);
            default:
                return direction;
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
    void ClampRotation(){
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.y = Mathf.Round(eulerRotation.y / 90f) * 90f;

        eulerRotation.x = 0f;
        eulerRotation.z = 0f;
        transform.rotation = Quaternion.Euler(eulerRotation);
        botRigid.rotation = Quaternion.Euler(eulerRotation);
    }
    
    void Grab(){
        if (!PUC.hasObject){
            PUC.PickUp();
            hasObject = PUC.hasObject;
            ClampPosition();
            ClampRotation();
        }
    }
    
    void Turn(){
        Vector3 holderDir = Vector3.zero;
        switch(GetRotation()){
            case "N":
                holderDir = Vector3.forward;
                break;
            case "E":
                holderDir = -Vector3.right; // South
                break;
            case "S":
                holderDir = -Vector3.forward; // West
                break;
            case "W":
                holderDir = Vector3.right; // North
                break;
        }
        targetRotation = Quaternion.LookRotation(holderDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.05f){
            transform.rotation = targetRotation;
        }
        ClampRotation();
    }
    string GetID(string name){
        string[] parts = name.Split(' ');
        return parts[parts.Length - 1];
    }
    string GetRotation(){
        float yRotation = transform.eulerAngles.y;
        yRotation = (yRotation + 360f) % 360f;  // Normalize to 0-360 range

        if (yRotation > 315f || yRotation <= 45f)
            return "W";
        if (yRotation > 45f && yRotation <= 135f)
            return "N";
        if (yRotation > 135f && yRotation <= 225f)
            return "E";
        if (yRotation > 225f && yRotation <= 315f)
            return "S";

        return "Not valid";
    }
}
