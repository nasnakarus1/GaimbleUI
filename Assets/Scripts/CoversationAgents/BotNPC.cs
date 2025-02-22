using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NPC
{
    public int BotCurrentBalance;
    public int BotSelectedBet;
    public int BotBetAmount;

    public NPC(int v1, int v2, int v3) : this()
    {
        this.BotCurrentBalance = v1;
        this.BotSelectedBet = v2;
        this.BotBetAmount = v3;
    }
    public void SetBalance(int Balance)
    {
        this.BotCurrentBalance = Balance;
    }
}

public class BotNPC : MonoBehaviour
{
    [HideInInspector]
    public List<int> BotBetAmount = new();
    [HideInInspector]
    public List<int> BotSelectedBet = new();
    [HideInInspector]
    public List<int> BotNumber = new();

    private Dictionary<int, NPC> BotProfile = new Dictionary<int, NPC>();

    private void Start()
    {
        InitialiseBots();
    }

    private void InitialiseBots()
    {
        for (int i = 1; i <= 6; i++)
        {
            BotProfile.Add(i, new NPC( 1000, 0, 0 ));
        }
    }

    public void SelectBetsForRound()
    {
        for (int i = 1; i <= 6; i++)
        {
            int SelectedBet = Random.Range(0, 4);
            int SelectedBetAmount = Random.Range(0, BotProfile[i].BotCurrentBalance);
            BotProfile[i].SetBalance(BotProfile[i].BotCurrentBalance - SelectedBetAmount);

            BotBetAmount.Add(SelectedBetAmount);
            BotSelectedBet.Add(SelectedBet);
            BotNumber.Add(i);
        }
    }

    public void ResetLists()
    {
        BotBetAmount.Clear();
        BotSelectedBet.Clear();
        BotNumber.Clear();
    }


}
