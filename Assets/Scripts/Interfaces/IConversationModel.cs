using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConversationModel
{
    List<string> Agent1 { get; }
    List<string> Agent2 { get; }
    List<string> Results { get; }

    public void RequestForAPIResponse();
}
