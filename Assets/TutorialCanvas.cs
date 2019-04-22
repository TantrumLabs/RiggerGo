using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text m_tutorialText;

    [Multiline]
    public string m_triggerText, m_hoverText, m_releaseText, m_completeText;

    public UnityEngine.Events.UnityEvent onComplete;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();
    private Mouledoux.Callback.Callback onTrigger, offTrigger, onHover, offHover, onRelease;


    void Start()
    {
        onTrigger   = ((Mouledoux.Callback.Packet p) => SetText(m_hoverText)    );
        offTrigger  = ((Mouledoux.Callback.Packet p) => SetText(m_triggerText)  );
        onHover     = ((Mouledoux.Callback.Packet p) => SetText(m_releaseText)  );
        offHover    = ((Mouledoux.Callback.Packet p) => SetText(m_hoverText)    );

        onRelease   = ((Mouledoux.Callback.Packet p) => {
            SetText(m_completeText);
            m_subscriptions.UnsubscribeAll();
            onComplete.Invoke();
        });
        
        m_subscriptions.Subscribe("lasertriggeron", onTrigger   );
        m_subscriptions.Subscribe("lasertriggeroff", offTrigger );
        m_subscriptions.Subscribe("onhighlight", onHover        );
        m_subscriptions.Subscribe("offhighlight", offHover      );
        m_subscriptions.Subscribe("offinteract", onRelease      );

        SetText(m_triggerText);
        //onComplete.AddListener(delegate{gameObject.SetActive(false);});
    }

    void Update()
    {
        
    }

    public void SetText(string aNewText)
    {
        m_tutorialText.text = aNewText;
    }

    public void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }
}
