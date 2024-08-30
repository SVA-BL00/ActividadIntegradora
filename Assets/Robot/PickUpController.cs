using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] GameObject CarryPlace;
    public GameObject carrying;
    public bool hasObject = false;
    private RobotMovementManager RMM;
    public BotInstantiator BIS;
    //private ObjectPile OP;

    void Start(){
        RMM = GetComponent<RobotMovementManager>();
        GameObject instantiatorObject = GameObject.Find("Instantiator");
        BIS = instantiatorObject.GetComponent<BotInstantiator>();
    }

    public void PickUp(string name) {
        GameObject pickup = GameObject.Find(name);

        Rigidbody foundRB = pickup.GetComponent<Rigidbody>();
        if(foundRB == null){
            Debug.LogError("Rigidbody component missing on object");
            return;
        }

        foundRB.isKinematic = true;

        pickup.transform.SetParent(CarryPlace.transform);
        pickup.transform.localPosition = Vector3.zero;
        pickup.transform.localRotation = Quaternion.identity;

        if(pickup.transform.parent == CarryPlace.transform){
            hasObject = true;
            carrying = pickup;
        }
    }


    public void Drop(string name){
        GameObject shelfToDrop = GameObject.Find(name);

        ObjectPile objectPile = shelfToDrop.GetComponent<ObjectPile>();
        Destroy(carrying);
        carrying = null;
        hasObject = false;
        objectPile.objectsPiled++;
        BIS.totalPlaced++;
    }

}
