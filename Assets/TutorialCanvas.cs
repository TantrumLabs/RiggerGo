using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI m_tutorialText;

    public AudioOffset m_audioOffset;

    [Multiline]
    public string m_triggerText, m_hoverText, m_releaseText, m_completeText;

    [Space]

    [Multiline]
    public string m_triggerTextUK, m_hoverTextUK, m_releaseTextUK, m_completeTextUK;

    public UnityEngine.Events.UnityEvent onComplete;

    public float m_onTriggerStart, m_onTriggerOffset, m_offTriggerStart, m_offTriggerOffset, m_onHoverStart, m_onHoverOffset, m_completeStart, m_completeOffset;

    public List<AudioClip> m_onTrigger, m_offTrigger, m_onHover, m_offHover, m_onRelease;
    
    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();
    private Mouledoux.Callback.Callback onTrigger, offTrigger, onHover, offHover, onRelease;


    void OnEnable()
    {
        //UK Voice over
        if(AudioOffset.m_UKVersion){
            onTrigger   = ((object[] args) => SetText(m_hoverTextUK, m_onTrigger)    );
            offTrigger  = ((object[] args) => SetText(m_triggerTextUK, m_offTrigger)  );
            //onHover     = ((object[] args) => SetText(m_releaseTextUK, m_onHover)  );
            //offHover    = ((object[] args) => SetText(m_releaseTextUK, m_offHover)    );

            onRelease   = ((object[] args) => {
                SetText(m_completeText, m_onRelease);
                m_subscriptions.UnsubscribeAll();
                onComplete.Invoke();
            });
        
            m_subscriptions.Subscribe("lasertriggeron", onTrigger   );
            m_subscriptions.Subscribe("lasertriggeroff", offTrigger );
            // m_subscriptions.Subscribe("onhighlight", onHover        );
            // m_subscriptions.Subscribe("offhighlight", offHover      );
            m_subscriptions.Subscribe("offinteract", onRelease      );

            offTrigger.Invoke(new object[]{});
        }

        //Merica Voice over
        else{
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

            SetText(m_triggerText, m_offTrigger);
            //onComplete.AddListener(delegate{gameObject.SetActive(false);});
        }
        
    }

    void Update()
    {
    
    }

    // private void OnEnable() {
    //     if(AudioOffset.m_UKVersion)


    //     onTrigger.Invoke(new object[]{});    
    // }

    [ContextMenu("On Release")]
    public void TriggerRelease(){
        
        var obj = new object[]{};
        onRelease.Invoke(obj);
    }

    [ContextMenu("On Trigger")]
    public void OnTrigger(){
        
        var obj = new object[]{};
        onTrigger.Invoke(obj);
    }

    [ContextMenu("Off Trigger")]
    public void OffTrigger(){
        
        var obj = new object[]{};
        offTrigger.Invoke(obj);
    }

    [ContextMenu("Off Hover")]
    public void OffHover(){
        
        var obj = new object[]{};
        offHover.Invoke(obj);
    }

    public void SetText(string aNewText, float startAt, float offSet)
    {
        m_tutorialText.text = aNewText;
        m_audioOffset.StartAudioAt(startAt);
        m_audioOffset.SetAudioOffset(offSet);
    }

    public void SetText(string aNewText, List<AudioClip> audioClips)
    {
        m_audioOffset.UKAudioClip = audioClips;
        m_tutorialText.text = aNewText;
        m_audioOffset.StartAudioAt(0);
    }

    public void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }
}
