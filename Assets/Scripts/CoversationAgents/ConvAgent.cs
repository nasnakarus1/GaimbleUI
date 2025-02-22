using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solana.Unity.SDK;
using UnityEngine;

namespace OpenAI
{
    public class ConvAgent : MonoBehaviour
    {
        // This is the General Conversation Agent and the game will have two of these
        private OpenAIApi OpenAIAgent1;
        private List<ChatMessage> MessagesAgent1 = new List<ChatMessage>();

        public LoadKeys keys;


        public async Task<string> SendMessageToAgent1(string InputMessage)
        {
            OpenAIAgent1 = new(keys.GetNextKey());
            MessagesAgent1.Clear();
            var AgentMessage = new ChatMessage()
            {
                Role = "system",
                Content = InputMessage
            };

            MessagesAgent1.Add(AgentMessage);
            int i = 0;
            while (i < 5)
            {
                var ResponseMessage = await OpenChat(OpenAIAgent1, MessagesAgent1);
                if (!string.Equals(ResponseMessage.Content, "I have no response"))
                {
                    MessagesAgent1.Add(ResponseMessage);
                    break;
                }
                OpenAIAgent1 = new(keys.GetNextKey());
                i++;
            }

            return MessagesAgent1[^1].Content;
        }

        #region Setup and call to OpenAI API

        async Task<ChatMessage> OpenChat(OpenAIApi OpenAI, List<ChatMessage> ChatMessages)
        {
            var CompletionRequest = await OpenAI.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4o-mini",
                Messages = ChatMessages
            });

            var message = CompletionRequest.Choices != null && CompletionRequest.Choices.Count > 0 ?
                CompletionRequest.Choices[0].Message : new ChatMessage() { Role = "system", Content = "I have no response" };

            message.Content = message.Content.Trim();
            return message;
        }
        #endregion
    }
}
