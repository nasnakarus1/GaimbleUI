using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContentInfo : MonoBehaviour
{
    public TextMeshProUGUI RankingText;
    public TextMeshProUGUI AddressText;
    public TextMeshProUGUI TotalLSText;
    public TextMeshProUGUI TotalEarnText;

    public void SetLeaderboardText(string Address, string LeaderboardScore, string WinText)
    {
        Address = TrimAddress(Address);
        AddressText.SetText(Address);
        TotalLSText.SetText(LeaderboardScore);
        TotalEarnText?.SetText(WinText);
    }

    public void SetColorText()
    {
        RankingText.color = Color.yellow;
        AddressText.color = Color.yellow;
        TotalLSText.color = Color.yellow;
        if (TotalEarnText != null)
        {
            TotalEarnText.color = Color.yellow;
        }
    }

    string TrimAddress(string address)
    {
        if (address.Length <= 6)
            return address; // Return as is if too short

        return address.Substring(0, 3) + "..." + address.Substring(address.Length - 3);
    }
}
