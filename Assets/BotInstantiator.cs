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
    [SerializeField] int minRange = 1;
    [Tooltip("Velocity max range")]
    [SerializeField] int maxRange = 4;
    
    [Header("Box settings")]
    public GameObject boxPrefab;
    [SerializeField] int boxAmount;

    [Header("Book settings")]
    public GameObject bookPrefab;
    [SerializeField] int bookAmount;

    [Header("Kit settings")]
    public GameObject kitPrefab;
    [SerializeField] int kitAmount;

    Dictionary<string, Vector3> positionTracker = new Dictionary<string, Vector3>(); //tambien mandar esto
    void Start()
    {
        BotSpawner();
        BoxSpawner();
        BookSpawner();
        KitSpawner();
        foreach (var kvp in positionTracker)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
    }

    void BotSpawner(){
        for (int i = 0; i < botAmount; i++){
            GameObject currentbot = Instantiate(botPrefab);
            RobotMovementManager currentBotMove = currentbot.GetComponent<RobotMovementManager>();
            RobotExtrasManager currentBotExtras = currentbot.GetComponent<RobotExtrasManager>();

            float velocity = Random.Range(minRange, maxRange);
            int colorStarter = Random.Range(0,2);
            currentbot.name = "Bot " + i;

            Vector3 position = positionFinder(currentbot.name);
            Quaternion rotation = rotationSet();

            if (currentBotExtras != null){
                currentBotExtras.Init(colorStarter, position, rotation);
            }
            if (currentBotMove != null){
                currentBotMove.Init(velocity);
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
    void BookSpawner(){
        for (int i = 0; i < bookAmount; i++){
            GameObject currentBook = Instantiate(bookPrefab);
            BookPlacer currentBookScript = currentBook.GetComponent<BookPlacer>();
            currentBook.name = "Book " + i;

            Vector3 position = positionFinder(currentBook.name);
            Quaternion rotation = rotationSet();

            if (currentBookScript != null){
                currentBookScript.Init(position, rotation);
            }
        }
    }
    void KitSpawner(){
        for (int i = 0; i < kitAmount; i++){
            GameObject currentKit = Instantiate(kitPrefab);
            KitPlacer currentKitScript = currentKit.GetComponent<KitPlacer>();
            currentKit.name = "Kit " + i;

            Vector3 position = positionFinder(currentKit.name);
            Quaternion rotation = rotationSet();

            if (currentKitScript != null){
                currentKitScript.Init(position, rotation);
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

        positionTracker.Add(objectName,position);
        if (objectName.StartsWith("Box")){
            position.y += 0.5f;
        }
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
