using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MironDB
{
	public class MironDB_Manager : MonoBehaviour
	{
		[SerializeField]
		public static string dbURI = "https://tantrum.raxxar.com/api";
		public static string companyCode;
		
		private static MironDB_Manager _instance;
		public static MironDB_Manager instance
		{
			get 
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<MironDB_Manager>();
					DontDestroyOnLoad(_instance.gameObject);
				}

				return _instance;
			}
		}

		public static ErrorReturn statusReturn;
		public static UserProfile currentUser;
		public static TestStatus testStatus;

		public static bool m_operating = false;

		public void Start()
		{
			if(instance != null && instance != this) Destroy(gameObject);
		}


		public static void Login(string email, string password, string output)
		{
			instance.StartCoroutine(instance.LoginRoutine(dbURI, email, password, output));
		}

		public static void Register(string email, string password, string firstName, string lastName, string output)
		{
			instance.StartCoroutine(instance.RegisterRoutine(dbURI, email, password, firstName, lastName, output));
		}

		public static void ForgotPassword(string email)
		{
			instance.StartCoroutine(instance.PasswordResetRoutine(dbURI, email));
		}

		public static UserProfile GetuserInformation()
		{
			instance.StartCoroutine(instance.UserProfileRoutine(dbURI));

			return currentUser;
		}

		public static void Logout()
		{
			instance.StartCoroutine(instance.LogoutRoutine(dbURI));
		}
		
		public static void StartTest(int moduleID)
		{
			instance.StartCoroutine(instance.StartTestRoutine(dbURI, moduleID, companyCode));
		}

		public static void UpdateTest(int moduleID, int eventCode, string notes)
		{
			if(testStatus == null) return;

			int difID = 1;

			instance.StartCoroutine(instance.UpdateTestRoutine(dbURI,
				moduleID, testStatus.sessionid, difID, eventCode, 1, notes));
		}

		public static void FinishTest()
		{
			if(testStatus == null) return;

			int sessionid = testStatus.sessionid;
			instance.StartCoroutine(instance.FinishTestRoutine(dbURI, sessionid));
		}

#region Coroutines

		// Login
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		public IEnumerator LoginRoutine(string uri, string email, string password, string output)
		{
			m_operating = true;
			uri += "/post/user/login";

			WWWForm form = new WWWForm();
			form.AddField("email", email);
			form.AddField("password", password);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
			output = $"{statusReturn.status}: {statusReturn.error_description}";
			m_operating = false;
		}


		// Logout
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator LogoutRoutine(string uri)
		{
			uri += "/get/user/logout";

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			DebugResults(www);
		}


		// Register
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator RegisterRoutine(string uri, string email, string password, string firstName, string lastName, string output)
		{
			m_operating = true;
			uri += "/post/user/register";

			WWWForm form = new WWWForm();
			form.AddField("email", email);
			form.AddField("password", password);
			form.AddField("firstname", firstName);
			form.AddField("lastname", lastName);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
			output = $"{statusReturn.status}: {statusReturn.error_description}";
			m_operating = false;
		}


		IEnumerator UserProfileRoutine(string uri)
		{
			uri += "/get/user/profile";

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			DebugResults(www);
			currentUser = JsonUtility.FromJson<UserProfile>(www.downloadHandler.text);
		}


		// Login reminder
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator LoginReminderRoutine(string uri, string email)
		{
			uri += "/post/user/remind";

			WWWForm form = new WWWForm();
			form.AddField("email", email);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
		}
		

		// Password reset
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator PasswordResetRoutine(string uri, string email)
		{
			uri += "/post/user/reset";

			WWWForm form = new WWWForm();
			form.AddField("email", email);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
		}


		// Login status
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator LoginStatusRoutine(string uri)
		{
			uri += "/post/user/reset";
			
			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			DebugResults(www);
		}


		// Difficulty levels
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator DifficultyLevelRoutine(string uri)
		{
			uri += "/get/general/diflevels";

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			DebugResults(www);
		}


		// Module modes
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator ModuleModesRoutine(string uri)
		{
			uri += "/get/general/modulemodes";

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			DebugResults(www);
		}


		// Module List
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator ModuleListRoutine(string uri, int difLevel, int moduleMode)
		{
			uri += "/post/general/modulelist";

			WWWForm form = new WWWForm();
			form.AddField("diflevel", difLevel);
			form.AddField("modulemode", moduleMode);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
		}


		// Module details
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator ModuleDetails(string uri, int moduleID)
		{
			uri += "/post/general/moduledetails";

			WWWForm form = new WWWForm();
			form.AddField("moduleid", moduleID);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
		}

		
		// Start test
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator StartTestRoutine(string uri, int moduleID, string machineID)
		{
			uri += "/post/general/starttest";

			WWWForm form = new WWWForm();
			form.AddField("moduleid", moduleID);
			form.AddField("machineKey", machineID);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
			testStatus = JsonUtility.FromJson<TestStatus>(www.downloadHandler.text);
		}

		
		// Update test
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator UpdateTestRoutine(string uri, int moduleID, int sessionid, int difID, int eventCode, int examID, string notes)
		{
			uri += "/post/general/updatetest";

			WWWForm form = new WWWForm();
			form.AddField("moduleid", moduleID); 	// scenario index
			form.AddField("sessionid", sessionid);		// DB only
			form.AddField("difid", difID);			// diff
			form.AddField("code", eventCode);		// special events
			form.AddField("examid", examID);		// list of modules
			form.AddField("codeDescription", notes);			// notes

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
		}


		// Finish test
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator FinishTestRoutine(string uri, int sessionid)
		{
			m_operating = true;
			uri += "/post/general/finishtest";

			WWWForm form = new WWWForm();
			form.AddField("sessionid", sessionid);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			DebugResults(www);
			testStatus = null;
			m_operating = false;
		}
#endregion


		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		private void DebugResults(UnityEngine.Networking.UnityWebRequest webReq)
		{
			Debug.Log(webReq.downloadHandler.text);
			statusReturn = JsonUtility.FromJson<ErrorReturn>(webReq.downloadHandler.text);
		}



		private void OnApplicationQuit()
		{
			print("ded");
			Logout();
		}


		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		[System.Serializable]
		public sealed class ErrorReturn
		{
			public string status;
			public string error_code;
			public string error_description;
		}
		
		[System.Serializable]
		public sealed class UserProfile
		{
			public string status;
			public string _errors;

			public string id;
			public string name;
			public string username;
			public string email;
			
			public string block;
			public string[] groups;

			public string sendEmail;
			public string registerDate;
			public string lastvisitDate;
			public string activation;
			public string lastResetTime;
			
			public string resetCount;
			public string requireReset;
		}

		[System.Serializable]
		public sealed class UserLoginStatus
		{
			public string status;
			public string is_guest;
			public string user_id;
			public string session_id;
			public string session_expire;
		}

		[System.Serializable]
		public sealed class TestStatus
		{
			public string status;
			public int sessionid;
		}
	}
}