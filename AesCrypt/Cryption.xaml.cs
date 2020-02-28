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

namespace AesCrypt
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Cryption : Window
    {
        public Cryption()
        {
            InitializeComponent();
            CustomizationView();
        }

        private void CustomizationView()
        {
            if (!MainWindow.emptyFileBool.GetValueOrDefault())
            {
                saveNewFileButton.Visibility = Visibility.Visible;
                showPassPanelButton.Visibility = Visibility.Visible;
                passPanel.Visibility = Visibility.Collapsed;
            }
            if (!MainWindow.encStateBool)
            {
                this.Title = "Decryption";
                passCheckLabel.Visibility = Visibility.Collapsed;
                passCheckField.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Title = "Encryption";
                passCheckLabel.Visibility = Visibility.Visible;
                passCheckField.Visibility = Visibility.Visible;
            }
        }

        private void ShowPassPanel(object sender, RoutedEventArgs e)
        {
            passPanel.Visibility = Visibility.Visible;
            showPassPanelButton.Visibility = Visibility.Collapsed;
            hidePassPanelButton.Visibility = Visibility.Visible;
        }
        private void HidePassPanel(object sender, RoutedEventArgs e)
        {
            passPanel.Visibility = Visibility.Collapsed;
            showPassPanelButton.Visibility = Visibility.Visible;
            hidePassPanelButton.Visibility = Visibility.Collapsed;
        }

        private void OpenDataLocal(object sender, RoutedEventArgs e)
        {
            if (dataContent.Text == "" || passField.Password == "")
            {
                MessageBox.Show("Text is empty!");
                dataContent.Text = "";
                passField.Password = "";
                passCheckField.Password = "";
                return;
            }
            DataCrypto dataCrypto = new DataCrypto();
            if (!MainWindow.encStateBool) //Decryption
            {
                string text = dataCrypto.OpenSSLDecrypt(Encoding.ASCII.GetBytes(dataContent.Text), passField.Password);
                if(text.Equals(""))
                {
                    passField.Password = "";
                    passCheckField.Password = "";
                    dataCrypto = null;
                    text = null;
                    MessageBox.Show("Error while trying to decrypt data!");
                    return;
                }
                MainWindow.emptyFileBool = false;
                MainWindow.encStateBool = true;
                CustomizationView();
                dataContent.Text = text;
                text = null;
            }
            else //Encryption
            {
                if (passField.Password.Equals(passCheckField.Password))
                {
                    byte[] data = dataCrypto.OpenSSLEncrypt(dataContent.Text, passField.Password);
                    MainWindow.emptyFileBool = false;
                    MainWindow.encStateBool = false;
                    CustomizationView();
                    dataContent.Text = Encoding.ASCII.GetString(data);
                    data = null;
                }
                else
                    MessageBox.Show("Passwords are not same!");
            }
            passField.Password = "";
            passCheckField.Password = "";
            dataCrypto = null;
        }
        private void SaveDataToFile(object sender, RoutedEventArgs e)
        {
            if (dataContent.Text == "" || passField.Password == "")
            {
                MessageBox.Show("Text is empty!");
                dataContent.Text = "";
                passField.Password = "";
                passCheckField.Password = "";
                return;
            }

            if (!MainWindow.encStateBool)
            {
                CrudFile.SaveDecryptedFile(Encoding.ASCII.GetBytes(dataContent.Text), passField.Password);
            }
            else
            {
                if (passField.Password.Equals(passCheckField.Password))
                {
                    CrudFile.SaveEncryptedFile(dataContent.Text, passField.Password);
                }
                else
                    MessageBox.Show("Passwords are not same!");
            }
            MessageBox.Show("Completed!");

            dataContent.Text = "";
            passField.Password = "";
            passCheckField.Password = "";
            Application.Current.Shutdown();
        }
        private void BackToMainMenu(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        
        private void SaveNewFile(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.encStateBool) //decrypted
            {
                CrudFile.CallFileDialog(dataContent.Text);
            }
            else //encrypted
            {
                CrudFile.CallFileDialog(dataContent.Text);
            }
            dataContent.Text = "";
            MessageBox.Show("Successfully!");
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        
        private void ExitEverywhere(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
