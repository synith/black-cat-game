using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

namespace Synith
{
    public class TwitchConnect : MonoBehaviour
    {

        [SerializeField] List<string> prompts;
        [SerializeField] List<string> responses;

        public static event Action<string> OnAnyChatMessage;

        TcpClient twitch;
        StreamReader reader;
        StreamWriter writer;

        const string URL = "irc.chat.twitch.tv";
        const int PORT = 6667;

        string user = "bottest54321";
        string oAuth = File.ReadAllText(@"D:\Resources\token.txt");
        string channel = "manasen";

        void ConnectToTwitch()
        {
            twitch = new TcpClient(URL, PORT);
            reader = new StreamReader(twitch.GetStream());
            writer = new StreamWriter(twitch.GetStream());

            writer.WriteLine("PASS " + oAuth);
            writer.WriteLine("NICK " + user.ToLower());
            writer.WriteLine("JOIN #" + channel.ToLower());
            writer.Flush();

            Debug.Log("Connecting....");
        }

        void Awake()
        {
            ConnectToTwitch();
        }

        void Update()
        {
            while (twitch.Available > 0)
            {
                string message = reader.ReadLine();
                Debug.Log(message);

                OnAnyChatMessage?.Invoke(message);


                for (int i = 0; i < prompts.Count; i++)
                {
                    string prompt = prompts[i].ToLower();
                    string response = responses[i];
                    RespondToChat(message, prompt, response);
                }
            }
        }

        void RespondToChat(string message, string prompt, string response)
        {
            if (message.ToLower().Contains(prompt.ToLower()))
            {
                writer.WriteLineAsync("PRIVMSG #" + channel + " :" + response);
                writer.Flush();
            }
        }

        void OnDestroy()
        {
            twitch.Close();            
        }
    }
}
