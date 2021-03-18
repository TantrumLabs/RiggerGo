using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEditor;

public class UIUtils : MonoBehaviour
{
    public void ClickInputField(TMPro.TMP_InputField p_input){
        StartCoroutine(SelectInputField(p_input));
    }

    private IEnumerator SelectInputField(TMPro.TMP_InputField p_input){
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(p_input.gameObject);
    }

    private IEnumerator WaitForAnimator(Animator a){
        
        yield return new WaitUntil(() => a.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
    }

    //public void StartTrackingScore(bool p_bool){
    //    ScoreKeeper.instance.SetTracking(p_bool);
    //}

    //public void AddToScoreLocal(int i){
    //    ScoreKeeper.instance.AddScoreLocally(i);
    //}

    //public void SetScenarioToTrue(string c)
    //{
    //    ScoreKeeper.instance.SetScenarioToTrue(c);
    //}

    //public void SetSceneMessage(string s){
    //    SceneLoader._instance.SetSoloMessage(s);
    //}

    public void GoToScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void GoToScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void ReloadCurrentScene()
    {
        GoToScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToScene(Dropdown dropdown)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(dropdown.captionText.text);
    }

    public void GoToSceneAsync(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
    }

    public void GoToSceneAsync(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
    }

    public void LoadRandomScene(Transform parent)
    {
        var buttonList = parent.GetComponentsInChildren<Button>();
        buttonList[Random.Range(0, buttonList.Length)].onClick.Invoke();
    }

    public void ReloadCurrentSceneAsync()
    {
        GoToSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToSceneAsync(TMPro.TMP_Dropdown dropdown)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(dropdown.captionText.text);
    }


    public static void StaticReloadCurrentSceneAsync()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ToggleGameobjectEnable(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void NotifySubscribers(string subscription)
    {
        Mouledoux.Components.Mediator.instance.NotifySubscribers(subscription, new object[]{});
    }
    
    public void ClickButton(Button b){
        b.onClick.Invoke();
    }

    public void ToggleBool(Toggle t){
        t.isOn = !t.isOn;
    }

    public void ChangeContentType(InputField field){
        if(field.contentType == InputField.ContentType.Password){
            field.contentType = InputField.ContentType.Standard;
        }

        else{
            field.contentType = InputField.ContentType.Password;
        }
    }

    // public static void StartVibration(){
        
    //    VibrateController();
    // }


    // private static IEnumerator VibrateController(){
    //     var controler = OVRInput.GetActiveController();
    //     OVRInput.SetControllerVibration(3f, 3f, controler);

    //     yield return new WaitForSeconds(.1f);

    //     OVRInput.SetControllerVibration(0f, 0f, controler);
    // }

    public void CheckInput(InputField field){
        if(field.text == "Play"){
            Mouledoux.Components.Mediator.instance.NotifySubscribers("startreel");
        }
    }

    public void DestroyGO(GameObject go){
        Destroy(go);
    }

    public void DisplaySerialNumber(TMPro.TextMeshProUGUI message)
    {
        var jo = new AndroidJavaObject("android.os.Build");

        var m_seriaNumber = jo.GetStatic<string>("SERIAL");

        message.text = $"The number is: \n{m_seriaNumber}";
    }

    public void DebugLog(string s)
    {
        Debug.Log(s);
    }
}