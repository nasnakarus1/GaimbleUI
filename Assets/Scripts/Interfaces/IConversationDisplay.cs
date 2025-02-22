using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConversationDisplay
{
    void DisplayText(string TextValue);
    void DisplayResult(List<string> ConvRes, int round);
    void DisplayLists(List<string> Agent1Conv, List<string> Agent2Conv);
    void StartingDisplay();
}
