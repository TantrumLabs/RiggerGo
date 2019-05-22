using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenTextManager : MonoBehaviour
{
    public ForceTeleport m_forceTeleport;
    public TMPro.TextMeshProUGUI m_text;

    // Update is called once per frame
    void Update()
    {
        m_text.text = "";

        if(m_forceTeleport.currentPoint <= 0)
            return;

        m_text.text += "Zone " + m_forceTeleport.currentPoint +"\n";

        switch(m_forceTeleport.currentPoint){
            case 0:
                m_text.text += "Tutorial";
                break;
            
            case 1:
                m_text.text += "Personal Protective Equipment";
                break;

            case 2:
                m_text.text += "Rigger Hand Signals";
                break;
            
            case 3:
                m_text.text += "Sling Straps";
                break;

            case 4:
                m_text.text += "Lift Types and Load Hazards";
                break;

            case 5:
                m_text.text += "Rig Load Pickups";
                break;

            case 6:
                m_text.text += "Supply Vessel Pre-Lift";
                break;

            case 7:
                m_text.text += "Supply Vessel Lift";
                break;

            case 8:
                m_text.text += "Lift from Supply Vessel";
                break;

            case 9:
                m_text.text += "Platform Pre-Lift Inspection";
                break;

            case 10:
                m_text.text += "Platform Lift";
                break;

            case 11:
                m_text.text += "Lift Completion Zone";
                break;

            case 12:
                m_text.text += "Certificate";
                break;

            default:
                break;
        }
    }

    
}
