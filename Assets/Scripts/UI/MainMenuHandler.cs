using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject DescriptionButton;
    public GameObject BotPlayerObjects;
    public TextMeshProUGUI TimerText;
    public float LobbyTimerSeconds = 30.0f;
    public GameObject ParentObject;
    public GameObject LobbyPanel;
    public GameObject GamePanel;

    private bool IsRunning = false;
    private float CurrentTime;
    private List<GameObject> InstantiatedObjects = new List<GameObject>();

    private void Start()
    {
        GamePanel.SetActive(true);
        StartLobbyTimer();
    }

    public void TimerComplete()
    {
        ResetTimer();
        SceneManager.LoadScene("MainGameplay Scene", LoadSceneMode.Single);
    }

    public void StartLobbyTimer()
    {
        //Handle UI and Timer
        if (!IsRunning)
        {
            IsRunning = true;
            CurrentTime = LobbyTimerSeconds;
            StartCoroutine(LobbyTimer());
        }

        StartCoroutine(ShowBotPlayers());
    }

    IEnumerator LobbyTimer()
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

    private void ResetTimer()
    {
        IsRunning = false;
        UpdateTimerDisplay();
        DestroyAllGameObjects();
        LobbyPanel.SetActive(false);
        StopAllCoroutines();
    }

    private void UpdateTimerDisplay()
    {
        if (IsRunning == false)
        {
            TimerText.text = "Starting Game";
        }
        
        int seconds = Mathf.FloorToInt(CurrentTime % 60);

        // Update UI text with formatted time
        TimerText.text = seconds.ToString();
    }

    IEnumerator ShowBotPlayers()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject gameObject = Instantiate(BotPlayerObjects, ParentObject.transform);
            if (gameObject)
            {
                InstantiatedObjects.Add(gameObject);
            }

            int InstantiationTime = Random.Range(0, 3);
            yield return new WaitForSeconds(InstantiationTime);
        }
    }
    private void DestroyAllGameObjects()
    {
        for (int i = 0; i < InstantiatedObjects.Count; i++)
        {
            Destroy(InstantiatedObjects[i]);
        }
        InstantiatedObjects.Clear();
    }
}
