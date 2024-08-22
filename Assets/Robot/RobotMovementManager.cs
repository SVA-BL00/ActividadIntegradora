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
        RaycastHit hit;
        Vector3 p1 = center.transform.position;

        if (Physics.Raycast(p1, center.transform.forward, out hit, 2.0f)){
            string hitTag = hit.collider.gameObject.tag;
            Debug.Log("Tag of the object hit: " + hitTag);
        }

        Debug.DrawRay(p1, center.transform.forward * 1.0f, Color.red);
    }
}
