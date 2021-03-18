using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MironDB
{
    [System.Serializable]
    public sealed class ClientUser
    {
        private ClientUser(){}

        private static ClientUser _instance;
        public static ClientUser instance
        {
            get
            {
                if(_instance == null)
                    _instance = new ClientUser();
                
                return _instance;
            }
        }

        [SerializeField]
        public string userName;
        [SerializeField]
        public string sessionId;
        [SerializeField]
        public float totalVrTime;

        [SerializeField]
        public float practiceLiftAcc, practiceDropAcc;

        [SerializeField]
        public mButton[] scenarioButtons, difficultyLevels;


        

        public static ClientUser LoadUserFromJSON(string json)
        {
            JsonUtility.FromJsonOverwrite(json, instance);
            return instance;
        }

        public static string SaveClientUserToString()
        {
            return JsonUtility.ToJson(instance);
        }
    }

    [System.Serializable]
    public class mButton
    {
        [SerializeField]
        bool enabled;
        [SerializeField]
        string text;
        [SerializeField]
        string uniqueID;
        [SerializeField]
        string message;
    }
}