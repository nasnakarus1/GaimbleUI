using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButtonInteractibility
{
    public int PlayerSelectedBet { get; }
    public int PlayerSelectedAmount { get; }
    public void EnableButtonInteractibility();
    public void DisableButtonInteractibility();
    public void SetupForNextRound();
}
