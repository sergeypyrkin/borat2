﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ApiWrapper.Core;
using Chat.Core;

namespace Chat.Gui
{
    /// <summary>
    /// Логика взаимодействия для PersonChat.xaml
    /// </summary>
    public partial class PersonChat : Canvas
    {

        //настройки PersonChat
        public int person_height = 232;
        public int person_width = 152;
        public int person_width_margin = 5;
        public int person_height_margin = 5;

        //настройка чата диалого
        public int ch_margin_top = 40;
        public int ch_width = 150;
        public int ch_height = 170;

        //кнопки
        public int b_margin_top = 212;
        public int b_width = 50;
        public int b_height = 19;
        public int b_margin = 1;

        public int top;
        public int left;
        public bool isMin = true;
        public ChatWindow ch;
        public long _v;
        public string _s;
        public long personId

        {
            get
            {
                return _v;

            }
            set
            {
                _v = value;

            }
        }    //vkId

        public string personChatId
        {
            get { return _s; }
            set
            {
                _s = value;
            }
        }

        public List<ChatMessage> chatMessages = new List<ChatMessage>();

        public PersonChat()
        {
            InitializeComponent();
            profileImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/notFound2.png"));
            profileChatNumber.Content = personChatId;


        }

        public void wire(ChatWindow ch)
        {
            this.ch = ch;
            //test();
        }


        public void normalize()
        {
            profileCicates.Visibility = Visibility.Hidden;
            profileFollowers.Visibility = Visibility.Hidden;
            profileInterests.Visibility = Visibility.Hidden;



            Canvas.SetTop(this, top);
            Canvas.SetLeft(this, left);
            this.Height = person_height;
            this.Width = person_width;
            Canvas.SetZIndex(this, 1);
            Background = Brushes.DarkGray;


            //max
            Canvas.SetTop(maxLabel, -5);
            Canvas.SetLeft(maxLabel, 125);
            maxLabel.FontSize = 12;

            //image
            profileImage.Width = 40;
            profileImage.Height = 40;

            //name
            Canvas.SetTop(profileName, 0);
            Canvas.SetLeft(profileName, 40);
            profileName.FontSize = 8;


            //age
            Canvas.SetTop(profileAge, 14);
            Canvas.SetLeft(profileAge, 40);
            profileAge.FontSize = 8;

            //profileLastTimeAnswer
            Canvas.SetTop(profileChatNumber, 28);
            Canvas.SetLeft(profileChatNumber, 40);
            profileChatNumber.FontSize = 8;

            //datagrid
            Canvas.SetTop(datagrid, ch_margin_top);
            datagrid.Width = ch_width;
            datagrid.Height = ch_height;

            //bmessage
            Canvas.SetTop(bmessage, b_margin_top);
            Canvas.SetLeft(bmessage, 0);
            bmessage.Width = b_width;
            bmessage.Height = b_height;

            //bwrite
            Canvas.SetTop(bwrite, b_margin_top);
            Canvas.SetLeft(bwrite, b_width + b_margin);
            bwrite.Width = b_width;
            bwrite.Height = b_height;

            //bclose
            Canvas.SetTop(bclose, b_margin_top);
            Canvas.SetLeft(bclose, 2 * (b_width + b_margin));
            bclose.Width = b_width;
            bclose.Height = b_height;



        }

        public void maximaze()
        {
            profileCicates.Visibility = Visibility.Visible;
            profileFollowers.Visibility = Visibility.Visible;
            profileInterests.Visibility = Visibility.Visible;


            Canvas.SetTop(this, 0);
            Canvas.SetLeft(this, 0);
            this.Height = ch.Height - 40;
            this.Width = ch.Width - 15;
            Canvas.SetZIndex(this, 10);
            Background = Brushes.Gray;


            //max
            Canvas.SetTop(maxLabel, -5);
            Canvas.SetLeft(maxLabel, this.Width - 55);
            maxLabel.FontSize = 20;

            //image
            profileImage.Width = 200;
            profileImage.Height = 200;

            //name
            Canvas.SetTop(profileName, 0);
            Canvas.SetLeft(profileName, 240);
            profileName.FontSize = 20;

            //age
            Canvas.SetTop(profileAge, 25);
            Canvas.SetLeft(profileAge, 240);
            profileAge.FontSize = 20;

            //profileLastTimeAnswer
            Canvas.SetTop(profileChatNumber, 50);
            Canvas.SetLeft(profileChatNumber, 240);
            profileChatNumber.FontSize = 20;

            //followers
            Canvas.SetTop(profileFollowers, 75);
            Canvas.SetLeft(profileFollowers, 240);
            profileFollowers.FontSize = 20;

            //citates
            Canvas.SetTop(profileCicates, 100);
            Canvas.SetLeft(profileCicates, 240);
            profileCicates.FontSize = 20;

            //interest
            Canvas.SetTop(profileInterests, 120);
            Canvas.SetLeft(profileInterests, 240);
            profileInterests.FontSize = 20;


            //datagrid
            Canvas.SetTop(datagrid, 200);
            datagrid.Width = Width - 270;
            datagrid.Height = 450;

            //bmessage
            Canvas.SetTop(bmessage, Height - 55);
            Canvas.SetLeft(bmessage, 10);
            bmessage.Width = 100;
            bmessage.Height = 50;

            //bmessage
            Canvas.SetTop(bwrite, Height - 55);
            Canvas.SetLeft(bwrite, 200);
            bwrite.Width = 100;
            bwrite.Height = 50;

            //bclose
            Canvas.SetTop(bclose, Height - 55);
            Canvas.SetLeft(bclose, 380);
            bclose.Width = 100;
            bclose.Height = 50;

        }

