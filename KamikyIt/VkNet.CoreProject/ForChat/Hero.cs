﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkNet.Examples.ForChat
{
    public class Hero
    {
        public int id;
        public string login;
        public string pass;
        public string log_name;
        public static Hero instance;

        public  static Hero getInstance(int i)
        {
            if (i == 1)
            {
                Hero h = new Hero();
                h.id = 1;
                h.login = "spyrkin@gmail.com";
                h.pass = "crescent912";
                h.log_name = "banlist1.txt";
                instance = h;
                return h;
            }

            if (i == 2)
            {
                Hero h = new Hero();
                h.id = 2;
                h.login = "+79022888672";
                h.pass = "RoflanEbalo1488";
                h.log_name = "banlist2.txt";
                instance = h;
                return h;
            }
            return null;
        }
    }
}
