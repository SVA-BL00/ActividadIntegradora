using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BotInstantiator : MonoBehaviour
{
    public GameObject botPrefab;
    [SerializeField] int botAmount;
    [SerializeField] float minRange = 3.0f;
    [SerializeField] float maxRange = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < botAmount; i++){
            GameObject currentbot = Instantiate(botPrefab);
            RobotMovementManager currentBotScript = currentbot.GetComponent<RobotMovementManager>();

            float velocity = Random.Range(minRange, maxRange);
            int colorStarter = Random.Range(1,3);
            Vector3 position = new Vector3(Random.Range(-4f,4f), 0, Random.Range(-7f,7f));
            if (currentBotScript != null){
                currentBotScript.Init(velocity, colorStarter, position);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
