using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MironDB_TEST : MonoBehaviour
{
	//public OfflineUserScriptableObject userInfo;
	public string dbURI = "";
	public TMPro.TextMeshProUGUI m_textObject;
	
	// Use this for initialization
	[ContextMenu("Restart")]
	IEnumerator Start ()
	{
		var httpClient = UnityEngine.Networking.UnityWebRequest.Get("https://api.ipify.org");
		yield return httpClient.SendWebRequest();
		print($"My public IP address is: {httpClient.downloadHandler.text}");

    	WWWForm form = new WWWForm();

		// form.AddField("username", "eric@tantrumlab.com");
		// form.AddField("password", "test123");
		// StartCoroutine(Upload("https://api.raxxarwebservices.com/Mouledoux/post/user/login", form));

		form.AddField("username", "eric@tantrumlab.com");
		form.AddField("password", "test123");
		StartCoroutine(Upload(dbURI, form));

	}

	IEnumerator Upload(string uri, WWWForm form)
	{
    //     WWWForm form = new WWWForm();
	// 	form.AddField("username", "eric@tantrumlab.com");
	// 	form.AddField("password", "test123");

        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post(uri, form);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
		{
            Debug.Log(www.error);
        }

        else
		{
			WriteToObject($"Text {www.downloadHandler.text}");
            Debug.Log("Form upload complete!");
			Debug.Log($"Text {www.downloadHandler.text}");
			Debug.Log($"Data {www.downloadHandler.data}");
        }


		JsonUtility.ToJson(www.downloadHandler.data);
    }

	IEnumerator Login(string uri, string username, string password)
	{
		WWWForm form = new WWWForm();
		form.AddField("username", username);
		form.AddField("password", password);

		StartCoroutine(Upload(uri, form));
		yield return new WaitForSeconds(3f);
	}

	private void WriteToObject(string text){
		m_textObject.text = text;
	}
}