﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ApiWrapper.Core;
using Chat.Gui;
using VkNet.Examples.ForChat;

namespace Chat.Core
{

    //будет выполнять таски
    public class TaskExecuter
    {
        public DispatcherTimer timerExecute;
        public int updateMilliseconds = 1000 * 60 * 1;
        public DispatcherTimer timerExecute2;
        public ChatWindow ch;
        public void wire(ChatWindow ch)
        {
            this.ch = ch;
        }

        public void init()
        {
            timerExecute = new DispatcherTimer();
            timerExecute.Interval = System.TimeSpan.FromMilliseconds(400);
            timerExecute.Tick += timerExecute_Tick;
            timerExecute.Start();


            timerExecute2 = new DispatcherTimer();
            timerExecute2.Interval = System.TimeSpan.FromMilliseconds(updateMilliseconds); //раз в две минуты
            timerExecute2.Tick += timerExecute_Tick2;
            timerExecute2.Start();
        }

        private void timerExecute_Tick(object sender, EventArgs e)
        {
            List<ChatTask> ct = ch.tasks;
            lock (ct)
            {
                List<ChatTask> rt = ct.Where(o => !o.isStopped && o.sekExpared < 0).ToList();
                if (rt.Count > 0)
                {
                    //выполняем
                    executeTasks(rt);
                    //удаляем
                    deleteTasks(rt);
                    //обновляем task дшые
                    ch.updateTaskList();
                }
            }
        }

        private void timerExecute_Tick2(object sender, EventArgs e)
        {
            if (ch.playedTime == null)
            {
                return;
            }

            DateTime exp = DateTime.Now;
            TimeSpan rez = exp - ch.playedTime;
            double result = rez.TotalMilliseconds;
            if (result < updateMilliseconds / 1.5)
            {
                return;
            }

            updateAllChats();
        }

        public void updateAllChats()
        {
            lock (ch.tasks)
            {
                foreach (PersonChat pc in ch.personWindows.Values)
                {
                    if (pc.personId == 0) continue;

                    addUpdateTask(pc.personChatId, 1);
                }
                ch.updateTaskList();
            }
        }

        private void deleteTasks(List<ChatTask> rt)
        {
            foreach (ChatTask task in rt)
            {
                ch.tasks.Remove(task);
            }
        }

        private void executeTasks(List<ChatTask> rt)
        {
            //типа выполняем
            foreach (ChatTask task in rt)
            {
                if (task.type == TaskEnum.MESSAGE)
                {
				    ChatCoreHelper.WriteMessage(task.vkId, task.message);
                    //шлем непроверенное сообщение
                    PersonChat pchat = ch.getPersonChat(task.personChatId);
                    pchat.sendVirtualMessage(task);
                    addUpdateTask(task.personChatId,30);
                }
	            if (task.type == TaskEnum.UPDATE)
	            {
                    List<string[]> messages = ChatCoreHelper.GetMessagesFromUser(task.vkId);
                    PersonChat pchat = ch.getPersonChat(task.personChatId);
                    pchat.updateMessage(messages);
                }
			}
        }

	    public void addUpdateTask(string personChatId, int dsek)
	    {
	        PersonChat pchat = ch.getPersonChat(personChatId);
	        long vkId = pchat.personId;
			ChatTask t = new ChatTask();
		    t.type = Chat.Core.TaskEnum.UPDATE;
		    t.message = "UPDATE";
		    t.vkId = vkId;
	        t.timeExpared = setTime(dsek);
		    t.personChatId = personChatId;
		    t.isStopped = false;
		    t.personName = ch.CurrentUser.Value;
		    ch.tasks.Add(t);
			ch.updateTaskList();
		}


        public DateTime setTime(int dsek)
        {
            List<ChatTask> ct = ch.tasks;
            DateTime result = DateTime.Now;
            lock (ct)
            {
                DateTime localDate = DateTime.Now;
                DateTime de = localDate.AddSeconds(dsek);
                result = getRecursiveTime(de);
            }
            return result;
        }

        private DateTime getRecursiveTime(DateTime de)
        {
            List<ChatTask> ct = ch.tasks;
            if (ct.Count == 0)
            {
                return de;
            }
            bool can = true;
            foreach (ChatTask ch in ct)
            {
                DateTime exp = ch.timeExpared;
                TimeSpan rez = de - exp;
                double result = rez.TotalSeconds;
                if (Math.Abs(result) < 1)
                {
                    can = false;
                    return getRecursiveTime(de.AddSeconds(1));
                }
            }
            
            return de;
        }
    }

}
