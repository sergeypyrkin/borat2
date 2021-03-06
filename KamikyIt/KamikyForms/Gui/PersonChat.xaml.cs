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
using KamikyForms.Bot;
using KamikyForms.Gui;
using VkNet.Examples.ForChat;

namespace Chat.Gui
{
    /// <summary>
    /// Логика взаимодействия для PersonChat.xaml
    /// </summary>
    public partial class PersonChat : Canvas
    {

        private ContextMenu contextMenuDataGrid = new ContextMenu();
        public SmartBot sm;
        public bool _goodTalk;      //с ней мы начали общение
        public string startUpFraze; //фраза с которой начали общение

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
        public bool isActive = true;
        public ChatWindow ch;
        public long _v;
        public string _s;
        public ChatMessage ovInfo;
        public long personId



        {
            get { return _v; }
            set { _v = value; }
        } //vkId

        public bool banned = false;
        public string bannedString = "";
        public bool selected = false;




        public string personChatId
        {
            get { return _s; }
            set { _s = value; }
        }

        public ChatMessage lastNotAnsweredMessage
        {
            get
            {
                if (chatMessages == null)
                {
                    return null;
                }

                if (isActive == false)
                {
                    return null;
                }

                if (chatMessages.Count == 0)
                {
                    return null;
                }
                ChatMessage lastMessage = chatMessages.Last();
                if (lastMessage.isBot == true)
                {
                    return null;
                }
                //теперь проверим что ответ может быть (в полете)
                if (ch.tasks.Exists(o => o.personChatId == personChatId && o.type == TaskEnum.MESSAGE))
                {
                    return null;
                }
                return lastMessage;
            }
        }

        public List<ChatMessage> chatMessages = new List<ChatMessage>();

        public PersonChat()
        {
            InitializeComponent();
            profileImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/notFound2.png"));
            profileChatNumber.Content = personChatId;
            contextMenuDataGrid.Opened += contextMenuDataGrid_Opened;
            datagrid.ContextMenu = contextMenuDataGrid;
            sm = new SmartBot(this);



        }

        private void contextMenuDataGrid_Opened(object sender, RoutedEventArgs e)
        {
        }

        public void wire(ChatWindow ch)
        {
            this.ch = ch;
            if (ch.debug)
            {
                test();
            }
        }


        public void normalize()
        {
            em1.Visibility = Visibility.Hidden;
            em2.Visibility = Visibility.Hidden;

            profileCicates.Visibility = Visibility.Hidden;
            profileFollowers.Visibility = Visibility.Hidden;
            profileInterests.Visibility = Visibility.Hidden;
            textblock.Visibility = Visibility.Hidden;
            adviseStack.Visibility = Visibility.Hidden;



            Canvas.SetTop(this, top);
            Canvas.SetLeft(this, left);
            this.Height = person_height;
            this.Width = person_width;
            Canvas.SetZIndex(this, 1);


            //max
            Canvas.SetTop(maxLabel, 0);
            Canvas.SetLeft(maxLabel, 135);
            maxLabel.FontSize = 12;


            //botNext
            Canvas.SetTop(botNext, 25);
            Canvas.SetLeft(botNext, 135);
            botNext.FontSize = 12;

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
            UpdateUi();
        }

