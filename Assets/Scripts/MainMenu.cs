using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class MainMenu : MonoBehaviour
{

    [SerializeField] TMP_InputField _nameInputField;
    // Start is called before the first frame update
    void Start()
    {
        // guest login with lootlocker
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");
                return;
            }

            Debug.Log("successfully started LootLocker session");
        });
        _nameInputField.text = PlayerPrefs.GetString(Constants.NamePlayerPref);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");


        AudioManager.Instance.PlayClick();
    }

    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");

        AudioManager.Instance.PlayClick();
    }
    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");

        AudioManager.Instance.PlayClick();
    }
    public void GoToTutorial()
    {
        SceneManager.LoadScene("HowToPlay");

        AudioManager.Instance.PlayClick();
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void StoreName(string name)
    {
        PlayerPrefs.SetString(Constants.NamePlayerPref, name);
    }
}
