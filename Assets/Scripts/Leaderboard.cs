using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] public GameObject rowPrefab;
    [SerializeField] public Transform rowsParent;
    private LootLockerLeaderboardMember[] leaderboardMembers;
    private 

    // Start is called before the first frame update
    void Start()
    {
        GetLeaderboardMembers(Constants.LeaderboardCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLeaderboardGet()
    {
        
        foreach (var item in leaderboardMembers)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = item.metadata != "" ? item.rank.ToString() + ". " + item.metadata : item.rank.ToString() + ". " + "Guest-" + item.player.id.ToString();
            //texts[0].text = item.rank.ToString() + ". " + "Guest-" + item.player.id.ToString();
            //Debug.Log("PLAYER: " + item.player.name);
            texts[1].text = item.score.ToString();

            Debug.Log(item.player.name);
        }
    }

    public void GetLeaderboardMembers(int numMembers)
    {
        LootLockerSDKManager.GetScoreList(Constants.LeaderboardKey, numMembers, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
                leaderboardMembers = response.items;
                OnLeaderboardGet();
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }
}
