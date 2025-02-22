using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BettingHandler : MonoBehaviour, IButtonInteractibility
{
    public int InitialChips = 1000;
    public TextMeshProUGUI ChipsText;
    public Slider BetSlider;
    public TextMeshProUGUI InputAmount;
    public TextMeshProUGUI CautionText;
    public List<Button> buttons = new List<Button>();

    [HideInInspector]
    public int maxValidValue;

    private int CurrentChips;
    private int BetAmount;
    private int SelectedBet = 4;

    public int PlayerSelectedBet => SelectedBet;
    public int PlayerSelectedAmount => BetAmount;

    // Start is called before the first frame update
    void Start()
    {
        CurrentChips = InitialChips;
        maxValidValue = InitialChips;
        SetChipsText(CurrentChips);

        if (BetSlider != null)
        {
            BetSlider.minValue = 0;
            BetSlider.maxValue = maxValidValue;
            InputAmount.text = BetSlider.minValue.ToString();
            BetSlider.onValueChanged.AddListener(SliderValueOnChange);
        }
    }

    public void SliderValueOnChange(float Value)
    {
        InputAmount.text = Value.ToString();
    }

    public void MinimumBetButton(int Value)
    {
        if (Value < CurrentChips)
        {
            BetSlider.value = Value;
            InputAmount.text = Value.ToString();
        }
        else
        {
            BetSlider.value = CurrentChips;
            InputAmount.text = CurrentChips.ToString();
        }
    }

    public void MaximumBetButton()
    {
        BetSlider.value = maxValidValue;
        InputAmount.text = maxValidValue.ToString();
    }

    public void BetButtons(int Value)
    {
        SelectedBet = Value;
        if (Value == 4)
        {
            BetSlider.value = 0;
        }
        BetAmount = (int)BetSlider.value;
        SetChipsText(CurrentChips - BetAmount);
    }

    public void FinalBetButton()
    {
        CalculateBalanceAmount();
    }

    private void CalculateBalanceAmount()
    {
        CurrentChips -= BetAmount;
        SetChipsText(CurrentChips);
    }

    private void SetChipsText(int Value)
    {
        ChipsText.SetText("Chips: " +  Value.ToString());
    }

    public void EnableButtonInteractibility()
    {
        CautionText.SetText("Please place your bets before the time runs out");
        BetSlider.interactable = true;
        if (buttons.Count > 0)
        {
            foreach (Button btn in buttons)
            {
                btn.interactable = true; // Disable button interaction
            }
        }
    }

    public void DisableButtonInteractibility()
    {
        CautionText.SetText("Time for bets placement is over");
        BetSlider.interactable = false;
        if (buttons.Count > 0)
        {
            foreach (Button btn in buttons)
            {
                btn.interactable = false; // Enable button interaction
            }
        }
        // Bets taken into consideration
        FinalBetButton();
    }

    public void SetupForNextRound()
    {
        //Setup Slider for Next Round
        BetSlider.interactable = true;
        maxValidValue = CurrentChips;
        BetSlider.minValue = 0;
        BetSlider.maxValue = maxValidValue;
        BetAmount = 0;
        BetSlider.value = 0;
        InputAmount.text = BetSlider.value.ToString();
    }
}
