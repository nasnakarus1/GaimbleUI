using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConversionTexts : MonoBehaviour
{
    public TextMeshProUGUI Conversation1;
    public GameObject Conversation1Object;
    public TextMeshProUGUI Conversation2;
    public GameObject Conversation2Object;
    public float typingSpeed = 0.05f;

    public void SetConversationText1(string conversation)
    {
        Conversation1Object.SetActive(true);
        // Typewriter Effect
        StartCoroutine(TypeText(conversation, Conversation1));
        //Conversation1.SetText(conversation);
    }

    public void SetConversationText2(string conversation)
    {
        Conversation2Object.SetActive(true);
        //TypeWriter effect
        StartCoroutine(TypeText(conversation, Conversation2));
        //Conversation2.SetText(conversation);
    }

    private IEnumerator TypeText(string dialogue, TextMeshProUGUI ConversationText)
    {
        ConversationText.SetText("");
        foreach (char letter in dialogue)
        {
            ConversationText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
