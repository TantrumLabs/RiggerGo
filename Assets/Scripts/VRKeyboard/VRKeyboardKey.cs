using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKeyboardKey : MonoBehaviour
{
    private string m_keyboardID;

    [SerializeField] private string m_mainKey;
    [SerializeField] private string m_shiftKey;

    [Space]
    public bool isShifted;
    public bool isCapsShift;
    public bool isEnabled;

    [Header("TextMesh")]
    [SerializeField] private TMPro.TextMeshPro m_mainText;
    [SerializeField] private TMPro.TextMeshPro m_shiftText;

    [Header("Materials")]
    [SerializeField] private Material m_normalMat;
    [SerializeField] private Material m_highlightMat;
    [SerializeField] private Material m_pressedMat;
    [SerializeField] private Material m_disabledMat;
    [SerializeField] private MeshRenderer m_backgroundMesh;


    public void Initialize(string a_keyboardID, string a_mainString, string a_shiftString = "", bool a_capsShift = true)
    {
        m_keyboardID = a_keyboardID;
        m_mainKey = a_mainString.ToLower();
        m_shiftKey = a_shiftString == "" ? m_mainKey.ToUpper() : a_shiftString;
        isCapsShift = a_capsShift;

        DefaultKey();
        ResetMaterial();
    }


    


    public string GetCurrentKey()
    {
        return m_mainText.text;
    }

    public string GetMainKey()
    {
        return m_mainKey;
    }

    public string GetShiftKey()
    {
        return m_shiftKey;
    }



    public int DefaultKey()
    {
        isShifted = false;
        m_mainText.text = m_mainKey;
        m_shiftText.text = m_shiftKey;
        return 0;
    }

    public int ShiftKey()
    {
        if(isShifted)
        {
            DefaultKey();
            return 1;
        }

        isShifted = true;
        m_mainText.text = m_shiftKey;
        m_shiftText.text = m_mainKey;
        return 0;
    }



    public bool SetKeyEnabled(bool a_enabled)
    {
        isEnabled = a_enabled;
        SetMaterial(isEnabled ? m_normalMat : m_disabledMat);
        return isEnabled;
    }

    private void SetMaterial(Material mat)
    {
        m_backgroundMesh.material = mat;
    }
    private void ResetMaterial()
    {
        SetMaterial(m_normalMat);
    }
}