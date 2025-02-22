using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile
{
    public float TotalLS;
    public float TotalEarn;
    public string PlayerAddress;
}

public class RoundPlayerProfile
{
    public int PlayerNumber;
    public int BettingAmount;
    public int BettingValue;
}

public enum Bets
{
    BothSplit,           //0
    AIOneSteal,         //1
    AITwoSteal,        //2
    BothSteal,        //3
    NoBet,           //4
}

public class CalculationManager : MonoBehaviour, ICalculateRoundResult
{
    [HideInInspector]
    public Dictionary<int, PlayerProfile> Players = new Dictionary<int, PlayerProfile>();

    private readonly Dictionary<string, Bets> ResultToBetMapping = new()
    {
        { "Split,Split", Bets.BothSplit },
        { "Steal,Split", Bets.AIOneSteal },
        { "Split,Steal", Bets.AITwoSteal },
        { "Steal,Steal", Bets.BothSteal }
    };

    private List<string> RoundResult = new();

    private List<RoundPlayerProfile> RoundPlayerProfiles = new();

    private int TotalAmountOfBets = 0;

    private bool IsRoundThree = false;

    private IConversationDisplay MiniLeaderboardDisplay;

    private void Start()
    {
        MiniLeaderboardDisplay = FindObjectOfType<MiniLeaderboard>();
    }

    public void SendBotBets(List<int> NPCBetAmount, List<int> NPCBetValue, List<int> NPCNumber)
    {
        for (int i = 0; i < NPCBetAmount.Count; i++)
        {
            TotalAmountOfBets += NPCBetAmount[i];
            RoundPlayerProfiles.Add(new RoundPlayerProfile
            {
                PlayerNumber = NPCNumber[i],
                BettingAmount = NPCBetAmount[i],
                BettingValue = NPCBetValue[i]
            });
        }
        // Clean previous times Results panel to show Current Result Panle
        MiniLeaderboardDisplay.DisplayText("Delete_Objects");
    }

    public void SendPlayerBets(int BetAmount, int BetValue, int PlayerNumber)
    {
        TotalAmountOfBets += BetAmount;
        RoundPlayerProfiles.Add(new RoundPlayerProfile
        {
            PlayerNumber = PlayerNumber,
            BettingAmount = BetAmount,
            BettingValue = BetValue
        });
    }

    public void SendResults(List<string> Results, int round)
    {
        if (round == 0)
        {
            RoundResult.Clear();
            RoundResult.Add(Results[0]);
            RoundResult.Add(Results[1]);
        }
        else if (round == 1)
        {
            RoundResult.Clear();
            RoundResult.Add(Results[2]);
            RoundResult.Add(Results[3]);
        }
        else
        {
            RoundResult.Clear();
            RoundResult.Add(Results[4]);
            RoundResult.Add(Results[5]);

            //As round 3 last Round
            GameScript.Instance.SetAIResults(Results);
            IsRoundThree = true;
        }

        CalculateLSPerRound();
    }

    private Bets GetBetResult()
    {
        string key = $"{RoundResult[0]},{RoundResult[1]}";
        return ResultToBetMapping.TryGetValue(key, out Bets bet) ? bet : Bets.NoBet;

    }

    private void CalculateLSPerRound()
    {
        int TotalWinningBets = 0;
        //Getting all the Winning Bets
        for (int i = 0; i < RoundPlayerProfiles.Count; i++)
        {
            if (GetBetResult() == (Bets)RoundPlayerProfiles[i].BettingValue)
            {
                TotalWinningBets += RoundPlayerProfiles[i].BettingAmount;
            }
        }
        //To avoid Divide by zero exception
        TotalWinningBets = TotalWinningBets == 0 ? 1 : TotalWinningBets;
        //Calculating Total LS
        for (int j = 0; j < RoundPlayerProfiles.Count; j++)
        {
            if (!Players.ContainsKey(RoundPlayerProfiles[j].PlayerNumber))
            {
                Players.Add(RoundPlayerProfiles[j].PlayerNumber, new PlayerProfile
                {
                    TotalLS = 0,
                    TotalEarn = 0,
                    PlayerAddress = ""
                });
            }
            if (GetBetResult() == (Bets)RoundPlayerProfiles[j].BettingValue)
            {
                float RoundLS = TotalAmountOfBets * RoundPlayerProfiles[j].BettingAmount / TotalWinningBets;
                Players[RoundPlayerProfiles[j].PlayerNumber].TotalLS += RoundLS;
            }

            // Result Screen
            if (RoundPlayerProfiles[j].PlayerNumber == 0)
            {
                MiniLeaderboardDisplay.DisplayResult(new List<string>
                {
                    GameScript.Instance.GetPlayerPublicKey(),
                    Players[RoundPlayerProfiles[j].PlayerNumber].TotalLS.ToString()
                }, 0);
                continue;
            }

            MiniLeaderboardDisplay.DisplayResult(new List<string>
            {
                GameScript.Instance.GetPlayerPublicKey() + Random.Range(0, 100),
                Players[RoundPlayerProfiles[j].PlayerNumber].TotalLS.ToString()
            }, 1);
        }

        // Clear RoundPlayersProfile
        RoundPlayerProfiles.Clear();
        TotalAmountOfBets = 0;
        //Send the data to the
        if (IsRoundThree)
        {
            GameScript.Instance.SetLeaderboardContent(Players);
        }
    }

    private void OnDisable()
    {
        RoundPlayerProfiles.Clear();
        RoundResult.Clear();
    }
}
