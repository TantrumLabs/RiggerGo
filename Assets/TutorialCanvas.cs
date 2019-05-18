using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text m_tutorialText;

    public AudioOffset m_audioOffset;

    [Multiline]
    public string m_triggerText, m_hoverText, m_releaseText, m_completeText;

    public UnityEngine.Events.UnityEvent onComplete;

    public float m_onTriggerStart, m_onTriggerOffset, m_offTriggerStart, m_offTriggerOffset, m_onHoverStart, m_onHoverOffset, m_completeStart, m_completeOffset;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();
    private Mouledoux.Callback.Callback onTrigger, offTrigger, onHover, offHover, onRelease;


    void Start()
    {
        onTrigger   = ((object[] args) => SetText(m_hoverText,m_onTriggerStart,m_onTriggerOffset)    );
        offTrigger  = ((object[] args) => SetText(m_triggerText,m_offTriggerStart,m_offTriggerOffset)  );
        onHover     = ((object[] args) => SetText(m_releaseText,m_onHoverStart, m_onHoverOffset)  );
        offHover    = ((object[] args) => SetText(m_hoverText,0,0)    );

        onRelease   = ((object[] args) => {
            SetText(m_completeText,m_completeStart,m_completeOffset);
            m_subscriptions.UnsubscribeAll();
            onComplete.Invoke();
        });
        
        m_subscriptions.Subscribe("lasertriggeron", onTrigger   );
        m_subscriptions.Subscribe("lasertriggeroff", offTrigger );
        m_subscriptions.Subscribe("onhighlight", onHover        );
        m_subscriptions.Subscribe("offhighlight", offHover      );
        m_subscriptions.Subscribe("offinteract", onRelease      );

        SetText(m_triggerText, 0, 0);
        //onComplete.AddListener(delegate{gameObject.SetActive(false);});
    }

    void Update()
    {
    
    }

    public void TriggerRelease(){
        
        var obj = new object[]{};
        onRelease.Invoke(obj);
    }

    public void SetText(string aNewText, float startAt, float offSet)
    {
        m_tutorialText.text = aNewText;
        m_audioOffset.StartAudioAt(startAt);
        m_audioOffset.SetAudioOffset(offSet);
    }

    public void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }
}
