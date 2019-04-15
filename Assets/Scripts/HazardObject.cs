using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardObject : InteractableObject
{
    public float m_scoreValue;
    
    public new void Start(){
        //offInteract += AppendName;
        base.Start();
    }

    public void IncreaseScore()
    {
        var packet = new Mouledoux.Callback.Packet();
        packet.floats = new float[]{m_scoreValue};
        packet.bools = new bool[]{false};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("incrementcurrentscore", packet);
    }

    public void NotActive(){
        var packet = new Mouledoux.Callback.Packet();
        packet.ints = new int[]{gameObject.GetInstanceID()};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("nolongeractive", packet);
    }

    private void AppendName(Mouledoux.Callback.Packet packet){
        var data = new Mouledoux.Callback.Packet();
        data.strings = new string[]{gameObject.name + ", "};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("appendbigtext", data);
    }
}