        public void maximaze()
        {
            em1.Visibility = Visibility.Visible;
            em2.Visibility = Visibility.Visible;

            profileCicates.Visibility = Visibility.Visible;
            profileFollowers.Visibility = Visibility.Visible;
            profileInterests.Visibility = Visibility.Visible;
            textblock.Visibility = Visibility.Visible;
            adviseStack.Visibility = Visibility.Visible;



            Canvas.SetTop(this, 0);
            Canvas.SetLeft(this, 0);
            this.Height = ch.Height - 40;
            this.Width = ch.Width - 15;
            Canvas.SetZIndex(this, 10);


            //max
            Canvas.SetTop(maxLabel, 10);
            Canvas.SetLeft(maxLabel, this.Width - 295);
            maxLabel.FontSize = 20;

            //nextBot
            Canvas.SetTop(botNext, 552);
            Canvas.SetLeft(botNext, 10);
            botNext.FontSize = 20;

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
            datagrid.Height = 350;

            //textblock
            Canvas.SetTop(textblock, 550);
            textblock.Width = Width - 270;
            textblock.Height = 70;

            //bmessage
            Canvas.SetTop(bmessage, Height - 55);
            Canvas.SetLeft(bmessage, 10);
            bmessage.Width = 100;
            bmessage.Height = 50;

            //bmessage
            Canvas.SetTop(bwrite, Height - 55);
            Canvas.SetLeft(bwrite, 120);
            bwrite.Width = 100;
            bwrite.Height = 50;

            //bclose
            Canvas.SetTop(bclose, Height - 55);
            Canvas.SetLeft(bclose, 230);
            bclose.Width = 100;
            bclose.Height = 50;


            Canvas.SetTop(em1, Height - 70);
            Canvas.SetLeft(em1, 330);

            Canvas.SetTop(em2, Height - 35);
            Canvas.SetLeft(em2, 330);

            //adviceStack
            Canvas.SetTop(adviseStack, 170);
            Canvas.SetLeft(adviseStack, 210);
            UpdateUi();


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


        //нажимаем на кнопку руками
        public void writeMsg(Object sender,
            EventArgs e)
        {
            //test();
            string mess = "";
            if (isMin == false && textblock.Text != "")
            {
                mess = textblock.Text;

            }
            if (isMin == false && textblock.Text == "")
            {
                return;

            }
            textblock.Text = "";
            if (isMin)
            {
                List<PersonChat> resevers = new List<PersonChat> {this};
                SimpleMessageBox box = new SimpleMessageBox(ch, resevers);
                bool? res = box.ShowDialog();
            }
            else
            {
                if (String.IsNullOrEmpty(mess))
                {
                    return;
                }
                writeMyMessage(mess);

            }


        }


        //пишем сообщение сами
        public void writeMyMessage(string message)
        {
            ChatTask t = new ChatTask();
            t.type = Chat.Core.TaskEnum.MESSAGE;
            t.message = message;
            t.vkId = personId;
            t.timeExpared = ch.te.setTime(5);
            t.personChatId = personChatId;
            t.isStopped = false;
            t.personName = ch.CurrentUser.Value;
            
            
            string fullname = Person.name;
            string[] words = fullname.Split(new string[] { " " }, StringSplitOptions.None);
            string name = words[0];
            
            t.message = t.message.Replace("$name", name);
            t.message = t.message.Replace("$fullname", fullname);

            ch.tasks.Add(t);
            ch.updateTaskList();
            ch.UpdateUI();
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
            if (personChatId == "person1")
            {
                return;
            }
            for (int i = 0; i < 10; i++)
            {
                {
                    ChatMessage newmessage = new ChatMessage();
                    newmessage.isVirtual = false;
                    newmessage.message =
                        "Привет,ничем помочь не могу,я работаю и";
                    newmessage.isBot = false;
                    newmessage.personChatId = personChatId;
                    newmessage.time = DateTime.Now;
                    newmessage.vkId = 111;
                    newmessage.personName = "Катя";
                    chatMessages.Add(newmessage);

                }

                {
                    ChatMessage newmessage = new ChatMessage();
                
                newmessage.isVirtual = false;
                newmessage.message =
                    "Привет,ничем помочь не могу,я работаю и мне не скучно и моя работа не развлекать других парней";
                newmessage.isBot = false;
                newmessage.personChatId = personChatId;
                newmessage.time = DateTime.Now;
                newmessage.vkId = 111;
                newmessage.personName = "Катя";
                chatMessages.Add(newmessage);
            }

            //ChatMessage newmessage2 = new ChatMessage();
                //newmessage2.isVirtual = false;
                //newmessage2.message = "Хуй тебе на воротник не предложить?";
                //newmessage2.isBot = true;
                //newmessage2.personChatId = personChatId;
                //newmessage2.time = DateTime.Now;
                //newmessage2.vkId = 111;
                //newmessage2.personName = "Сергей";
                //chatMessages.Add(newmessage2);
            }
            UpdateUi();
        }

        public void UpdateUi()
        {
            datagrid.ItemsSource = chatMessages.OrderBy(o => o.time);
            datagrid.Items.Refresh();
            if (chatMessages.Count > 0)
            {
                datagrid.ScrollIntoView(chatMessages.Last());
            }
            updateBotAnswersNumber(chatMessages);
            this.bclose.IsEnabled = banned == false;
            this.bwrite.IsEnabled = banned == false;
            this.bmessage.IsEnabled = banned == false;
            if (!isMin)
            {
                profileImage.Opacity = 1;

            }
            else
            {
                profileImage.Opacity = selected ? 0.3 : 1;

            }




            if (banned)
            {
                maxLabel.Background = Brushes.Red;
                return;
            }
            maxLabel.Background = isActive ? Brushes.LightGreen : Brushes.LightCoral;

        }

        private void updateBotAnswersNumber(List<ChatMessage> list)
        {
            if (ch == null)
            {
                return;
            }
            if (ch.bot == null)
            {
                return;
            }
            string message = "";
            if (chatMessages.Count < 2)
            {
                bclose.Content = "";
                return;
            }
            ChatMessage lastMessage = chatMessages.Last();
            if (lastMessage.isBot == true)
            {
                bclose.Content = "";
                return;
            }

            List<string> mm = ch.bot.getMessages(lastMessage.message);
            if (mm.Count == 0)
            {
                bclose.Content = "";
                return;
            }
            bclose.Content = mm.Count;
        }


        
        //хрень вызывается когда приходят сообщения от сервера
        public void updateMessage(List<string[]> messages)
        {

            if (chatMessages == null || chatMessages.Count == 0)
            {
                return;
            }
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
                string pName = recever == ch.CurrentUser.Key.ToString()
                    ? ch.CurrentUser.Value
                    : this.profileName.Content.ToString();
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
            ch.UpdateUI();
            sm.findAnswer();

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

        private void onBotExecute(object sender, ToolTipEventArgs e)
        {
            Button b = sender as Button;
            if (chatMessages.Count < 2)
            {
                e.Handled = true;
                return;
            }
            ChatMessage lastMessage = chatMessages.Last();
            if (lastMessage.isBot == true)
            {
                e.Handled = true;
                return;
            }

            List<string> mm = ch.bot.getMessages(lastMessage.message);
            string result = "";
            foreach (String m in mm)
            {
                result = result + m + "\r\n";
            }
            b.ToolTip = result;
            return;


        }

        private void botOpenMessages(object sender, RoutedEventArgs e)
        {
            if (ch.bot == null)
            {
                return;
            }
            string message = "";
            if (chatMessages.Count < 2)
            {
                bclose.Content = "";
                return;
            }
            //SmartBotWindow win = new SmartBotWindow(this);
            //win.ShowDialog();
            //ChatMessage lastMessage = chatMessages.Last();
            //if (lastMessage.isBot == true)
            //{
            //    bclose.Content = "";
            //    return;
            //}

            //List<string> mm = ch.bot.getMessages(lastMessage.message);
            //if (mm.Count == 0)
            //{
            //    bclose.Content = "";
            //    return;
            //}

            //BotVariantsWindow bw = new BotVariantsWindow(lastMessage.message, mm, this);
            //bw.ShowDialog();

        }

        private void Datagrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            onChangeState(null, null);
        }

        private void changeActive(object sender, RoutedEventArgs e)
        {
            if (banned)
            {
                isActive = false;
                UpdateUi();
                return;
            }
            isActive = !isActive;
            UpdateUi();
        }

        private void MaxLabel_OnToolTipOpening(object sender, ToolTipEventArgs e)
        {
            if (banned == false)
            {
                e.Handled = true;
            }
            ToolTip = bannedString;
        }

        private void selectItem(object sender, MouseButtonEventArgs e)
        {
            selected = !selected;
            UpdateUi();
        }

        #region for advices

        public static List<T> GetVisualChilds<T>(DependencyObject parent) where T : DependencyObject
        {
            List<T> childs = new List<T>();
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject v = VisualTreeHelper.GetChild(parent, i);
                if (v is T)
                    childs.Add(v as T);
                childs.AddRange(GetVisualChilds<T>(v));
            }
            return childs;
        }

