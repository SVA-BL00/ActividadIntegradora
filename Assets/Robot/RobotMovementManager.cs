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
        -Vector3.forward, // W
        Vector3.forward   // E
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
        //     case "RotateN":
        //     case "RotateS":
        //     case "RotateE":
        //     case "RotateW":
        //     case "Grab":
        //     case "Drop":
        //         velocity = 0;
        //         ClampPosition();
        //         if(function.StartsWith("Rotate")){
        //             switch(function){
        //                 case "RotateN": Rotate(Vector3.right); break;
        //                 case "RotateS": Rotate(-Vector3.right); break;
        //                 case "RotateW": Rotate(-Vector3.forward); break;
        //                 case "RotateE": Rotate(Vector3.forward); break;
        //             }
                        
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

        for (int i = 0; i < directions.Length; i++){
            if (Physics.Raycast(p1, center.transform.TransformDirection(directions[i]), out RaycastHit hit, raySize)){
                if (hit.collider.gameObject != this.gameObject){
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

        Debug.DrawRay(p1, center.transform.forward * raySize, Color.red);
        Debug.DrawRay(p1, -center.transform.forward * raySize, Color.blue);
        Debug.DrawRay(p1, -center.transform.right * raySize, Color.green);
        Debug.DrawRay(p1, center.transform.right * raySize, Color.yellow);

        for (int i = 0; i < resultTuple.Length; i++){
            Debug.Log($"{resultTuple[i].direction},{resultTuple[i].objectLabel}");
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
    
    void Rotate(Vector3 _direction){
        targetRotation = Quaternion.LookRotation(_direction);
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
        yRotation = (yRotation + 360f) % 360f;
        if (yRotation >= 315f || yRotation < 45f)
            return "E";
        if (yRotation >= 45f && yRotation < 135f)
            return "N";
        if (yRotation >= 135f && yRotation < 225f)
            return "W";
        if (yRotation >= 225f && yRotation < 315f)
            return "S";

        return "Not valid";
    }
}
