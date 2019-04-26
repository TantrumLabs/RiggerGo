using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerManager : MonoBehaviour
{
    private float m_scoreValue;

    public void AddToScore(float value){
        m_scoreValue += value;
    }

    public void SendScore(){
        var packet = new Mouledoux.Callback.Packet();
        packet.floats = new float[]{m_scoreValue};
        packet.bools = new bool[]{false};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("incrementcurrentscore", packet);
    }
}
