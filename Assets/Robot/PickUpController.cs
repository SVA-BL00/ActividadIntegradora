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

    void Start()
    {
        RMM = GetComponent<RobotMovementManager>();
        GameObject instantiatorObject = GameObject.Find("Instantiator");
        BIS = instantiatorObject.GetComponent<BotInstantiator>();
    }

    void Update()
    {
        
    }

    public void PickUp()
    {
        string objectPickUp = RMM.objectRecognized;
        GameObject foundObject = GameObject.Find(objectPickUp);
        
        Rigidbody foundRB = foundObject.GetComponent<Rigidbody>();
        foundRB.isKinematic = true;

        foundObject.transform.SetParent(CarryPlace.transform);
        foundObject.transform.localPosition = Vector3.zero;
        foundObject.transform.localRotation = Quaternion.identity;
        
        if (foundObject.transform.parent == CarryPlace.transform)
        {
            hasObject = true;
            carrying = foundObject;
        }
    }

    public void Drop()
    {
        if (!hasObject || carrying == null)
        {
            return;
        }

        string shelf = RMM.objectRecognized;
        GameObject shelfToDrop = GameObject.Find(shelf);
        
        ObjectPile objectPile = shelfToDrop.GetComponent<ObjectPile>();
        
        // if (objectPile.objectsPiled >= 5){
        //     Debug.Log("Shelf is full. Cannot drop object.");
        //     return;
        // }

        // string emptySlotName = objectPile.slotReturn();
        
        // GameObject emptySlot = GameObject.Find(emptySlotName);
        
        // carrying.transform.localScale *= 0.25f;

        // carrying.transform.SetParent(emptySlot.transform);
        // carrying.transform.localPosition = Vector3.zero;
        Destroy(carrying);
        carrying = null;
        hasObject = false;
        objectPile.objectsPiled++;
        BIS.totalPlaced++;
    }
}
