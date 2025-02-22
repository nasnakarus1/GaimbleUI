using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniLeaderboard : MonoBehaviour, IConversationDisplay
{
    public GameObject MiniLeaderboardContent;
    public GameObject ParentGameObject;

    private List<GameObject> MiniLeaderboardCtns = new();
    public void DisplayLists(List<string> Agent1Conv, List<string> Agent2Conv)
    {
        
    }

    public void DisplayResult(List<string> ConvRes, int round)
    {
        GameObject gameObject = Instantiate(MiniLeaderboardContent, ParentGameObject.transform);
        ContentInfo Info = gameObject.GetComponent<ContentInfo>();
        Info.SetLeaderboardText(ConvRes[0], ConvRes[1],"");
        if (round == 0)
        {
            Info.SetColorText();
        }
        MiniLeaderboardCtns.Add(gameObject);
    }

    public void DisplayText(string TextValue)
    {
        if (string.Equals(TextValue, "Delete_Objects"))
        {
            for (int i = 0; i < MiniLeaderboardCtns.Count; i++)
            {
                Destroy(MiniLeaderboardCtns[i]);
            }
            MiniLeaderboardCtns.Clear();
        }
    }

    public void StartingDisplay()
    {
       
    }
}
