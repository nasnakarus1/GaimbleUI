using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameManager
{
    Idle,           //0
    Conversation,   //1
    Betting,        //2
    Results,        //3
    GameOver        //4
}

public class GameSystem : MonoBehaviour
{
    public float IdleTimePeriod = 30f;
    public float ConvTimePeriod = 30f;
    public float BettingTimePeriod = 30f;
    public float Results = 30f;
    public int NumberOfRounds = 3;
    public GameManager Manager = GameManager.Idle;
    public BotNPC Bots;

    private int rounds = 0;
    private IConversationModel Model;
    private IConversationDisplay Display;
    private IButtonInteractibility BettingButtons;
    private ICalculateRoundResult CalculateRoundResult;

    private void Awake()
    {
        Model = FindObjectOfType<ConversationManager>();
        Display = FindObjectOfType<ChatUI>();
        BettingButtons = FindObjectOfType<BettingHandler>();
        CalculateRoundResult = FindObjectOfType<CalculationManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Starting the Game.
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(5.0f);
        Model.RequestForAPIResponse();
        ChangeState(GameManager.Idle);
    }

    public void ChangeState(GameManager manager)
    {
        Manager = manager;
        HandleState();
    }

    private void HandleState()
    {
        switch (Manager)
        {
            case (GameManager.Idle):
                if (rounds >= 3)
                {
                    ChangeState(GameManager.GameOver);
                    break;
                }
                GameManagerIdleFunction();
                break;

            case (GameManager.Conversation):
                GameManagerConversationFunction();
                break;

            case (GameManager.Betting):
                StartCoroutine(ChangeStateHandler(GameManager.Results, BettingTimePeriod));
                //Tell the UI Manager to Bet
                Bots.SelectBetsForRound();
                break;

            case (GameManager.Results):
                GameManagerResultsFunction();
                break;

            case (GameManager.GameOver):
                rounds = 0;
                //Change Scene to the final results round
                SceneManager.LoadScene("LeaderBoardScene", LoadSceneMode.Single);
                break;
        }
    }

    private void GameManagerIdleFunction()
    {
        StartCoroutine(ChangeStateHandler(GameManager.Conversation, IdleTimePeriod));
        //Tell UI Manager to show something on Screen
        Display.StartingDisplay();
        Display.DisplayText("ROUND " + (rounds + 1) + " Waiting for players to get ready");
        Bots.ResetLists();
    }

    private void GameManagerConversationFunction()
    {
        StartCoroutine(ChangeStateHandler(GameManager.Betting, ConvTimePeriod));
        //Tell the UI Manager to Show the Conversation on Screen
        UpdateConversation();
        BettingButtons.EnableButtonInteractibility();
    }

    private void GameManagerResultsFunction()
    {
        StartCoroutine(ChangeStateHandler(GameManager.Idle, Results));
        //Possibly start next round
        //Display and consider bets
        BettingButtons.DisableButtonInteractibility();
        Display.DisplayText("ROUND" + (rounds + 1));
        Display.DisplayResult(Model.Results, rounds);
        //Calculation
        Debug.Log("Just Before Sending the Bet amount over for calculation: " + BettingButtons.PlayerSelectedAmount);
        CalculateRoundResult.SendPlayerBets(BettingButtons.PlayerSelectedAmount, BettingButtons.PlayerSelectedBet, 0);
        CalculateRoundResult.SendBotBets(Bots.BotBetAmount, Bots.BotSelectedBet, Bots.BotNumber);
        CalculateRoundResult.SendResults(Model.Results, rounds);
        //NextRound Manipulation
        BettingButtons.SetupForNextRound();
        IdleTimePeriod = 3.0f;
        rounds += 1;
    }

    IEnumerator ChangeStateHandler(GameManager gameManager, float timePeriod)
    {
        yield return StartCoroutine(SystemCountdown(timePeriod));
        ChangeState(gameManager);
    }

    IEnumerator SystemCountdown(float TimePeriod)
    {
        float timeLeft = TimePeriod;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }

    private void UpdateConversation()
    {
        Display.DisplayLists(Model.Agent1,Model.Agent2);
    }
}
