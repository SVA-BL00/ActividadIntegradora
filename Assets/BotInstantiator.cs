using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BotInstantiator : MonoBehaviour
{
    [Header("Robot settings")]
    public GameObject botPrefab;
    [SerializeField] int botAmount;
    [Tooltip("Velocity min range")]
    [SerializeField] float minRange = 0.3f;
    [Tooltip("Velocity max range")]
    [SerializeField] float maxRange = 1.5f;
    
    [Header("Box settings")]
    public GameObject boxPrefab;
    [SerializeField] int boxAmount;

    [Header("Storage box settings")]
    public GameObject storagePrefab;
    [SerializeField] int binAmount;

    Dictionary<string, Vector3> positionTracker = new Dictionary<string, Vector3>();
    void Start()
    {
        BotSpawner();
        BoxSpawner();
        
    }

    void BotSpawner(){
        for (int i = 0; i < botAmount; i++){
            GameObject currentbot = Instantiate(botPrefab);
            RobotMovementManager currentBotScript = currentbot.GetComponent<RobotMovementManager>();

            float velocity = Random.Range(minRange, maxRange);
            int colorStarter = Random.Range(0,2);
            currentbot.name = "Bot " + i;

            Vector3 position = positionFinder(currentbot.name);
            Quaternion rotation = rotationSet();

            if (currentBotScript != null){
                currentBotScript.Init(velocity, colorStarter, position, rotation);
            }
        }
    }

    void BoxSpawner(){
        for (int i = 0; i < boxAmount; i++){
            GameObject currentBox = Instantiate(boxPrefab);
            BoxPlacer currentBoxScript = currentBox.GetComponent<BoxPlacer>();
            currentBox.name = "Box " + i;

            Vector3 position = positionFinder(currentBox.name);
            Quaternion rotation = rotationSet();

            if (currentBoxScript != null){
                currentBoxScript.Init(position, rotation);
            }
        }
    }
    Vector3 positionFinder(string objectName){
        Vector3 position;
        do{
            position = new Vector3(
                Mathf.Round(Random.Range(-10f, 9f)) + 0.5f, 
                0, 
                Mathf.Round(Random.Range(-7f, 6f)) + 0.5f
            );
        }while(positionTracker.ContainsValue(position));

        if (objectName.StartsWith("Box")){
            position.y += 0.5f;
        }

        positionTracker.Add(objectName,position);
        return position;
    }

    Quaternion rotationSet(){
        int randInt = Random.Range(1,5);
        Quaternion rotation;
        switch(randInt){
            case 1:
                rotation = Quaternion.Euler(0,0,0);
                break;
            case 2:
                rotation = Quaternion.Euler(0,90,0);
                break;
            case 3:
                rotation = Quaternion.Euler(0,180,0);
                break;
            case 4:
                rotation = Quaternion.Euler(0,270,0);
                break;
            default:
                rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
        return rotation;
    }
}