        private void onChangeState(object sender, RoutedEventArgs e)
        {
            isMin = !isMin;
            changeState();
        }

        private void changeState()
        {
            if (isMin)
            {
                normalize();
            }
            else
            {
                maximaze();
            }
        }



        public void writeMsg(Object sender,
                       EventArgs e)
        {
            SimpleMessageBox box = new SimpleMessageBox();
            var res = box.ShowDialog();
            if (res == true)
            {
                ChatTask t = new ChatTask();
                t.type = Chat.Core.TaskEnum.MESSAGE;
                t.message = box.msg;
                t.vkId = personId;
                t.timeExpared = ch.te.setTime(5);
                t.personChatId = personChatId;
                t.isStopped = false;
                t.personName = ch.CurrentUser.Value;
                ch.tasks.Add(t);
                ch.updateTaskList();
            }
        }


        public void sendVirtualMessage(ChatTask task)
        {
            ChatMessage message = new ChatMessage();
            message.isVirtual = true;
            message.message = task.message;
            message.isBot = true;
            message.personChatId = personChatId;
            message.time = DateTime.Now;
            message.vkId = ch.CurrentUser.Key;
            message.personName = task.personName;
            chatMessages.Add(message);
            UpdateUi();

        }

        public void test()
        {
            for (int i = 0; i < 10; i++)
            {
                ChatMessage newmessage = new ChatMessage();
                newmessage.isVirtual = false;
                newmessage.message = "Привет, давай знакомиться и я тоже люблю макароны";
                newmessage.isBot = false;
                newmessage.personChatId = personChatId;
                newmessage.time = DateTime.Now;
                newmessage.vkId = 111;
                newmessage.personName = "Сергей";
                chatMessages.Add(newmessage);
            }
            UpdateUi();
        }

        private void UpdateUi()
        {
            datagrid.ItemsSource = chatMessages.OrderBy(o => o.time);
            datagrid.Items.Refresh();
            datagrid.ScrollIntoView(chatMessages.Last());
        }

        public void updateMessage(List<string[]> messages)
        {
            ChatMessage firstMessage = chatMessages.First();
            int i = 0;
            List<String[]> receved = new List<string[]>();
            foreach (String[] mess in messages)
            {
                string message = mess[0];
                string recever = mess[1];
                string sdate = mess[2];
                if (recever == firstMessage.vkId.ToString() && message == firstMessage.message)
                {
                    receved.Add(mess);
                    break;
                }
                receved.Add(mess);
                i++;
            }
            chatMessages.Clear();

            foreach (String[] rec in receved)
            {

                string message = rec[0];
                string recever = rec[1];
                string sdate = rec[2];
                string pName = recever == ch.CurrentUser.Key.ToString() ? ch.CurrentUser.Value : this.profileName.Content.ToString();
                ChatMessage newmessage = new ChatMessage();
                newmessage.isVirtual = false;
                newmessage.message = message;
                newmessage.isBot = recever == ch.CurrentUser.Key.ToString();
                newmessage.personChatId = personChatId;
                newmessage.time = DateTime.Parse(sdate);
                newmessage.vkId = Convert.ToInt64(recever);
                newmessage.personName = pName;
                chatMessages.Add(newmessage);
            }
            chatMessages = chatMessages.OrderBy(o => o.time).ToList();
            UpdateUi();

        }

        public PersonModel Person
        {
            get
            {
                PersonModel person = null;
                person = ch.Persons.FirstOrDefault(o => o.id == personId);
                return person;
            }
        }

        private void update(object sender, RoutedEventArgs e)
        {
            ch.te.addUpdateTask(personChatId, 1);
        }

        private void openVk(object sender, MouseButtonEventArgs e)
        {
            if (Person == null)
            {
                return;
            }
            string profilePage = "https://vk.com/" + Person.Domain;
            System.Diagnostics.Process.Start(profilePage);
        }
        private void vkOpenTooltip(object sender, ToolTipEventArgs e)
        {
            if (Person == null)
            {
                e.Handled = true;
                return;
            }
            String result = "";
            result = result + "П: " + Person.followers + "\n" + "Ц: " + Person.Status + "\n" + "И: " + Person.interests;
            (sender as Image).ToolTip = result;
        }
    }
}
