using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardSceneManager : MonoBehaviour
{
    public List<TextMeshProUGUI> ChoiceText;
    public GameObject LeaderboardContentInfo;
    public GameObject ParentGameObject;
    public GameObject PanelPopup;
    public float TimePeriod;

    private PlayerEarnPopUp PlayerEarnPop;
    private float EarnAmount;
    // Start is called before the first frame update
    void Start()
    {
        PlayerEarnPop = GetComponent<PlayerEarnPopUp>();
        //AI Result
        SetAIResultText();
        //Leaderboard 
        LeaderboardContent();
        //Popup
        StartCoroutine(PanelShowDelay());
    }

    private void SetAIResultText()
    {
        List<string> Results = GameScript.Instance.GetFinalResult();
        for (int i = 0; i < Results.Count; i++)
        {
            ChoiceText[i].SetText(Results[i]);
        }
    }

    private void LeaderboardContent()
    {
        Dictionary<int, PlayerProfile> ValuePairs = GameScript.Instance.FinalResultOfPlayers;
        float CumulativeLS = 0.0f;
        float TotalWagers = 100.0f;
        for (int i = 0; i < ValuePairs.Count; i++)
        {
            CumulativeLS += ValuePairs[i].TotalLS;
        }
        //To avoid divide by 0
        CumulativeLS = CumulativeLS == 0.0f ? 1.0f : CumulativeLS;

        for (int i = 0; i < ValuePairs.Count; i++)
        {
            float Win = TotalWagers * ValuePairs[i].TotalLS / CumulativeLS;
            ValuePairs[i].TotalEarn = Win;
            GameObject gameObject = Instantiate(LeaderboardContentInfo, ParentGameObject.transform);
            ContentInfo Info = gameObject.GetComponent<ContentInfo>();
            string Address = GameScript.Instance.GetPlayerPublicKey() + Random.Range(0, 100);
            if (i == 0)
            {
                Info.SetColorText();
                EarnAmount = ValuePairs[i].TotalEarn;
                Address = GameScript.Instance.GetPlayerPublicKey();
            }
            Info.SetLeaderboardText(Address, ValuePairs[i].TotalLS.ToString(), ValuePairs[i].TotalEarn.ToString());
        }
    }

    IEnumerator PanelShowDelay()
    {
        yield return StartCoroutine(DelayFunction());
        PanelPopup.SetActive(true);
        PlayerEarnPop.SetEarnText(EarnAmount.ToString());
    }

    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(TimePeriod);
    }
}
