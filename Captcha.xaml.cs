using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Resources;
using System.Net;
using System.IO;
using FinewareWPF;

namespace catpcha
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Captcha : Window
    {
        public bool cap = false;
        public Captcha()
        {
            InitializeComponent();
            GetCaptcha();
            
        }

        int count = 0;
        public string GetCaptcha()
        {
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = " ";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            if (count == 0)
            {
                TextBox1.Text = pwd;
                count++;
            }
            return pwd;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetCaptcha();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if(AnswBox.Text == TextBox1.Text)
            {
                MessageBox.Show("Welcome!");
                cap = true;
                Close();

            }
            else if(AnswBox.Text == "")
            {
                MessageBox.Show("Please enter the Captcha before continuing");
            }
            else
            {
                MessageBox.Show("Captcha code incorrect! - Try again");
                AnswBox.Text = "";
                TextBox1.Text = GetCaptcha();
            }

        }

        private void AnswBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
