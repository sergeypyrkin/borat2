﻿using System;
using System.Collections.Generic;
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
using MahApps.Metro.Controls;

namespace KamikyForms.Gui
{
    /// <summary>
    /// Логика взаимодействия для PreparePersonsForm.xaml
    /// </summary>
    public partial class PreparePersonsForm : MetroWindow
    {
        List<ApiWrapper.Core.PersonModel> people = new List<PersonModel>();
        public List<ApiWrapper.Core.PersonModel> pl1 = new List<PersonModel>();
        public List<ApiWrapper.Core.PersonModel> pl2 = new List<PersonModel>();
        public List<ApiWrapper.Core.PersonModel> allp = new List<PersonModel>();

        public ApiWrapper.Core.PersonModel ovPlayer = null;

        public readonly int MAX_COUNT = 20;

        public PreparePersonsForm(List<ApiWrapper.Core.PersonModel> peoples)
        {
            InitializeComponent();
            this.people = peoples;
            foreach (PersonModel pl in people)
            {
                pl1.Add(pl);
                allp.Add(pl);

            }

            refreshDg();
        }

        public void refreshDg()
        {

            dataGridView1.ItemsSource = pl1;
            dataGridView1.Items.Refresh();
            dataGridView2.ItemsSource = pl2;
            dataGridView2.Items.Refresh();
            s1.Content = "Все: " + pl1.Count;
            s2.Content = "Выбрано: " + pl2.Count;

        }



        private void lvi_MouseEnter(object sender, MouseEventArgs e)
        {

            ListViewItem lv = sender as ListViewItem;
            ovPlayer = lv.Content as PersonModel;
        }

        private void dataGridView1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (pl2.Count == MAX_COUNT)
            {
                MessageBox.Show("Больше не влезет ... совсем не влезет ");
                return;
            }
            pl1 = RemovePlayer(pl1, ovPlayer);
            pl2 = AddPlayer(pl2, ovPlayer);

            refreshDg();
        }

        private void dataGridView2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pl1 = AddPlayer(pl1, ovPlayer);
            pl2 = RemovePlayer(pl2, ovPlayer);


            refreshDg();
        }

        public List<PersonModel> RemovePlayer(List<PersonModel> p, PersonModel pl)
        {
            if (pl == null) return p;
            p = p.Where(o => o.id != pl.id).ToList();
            return p;
        }


        public List<PersonModel> AddPlayer(List<PersonModel> p, PersonModel pl)
        {
            if (pl == null) return p;
            PersonModel s = allp.First(o => o.id == pl.id);
            p.Add(s);
            return p;
        }


        private void clear_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (PersonModel p in pl2)
            {
                pl1 = AddPlayer(pl1, p);
                pl2 = RemovePlayer(pl2, p);
            }
            refreshDg();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
