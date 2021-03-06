﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chat.Gui;
using KamikyForms.Core;
using VkNet.Examples.ForChat;

namespace KamikyForms.Bot
{
    public class Bot
    {

        Dictionary<string, List<string>> answers = new Dictionary<string, List<string>>();
        List<string> lines = new List<string>();
        public bool loaded = false;
        public ChatWindow cw;

        public void wire(ChatWindow cw)
        {
            this.cw = cw;
            LoadDataTask();
        }

        public void LoadDataTask()
        {
            Task.Factory.StartNew(() =>
            {
                loadAction();
                loaded = true;
                cw.addConsoleMsg("Bot loaded: "+ answers.Keys.Count);

            });
        }


        //проверяем что нету в словаре
        public bool exist(string mess1, string mess2)
        {
            if (!answers.ContainsKey(mess1))
            {
                return false;
            }
            List<string> messages = answers[mess1];
            if (!messages.Contains(mess2))
            {
                return false;
            }
            return true;
        }


        public void loadAction()
        {
            lines = FileParser.getAnswer();
            List<string> lines2 = FileParser.getMyAnswer();
            foreach (string s in lines2)
            {
                lines.Add(s);
            }
            foreach (string s in lines)
            {
                string[] words = s.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length != 2) continue;
                string w1 = BotHelper.prepareString(words[0]);
               
                string w2 = words[1];
                addM1M2(w1, w2);
            }
        }

        public void addM1M2(string w1, string w2)
        {
            if (answers.ContainsKey(w1))
            {
                answers[w1].Add(w2);
            }
            else
            {
                answers.Add(w1, new List<string>() { w2 });
            }
        }


        public List<String> getMessages(String message)
        {
            string prepared = BotHelper.prepareString(message);
            if (answers.ContainsKey(prepared))
            {
                List<string> all = answers[prepared];
                List<string> result = new List<string>();
                //будем возвращать только максимум 30;
                int MAX_COUNT = 30;
                if (all.Count < MAX_COUNT)
                {
                    return all;
                }
                for (int i = 0; i < MAX_COUNT; i++)
                {
                    result.Add(all[i]);
                }
                return result;
            }
            else
            {
                return new List<string>();
            }
        }


    }
}
