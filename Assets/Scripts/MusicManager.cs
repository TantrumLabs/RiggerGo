using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager s_instance;
    public static MusicManager instance{
        get{
            if(s_instance == null){
                s_instance = FindObjectOfType<MusicManager>();
            }

            if(s_instance == null){
                s_instance = new MusicManager();
            }

            return s_instance;
        }
    }

    public List<AudioSource> m_audioSources;

    Mouledoux.Components.Mediator.Subscriptions m_subs = new Mouledoux.Components.Mediator.Subscriptions();

    private void Awake() {
        if(instance != this){
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        //instance.PlayMusic();
    }

    void Start()
    {
        m_subs.Subscribe("TurnMusicOff", delegate{instance.TurnMusicOff();});
        m_subs.Subscribe("TurnMusicOn", delegate{instance.TurnMusicOn();});
        m_subs.Subscribe("MuteMusic", delegate{instance.MuteMusic();});
        m_subs.Subscribe("UnmuteMusic", delegate{instance.UnmuteMusic();});
        m_subs.Subscribe("TurnMusicDown", delegate{instance.TurnMusicDown();});
        m_subs.Subscribe("TurnMusicDownMore", delegate { instance.TurnMusicDownMore(); });
        m_subs.Subscribe("TurnMusicUp", delegate{instance.TurnMusicUp();});
        m_subs.Subscribe("PauseMusic", delegate{instance.PauseMusic();});
        m_subs.Subscribe("PlayMusic", delegate{instance.PlayMusic();});
        m_subs.Subscribe("StopMusic", delegate { instance.StopMusic(); });
    }

    private void OnLevelWasLoaded(){
        StartCoroutine(NewSceneSetter());
    }

    private IEnumerator NewSceneSetter(){
        if(!m_audioSources[0].isPlaying){
            yield break;
        }
        
        instance.PauseMusic();
        yield return new WaitForSeconds(1);
        instance.PlayMusic();
    }

    private void TurnMusicOff(){
        foreach (AudioSource audio in m_audioSources)
        {
            audio.gameObject.SetActive(false);
        }
    }

    private void TurnMusicOn(){
        foreach (AudioSource audio in m_audioSources)
        {
            audio.gameObject.SetActive(true);
        }
    }

    private void MuteMusic(){
        foreach(AudioSource audio in m_audioSources){
            audio.mute = true;
        }
    }

    [ContextMenu("Unmute")]
    private void UnmuteMusic(){
        foreach(AudioSource audio in m_audioSources){
            audio.mute = false;
        }
    }

    private void TurnMusicDown(){
        foreach(AudioSource audio in m_audioSources){
            audio.volume = 0.15f;
        }
    }

    private void TurnMusicDownMore()
    {
        foreach (AudioSource audio in m_audioSources)
        {
            audio.volume = 0.1f;
        }
    }

    private void TurnMusicUp(){
        foreach(AudioSource audio in m_audioSources){
            audio.volume = 0.4f;
        }
    }

    private void PauseMusic(){
        foreach(AudioSource audio in m_audioSources){
            audio.Pause();
        }
    }

    [ContextMenu("Play music")]
    private void PlayMusic(){
        foreach(AudioSource audio in m_audioSources){
            audio.mute = false;
            audio.Play();
        }
    }

    private void StopMusic()
    {
        foreach (AudioSource audio in m_audioSources)
        {
            audio.Stop();
        }
    }

    private void OnDestroy(){
        m_subs.UnsubscribeAll();
    }
}
