using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICalculateRoundResult
{
    public void SendPlayerBets(int BetAmount, int BetValue, int PlayerNumber);
    public void SendBotBets(List<int> NPCBetAmount, List<int> NPCBetValue, List<int> NPCNumber);
    public void SendResults(List<string> Results, int round);
}