        private void wireAdvisec()
        {
            List<AdviceControl> advControls =
                GetVisualChilds<AdviceControl>(this as DependencyObject);
            foreach (AdviceControl ac in advControls)
            {
                ac.wire(ac.Tag, textblock, new List<PersonChat>() {this});
                ac.setResourcec();
            }
        }

        private void PersonChat_OnLoaded(object sender, RoutedEventArgs e)
        {
            wireAdvisec();
        }

        #endregion

        private void emodziClick(object sender, RoutedEventArgs e)
        {
            string em = (sender as Button).Content.ToString();
            textblock.Text = textblock.Text + em;
        }

        private void dataGridView1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void dataGridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            contextMenuDataGrid.Items.Clear();
            if (datagrid.SelectedItems.Count == 1)
            {
                ChatMessage ch = (ChatMessage)datagrid.SelectedItems[0];
                if (ch.isBot == false)
                {
                    return;
                }
                {
                    MenuItem mi11 = new MenuItem();
                    mi11.Header = "УВЛЕЧЕНИЯ";
                    mi11.Click += mi_opr;
                    mi11.Tag = ch;
                    contextMenuDataGrid.Items.Add(mi11);
                }
                {
                    MenuItem mi11 = new MenuItem();
                    mi11.Header = "ВОПРОСЫ";
                    mi11.Click += mi_opr;
                    mi11.Tag = ch;
                    contextMenuDataGrid.Items.Add(mi11);
                }
                {
                    MenuItem mi11 = new MenuItem();
                    mi11.Header = "ПОЧЕМУ";
                    mi11.Click += mi_opr;
                    mi11.Tag = ch;
                    contextMenuDataGrid.Items.Add(mi11);
                }
                {
                    MenuItem mi11 = new MenuItem();
                    mi11.Header = "ИСТОРИИ";
                    mi11.Click += mi_opr;
                    mi11.Tag = ch;
                    contextMenuDataGrid.Items.Add(mi11);
                }
                {
                    MenuItem mi11 = new MenuItem();
                    mi11.Header = "КОМПЛ.";
                    mi11.Click += mi_opr;
                    mi11.Tag = ch;
                    contextMenuDataGrid.Items.Add(mi11);
                }
                {
                    MenuItem mi11 = new MenuItem();
                    mi11.Header = "CURRENT";
                    mi11.Click += mi_opr;
                    mi11.Tag = ch;
                    contextMenuDataGrid.Items.Add(mi11);
                }
                //MenuItem mi12 = new MenuItem();
                //mi12.Header = "Неоправданный";
                //mi12.Click += mi_neopr;
                //mi12.Tag = mk;
                //contextMenuDataGrid.Items.Add(mi12);


            }
        }
        //if (tag == "hobby")
        //{
        //    ruName = "УВЛЕЧЕНИЯ";
        //}
        //if (tag == "quastins")
        //{
        //    ruName = "ВОПРОСЫ";
        //}

