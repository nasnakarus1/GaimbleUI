using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatUI : MonoBehaviour, IConversationDisplay
{
    public TextMeshProUGUI RoundText;
    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI TimerText;
    public GameObject ResultObject;
    public GameObject RoundObject;
    public GameObject ResultPanel;
    public GameObject StartingImage;

    public GameObject ParentObject;
    public GameObject ConversationObject;
    public float GameTime = 60.0f;

    private int pointerToIndex = 0;
    private int ResultIndex = 0;
    private float CurrentTime;
    private bool IsRunning = false;
    private List<GameObject> InstantiatedObject = new List<GameObject>();
    private readonly Dictionary<string, string> ResultToBetMapping = new()
    {
        { "Split,Split", "Both Split" },
        { "Steal,Split", "AI One Steal" },
        { "Split,Steal", "AI Two Steal" },
        { "Steal,Steal", "Both Steal" }
    };
    private ISoundAndAnimation Sound;

    private void Start()
    {
        Sound = GetComponent<SoundAndAnimation>();
    }

    public void StartingDisplay()
    {
        StartingImage.SetActive(true);
    }

    public void DisplayText(string TextValue)
    {
        ResultPanel.SetActive(false);
        ResultObject.SetActive(false);
        RoundObject.SetActive(true);
        RoundText.SetText(TextValue);
    }

    public void DisplayResult(List<string> ConvRes, int round)
    {
        Sound.SwitchOnSoundType(SoundManager.RoundEnd);
        ResultPanel.SetActive(true);
        ResultObject.SetActive(true);
        string key = ConvRes[round + ResultIndex] +","+ ConvRes[round + ++ResultIndex];
        ResultToBetMapping.TryGetValue(key, out string MappedResult);
        ResultText.SetText(MappedResult);
    }

    public void DisplayLists(List<string> Agent1Conv, List<string> Agent2Conv)
    {
        RoundObject.SetActive(false);
        StartingImage.SetActive(false);
        //Start Round Timer
        if (!IsRunning)
        {
            IsRunning = true;
            CurrentTime = GameTime;
            StartCoroutine(TimerDisplay());
        }
        //Start Dialogue
        StartCoroutine(DisplayDialogue(Agent1Conv, Agent2Conv));
    }

    IEnumerator DisplayDialogue(List<string>Conv1, List<string> Conv2)
    {
        int NumberOfDialogues = pointerToIndex + 3;
        for (int i = pointerToIndex; i < NumberOfDialogues && i < Conv1.Count; i++)
        {
            GameObject gameObject = Instantiate(ConversationObject, ParentObject.transform);
            InstantiatedObject.Add(gameObject);
            if (gameObject)
            {
                ConversionTexts conversion = gameObject.GetComponent<ConversionTexts>();
                if (conversion)
                {
                    conversion.SetConversationText1(Conv1[i]);
                    Sound.SwitchOnSoundType(SoundManager.DialogueInstantiation1);
                    yield return StartCoroutine(DisplayCooldown(5.0f));
                    conversion.SetConversationText2(Conv2[i]);
                    Sound.SwitchOnSoundType(SoundManager.DialogueInstantiation2);
                    yield return StartCoroutine(DisplayCooldown(5.0f));
                }
            }
            pointerToIndex++;
        }
    }

    IEnumerator DisplayCooldown(float TimeToWait)
    {
        float timeLeft = TimeToWait;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator TimerDisplay()
    {
        while (IsRunning && CurrentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            CurrentTime--;
            UpdateTimerDisplay();

            if (CurrentTime <= 0)
            {
                TimerComplete();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        if (IsRunning == false)
        {
            TimerText.text = "Starting Round";
        }

        int seconds = Mathf.FloorToInt(CurrentTime % 60);

        // Update UI text with formatted time
        TimerText.text = seconds.ToString();
    }

    private void TimerComplete()
    {
        for (int i = 0; i < InstantiatedObject.Count; i++)
        {
            Destroy(InstantiatedObject[i]);
        }
        InstantiatedObject.Clear();
        IsRunning = false;
        StopCoroutine(TimerDisplay());
        TimerText.text = "Starting next Round";
    }
}
