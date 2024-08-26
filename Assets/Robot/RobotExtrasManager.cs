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
    
    public RobotMovementManager RobotMovementManager;
    public void Init(int _randomColorStart, Vector3 _position, Quaternion _rotation){
        randomColorStart = _randomColorStart;
        transform.position = _position;
        transform.rotation = _rotation;
    }
    
    void Start()
    {
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

}
