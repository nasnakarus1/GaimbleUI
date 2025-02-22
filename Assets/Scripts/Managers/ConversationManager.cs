using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public List<string> Conversation;
    public List<string> Choices;
}

[System.Serializable]
public class GameData
{
    public List <RoundData> Rounds;
}

public class ConversationManager : MonoBehaviour, IConversationModel
{
    [Header("Inputs")]
    public string PersonalityAgent1;
    public OpenAI.ConvAgent ConvAgent;

    private List<string> Agent1Conversations = new List<string>();
    private List<string> Agent2Conversations = new List<string>();
    private List<string> Choices = new List<string>();

    public List<string> Agent1 => Agent1Conversations;
    public List<string> Agent2 => Agent2Conversations;
    public List<string> Results => Choices;


    public void RequestForAPIResponse()
    {
        MessageToAgent1(PersonalityAgent1);
    }

    async void MessageToAgent1(string extraInput)
    {
        try
        {
            var response = await ConvAgent.SendMessageToAgent1(extraInput);
            //var response = GetResponse(); //For Debugging purpose
            if (response != null)
            {
                Debug.Log(response);
                ParseGameData(response);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("This is an error while sending message to Agent1: " + e.Message);
        }
    }

    void ParseGameData(string json)
    {
        string result = RemoveCharacters(json);

        GameData gameData = JsonConvert.DeserializeObject<GameData>(result);

        foreach (var round in gameData.Rounds)
        {
            for (int i = 0; i < round.Conversation.Count; i++)
            {
                if (i % 2 == 0) // Even index -> Agent 1
                    Agent1Conversations.Add(round.Conversation[i]);
                else            // Odd index -> Agent 2
                    Agent2Conversations.Add(round.Conversation[i]);
            }

            Choices.Add(round.Choices[0]); // Agent 1's choice
            Choices.Add(round.Choices[1]); // Agent 2's choice
        }
    }

    string RemoveCharacters(string input)
    {
        if (input.Length <= 10) // Ensure we have enough characters to remove
            return "";

        return input.Substring(7, input.Length - 10); // Remove first 7 and last 3
    }

    private string GetResponse()
    {
        string result = "```json{\"Rounds\":[{\"Conversation\":[\"Agent 1: I think we should both split the money. It's a fair deal.\",\"Agent 2: Splitting sounds good, but how do I know you won't steal?\",\"Agent 1: Trust me, I value fairness. If we both split, we both win.\",\"Agent 2: I'm logical, but I have to think about my own gain. What if you change your mind?\",\"Agent 1: If we both agree to split, there's no need for deception. Let’s just be honest.\",\"Agent 2: I want to trust you, but it's hard when there's risk involved.\"],\"Choices\":[\"Split\",\"Split\"]},{\"Conversation\":[\"Agent 1: I'm feeling confident about our decision to split last time.\",\"Agent 2: That decision was good, but I’m still wary. Are you really sticking with split?\",\"Agent 1: Absolutely. I believe that's the best choice for both of us.\",\"Agent 2: If you say that, I will take your word for it. But I won't be fooled again.\",\"Agent 1: I understand your caution. Let’s keep it simple: split is the way to go.\",\"Agent 2: All right, let’s keep the trust. That means I’ll go with split too.\"],\"Choices\":[\"Split\",\"Split\"]},{\"Conversation\":[\"Agent 1: I hope we can continue this approach for the final round.\",\"Agent 2: I want to, but I can’t ignore the possibility of you going for steal this time.\",\"Agent 1: Why would I break our agreement? We've done so well together.\",\"Agent 2: Because sometimes greed can take over, and I won't risk it.\",\"Agent 1: You know I’m logical as well. Stealing jeopardizes both our gains.\",\"Agent 2: True, but it's hard to shake off the fear of deception, especially in a game like this.\"],\"Choices\":[\"Split\",\"Steal\"]}]}```";
        return result;
    }
}
