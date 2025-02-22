using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System;

public class GameScript : MonoBehaviour
{
    public static GameScript Instance { get; private set; }
    private string PlayerPublicKey; // The persistent variable
    private List<string> FinalResult = new();
    public Dictionary<int, PlayerProfile> FinalResultOfPlayers  = new();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it alive across scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }

    private void OnEnable()
    {
        Web3.OnLogin += OnLogin;
    }

    private void OnDisable()
    {
        Web3.OnLogin -= OnLogin;
    }

    private void OnLogin(Account account)
    {
        PlayerPublicKey = account.PublicKey;
    }

    public string GetPlayerPublicKey()
    {
        return PlayerPublicKey;
    }

    public void SetLeaderboardContent(Dictionary<int, PlayerProfile> PlayersInfo)
    {
        FinalResultOfPlayers = PlayersInfo;
    }

    public void SetAIResults(List<string> ResultString)
    {
        FinalResult = ResultString;
    }

    public List<string> GetFinalResult()
    {
        return FinalResult;
    }


}
