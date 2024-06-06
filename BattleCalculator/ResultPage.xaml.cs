using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleCalculator
{
    /// <summary>
    /// Logika interakcji dla klasy ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        public event EventHandler? goToSelect;
        public ResultPage()
        {
            InitializeComponent();
        }
        private void ChangePageToSelection(object sender, RoutedEventArgs e)
        {
            goToSelect?.Invoke(this, EventArgs.Empty);
        }

        private void btnNewBattle_Click(object sender, RoutedEventArgs e)
        {
            ChangePageToSelection(sender, e);
        }
        void SaveArmyBtnClick(string armyStringList)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Zapisz armię",
                Filter = "Text Document (*.txt) | *.txt",
                FileName = "army"
            };
            if (sfd.ShowDialog() == true)
            {
                StreamWriter sw = new StreamWriter(File.Create(sfd.FileName));
                sw.Write(armyStringList);
                sw.Dispose();
            }
        }

        private void btnResultArmy1Save_Click(object sender, RoutedEventArgs e)
        {
            SaveArmyBtnClick(tbxTeam1List.Text);
        }

        private void btnResultArmy2Save_Click(object sender, RoutedEventArgs e)
        {
            SaveArmyBtnClick(tbxTeam2List.Text);
        }
    }
}
