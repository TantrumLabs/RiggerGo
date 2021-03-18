using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MironDB
{
	public class MironDB_Manager : MonoBehaviour
	{
		[SerializeField]
		public static string dbURI = "https://training.tantrumlab.com/v2";
		
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


		public static bool isExam = false;
		public static string machineID;
		public static UserLoginStatus userLoginStatus;
		public static ErrorReturn statusReturn;
		public static UserProfile currentUser;
		public static TestStatus testStatus;
		public static ModuleDetails moduleDetails;
		public static ExamDetails examDetails;
		public static TokenWallet tokenWallet;
		public static bool m_operating;



		public void Start()
		{
			if(instance != null && instance != this) Destroy(gameObject);
		}

		public static void Login(string email, string password)
		{
			instance.StartCoroutine(instance.LoginRoutine(dbURI, email, password));
		}


		public static void Register(string email, string password, string firstName, string lastName, UnityEngine.UI.Text output, string storeNo)
		{
			instance.StartCoroutine(instance.RegisterRoutine(dbURI, email, password, firstName, lastName, output, storeNo));
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

		[ContextMenu("Logout")]
		public static void Logout()
		{
			instance.StartCoroutine(instance.LogoutRoutine(dbURI));
		}
		

		public static void CheckTrainerPass(string email, string password)
		{
			if(testStatus == null) return;
			
			instance.StartCoroutine(instance.CheckTrainerPassRoutine(dbURI,
				userLoginStatus.session_id, email, password));
		}


		public static void StartTest(int moduleID)
		{
			instance.StartCoroutine(instance.StartTestRoutine(dbURI, moduleID));
		}


		public static void UpdateTest(int eventCode, string codeDescription, int scenarioID = 0)
		{
			if(testStatus == null) return;

			instance.StartCoroutine(instance.UpdateTestRoutine(dbURI,
				testStatus.sessionid, eventCode, codeDescription, scenarioID.ToString()));
		}


		public static void FinishTest()
		{
			if(testStatus == null) return;

			int testID = testStatus.sessionid;
			instance.StartCoroutine(instance.FinishTestRoutine(dbURI, testID));
		}


		public static void LoadExamDetails(int examID)
		{
			instance.StartCoroutine(instance.ExamDetailsRoutine(dbURI, examID));
		}

		public static void CheckTokens()
		{
			instance.StartCoroutine(instance.CheckTokenRoutine(dbURI));
		}

		public static void SpendToken()
		{
			instance.StartCoroutine(instance.SpendTokenRoutine(dbURI));
		}

		public static void CheckKey(string key)
		{
			instance.StartCoroutine(instance.CheckKeyRoutine(dbURI, key));
		}

#region Coroutines

		// Login
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator LoginRoutine(string uri, string email, string password)
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
			userLoginStatus = JsonUtility.FromJson<UserLoginStatus>(www.downloadHandler.text);
			Debug.Log(userLoginStatus.session_id);

			m_operating = false;
		}


		// Logout
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator LogoutRoutine(string uri)
		{
			m_operating = true;
			uri += "/get/user/logout";

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			DebugResults(www);
			m_operating = false;
		}


		// Register
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator RegisterRoutine(string uri, string email, string password, string firstName, string lastName, UnityEngine.UI.Text output, string storeNo)
		{
			m_operating = true;
			uri += "/post/user/register";

			WWWForm form = new WWWForm();
			form.AddField("email", email);
			form.AddField("password", password);
			form.AddField("firstname", firstName);
			form.AddField("lastname", lastName);
			form.AddField("machineKey", machineID);
			form.AddField("shopid", storeNo);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
			// output.text = $"{statusReturn.status}: {statusReturn.error_description}";
		}


		// Check Account
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator CheckTrainerPassRoutine(string uri, string sessionID, string email, string password)
		{
			m_operating = true;
			uri += "/post/generalv2v2/checktrainerpass";
			
			WWWForm form = new WWWForm();
			form.AddField("sessionid", sessionID);
			form.AddField("email", email);
			form.AddField("password", password);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}
		

		// User Profile
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator UserProfileRoutine(string uri)
		{
			m_operating = true;
			uri += "/get/user/profile";

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			DebugResults(www);
			currentUser = JsonUtility.FromJson<UserProfile>(www.downloadHandler.text);

            

			m_operating = false;
		}


		// Login reminder
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator LoginReminderRoutine(string uri, string email)
		{
			m_operating = true;
			uri += "/post/user/remind";

			WWWForm form = new WWWForm();
			form.AddField("email", email);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}
		

		// Password reset
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- 
		IEnumerator PasswordResetRoutine(string uri, string email)
		{
			m_operating = true;
			uri += "/post/user/reset";

			WWWForm form = new WWWForm();
			form.AddField("email", email);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}


		// Login status
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator LoginStatusRoutine(string uri)
		{
			m_operating = true;
			uri += "/get/user/status";
			
			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}


		// Difficulty levels
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator DifficultyLevelRoutine(string uri)
		{
			m_operating = true;
			uri += "/get/generalv2/diflevels";

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}


		// Module modes
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator ModuleModesRoutine(string uri)
		{
			m_operating = true;
			uri += "/get/generalv2/modulemodes";

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}


		// Module List
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator ModuleListRoutine(string uri, int difLevel, int moduleMode)
		{
			m_operating = true;
			uri += "/post/generalv2/modulelist";

			WWWForm form = new WWWForm();
			form.AddField("diflevel", difLevel);
			form.AddField("modulemode", moduleMode);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}


		// Module details
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator ModuleDetailsRoutine(string uri, int moduleID)
		{
			m_operating = true;
			uri += "/post/generalv2/moduledetails";

			WWWForm form = new WWWForm();
			form.AddField("moduleid", moduleID);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
			moduleDetails = JsonUtility.FromJson<ModuleDetails>(www.downloadHandler.text);
		}


		// Exam details
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator ExamDetailsRoutine(string uri, int examID)
		{
			m_operating = true;
			uri += "/post/generalv2/examdetails";

			WWWForm form = new WWWForm();
			form.AddField("itemid", examID);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
			examDetails = JsonUtility.FromJson<ExamDetails>(www.downloadHandler.text);
		}

		
		// Start test
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator StartTestRoutine(string uri, int moduleID)
		{
			m_operating = true;
			uri += "/post/generalv2/starttest";

			WWWForm form = new WWWForm();
            form.AddField("session_id", userLoginStatus.session_id);
			form.AddField("moduleid", moduleID);
			form.AddField("machineKey", machineID);
            

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
			testStatus = JsonUtility.FromJson<TestStatus>(www.downloadHandler.text);
		}

		
		// Update test
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator UpdateTestRoutine(string uri, int sessionID, int eventCode, string codeDescription, string scenarioID = null)
		{
			m_operating = true;
			uri += "/post/generalv2/updatetest";

			WWWForm form = new WWWForm();
            form.AddField("session_id", userLoginStatus.session_id);
            form.AddField("scenarioID", scenarioID);
			form.AddField("sessionID", sessionID);
			form.AddField("codeID", eventCode);
			form.AddField("codeContent", codeDescription);

            UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			Debug.Log("Sending update...");
			yield return www.SendWebRequest();
			DebugResults(www);

			m_operating = false;
		}


		// Finish test
		// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
		IEnumerator FinishTestRoutine(string uri, int sessionID)
		{
			m_operating = true;
			uri += "/post/generalv2/finishtest";

			WWWForm form = new WWWForm();
            form.AddField("session_id", userLoginStatus.session_id);
            form.AddField("sessionid", sessionID);

            UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
			testStatus = null;
			examDetails = null;
		}

		IEnumerator CheckTokenRoutine(string uri)
		{
			m_operating = true;
			uri += "/post/generalv2/checktokens";

			WWWForm form = new WWWForm();
			form.AddField("machineKey", machineID);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
			tokenWallet = JsonUtility.FromJson<TokenWallet>(www.downloadHandler.text);
		}

		
		IEnumerator SpendTokenRoutine(string uri)
		{
			m_operating = true;
			uri += "/post/generalv2/spendtoken";

			WWWForm form = new WWWForm();
			form.AddField("sessionid", userLoginStatus.session_id);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}


		IEnumerator CheckKeyRoutine(string uri, string key)
		{
			m_operating = true;
			uri += "/post/generalv2/checkkey";

			WWWForm form = new WWWForm();
			form.AddField("key", key);

			UnityEngine.Networking.UnityWebRequest www =
				UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			yield return www.SendWebRequest();

			m_operating = false;
			DebugResults(www);
		}


		IEnumerator CheckGemaltoKeyRoutine(string uri, string fingerprint)
		{
			m_operating = true;
			Debug.LogError("Not ready yet");
			m_operating = false;
			yield break;

			// uri += "/post/generalv2/gadfsgdf";
			
			// WWWForm form = new WWWForm();
			// form.AddField("password", fingerprint);

			// UnityEngine.Networking.UnityWebRequest www =
			// 	UnityEngine.Networking.UnityWebRequest.Post(uri, form);
			// yield return www.SendWebRequest();

			// DebugResults(www);

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

		[System.Serializable]
		public sealed class ExamDetails
		{
			public string status;
			public int examid;
			public Details details;
			public Requirements[] requirements;
		}

		[System.Serializable]
		public sealed class ModuleDetails
		{
			public string status;
			public Details[] itemList;
		}


		[System.Serializable]
		public sealed class Details
		{
			public int id;
			public string itemName;
			public int itemActive;
			public int parentItem;
			public int isExam;
		}

		[System.Serializable]
		public sealed class Requirements
		{
			public string labelText;
			public int id;
			public int examID;
			public string stepName;
			public int difficultyID;
			public int attemptsNo;
		}

		[System.Serializable]
		public sealed class TokenWallet
		{
			public string status;
			public string companyName;
			public int remainingTokens;
			public string error_description;
		}
	}
}