        //if (tag == "why")
        //{
        //    ruName = "ПОЧЕМУ";
        //}

        //if (tag == "history")
        //{
        //    ruName = "ИСТОРИИ";
        //}

        //if (tag == "comp")
        //{
        //    ruName = "КОМПЛ.";
        //}

        //if (tag == "current")
        //{
        //    ruName = "CURRENT";
        //}
        private void mi_opr(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (sender as MenuItem);
            ChatMessage mess = mi.Tag as ChatMessage;
            string text = mess.message.ToString();
            text = text.Replace("\n", "");
            text = text.Replace("\r", "");
            text = text.Replace("\t", "");
            string tag = "";
            if (mi.Header.ToString() == "УВЛЕЧЕНИЯ")
            {
                tag = "hobby";
            }
            if (mi.Header.ToString() == "ВОПРОСЫ")
            {
                tag = "quastins";
            }
            if (mi.Header.ToString() == "ПОЧЕМУ")
            {
                tag = "why";
            }
            if (mi.Header.ToString() == "ИСТОРИИ")
            {
                tag = "history";
            }
            if (mi.Header.ToString() == "КОМПЛ.")
            {
                tag = "comp";
            }
            if (mi.Header.ToString() == "CURRENT")
            {
                tag = "current";
            }
            FileParser.saveAdvise(tag, text);
            datagrid.SelectedItems.Clear();
            datagrid.SelectedItem = null;
            datagrid.Items.Refresh();

        }



        private void openTooltip(object sender, ToolTipEventArgs e)
        {
            (sender as Label).ToolTip = ovInfo.message;
        }

        private void lvi_MouseEnter(object sender, MouseEventArgs e)
        {
            ListViewItem lv = sender as ListViewItem;
            ovInfo = lv.Content as ChatMessage;
        }

        //следующий ответ от бота
        private void onBotNext(object sender, RoutedEventArgs e)
        {

        }


        //куррент тем
        private void openSmTooltip(object sender, ToolTipEventArgs e)
        {
            if (sm.currentTheme == null)
            {
                e.Handled = true;
                return;
            }
            botNext.ToolTip = sm.currentTheme.Name;
        }

        private void goodGirl(object sender, MouseButtonEventArgs e)
        {
            _goodTalk = !_goodTalk;
            profileName.Foreground = _goodTalk ? Brushes.Green : Brushes.Black;

        }
    }
}
