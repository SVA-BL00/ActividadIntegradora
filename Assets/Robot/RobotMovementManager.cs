using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class RobotMovementManager : MonoBehaviour
{
    Rigidbody botRigid;
    [SerializeField] GameObject center;
    public Light siren;
    public int randomColorStart = 0;
    public float velocity;
    private Color[] colors;
    public float sirenInterval = 0.5f;
    private float raySize = 1.0f;
    private float distanceMoved = 0f;

    public void Init(float _velocity, int _randomColorStart, Vector3 _position, Quaternion _rotation){
        velocity = _velocity;
        randomColorStart = _randomColorStart;
        transform.position = _position;
        transform.rotation = _rotation;
    }
    
    void Start()
    {
        botRigid = GetComponent<Rigidbody>();
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

    void FixedUpdate(){
        float movement = velocity * Time.deltaTime;
        distanceMoved += movement;
        if (distanceMoved >= 0.5f)
        {
            float overshoot = distanceMoved - 0.5f;
            movement -= overshoot;
            distanceMoved = 0f;
        }

        botRigid.MovePosition(botRigid.position + transform.forward * movement);
    }
    void Update(){
        
        Vector3 p1 = center.transform.position;

        if (Physics.Raycast(p1, center.transform.forward, out RaycastHit hitForward, raySize)){
            if (hitForward.collider.gameObject != this.gameObject)
            {
                string hitTagForward = hitForward.collider.gameObject.tag;
                Debug.Log("Forward Ray Hit: " + hitTagForward);
            }
        }

        if (Physics.Raycast(p1, -center.transform.forward, out RaycastHit hitBackward, raySize)){
            if (hitBackward.collider.gameObject != this.gameObject)
            {
                string hitTagBackward = hitBackward.collider.gameObject.tag;
                Debug.Log("Backward Ray Hit: " + hitTagBackward);
            }
        }

        if (Physics.Raycast(p1, -center.transform.right, out RaycastHit hitLeft, raySize)){
            if (hitLeft.collider.gameObject != this.gameObject)
            {
                string hitTagLeft = hitLeft.collider.gameObject.tag;
                Debug.Log("Left Ray Hit: " + hitTagLeft);
            }
        }

        if (Physics.Raycast(p1, center.transform.right, out RaycastHit hitRight, raySize)){
            if (hitRight.collider.gameObject != this.gameObject)
            {
                string hitTagRight = hitRight.collider.gameObject.tag;
                Debug.Log("Right Ray Hit: " + hitTagRight);
            }
        }

        Debug.DrawRay(p1, center.transform.forward * raySize, Color.red);
        Debug.DrawRay(p1, -center.transform.forward * raySize, Color.blue);
        Debug.DrawRay(p1, -center.transform.right * raySize, Color.green);
        Debug.DrawRay(p1, center.transform.right * raySize, Color.yellow);

    }
}
