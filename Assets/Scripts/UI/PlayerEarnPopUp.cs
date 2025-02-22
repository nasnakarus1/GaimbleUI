using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerEarnPopUp : MonoBehaviour
{
    public TextMeshProUGUI EarnText;

    public void SetEarnText(string EarnValue)
    {
        EarnText.SetText(EarnValue + " $tSOS");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainManuScene", LoadSceneMode.Single);
    }
}
