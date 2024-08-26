using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] GameObject CarryPlace;
    public bool hasObject;
    private RobotMovementManager RMM;
    void Start()
    {
        RMM = GetComponent<RobotMovementManager>();
        hasObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(){
        Debug.Log("Pick up called");
        hasObject = true;
        string objectPickUp = RMM.objectRecognized;
        Debug.Log(objectPickUp);
        GameObject foundObject = GameObject.Find(objectPickUp);
        
        Rigidbody foundRB = foundObject.GetComponent<Rigidbody>();
        foundRB.isKinematic = true;

        foundObject.transform.SetParent(CarryPlace.transform);
        foundObject.transform.localPosition = Vector3.zero;
        foundObject.transform.localRotation = Quaternion.identity;
    }
}
