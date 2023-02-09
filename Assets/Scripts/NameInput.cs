using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

// refer to https://stackoverflow.com/questions/64361008/unity-webgl-mobile-browser-workaround-and-keyboard-input-fix

public class NameInput : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern string GetWebTextInput();

    [DllImport("__Internal")]
    private static extern bool IsMobile();

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // this apparently must be here to fix a chrome android bug, check linked stackoverflow
    void OnApplicationFocus(bool hasFocus)
    {
    }

    public void OnFocus() {
        try
        {
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            //Debug.Log(inputField.text);
            if (IsMobile())
            {
                inputField.text = GetWebTextInput();
            }
            
        }
        catch (System.Exception e)
        {
            Debug.Log("JSLib had problems, either in editor or something else");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
