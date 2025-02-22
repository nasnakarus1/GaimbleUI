using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartGameScene : MonoBehaviour
{
    public GameObject StartGameButton;
    public List<GameObject> buttons;

    private ISoundAndAnimation Sound;
    // Start is called before the first frame update
    void Start()
    {
        Sound = FindObjectOfType<SoundAndAnimation>();
    }

    private void OnEnable()
    {
        Web3.OnLogin += OnLoggedIn;
    }

    private void OnDisable()
    {
        Web3.OnLogin -= OnLoggedIn;
    }

    private void OnLoggedIn(Account account)
    {
        foreach(GameObject btn in buttons)
        {
            btn.SetActive(false);
        }
        StartGameButton.SetActive(true);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MainManuScene", LoadSceneMode.Single);
    }
}
