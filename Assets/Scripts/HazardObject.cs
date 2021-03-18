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

    [ContextMenu("OnHighlight")]
    private void CallOnHighlight(){
        base.onHighlight.Invoke(new object[]{});
    }

    [ContextMenu("OffHighlight")]
    private void CallOffHighlight(){
        base.offHighlight.Invoke(new object[]{});
    }

    [ContextMenu("OffInteract")]
    private void CallOffInteract(){
        base.offInteract.Invoke(new object[]{});
    }

    [ContextMenu("OnInteract")]
    private void CallOnInteract()
    {
        base.onInteract.Invoke(new object[] { });
    }

    public void IncreaseScore()
    {
        var packet = new Mouledoux.Callback.Packet();
        packet.floats = new float[]{m_scoreValue};
        packet.bools = new bool[]{false};
        object[] data = {packet};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("incrementcurrentscore", data);
    }

    public void NotActive(){
        var packet = new Mouledoux.Callback.Packet();
        packet.ints = new int[]{gameObject.GetInstanceID()};
        object[] data = {packet};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("nolongeractive", data);
    }

    private void AppendName(Mouledoux.Callback.Packet a_packet){
        var packet = new Mouledoux.Callback.Packet();
        packet.strings = new string[]{gameObject.name + ", "};
        object[] data = {packet};
        Mouledoux.Components.Mediator.instance.NotifySubscribers("appendbigtext", data);
    }

    public void DisableSelf(){
        Destroy(this);
    }
}