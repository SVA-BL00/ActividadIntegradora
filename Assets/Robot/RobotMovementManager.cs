using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovementManager : MonoBehaviour
{
    Rigidbody botRigid;
    public float velocity;
    private float distanceMoved = 0f;
    public string whatTouched;
    public Vector3 hitPoint;
    public bool isHit = false;
    public float rotationSpeed = 1f;
    private Quaternion targetRotation;
    private bool isRotating = false;
    private bool decision = false;
    public Animator animator;

    public void Init(float _velocity){
        velocity = _velocity;
    }

    void Start()
    {
        botRigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        targetRotation = transform.rotation;
    }

    void FixedUpdate(){
        if (isRotating && !decision)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f){
                transform.rotation = targetRotation;
                isRotating = false;
                isHit = false;
                botRigid.velocity = Vector3.zero;
                botRigid.angularVelocity = Vector3.zero;
                decision = true;
                animator.SetBool("hasObject", true);
                DecisionHandler();
            }
        }else if (!decision){
            float movement = velocity * Time.deltaTime;
            distanceMoved += movement;
            if (distanceMoved >= 0.5f){
                float overshoot = distanceMoved - 0.5f;
                movement -= overshoot;
                distanceMoved = 0f;
            }

            botRigid.MovePosition(botRigid.position + transform.forward * movement);
        }
    }

    void Update()
    {
        if (isHit && !isRotating && !decision)
        {
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0;

            targetRotation = Quaternion.LookRotation(direction);
            isRotating = true;
        }
    }
    void DecisionHandler(){
        // Aquí es donde problablemente se llame el script para ver qué hace con lo que percibe
        
    }
}