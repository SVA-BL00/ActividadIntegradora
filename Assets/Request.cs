using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Request : MonoBehaviour
{
    public void SendDataToServer(JSONpy data, System.Action<string> callback)
    {
        StartCoroutine(PostRequest("http://127.0.0.1:5000/receiver", JsonUtility.ToJson(data), callback));
    }

    private IEnumerator PostRequest(string uri, string jsonData, System.Action<string> callback)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError 
                || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + webRequest.error);
                Debug.Log("Response Code: " + webRequest.responseCode);
                callback(null); // Return null in case of error
            }
            else
            {
                string response = webRequest.downloadHandler.text;
                callback(response); // Pass the response to the callback
            }
        }
    }
}

