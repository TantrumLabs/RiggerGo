using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetApplicationLocker : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onSerialCheckFail;
    private string m_seriaNumber;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(4f);

        var jo = new AndroidJavaObject("android.os.Build");

        m_seriaNumber = jo.GetStatic<string>("SERIAL");

        MironDB.MironDB_Manager.statusReturn = null;
        MironDB.MironDB_Manager.CheckKey(m_seriaNumber);

        yield return new WaitWhile(() => MironDB.MironDB_Manager.statusReturn == null);

        if(MironDB.MironDB_Manager.statusReturn.status != "ok"){
            MironDB.MironDB_Manager.statusReturn = null;
            MironDB.MironDB_Manager.UpdateTest(1, 
                $"ALARM:UNAUTHORIZED HEADSET DETECTED!!|||Company:{MironDB.MironDB_Manager.machineID}|||HeadsetID:{m_seriaNumber}", 2000);

            yield return new WaitWhile(() => MironDB.MironDB_Manager.statusReturn == null);

            MironDB.MironDB_Manager.Logout();
            MironDB.MironDB_Manager.machineID = null;
            onSerialCheckFail.Invoke();
        }
    }
}
