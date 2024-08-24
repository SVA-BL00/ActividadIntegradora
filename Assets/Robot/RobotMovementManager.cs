using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class RobotMovementManager : MonoBehaviour
{
    Rigidbody botRigid;
    [SerializeField] GameObject center;
    [SerializeField] SpotLight siren;
    public int randomColorStart;
    public float velocity;
    private float raySize = 1.0f;

    public void Init(float _velocity, int _randomColorStart, Vector3 _position)
    {
        velocity = _velocity;
        randomColorStart = _randomColorStart;
        transform.position = _position;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 p1 = center.transform.position;

        // Forward Ray
        if (Physics.Raycast(p1, center.transform.forward, out RaycastHit hitForward, raySize))
        {
            if (hitForward.collider.gameObject != this.gameObject)
            {
                string hitTagForward = hitForward.collider.gameObject.tag;
                Debug.Log("Forward Ray Hit: " + hitTagForward);
            }
        }

        // Backward Ray
        if (Physics.Raycast(p1, -center.transform.forward, out RaycastHit hitBackward, raySize))
        {
            if (hitBackward.collider.gameObject != this.gameObject)
            {
                string hitTagBackward = hitBackward.collider.gameObject.tag;
                Debug.Log("Backward Ray Hit: " + hitTagBackward);
            }
        }

        // Left Ray
        if (Physics.Raycast(p1, -center.transform.right, out RaycastHit hitLeft, raySize))
        {
            if (hitLeft.collider.gameObject != this.gameObject)
            {
                string hitTagLeft = hitLeft.collider.gameObject.tag;
                Debug.Log("Left Ray Hit: " + hitTagLeft);
            }
        }

        // Right Ray
        if (Physics.Raycast(p1, center.transform.right, out RaycastHit hitRight, raySize))
        {
            if (hitRight.collider.gameObject != this.gameObject)
            {
                string hitTagRight = hitRight.collider.gameObject.tag;
                Debug.Log("Right Ray Hit: " + hitTagRight);
            }
        }

        // Debug Ray Visualization
        Debug.DrawRay(p1, center.transform.forward * raySize, Color.red);
        Debug.DrawRay(p1, -center.transform.forward * raySize, Color.blue);
        Debug.DrawRay(p1, -center.transform.right * raySize, Color.green);
        Debug.DrawRay(p1, center.transform.right * raySize, Color.yellow);

    }
}
