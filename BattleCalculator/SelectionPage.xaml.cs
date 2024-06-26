﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Linq;
using static BattleCalculator.SelectionPage;

namespace BattleCalculator
{
    /// <summary>
    /// Logika interakcji dla klasy SelectionPage.xaml
    /// </summary>
    public partial class SelectionPage : Page
    {
        public event EventHandler? btnClick;
        
        public SelectionPage()
        {
            InitializeComponent();
        }

        private void ChangePageToResults(object sender, RoutedEventArgs e)
        {
            btnClick?.Invoke(this, EventArgs.Empty);
            
        }
        public string battleLog = "", team1StringList = "", team2StringList = "", team1StringDisplayList = "", team2StringDisplayList = "", resultString = "Wynik";
        int fortLevel = 0;
        bool isSkirmishAttack = false;
        bool isBattleLand = false;
        bool team1SpecialCondition = false;
        bool team2SpecialCondition = false;
        bool team1Win = false, team2Win = false;
        TerrainType terrain = new TerrainType("filler", 0, 0, 0, 0);
        SeaType[] seaTypes = new SeaType[1];
        //listy armii lub flot
        List<LandUnit> army1UnitsList = new List<LandUnit>();
        List<LandUnit> army2UnitsList = new List<LandUnit>();
        List<Ship> fleet1UnitsList = new List<Ship>();
        List<Ship> fleet2UnitsList = new List<Ship>();
        //og listy
        List<LandUnit> army1OgUnitsList = new List<LandUnit>();
        List<LandUnit> army2OgUnitsList = new List<LandUnit>();
        List<Ship> fleet1OgUnitsList = new List<Ship>();
        List<Ship> fleet2OgUnitsList = new List<Ship>();
        // button bools
        bool startUnits1 = false, startUnits2 = false, explorationUnits1 = false, explorationUnits2 = false, expansionUnits1 = false, expansionUnits2 = false, empireUnits1 = false, empireUnits2 = false, revUnits1 = false, revUnits2 = false, uniqueUnits1 = false, uniqueUnits2 = false;
        bool startShips1 = false, startShips2 = false, explorationShips1 = false, explorationShips2 = false, expansionShips1 = false, expansionShips2 = false, empireShips1 = false, empireShips2 = false, revShips1 = false, revShips2 = false, uniqueShips1 = false, uniqueShips2 = false;

        // lista checkbox'ow dla armii i flot
        List<CheckBox> army1CheckBoxList = new List<CheckBox>();
        List<CheckBox> army2CheckBoxList = new List<CheckBox>();
        List<CheckBox> fleet1CheckBoxList = new List<CheckBox>();
        List<CheckBox> fleet2CheckBoxList = new List<CheckBox>();
        // lista texbox'ow dla armii i flot
        List<TextBox> army1TextboxList = new List<TextBox>();
        List<TextBox> army2TextboxList = new List<TextBox>();
        List<TextBox> fleet1TextboxList = new List<TextBox>();
        List<TextBox> fleet2TextboxList = new List<TextBox>();
        //sprawdzenie czy wybrano wszystko potrzebne do bitwy
        void ActivateStartButton()
        {
            if (cbSkirmishVariant.Text != "Wybierz rodzaj spotkania")
            {
                if (cbBattleType.Text == "Bitwa lądowa" && cbTerrainType.Text != "Wybierz rodzaj terenu" && army1CheckBoxList.Count > 0 && army2CheckBoxList.Count > 0)
                {
                    btnStart.IsEnabled = true;
                }
                else if (cbBattleType.Text == "Bitwa morska" && cbSeaType.Text != "Wybierz głębokość morza" && fleet1CheckBoxList.Count > 0 && fleet2CheckBoxList.Count > 0)
                {
                    btnStart.IsEnabled = true;
                }
                else
                {
                    btnStart.IsEnabled = false;
                }
            }
            else
            {
                btnStart.IsEnabled = false;
            }
        }
        private void cbBattleType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cbBattleType.Text == "Bitwa lądowa")
            {
                isBattleLand = true;
                gridFortLevel.IsEnabled = false;//zmienic kiedy zrobie walke z fortem
                tbFortLevel.Text = $"Poziom fortu: {fortLevel}";
                cbTerrainSeaHidden.Visibility = Visibility.Hidden;
                cbSeaType.Visibility = Visibility.Hidden;
                scFleet1Selection.Visibility = Visibility.Hidden;
                scFleet2Selection.Visibility = Visibility.Hidden;
                cbTerrainType.Visibility = Visibility.Visible;
                scArmy1Selection.Visibility = Visibility.Visible;
                scArmy2Selection.Visibility = Visibility.Visible;
                txbArmy1.Text = "Armia 1";
                txbArmy2.Text = "Armia 2";
                ckArmy1Condition.Content = "Armia przekroczyła rzekę lub się zdesantowała";
                ckArmy2Condition.Content = "Armia przekroczyła rzekę lub się zdesantowała";

            }
            else if (cbBattleType.Text == "Bitwa morska")
            {
                isBattleLand = false;
                gridFortLevel.IsEnabled = false;
                tbFortLevel.Text = "Forty są niedostępne";
                cbTerrainSeaHidden.Visibility = Visibility.Hidden;
                cbSeaType.Visibility = Visibility.Visible;
                scFleet1Selection.Visibility = Visibility.Visible;
                scFleet2Selection.Visibility = Visibility.Visible;
                cbTerrainType.Visibility = Visibility.Hidden;
                scArmy1Selection.Visibility = Visibility.Hidden;
                scArmy2Selection.Visibility = Visibility.Hidden;
                txbArmy1.Text = "Flota 1";
                txbArmy2.Text = "Flota 2";
                ckArmy1Condition.Content = "Ten kraj wynalazł taktyki nelsońskie";
                ckArmy2Condition.Content = "Ten kraj wynalazł taktyki nelsońskie";
            }
            ActivateStartButton();
        }

        private void cbSkirmishVariant_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cbSkirmishVariant.Text == "Atak")
            {
                isSkirmishAttack = true;
            }
            else if (cbSkirmishVariant.Text == "Spotkanie")
            {
                isSkirmishAttack = false;
            }
            ActivateStartButton();
        }
        private void cbTerrainType_LostFocus(object sender, RoutedEventArgs e)
        {
            ActivateStartButton();
        }
        private void btnFortLevelMinus_Click(object sender, RoutedEventArgs e)
        {
            if (fortLevel > 0)
            {
                fortLevel -= 1;
                tbFortLevel.Text = $"Poziom fortu: {fortLevel}";
            }
        }

        private void btnFortLevelPlus_Click(object sender, RoutedEventArgs e)
        {
            if (fortLevel < 3)
            {
                fortLevel += 1;
                tbFortLevel.Text = $"Poziom fortu: {fortLevel}";
            }
        }
        // rozwijanie epok dla armii 1
        void DropdownBtn(ref bool isBtnSelected, StackPanel stackpanel)
        {
            if (!isBtnSelected)
            {
                stackpanel.Visibility = Visibility.Visible;
                isBtnSelected = true;
            }
            else
            {
                stackpanel.Visibility = Visibility.Collapsed;
                isBtnSelected = false;
            }
        }
        private void btnStartUnits1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref startUnits1, stpStartUnits1);
        }
        private void btnExplorationEra1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref explorationUnits1, stpExplorationEra1);
        }
        private void btnExpansionEra1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref expansionUnits1, stpExpansionEra1);
        }
        private void btnEmpireEra1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref empireUnits1, stpEmpireEra1);
        }
        private void btnRevolutionEra1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref revUnits1, stpRevolutionEra1);
        }
        private void btnUniqueUnitsEra1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref uniqueUnits1, stpUniqueUnits1);
        }

        // rozwijanie epok dla armii 2
        private void btnStartUnits2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref startUnits2, stpStartUnits2);
        }
        private void btnExplorationEra2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref explorationUnits2, stpExplorationEra2);
        }
        private void btnExpansionEra2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref expansionUnits2, stpExpansionEra2);
        }
        private void btnEmpireEra2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref empireUnits2, stpEmpireEra2);
        }
        private void btnRevolutionEra2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref revUnits2, stpRevolutionEra2);
        }
        private void btnUniqueUnitsEra2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref uniqueUnits2 , stpUniqueUnits2);
        }
        // rozwijanie epok dla floty 1
        private void btnStartShips1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref startShips1 , stpStartShips1);
        }
        private void btnExplorationEraShips1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref explorationShips1 , stpExplorationEraShips1);
        }

        private void btnExpansionEraShips1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref expansionShips1 , stpExpansionEraShips1);
        }
        private void btnEmpireEraShips1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref empireShips1 , stpEmpireEraShips1);
        }
        private void btnRevolutionEraShips1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref revShips1, stpRevolutionEraShips1);
        }
        private void btnUniqueUnitsShipsEra1_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref uniqueShips1, stpUniqueShips1);
        }
        //rozwijanie epok dla floty 2
        private void btnStartShips2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref startShips2 , stpStartShips2);
        }
        private void btnExplorationEraShips2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref explorationShips2 , stpExplorationEraShips2);
        }
        private void btnExpansionEraShips2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref expansionShips2 , stpExpansionEraShips2);
        }
        private void btnEmpireEraShips2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref empireShips2 , stpEmpireEraShips2);
        }
        private void btnRevolutionEraShips2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref revShips2, stpRevolutionEraShips2);
        }
        private void btnUniqueUnitsShipsEra2_Click(object sender, RoutedEventArgs e)
        {
            DropdownBtn(ref uniqueShips2, stpUniqueShips2);
        }
        //zapisywanie wczytywanie etc armii i flot
        void SaveArmyClicked(bool isTeam1, bool isArmy)
        {
            List<CheckBox> clickedCks = new List<CheckBox>();
            List<TextBox> viableInputs = new List<TextBox>();
            if(isTeam1 && isArmy)
            {
                CheckBox[] army1AllCks = { ckArmy1Pikemen, ckArmy1Arquebusiers, ckArmy1Archers, ckArmy1Crossbowmen, ckArmy1Knights, ckArmy1Horsemen, ckArmy1Bombard, ckArmy1PikeShotArq, ckArmy1HeavyHussars, ckArmy1Reiters, ckArmy1FieldCannon, ckArmy1HeavyCannon, ckArmy1PikeShotMusk, ckArmy1EarlyFusiliers, ckArmy1EarlyCuirassier, ckArmy1Harquebusers, ckArmy1Lancers, ckArmy1SiegeHowitzer, ckArmy1Fusiliers, ckArmy1Grenadiers, ckArmy1Militia, ckArmy1CarbineCav, ckArmy1Dragoons, ckArmy1Hussars, ckArmy1Cuiraissiers, ckArmy1FieldGun, ckArmy1Mortars, ckArmy1LightInfantry, ckArmy1LineInfantry, ckArmy1FieldHowitzer, ckArmy1Cossacks, ckArmy1HorseArcher, ckArmy1TribalWarriors, ckArmy1TribalRanger, ckArmy1TribalHorsemen };
                TextBox[] army1AllTbx = { tbxArmy1Pikemen, tbxArmy1Arquebusiers, tbxArmy1Archers, tbxArmy1Crossbowmen, tbxArmy1Knights, tbxArmy1Horsemen, tbxArmy1Bombard, tbxArmy1PikeShotArq, tbxArmy1HeavyHussars, tbxArmy1Reiters, tbxArmy1FieldCannon, tbxArmy1HeavyCannon, tbxArmy1PikeShotMusk, tbxArmy1EarlyFusiliers, tbxArmy1EarlyCuirassier, tbxArmy1Harquebusers, tbxArmy1Lancers, tbxArmy1SiegeHowitzer, tbxArmy1Fusiliers, tbxArmy1Grenadiers, tbxArmy1Militia, tbxArmy1CarbineCav, tbxArmy1Dragoons, tbxArmy1Hussars, tbxArmy1Cuiraissiers, tbxArmy1FieldGun, tbxArmy1Mortars, tbxArmy1LightInfantry, tbxArmy1LineInfantry, tbxArmy1FieldHowitzer, tbxArmy1Cossacks, tbxArmy1HorseArcher, tbxArmy1TribalWarriors, tbxArmy1TribalRanger, tbxArmy1TribalHorsemen };
                foreach(CheckBox ck in army1AllCks) if (ck.IsChecked == true) clickedCks.Add(ck);
                foreach (TextBox tbx in army1AllTbx)
                {
                    int num = 0;
                    bool isNum = int.TryParse(tbx.Text.Trim(), out num);
                    if (num > 0 && isNum) viableInputs.Add(tbx);
                }
                List<LandUnit> landUnits = new List<LandUnit>();
                FillArmyList(clickedCks, viableInputs, ref landUnits);
                string armyString = ConvertArmyToString(landUnits, false);
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    Title = "Zapisz armię",
                    Filter = "Text Document (*.txt) | *.txt",
                    FileName = $"army"
                };
                if (sfd.ShowDialog() == true)
                {
                    StreamWriter sw = new StreamWriter(File.Create(sfd.FileName));
                    sw.Write(armyString);
                    sw.Dispose();
                }
            }
            else if(!isTeam1 && isArmy)
            {
                CheckBox[] army2AllCks = { ckArmy2Pikemen, ckArmy2Arquebusiers, ckArmy2Archers, ckArmy2Crossbowmen, ckArmy2Knights, ckArmy2Horsemen, ckArmy2Bombard, ckArmy2PikeShotArq, ckArmy2HeavyHussars, ckArmy2Reiters, ckArmy2FieldCannon, ckArmy2HeavyCannon, ckArmy2PikeShotMusk, ckArmy2EarlyFusiliers, ckArmy2EarlyCuirassier, ckArmy2Harquebusers, ckArmy2Lancers, ckArmy2SiegeHowitzer, ckArmy2Fusiliers, ckArmy2Grenadiers, ckArmy2Militia, ckArmy2CarbineCav, ckArmy2Dragoons, ckArmy2Hussars, ckArmy2Cuiraissiers, ckArmy2FieldGun, ckArmy2Mortars, ckArmy2LightInfantry, ckArmy2LineInfantry, ckArmy2FieldHowitzer, ckArmy2Cossacks, ckArmy2HorseArcher, ckArmy2TribalWarriors, ckArmy2TribalRanger, ckArmy2TribalHorsemen };
                TextBox[] army2AllTbx = { tbxArmy2Pikemen, tbxArmy2Arquebusiers, tbxArmy2Archers, tbxArmy2Crossbowmen, tbxArmy2Knights, tbxArmy2Horsemen, tbxArmy2Bombard, tbxArmy2PikeShotArq, tbxArmy2HeavyHussars, tbxArmy2Reiters, tbxArmy2FieldCannon, tbxArmy2HeavyCannon, tbxArmy2PikeShotMusk, tbxArmy2EarlyFusiliers, tbxArmy2EarlyCuirassier, tbxArmy2Harquebusers, tbxArmy2Lancers, tbxArmy2SiegeHowitzer, tbxArmy2Fusiliers, tbxArmy2Grenadiers, tbxArmy2Militia, tbxArmy2CarbineCav, tbxArmy2Dragoons, tbxArmy2Hussars, tbxArmy2Cuiraissiers, tbxArmy2FieldGun, tbxArmy2Mortars, tbxArmy2LightInfantry, tbxArmy2LineInfantry, tbxArmy2FieldHowitzer, tbxArmy2Cossacks, tbxArmy2HorseArcher, tbxArmy2TribalWarriors, tbxArmy2TribalRanger, tbxArmy2TribalHorsemen };
                foreach (CheckBox ck in army2AllCks) if (ck.IsChecked == true) clickedCks.Add(ck);
                foreach (TextBox tbx in army2AllTbx)
                {
                    int num = 0;
                    bool isNum = int.TryParse(tbx.Text.Trim(), out num);
                    if (num > 0 && isNum) viableInputs.Add(tbx);
                }
                List<LandUnit> landUnits = new List<LandUnit>();
                FillArmyList(clickedCks, viableInputs, ref landUnits);
                string armyString = ConvertArmyToString(landUnits, false);
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    Title = "Zapisz armię",
                    Filter = "Text Document (*.txt) | *.txt",
                    FileName = "army"
                };
                if (sfd.ShowDialog() == true)
                {
                    StreamWriter sw = new StreamWriter(File.Create(sfd.FileName));
                    sw.Write(armyString);
                    sw.Dispose();
                }
            }
            else if(isTeam1 && !isArmy)
            {
                CheckBox[] fleet1AllCks = { ckFleet1Carrack, ckFleet1Caravel, ckFleet1Galley, ckFleet1Galleon, ckFleet1Schooner, ckFleet1Brig, ckFleet1Frigate, ckFleet1GreatFrigate, ckFleet1ShipOfLine, ckFleet1Eastindiaman, ckFleet1ArmoredFrigate, ckFleet1AsianShip };
                TextBox[] fleet1AllTbx = { tbxFleet1Carrack, tbxFleet1Caravel, tbxFleet1Galley, tbxFleet1Galleon, tbxFleet1Schooner, tbxFleet1Brig, tbxFleet1Frigate, tbxFleet1GreatFrigate, tbxFleet1ShipOfLine, tbxFleet1Eastindiaman, tbxFleet1ArmoredFrigate, tbxFleet1AsianShip };
                //to do
            }
            else
            {
                CheckBox[] fleet2AllCks = { ckFleet2Carrack, ckFleet2Caravel, ckFleet2Galley, ckFleet2Galleon, ckFleet2Schooner, ckFleet2Brig, ckFleet2Frigate, ckFleet2GreatFrigate, ckFleet2ShipOfLine, ckFleet2Eastindiaman, ckFleet2ArmoredFrigate, ckFleet2AsianShip };
                TextBox[] fleet2AllTbx = { tbxFleet2Carrack, tbxFleet2Caravel, tbxFleet2Galley, tbxFleet2Galleon, tbxFleet2Schooner, tbxFleet2Brig, tbxFleet2Frigate, tbxFleet2GreatFrigate, tbxFleet2ShipOfLine, tbxFleet2Eastindiaman, tbxFleet2ArmoredFrigate, tbxFleet2AsianShip };
                //to do
            }
        }
        private void btnSaveArmy1_Click(object sender, RoutedEventArgs e)
        {
            SaveArmyClicked(true, true);
        }

        private void btnSaveArmy2_Click(object sender, RoutedEventArgs e)
        {
            SaveArmyClicked(false, true);
        }
        void LoadArmyBtnClicked(bool isTeam1, bool isArmy, ref CheckBox[] armyAllCks, ref TextBox[] armyAllTbx, bool add = false)
        {
            //to do
            if (isTeam1 && isArmy)
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Title = "Otwórz plik",
                    Filter = "Text Document (*.txt) | *.txt"
                };
                if (ofd.ShowDialog() == true)
                {
                    StreamReader sr = new StreamReader(File.OpenRead(ofd.FileName));
                    string armyList = sr.ReadToEnd();
                    sr.Dispose();
                    string name = "";
                    string strNum = "";
                    foreach (char c in armyList)
                    {
                        bool nameReading = true, numReading = false;
                        if (nameReading)
                        {
                            if (c == ':')
                            {
                                nameReading = false;
                                numReading = true;
                            }
                            else name += c;
                        }
                        if (numReading)
                        {
                            if (c == '\n') numReading = false;
                            else strNum += c;
                        }
                        if ( !nameReading && !numReading)
                        {
                            int num = 0;
                            int.TryParse(strNum.Trim(), out num);
                            for (int i = 0; i < armyAllCks.Length; i++)
                            {
                                if(name == Convert.ToString(armyAllCks[i].Content) && num > 0)
                                {
                                    Debug.WriteLine($"Znaleziono ck Name:{name} Num:{strNum}");
                                    armyAllCks[i].IsChecked = true;
                                    if (add)
                                    {
                                        int input = 0;
                                        int.TryParse(armyAllTbx[i].Text.Trim(), out input);
                                        input += num;
                                        armyAllTbx[i].Text = $"{input}";
                                    }
                                    else armyAllTbx[i].Text = strNum;
                                }
                            }
                            nameReading = true;
                            name = "";
                            strNum = "";
                        }
                    }
                    sr.Dispose();
                }
            }
            else if (!isTeam1 && isArmy)
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Title = "Otwórz plik",
                    Filter = "Text Document (*.txt) | *.txt"
                };
                if (ofd.ShowDialog() == true)
                {
                    StreamReader sr = new StreamReader(File.OpenRead(ofd.FileName));
                    string armyList = sr.ReadToEnd();
                    string name = "";
                    string strNum = "";
                    foreach (char c in armyList)
                    {
                        bool nameReading = true, numReading = false;
                        
                        if (nameReading)
                        {
                            if (c == ':')
                            {
                                nameReading = false;
                                numReading = true;
                            }
                            else name += c;
                        }
                        if (numReading)
                        {
                            if (c == '\n') numReading = false;
                            else strNum += c;
                        }
                        if (!nameReading && !numReading)
                        {
                            int num = 0;
                            int.TryParse(strNum.Trim(), out num);
                            for (int i = 0; i < armyAllCks.Length; i++)
                            {
                                if (name == Convert.ToString(armyAllCks[i].Content) && num > 0)
                                {
                                    armyAllCks[i].IsChecked = true;
                                    if (add)
                                    {
                                        int input = 0;
                                        int.TryParse(armyAllTbx[i].Text.Trim(), out input);
                                        input += num;
                                        armyAllTbx[i].Text = $"{input}";
                                    }
                                    else armyAllTbx[i].Text = strNum;

                                }
                            }
                            nameReading = true;
                            name = "";
                            strNum = "";
                        }
                    }
                    sr.Dispose();
                }
            }
            else if (isTeam1 && !isArmy)
            {
                CheckBox[] fleet1AllCks = { ckFleet1Carrack, ckFleet1Caravel, ckFleet1Galley, ckFleet1Galleon, ckFleet1Schooner, ckFleet1Brig, ckFleet1Frigate, ckFleet1GreatFrigate, ckFleet1ShipOfLine, ckFleet1Eastindiaman, ckFleet1ArmoredFrigate, ckFleet1AsianShip };
                TextBox[] fleet1AllTbx = { tbxFleet1Carrack, tbxFleet1Caravel, tbxFleet1Galley, tbxFleet1Galleon, tbxFleet1Schooner, tbxFleet1Brig, tbxFleet1Frigate, tbxFleet1GreatFrigate, tbxFleet1ShipOfLine, tbxFleet1Eastindiaman, tbxFleet1ArmoredFrigate, tbxFleet1AsianShip };
                //to do
            }
            else
            {
                CheckBox[] fleet2AllCks = { ckFleet2Carrack, ckFleet2Caravel, ckFleet2Galley, ckFleet2Galleon, ckFleet2Schooner, ckFleet2Brig, ckFleet2Frigate, ckFleet2GreatFrigate, ckFleet2ShipOfLine, ckFleet2Eastindiaman, ckFleet2ArmoredFrigate, ckFleet2AsianShip };
                TextBox[] fleet2AllTbx = { tbxFleet2Carrack, tbxFleet2Caravel, tbxFleet2Galley, tbxFleet2Galleon, tbxFleet2Schooner, tbxFleet2Brig, tbxFleet2Frigate, tbxFleet2GreatFrigate, tbxFleet2ShipOfLine, tbxFleet2Eastindiaman, tbxFleet2ArmoredFrigate, tbxFleet2AsianShip };
                //to do
            }
            
        }
        private void btnLoadArmy1_Click(object sender, RoutedEventArgs e)
        {
            CheckBox[] army1AllCks = { ckArmy1Pikemen, ckArmy1Arquebusiers, ckArmy1Archers, ckArmy1Crossbowmen, ckArmy1Knights, ckArmy1Horsemen, ckArmy1Bombard, ckArmy1PikeShotArq, ckArmy1HeavyHussars, ckArmy1Reiters, ckArmy1FieldCannon, ckArmy1HeavyCannon, ckArmy1PikeShotMusk, ckArmy1EarlyFusiliers, ckArmy1EarlyCuirassier, ckArmy1Harquebusers, ckArmy1Lancers, ckArmy1SiegeHowitzer, ckArmy1Fusiliers, ckArmy1Grenadiers, ckArmy1Militia, ckArmy1CarbineCav, ckArmy1Dragoons, ckArmy1Hussars, ckArmy1Cuiraissiers, ckArmy1FieldGun, ckArmy1Mortars, ckArmy1LightInfantry, ckArmy1LineInfantry, ckArmy1FieldHowitzer, ckArmy1Cossacks, ckArmy1HorseArcher, ckArmy1TribalWarriors, ckArmy1TribalRanger, ckArmy1TribalHorsemen };
            TextBox[] army1AllTbx = { tbxArmy1Pikemen, tbxArmy1Arquebusiers, tbxArmy1Archers, tbxArmy1Crossbowmen, tbxArmy1Knights, tbxArmy1Horsemen, tbxArmy1Bombard, tbxArmy1PikeShotArq, tbxArmy1HeavyHussars, tbxArmy1Reiters, tbxArmy1FieldCannon, tbxArmy1HeavyCannon, tbxArmy1PikeShotMusk, tbxArmy1EarlyFusiliers, tbxArmy1EarlyCuirassier, tbxArmy1Harquebusers, tbxArmy1Lancers, tbxArmy1SiegeHowitzer, tbxArmy1Fusiliers, tbxArmy1Grenadiers, tbxArmy1Militia, tbxArmy1CarbineCav, tbxArmy1Dragoons, tbxArmy1Hussars, tbxArmy1Cuiraissiers, tbxArmy1FieldGun, tbxArmy1Mortars, tbxArmy1LightInfantry, tbxArmy1LineInfantry, tbxArmy1FieldHowitzer, tbxArmy1Cossacks, tbxArmy1HorseArcher, tbxArmy1TribalWarriors, tbxArmy1TribalRanger, tbxArmy1TribalHorsemen };
            LoadArmyBtnClicked(true, true, ref army1AllCks, ref army1AllTbx);
        }

        private void btnAddArmy1_Click(object sender, RoutedEventArgs e)
        {
            CheckBox[] army1AllCks = { ckArmy1Pikemen, ckArmy1Arquebusiers, ckArmy1Archers, ckArmy1Crossbowmen, ckArmy1Knights, ckArmy1Horsemen, ckArmy1Bombard, ckArmy1PikeShotArq, ckArmy1HeavyHussars, ckArmy1Reiters, ckArmy1FieldCannon, ckArmy1HeavyCannon, ckArmy1PikeShotMusk, ckArmy1EarlyFusiliers, ckArmy1EarlyCuirassier, ckArmy1Harquebusers, ckArmy1Lancers, ckArmy1SiegeHowitzer, ckArmy1Fusiliers, ckArmy1Grenadiers, ckArmy1Militia, ckArmy1CarbineCav, ckArmy1Dragoons, ckArmy1Hussars, ckArmy1Cuiraissiers, ckArmy1FieldGun, ckArmy1Mortars, ckArmy1LightInfantry, ckArmy1LineInfantry, ckArmy1FieldHowitzer, ckArmy1Cossacks, ckArmy1HorseArcher, ckArmy1TribalWarriors, ckArmy1TribalRanger, ckArmy1TribalHorsemen };
            TextBox[] army1AllTbx = { tbxArmy1Pikemen, tbxArmy1Arquebusiers, tbxArmy1Archers, tbxArmy1Crossbowmen, tbxArmy1Knights, tbxArmy1Horsemen, tbxArmy1Bombard, tbxArmy1PikeShotArq, tbxArmy1HeavyHussars, tbxArmy1Reiters, tbxArmy1FieldCannon, tbxArmy1HeavyCannon, tbxArmy1PikeShotMusk, tbxArmy1EarlyFusiliers, tbxArmy1EarlyCuirassier, tbxArmy1Harquebusers, tbxArmy1Lancers, tbxArmy1SiegeHowitzer, tbxArmy1Fusiliers, tbxArmy1Grenadiers, tbxArmy1Militia, tbxArmy1CarbineCav, tbxArmy1Dragoons, tbxArmy1Hussars, tbxArmy1Cuiraissiers, tbxArmy1FieldGun, tbxArmy1Mortars, tbxArmy1LightInfantry, tbxArmy1LineInfantry, tbxArmy1FieldHowitzer, tbxArmy1Cossacks, tbxArmy1HorseArcher, tbxArmy1TribalWarriors, tbxArmy1TribalRanger, tbxArmy1TribalHorsemen };
            LoadArmyBtnClicked(true, true, ref army1AllCks, ref army1AllTbx, true);
        }

        private void btnLoadArmy2_Click(object sender, RoutedEventArgs e)
        {
            CheckBox[] army2AllCks = { ckArmy2Pikemen, ckArmy2Arquebusiers, ckArmy2Archers, ckArmy2Crossbowmen, ckArmy2Knights, ckArmy2Horsemen, ckArmy2Bombard, ckArmy2PikeShotArq, ckArmy2HeavyHussars, ckArmy2Reiters, ckArmy2FieldCannon, ckArmy2HeavyCannon, ckArmy2PikeShotMusk, ckArmy2EarlyFusiliers, ckArmy2EarlyCuirassier, ckArmy2Harquebusers, ckArmy2Lancers, ckArmy2SiegeHowitzer, ckArmy2Fusiliers, ckArmy2Grenadiers, ckArmy2Militia, ckArmy2CarbineCav, ckArmy2Dragoons, ckArmy2Hussars, ckArmy2Cuiraissiers, ckArmy2FieldGun, ckArmy2Mortars, ckArmy2LightInfantry, ckArmy2LineInfantry, ckArmy2FieldHowitzer, ckArmy2Cossacks, ckArmy2HorseArcher, ckArmy2TribalWarriors, ckArmy2TribalRanger, ckArmy2TribalHorsemen };
            TextBox[] army2AllTbx = { tbxArmy2Pikemen, tbxArmy2Arquebusiers, tbxArmy2Archers, tbxArmy2Crossbowmen, tbxArmy2Knights, tbxArmy2Horsemen, tbxArmy2Bombard, tbxArmy2PikeShotArq, tbxArmy2HeavyHussars, tbxArmy2Reiters, tbxArmy2FieldCannon, tbxArmy2HeavyCannon, tbxArmy2PikeShotMusk, tbxArmy2EarlyFusiliers, tbxArmy2EarlyCuirassier, tbxArmy2Harquebusers, tbxArmy2Lancers, tbxArmy2SiegeHowitzer, tbxArmy2Fusiliers, tbxArmy2Grenadiers, tbxArmy2Militia, tbxArmy2CarbineCav, tbxArmy2Dragoons, tbxArmy2Hussars, tbxArmy2Cuiraissiers, tbxArmy2FieldGun, tbxArmy2Mortars, tbxArmy2LightInfantry, tbxArmy2LineInfantry, tbxArmy2FieldHowitzer, tbxArmy2Cossacks, tbxArmy2HorseArcher, tbxArmy2TribalWarriors, tbxArmy2TribalRanger, tbxArmy2TribalHorsemen };
            LoadArmyBtnClicked(true, false, ref army2AllCks, ref army2AllTbx);
        }

        private void btnAddArmy2_Click(object sender, RoutedEventArgs e)
        {
            CheckBox[] army2AllCks = { ckArmy2Pikemen, ckArmy2Arquebusiers, ckArmy2Archers, ckArmy2Crossbowmen, ckArmy2Knights, ckArmy2Horsemen, ckArmy2Bombard, ckArmy2PikeShotArq, ckArmy2HeavyHussars, ckArmy2Reiters, ckArmy2FieldCannon, ckArmy2HeavyCannon, ckArmy2PikeShotMusk, ckArmy2EarlyFusiliers, ckArmy2EarlyCuirassier, ckArmy2Harquebusers, ckArmy2Lancers, ckArmy2SiegeHowitzer, ckArmy2Fusiliers, ckArmy2Grenadiers, ckArmy2Militia, ckArmy2CarbineCav, ckArmy2Dragoons, ckArmy2Hussars, ckArmy2Cuiraissiers, ckArmy2FieldGun, ckArmy2Mortars, ckArmy2LightInfantry, ckArmy2LineInfantry, ckArmy2FieldHowitzer, ckArmy2Cossacks, ckArmy2HorseArcher, ckArmy2TribalWarriors, ckArmy2TribalRanger, ckArmy2TribalHorsemen };
            TextBox[] army2AllTbx = { tbxArmy2Pikemen, tbxArmy2Arquebusiers, tbxArmy2Archers, tbxArmy2Crossbowmen, tbxArmy2Knights, tbxArmy2Horsemen, tbxArmy2Bombard, tbxArmy2PikeShotArq, tbxArmy2HeavyHussars, tbxArmy2Reiters, tbxArmy2FieldCannon, tbxArmy2HeavyCannon, tbxArmy2PikeShotMusk, tbxArmy2EarlyFusiliers, tbxArmy2EarlyCuirassier, tbxArmy2Harquebusers, tbxArmy2Lancers, tbxArmy2SiegeHowitzer, tbxArmy2Fusiliers, tbxArmy2Grenadiers, tbxArmy2Militia, tbxArmy2CarbineCav, tbxArmy2Dragoons, tbxArmy2Hussars, tbxArmy2Cuiraissiers, tbxArmy2FieldGun, tbxArmy2Mortars, tbxArmy2LightInfantry, tbxArmy2LineInfantry, tbxArmy2FieldHowitzer, tbxArmy2Cossacks, tbxArmy2HorseArcher, tbxArmy2TribalWarriors, tbxArmy2TribalRanger, tbxArmy2TribalHorsemen };
            LoadArmyBtnClicked(true, false, ref army2AllCks, ref army2AllTbx, true);
        }
        //zaznaczanie jednostek i wyznaczanie ich ilosci
        void SelectAnUnit(ref CheckBox ckUnit, ref TextBox tbxUnitNum, ref List<CheckBox> ckList, ref List<TextBox> tbxList)
        {
            bool isChecked = ckUnit.IsChecked == true;
            if (isChecked)
            {
                tbxUnitNum.IsEnabled = true;
                if (!ckList.Contains(ckUnit)) ckList.Add(ckUnit);
                if (!tbxList.Contains(tbxUnitNum)) tbxList.Add(tbxUnitNum);
            }
            else
            {
                tbxUnitNum.IsEnabled = false;
                if (ckList.Contains(ckUnit)) ckList.Remove(ckUnit);
                if (tbxList.Contains(tbxUnitNum)) tbxList.Remove(tbxUnitNum);
            }
            ActivateStartButton();
        }
        void VerifyInputNumberWhile(ref TextBox tbxUnitNum)
        {
            int numOfUnit;
            bool tbxIsNumber = false;
            tbxIsNumber = int.TryParse(tbxUnitNum.Text.Trim(), out numOfUnit);
            if (tbxUnitNum.Text.Trim() != "" || tbxIsNumber)
            {
                if (numOfUnit <= 0)
                {
                    tbxUnitNum.Text = "0";
                }
            }
        }
        void VerifyInputNumberAfter(ref TextBox tbxUnitNum)
        {
            int numOfUnit;
            bool tbxIsNumber = false;
            tbxIsNumber = int.TryParse(tbxUnitNum.Text.Trim(), out numOfUnit);
            if (tbxUnitNum.Text.Trim() == "" || !tbxIsNumber)
            {
                tbxUnitNum.Text = "0";
            }
        }
        //dla armii 1
        private void ckArmy1Pikemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Pikemen, ref tbxArmy1Pikemen, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Pikemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Pikemen);
        }
        private void ckArmy1Arquebusiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Arquebusiers, ref tbxArmy1Arquebusiers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Arquebusiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Arquebusiers);
        }

        private void ckArmy1Archers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Archers, ref tbxArmy1Archers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Archers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Archers);
        }

        private void ckArmy1Crossbowmen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Crossbowmen, ref tbxArmy1Crossbowmen, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Crossbowmen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Crossbowmen);
        }

        private void ckArmy1Knights_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Knights, ref tbxArmy1Knights, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Knights_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Knights);
        }
        private void ckArmy1Horsemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Horsemen, ref tbxArmy1Horsemen, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Horsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Horsemen);
        }

        private void ckArmy1Bombard_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Bombard, ref tbxArmy1Bombard, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Bombard_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Bombard);
        }

        private void ckArmy1PikeShotArq_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1PikeShotArq, ref tbxArmy1PikeShotArq, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1PikeShotArq_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1PikeShotArq);
        }

        private void ckArmy1HeavyHussars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1HeavyHussars, ref tbxArmy1HeavyHussars, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1HeavyHussars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1HeavyHussars);
        }

        private void ckArmy1Cossacks_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Cossacks, ref tbxArmy1Cossacks, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Cossacks_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Cossacks);
        }

        private void ckArmy1Reiters_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Reiters, ref tbxArmy1Reiters, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Reiters_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Reiters);
        }
        private void ckArmy1FieldCannon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1FieldCannon, ref tbxArmy1FieldCannon, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1FieldCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1FieldCannon);
        }

        private void ckArmy1HeavyCannon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1HeavyCannon, ref tbxArmy1HeavyCannon, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1HeavyCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1HeavyCannon);
        }

        private void ckArmy1PikeShotMusk_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1PikeShotMusk, ref tbxArmy1PikeShotMusk, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1PikeShotMusk_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1PikeShotMusk);
        }

        private void ckArmy1EarlyFusiliers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1EarlyFusiliers, ref tbxArmy1EarlyFusiliers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1EarlyFusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1EarlyFusiliers);
        }

        private void ckArmy1EarlyCuirassier_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1EarlyCuirassier, ref tbxArmy1EarlyCuirassier, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1EarlyCuirassier_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1EarlyCuirassier);
        }

        private void ckArmy1Harquebusers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Harquebusers, ref tbxArmy1Harquebusers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Harquebusers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Harquebusers);
        }

        private void ckArmy1Lancers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Lancers, ref tbxArmy1Lancers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Lancers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Lancers);
        }

        private void ckArmy1SiegeHowitzer_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1SiegeHowitzer, ref tbxArmy1SiegeHowitzer, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1SiegeHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1SiegeHowitzer);
        }

        private void ckArmy1Fusiliers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Fusiliers, ref tbxArmy1Fusiliers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Fusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Fusiliers);
        }
        private void ckArmy1Grenadiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Grenadiers, ref tbxArmy1Grenadiers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Grenadiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Grenadiers);
        }
        private void ckArmy1Militia_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Militia, ref tbxArmy1Militia, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Militia_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Militia);
        }

        private void ckArmy1CarbineCav_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1CarbineCav, ref tbxArmy1CarbineCav, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1CarbineCav_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1CarbineCav);
        }

        private void ckArmy1Dragoons_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Dragoons, ref tbxArmy1Dragoons, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Dragoons_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Dragoons);
        }

        private void ckArmy1Hussars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Hussars, ref tbxArmy1Hussars, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Hussars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Hussars);
        }

        private void ckArmy1Cuiraissiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Cuiraissiers, ref tbxArmy1Cuiraissiers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Cuiraissiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Cuiraissiers);
        }

        private void ckArmy1FieldGun_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1FieldGun, ref tbxArmy1FieldGun, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1FieldGun_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1FieldGun);
        }

        private void ckArmy1Mortars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1Mortars, ref tbxArmy1Mortars, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Mortars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Mortars);
        }

        private void ckArmy1LightInfantry_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1LightInfantry, ref tbxArmy1LightInfantry, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1LightInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1LightInfantry);
        }

        private void ckArmy1LineInfantry_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1LineInfantry, ref tbxArmy1LineInfantry, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1LineInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1LineInfantry);
        }

        private void ckArmy1FieldHowitzer_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1FieldHowitzer, ref tbxArmy1FieldHowitzer, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1FieldHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1FieldHowitzer);
        }

        private void ckArmy1TribalWarriors_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1TribalWarriors, ref tbxArmy1TribalWarriors, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1TribalWarriors_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1TribalWarriors);
        }

        private void ckArmy1TribalRanger_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1TribalRanger, ref tbxArmy1TribalRanger, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1TribalRanger_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1TribalRanger);
        }

        private void ckArmy1TribalHorsemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1TribalHorsemen, ref tbxArmy1TribalHorsemen, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1TribalHorsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1TribalHorsemen);
        }
        private void tbxArmy1HorseArcher_LostFocus(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy1HorseArcher, ref tbxArmy1HorseArcher, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void ckArmy1HorseArcher_Click(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1HorseArcher);
        }
        // dla floty 1
        private void ckFleet1Carrack_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1Carrack, ref tbxFleet1Carrack, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Carrack_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Carrack);
        }

        private void ckFleet1Caravel_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1Caravel, ref tbxFleet1Caravel, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Caravel_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Caravel);
        }

        private void ckFleet1Galley_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1Galley, ref tbxFleet1Galley, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Galley_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Galley);
        }

        private void ckFleet1Galleon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1Galleon, ref tbxFleet1Galleon, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Galleon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Galleon);
        }

        private void ckFleet1Schooner_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1Schooner, ref tbxFleet1Schooner, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Schooner_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Schooner);
        }

        private void ckFleet1Brig_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1Brig, ref tbxFleet1Brig, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Brig_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Brig);
        }

        private void ckFleet1Frigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1Frigate, ref tbxFleet1Frigate, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Frigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Frigate);
        }

        private void ckFleet1GreatFrigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1GreatFrigate, ref tbxFleet1GreatFrigate, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1GreatFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1GreatFrigate);
        }

        private void ckFleet1ShipOfLine_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1ShipOfLine, ref tbxFleet1ShipOfLine, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1ShipOfLine_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1ShipOfLine);
        }

        private void ckFleet1Eastindiaman_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1Eastindiaman, ref tbxFleet1Eastindiaman, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Eastindiaman_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Eastindiaman);
        }

        private void ckFleet1ArmoredFrigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1ArmoredFrigate, ref tbxFleet1ArmoredFrigate, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1ArmoredFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1ArmoredFrigate);
        }

        private void ckFleet1AsianShip_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet1AsianShip, ref tbxFleet1AsianShip, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1AsianShip_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1AsianShip);
        }
        // dla armii 1
        private void ckArmy2Pikemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Pikemen, ref tbxArmy2Pikemen, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Pikemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Pikemen);
        }
        private void ckArmy2Arquebusiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Arquebusiers, ref tbxArmy2Arquebusiers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Arquebusiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Arquebusiers);
        }

        private void ckArmy2Archers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Archers, ref tbxArmy2Archers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Archers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Archers);
        }

        private void ckArmy2Crossbowmen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Crossbowmen, ref tbxArmy2Crossbowmen, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Crossbowmen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Crossbowmen);
        }

        private void ckArmy2Knights_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Knights, ref tbxArmy2Knights, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Knights_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Knights);
        }
        private void ckArmy2Horsemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Horsemen, ref tbxArmy2Horsemen, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Horsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Horsemen);
        }

        private void ckArmy2Bombard_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Bombard, ref tbxArmy2Bombard, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Bombard_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Bombard);
        }

        private void ckArmy2PikeShotArq_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2PikeShotArq, ref tbxArmy2PikeShotArq, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2PikeShotArq_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2PikeShotArq);
        }

        private void ckArmy2HeavyHussars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2HeavyHussars, ref tbxArmy2HeavyHussars, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2HeavyHussars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2HeavyHussars);
        }

        private void ckArmy2Cossacks_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Cossacks, ref tbxArmy2Cossacks, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Cossacks_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Cossacks);
        }

        private void ckArmy2Reiters_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Reiters, ref tbxArmy2Reiters, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Reiters_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Reiters);
        }
        private void ckArmy2FieldCannon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2FieldCannon, ref tbxArmy2FieldCannon, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2FieldCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2FieldCannon);
        }

        private void ckArmy2HeavyCannon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2HeavyCannon, ref tbxArmy2HeavyCannon, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2HeavyCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2HeavyCannon);
        }

        private void ckArmy2PikeShotMusk_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2PikeShotMusk, ref tbxArmy2PikeShotMusk, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2PikeShotMusk_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2PikeShotMusk);
        }

        private void ckArmy2EarlyFusiliers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2EarlyFusiliers, ref tbxArmy2EarlyFusiliers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2EarlyFusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2EarlyFusiliers);
        }

        private void ckArmy2EarlyCuirassier_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2EarlyCuirassier, ref tbxArmy2EarlyCuirassier, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2EarlyCuirassier_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2EarlyCuirassier);
        }

        private void ckArmy2Harquebusers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Harquebusers, ref tbxArmy2Harquebusers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Harquebusers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Harquebusers);
        }

        private void ckArmy2Lancers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Lancers, ref tbxArmy2Lancers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Lancers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Lancers);
        }

        private void ckArmy2SiegeHowitzer_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2SiegeHowitzer, ref tbxArmy2SiegeHowitzer, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2SiegeHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2SiegeHowitzer);
        }

        private void ckArmy2Fusiliers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Fusiliers, ref tbxArmy2Fusiliers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Fusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Fusiliers);
        }
        private void ckArmy2Grenadiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Grenadiers, ref tbxArmy2Grenadiers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Grenadiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Grenadiers);
        }

        private void ckArmy2Militia_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Militia, ref tbxArmy2Militia, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Militia_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Militia);
        }

        private void ckArmy2CarbineCav_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2CarbineCav, ref tbxArmy2CarbineCav, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2CarbineCav_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2CarbineCav);
        }

        private void ckArmy2Dragoons_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Dragoons, ref tbxArmy2Dragoons, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Dragoons_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Dragoons);
        }

        private void ckArmy2Hussars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Hussars, ref tbxArmy2Hussars, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Hussars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Hussars);
        }

        private void ckArmy2Cuiraissiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Cuiraissiers, ref tbxArmy2Cuiraissiers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Cuiraissiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Cuiraissiers);
        }

        private void ckArmy2FieldGun_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2FieldGun, ref tbxArmy2FieldGun, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2FieldGun_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2FieldGun);
        }

        private void ckArmy2Mortars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2Mortars, ref tbxArmy2Mortars, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Mortars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Mortars);
        }

        private void ckArmy2LightInfantry_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2LightInfantry, ref tbxArmy2LightInfantry, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2LightInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2LightInfantry);
        }

        private void ckArmy2LineInfantry_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2LineInfantry, ref tbxArmy2LineInfantry, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2LineInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2LineInfantry);
        }

        private void ckArmy2FieldHowitzer_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2FieldHowitzer, ref tbxArmy2FieldHowitzer, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2FieldHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2FieldHowitzer);
        }

        private void ckArmy2TribalWarriors_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2TribalWarriors, ref tbxArmy2TribalWarriors, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2TribalWarriors_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2TribalWarriors);
        }

        private void ckArmy2TribalRanger_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2TribalRanger, ref tbxArmy2TribalRanger, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2TribalRanger_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2TribalRanger);
        }

        private void ckArmy2TribalHorsemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2TribalHorsemen, ref tbxArmy2TribalHorsemen, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2TribalHorsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2TribalHorsemen);
        }
        private void ckArmy2HorseArcher_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckArmy2HorseArcher, ref tbxArmy2HorseArcher, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2HorseArcher_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2HorseArcher);
        }
        // flota 2
        private void ckFleet2Carrack_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2Carrack, ref tbxFleet2Carrack, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Carrack_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Carrack);
        }
        private void ckFleet2Caravel_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2Caravel, ref tbxFleet2Caravel, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Caravel_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Caravel);
        }
        private void ckFleet2Galley_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2Galley, ref tbxFleet2Galley, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Galley_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Galley);
        }

        private void ckFleet2Galleon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2Galleon, ref tbxFleet2Galleon, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Galleon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Galleon);
        }

        private void ckFleet2Schooner_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2Schooner, ref tbxFleet2Schooner, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Schooner_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Schooner);
        }

        private void ckFleet2Brig_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2Brig, ref tbxFleet2Brig, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Brig_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Brig);
        }

        private void ckFleet2Frigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2Frigate, ref tbxFleet2Frigate, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Frigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Frigate);
        }

        private void ckFleet2GreatFrigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2GreatFrigate, ref tbxFleet2GreatFrigate, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2GreatFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2GreatFrigate);
        }

        private void ckFleet2ShipOfLine_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2ShipOfLine, ref tbxFleet2ShipOfLine, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }
        private void tbxFleet2ShipOfLine_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2ShipOfLine);
        }

        private void tbxArmy1Pikemen_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Pikemen);
        }

        private void tbxArmy1Pikemen_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Pikemen);
        }

        private void tbxArmy1Arquebusiers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Arquebusiers);
        }

        private void tbxArmy1Arquebusiers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Arquebusiers);
        }

        private void tbxArmy1Archers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Archers);
        }

        private void tbxArmy1Archers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Archers);
        }

        private void tbxArmy1Crossbowmen_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Crossbowmen);
        }

        private void tbxArmy1Crossbowmen_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Crossbowmen);
        }

        private void tbxArmy1Knights_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Knights);
        }

        private void tbxArmy1Knights_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Knights);
        }

        private void tbxArmy1Horsemen_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Horsemen);
        }

        private void tbxArmy1Horsemen_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Horsemen);
        }

        private void tbxArmy1Bombard_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Bombard);
        }

        private void tbxArmy1Bombard_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Bombard);
        }

        private void tbxArmy1PikeShotArq_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1PikeShotArq);
        }

        private void tbxArmy1PikeShotArq_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1PikeShotArq);
        }

        private void tbxArmy1HeavyHussars_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1HeavyHussars);
        }

        private void tbxArmy1HeavyHussars_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1HeavyHussars);
        }

        private void tbxArmy1Reiters_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Reiters);
        }

        private void tbxArmy1Reiters_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Reiters);
        }

        private void tbxArmy1FieldCannon_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1FieldCannon);
        }

        private void tbxArmy1FieldCannon_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1FieldCannon);
        }

        private void tbxArmy1HeavyCannon_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1HeavyCannon);
        }

        private void tbxArmy1HeavyCannon_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1HeavyCannon);
        }

        private void tbxArmy1PikeShotMusk_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1PikeShotMusk);
        }

        private void tbxArmy1PikeShotMusk_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1PikeShotMusk);
        }

        private void tbxArmy1EarlyFusiliers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1EarlyFusiliers);
        }

        private void tbxArmy1EarlyFusiliers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1EarlyFusiliers);
        }

        private void tbxArmy1EarlyCuirassier_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1EarlyCuirassier);
        }

        private void tbxArmy1EarlyCuirassier_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1EarlyCuirassier);
        }

        private void tbxArmy1Harquebusers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Harquebusers);
        }

        private void tbxArmy1Harquebusers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Harquebusers);
        }

        private void tbxArmy1Lancers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Lancers);
        }

        private void tbxArmy1Lancers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Lancers);
        }

        private void tbxArmy1SiegeHowitzer_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1SiegeHowitzer);
        }

        private void tbxArmy1SiegeHowitzer_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1SiegeHowitzer);
        }

        private void tbxArmy1Fusiliers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Fusiliers);
        }

        private void tbxArmy1Fusiliers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Fusiliers);
        }

        private void tbxArmy1Grenadiers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Grenadiers);
        }

        private void tbxArmy1Grenadiers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Grenadiers);
        }

        private void tbxArmy1Militia_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Militia);
        }

        private void tbxArmy1Militia_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Militia);
        }

        private void tbxArmy1CarbineCav_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1CarbineCav);
        }

        private void tbxArmy1CarbineCav_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1CarbineCav);
        }

        private void tbxArmy1Dragoons_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Dragoons);
        }

        private void tbxArmy1Dragoons_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Dragoons);
        }

        private void tbxArmy1Hussars_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Hussars);
        }

        private void tbxArmy1Hussars_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Hussars);
        }

        private void tbxArmy1Cuiraissiers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Cuiraissiers);
        }

        private void tbxArmy1Cuiraissiers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Cuiraissiers);
        }

        private void tbxArmy1FieldGun_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1FieldGun);
        }

        private void tbxArmy1FieldGun_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1FieldGun);
        }

        private void tbxArmy1Mortars_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Mortars);
        }

        private void tbxArmy1Mortars_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Mortars);
        }

        private void tbxArmy1LightInfantry_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1LightInfantry);
        }

        private void tbxArmy1LightInfantry_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1LightInfantry);
        }

        private void tbxArmy1LineInfantry_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1LineInfantry);
        }

        private void tbxArmy1LineInfantry_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1LineInfantry);
        }

        private void tbxArmy1FieldHowitzer_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1FieldHowitzer);
        }

        private void tbxArmy1FieldHowitzer_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1FieldHowitzer);
        }

        private void tbxArmy1Cossacks_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1Cossacks);
        }

        private void tbxArmy1Cossacks_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1Cossacks);
        }

        private void tbxArmy1HorseArcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1HorseArcher);
        }

        private void tbxArmy1HorseArcher_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1HorseArcher);
        }

        private void tbxArmy1TribalWarriors_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1TribalWarriors);
        }

        private void tbxArmy1TribalWarriors_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1TribalWarriors);
        }

        private void tbxArmy1TribalRanger_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1TribalRanger);
        }

        private void tbxArmy1TribalRanger_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1TribalRanger);
        }

        private void tbxArmy1TribalHorsemen_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1TribalHorsemen);
        }

        private void tbxArmy1TribalHorsemen_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1TribalHorsemen);
        }

        private void tbxFleet1Carrack_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1Carrack);
        }

        private void tbxFleet1Carrack_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Carrack);
        }

        private void tbxFleet1Caravel_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1Caravel);
        }

        private void tbxFleet1Caravel_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Caravel);
        }

        private void tbxFleet1Galley_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1Galley);
        }

        private void tbxFleet1Galley_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Galley);
        }

        private void tbxFleet1Galleon_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1Galleon);
        }

        private void tbxFleet1Galleon_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Galleon);
        }

        private void tbxFleet1Schooner_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1Schooner);
        }

        private void tbxFleet1Schooner_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Schooner);
        }

        private void tbxFleet1Brig_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1Brig);
        }

        private void tbxFleet1Brig_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Brig);
        }

        private void tbxFleet1Frigate_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1Frigate);
        }

        private void tbxFleet1Frigate_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Frigate);
        }

        private void tbxFleet1GreatFrigate_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1GreatFrigate);
        }

        private void tbxFleet1GreatFrigate_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1GreatFrigate);
        }

        private void tbxFleet1ShipOfLine_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1ShipOfLine);
        }

        private void tbxFleet1ShipOfLine_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1ShipOfLine);
        }

        private void tbxFleet1Eastindiaman_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1Eastindiaman);
        }

        private void tbxFleet1Eastindiaman_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1Eastindiaman);
        }

        private void tbxFleet1ArmoredFrigate_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1ArmoredFrigate);
        }

        private void tbxFleet1ArmoredFrigate_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1ArmoredFrigate);
        }

        private void tbxFleet1AsianShip_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet1AsianShip);
        }

        private void tbxFleet1AsianShip_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet1AsianShip);
        }

        private void tbxArmy2Pikemen_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Pikemen);
        }

        private void tbxArmy2Pikemen_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Pikemen);
        }

        private void tbxArmy2Arquebusiers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Arquebusiers);
        }

        private void tbxArmy2Arquebusiers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Arquebusiers);
        }

        private void tbxArmy2Archers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Archers);
        }

        private void tbxArmy2Archers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Archers);
        }

        private void tbxArmy2Crossbowmen_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Crossbowmen);
        }

        private void tbxArmy2Crossbowmen_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Crossbowmen);
        }

        private void tbxArmy2Knights_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Knights);
        }

        private void tbxArmy2Knights_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Knights);
        }

        private void tbxArmy2Horsemen_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Horsemen);
        }

        private void tbxArmy2Horsemen_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Horsemen);
        }

        private void tbxArmy2Bombard_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Bombard);
        }

        private void tbxArmy2Bombard_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Bombard);
        }

        private void tbxArmy2PikeShotArq_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2PikeShotArq);
        }

        private void tbxArmy2PikeShotArq_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2PikeShotArq);
        }

        private void tbxArmy2HeavyHussars_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2HeavyHussars);
        }

        private void tbxArmy2HeavyHussars_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2HeavyHussars);
        }

        private void tbxArmy2Reiters_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Reiters);
        }

        private void tbxArmy2Reiters_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Reiters);
        }

        private void tbxArmy2FieldCannon_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2FieldCannon);
        }

        private void tbxArmy2FieldCannon_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2FieldCannon);
        }

        private void tbxArmy2HeavyCannon_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2HeavyCannon);
        }

        private void tbxArmy2HeavyCannon_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2HeavyCannon);
        }

        private void tbxArmy2PikeShotMusk_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2PikeShotMusk);
        }

        private void tbxArmy2PikeShotMusk_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2PikeShotMusk);
        }

        private void tbxArmy2EarlyFusiliers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2EarlyFusiliers);
        }

        private void tbxArmy2EarlyFusiliers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2EarlyFusiliers);
        }

        private void tbxArmy2EarlyCuirassier_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2EarlyCuirassier);
        }

        private void tbxArmy2EarlyCuirassier_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2EarlyCuirassier);
        }

        private void tbxArmy2Harquebusers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Harquebusers);
        }

        private void tbxArmy2Harquebusers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Harquebusers);
        }

        private void tbxArmy2Lancers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Lancers);
        }

        private void tbxArmy2Lancers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Lancers);
        }

        private void tbxArmy2SiegeHowitzer_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2SiegeHowitzer);
        }

        private void tbxArmy2SiegeHowitzer_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2SiegeHowitzer);
        }

        private void tbxArmy2Fusiliers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Fusiliers);
        }

        private void tbxArmy2Fusiliers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Fusiliers);
        }

        private void tbxArmy2Grenadiers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Grenadiers);
        }

        private void tbxArmy2Grenadiers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Grenadiers);
        }

        private void tbxArmy2Militia_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Militia);
        }

        private void tbxArmy2Militia_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Militia);
        }

        private void tbxArmy2CarbineCav_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2CarbineCav);
        }

        private void tbxArmy2CarbineCav_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2CarbineCav);
        }

        private void tbxArmy2Dragoons_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Dragoons);
        }

        private void tbxArmy2Dragoons_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Dragoons);
        }

        private void tbxArmy2Hussars_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Hussars);
        }

        private void tbxArmy2Hussars_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Hussars);
        }

        private void tbxArmy2Cuiraissiers_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Cuiraissiers);
        }

        private void tbxArmy2Cuiraissiers_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Cuiraissiers);
        }

        private void tbxArmy2FieldGun_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2FieldGun);
        }

        private void tbxArmy2FieldGun_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2FieldGun);
        }

        private void tbxArmy2Mortars_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Mortars);
        }

        private void tbxArmy2Mortars_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Mortars);
        }

        private void tbxArmy2LightInfantry_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2LightInfantry);
        }

        private void tbxArmy2LightInfantry_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2LightInfantry);
        }

        private void tbxArmy2LineInfantry_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2LineInfantry);
        }

        private void tbxArmy2LineInfantry_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2LineInfantry);
        }

        private void tbxArmy2FieldHowitzer_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy1FieldHowitzer);
        }

        private void tbxArmy2FieldHowitzer_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy1FieldHowitzer);
        }

        private void tbxArmy2Cossacks_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2Cossacks);
        }

        private void tbxArmy2Cossacks_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2Cossacks);
        }

        private void tbxArmy2HorseArcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2HorseArcher);
        }

        private void tbxArmy2HorseArcher_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2HorseArcher);
        }

        private void tbxArmy2TribalWarriors_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2TribalWarriors);
        }

        private void tbxArmy2TribalWarriors_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2TribalWarriors);
        }

        private void tbxArmy2TribalRanger_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2TribalRanger);
        }

        private void tbxArmy2TribalRanger_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2TribalRanger);
        }

        private void tbxArmy2TribalHorsemen_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxArmy2TribalHorsemen);
        }

        private void tbxArmy2TribalHorsemen_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxArmy2TribalHorsemen);
        }

        private void tbxFleet2Carrack_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2Carrack);
        }

        private void tbxFleet2Carrack_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Carrack);
        }

        private void tbxFleet2Caravel_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2Caravel);
        }

        private void tbxFleet2Caravel_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Caravel);
        }

        private void tbxFleet2Galley_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2Galley);
        }

        private void tbxFleet2Galley_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Galley);
        }

        private void tbxFleet2Galleon_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2Galleon);
        }

        private void tbxFleet2Galleon_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Galleon);
        }

        private void tbxFleet2Schooner_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2Schooner);
        }

        private void tbxFleet2Schooner_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Schooner);
        }

        private void tbxFleet2Brig_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2Brig);
        }

        private void tbxFleet2Brig_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Brig);
        }

        private void tbxFleet2Frigate_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2Frigate);
        }

        private void tbxFleet2Frigate_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Frigate);
        }

        private void tbxFleet2GreatFrigate_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2GreatFrigate);
        }

        private void tbxFleet2GreatFrigate_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2GreatFrigate);
        }

        private void tbxFleet2ShipOfLine_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2ShipOfLine);
        }

        private void tbxFleet2ShipOfLine_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2ShipOfLine);
        }

        private void tbxFleet2Eastindiaman_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2Eastindiaman);
        }

        private void tbxFleet2Eastindiaman_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Eastindiaman);
        }

        private void tbxFleet2ArmoredFrigate_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2ArmoredFrigate);
        }

        private void tbxFleet2ArmoredFrigate_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2ArmoredFrigate);
        }

        private void tbxFleet2AsianShip_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputNumberWhile(ref tbxFleet2AsianShip);
        }

        private void tbxFleet2AsianShip_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2AsianShip);
        }

        private void ckFleet2Eastindiaman_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2Eastindiaman, ref tbxFleet2Eastindiaman, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        

        private void tbxFleet2Eastindiaman_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2Eastindiaman);
        }

        private void ckFleet2ArmoredFrigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2ArmoredFrigate, ref tbxFleet2ArmoredFrigate, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2ArmoredFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2ArmoredFrigate);
        }

        private void ckFleet2AsianShip_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ref ckFleet2AsianShip, ref tbxFleet2AsianShip, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2AsianShip_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyInputNumberAfter(ref tbxFleet2AsianShip);
        }
        // wszystko po kliknieciu start
        public void ApplyTerrainEffects(ref List<LandUnit> armyList)
        {
            foreach (LandUnit unit in armyList)
            {
                unit.Speed = unit.Speed - terrain.Mud - terrain.HighDiff;
                if (unit.Type == "SiegeArtillery" && unit.Speed < 1)
                {
                    unit.Speed = 1;
                }
                else if (unit.Speed < 2)
                {
                    unit.Speed = 2;
                }
                unit.ArtilleryDef += terrain.Concealment;
                if(unit.MediumRange > 0)
                {
                    unit.MediumRange -= terrain.HighDiff;
                    if (unit.MediumRange < 0) unit.MediumRange = 5;
                }
                if (unit.LongRange > 0)
                {
                    unit.LongRange -= terrain.HighDiff;
                    if (unit.LongRange < 0) unit.LongRange = 4;
                }
                if(unit.LowRange > 0)
                {
                    unit.LowRange -= terrain.Obstacles;
                    if (unit.LowRange < 0) unit.LowRange = 6;
                }
            }
        }
        public void FillArmyList(List<CheckBox> ckList, List<TextBox> tbxList, ref List<LandUnit> armyList)
        {
            armyList.Clear();
            string? unitName;
            int numOfUnits;
            for(int i = 0; i < ckList.Count; i++)
            {
                if (ckList[i].IsChecked == false) 
                {
                    Debug.WriteLine("Unchecked checkbox when filling armyList, skipping");
                    continue;
                }
                unitName = Convert.ToString(ckList[i].Content);
                numOfUnits = Convert.ToInt32(tbxList[i].Text.Trim());
                if(numOfUnits <= 0)
                {
                    Debug.WriteLine("Empty unit cought during filling, skipping");
                    continue;
                }
                switch (unitName)
                {
                    //jednostki startowe
                    case "Pikinierzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 5, 17, 19, 0, 4, 80, 100, 4, "MeleeInfantry", numOfUnits));
                        break;
                    case "Arkebuzerzy":
                        armyList.Add(new LandUnit(unitName, 0, 11, 21, 0, 8, 0, 0, 3, 90, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Łucznicy":
                        armyList.Add(new LandUnit(unitName, 0, 13, 15, 0, 8, 0, 0, 3, 65, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Kusznicy":
                        armyList.Add(new LandUnit(unitName, 0, 11, 17, 0, 8, 0, 5, 3, 70, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Rycerze":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 25, 23, 5, 5, 6, 105, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Konnica":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 15, 19, 5, 5, 6, 75, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Bombarda":
                        armyList.Add(new LandUnit(unitName, 12, 10, 0, 0, 4, 0, 5, 1, 30, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka eksploracji
                    case "Piki i arkebuzerzy":
                        armyList.Add(new LandUnit(unitName, 0, 11, 21, 5, 17, 19, 0, 4, 90, 100, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Ciężcy husarze":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 30, 25, 5, 10, 5, 105, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Reiterzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 14, 10, 18, 5, 10, 5, 105, 100, 6, "RangerCavalry", numOfUnits));
                        break;
                    case "Armata polowa":
                        armyList.Add(new LandUnit(unitName, 0, 22, 38, 0, 4, 0, 5, 2, 40, 100, 2, "FieldGuns", numOfUnits));
                        break;
                    case "Ciężka armata":
                        armyList.Add(new LandUnit(unitName, 20, 24, 12, 0, 4, 0, 5, 1, 40, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka ekspansji
                    case "Piki i muszkieterzy":
                        armyList.Add(new LandUnit(unitName, 0, 17, 28, 5, 17, 19, 0, 4, 90, 100, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Wcześni fusilierzy":
                        armyList.Add(new LandUnit(unitName, 0, 25, 34, 0, 13, 5, 5, 4, 70, 110, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Wcześni kirasjerzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 18, 26, 5, 5, 6, 95, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Harkebuzerzy":
                        armyList.Add(new LandUnit(unitName, 0, 12, 23, 10, 18, 5, 5, 6, 80, 100, 6, "RangerCavalry", numOfUnits));
                        break;
                    case "Lansjerzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 23, 26, 5, 5, 6, 80, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Haubica oblężnicza":
                        armyList.Add(new LandUnit(unitName, 28, 34, 14, 0, 4, 0, 5, 1, 40, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka imperiow
                    case "Fusilierzy":
                        armyList.Add(new LandUnit(unitName, 0, 28, 36, 13, 20, 14, 10, 4, 90, 100, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Grenadierzy":
                        armyList.Add(new LandUnit(unitName, 0, 28, 44, 14, 26, 17, 10, 4, 95, 110, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Milicja":
                        armyList.Add(new LandUnit(unitName, 0, 25, 32, 7, 18, 5, 9, 4, 75, 80, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Karbinerzy":
                        armyList.Add(new LandUnit(unitName, 0, 24, 30, 10, 19, 5, 5, 6, 80, 100, 6, "RangerCavalry", numOfUnits));
                        break;
                    case "Dragoni":
                        armyList.Add(new LandUnit(unitName, 0, 28, 36, 11, 20, 5, 8, 4, 80, 100, 6, "LineInfantry", numOfUnits));
                        break;
                    case "Huzarzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 17, 27, 5, 8, 6, 80, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Kirasjerzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 18, 29, 5, 8, 6, 100, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Działa polowe":
                        armyList.Add(new LandUnit(unitName, 0, 34, 51, 0, 6, 0, 6, 2, 40, 100, 2, "FieldGuns", numOfUnits));
                        break;
                    case "Moździerze":
                        armyList.Add(new LandUnit(unitName, 35, 32, 0, 0, 4, 0, 6, 1, 40, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka rewolucji
                    case "Lekka piechota":
                        armyList.Add(new LandUnit(unitName, 0, 30, 39, 10, 22, 11, 14, 4, 80, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Piechota liniowa":
                        armyList.Add(new LandUnit(unitName, 0, 31, 42, 13, 22, 16, 10, 4, 90, 100, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Haubice polowe":
                        armyList.Add(new LandUnit(unitName, 23, 42, 54, 0, 6, 0, 7, 2, 40, 100, 2, "FieldGuns", numOfUnits));
                        break;
                    //jednostki unikalne
                    case "Kozacy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 10, 15, 25, 3, 5, 6, 70, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Tubylcy wojownicy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 5, 10, 0, 0, 4, 60, 80, 4, "MeleeInfantry", numOfUnits));
                        break;
                    case "Tubylcy strzelcy":
                        armyList.Add(new LandUnit(unitName, 0, 5, 13, 0, 9, 0, 0, 4, 55, 80, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Konnica tubylców":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 12, 13, 0, 5, 6, 65, 90, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Łucznicy konni":
                        armyList.Add(new LandUnit(unitName, 0, 13, 15, 5, 18, 0, 5, 4, 80, 100, 6, "RangerCavalry", numOfUnits));
                        break;
                }
            }
        }
        public void SelectAttackingUnits(ref List<LandUnit> mainArmyList, ref List<LandUnit> attackingUnits)
        {
            attackingUnits.Clear();
            mainArmyList.Sort((x, y) => y.Initiative.CompareTo(x.Initiative));
            List<LandUnit> copyOfMainList = mainArmyList.ToList();
            if (GetArmyCount(mainArmyList) <= 0) return;
            int maxUnits = GetArmyCount(mainArmyList) / 6;
            int portionOfArmy = 6;
            while (maxUnits <= 0 && portionOfArmy > 3)
            {
                portionOfArmy--;
                maxUnits = GetArmyCount(mainArmyList) / portionOfArmy;
            }
            if (maxUnits <= 0)
            {
                foreach (LandUnit unit in mainArmyList)
                {
                    unit.Initiative--;
                    if (unit.Initiative < 1) unit.Initiative = 1;
                }
                return;
            }
            foreach(LandUnit unit in mainArmyList)
            {
                if(unit.Type != "SiegeArtillery" &&  unit.Type != "FieldGuns" && unit.NumberOf > 0 && unit.Initiative > 0)
                {
                    attackingUnits.Add(unit);
                }
                if(GetArmyCount(attackingUnits) > maxUnits)
                {
                    int numOfSplitOffUnits = GetArmyCount(attackingUnits) - maxUnits;
                    attackingUnits[attackingUnits.Count - 1].NumberOf -= numOfSplitOffUnits;
                    if(attackingUnits[attackingUnits.Count - 1].NumberOf <= 0) attackingUnits.RemoveAt(attackingUnits.Count - 1);
                    if(numOfSplitOffUnits > 0)
                    {
                        LandUnit copyOfTheLastLandunit = new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.MaxHealth, unit.Morale, unit.Speed, unit.Type, numOfSplitOffUnits);
                        copyOfMainList.Add(copyOfTheLastLandunit);
                        break;
                    }
                    else
                    {
                        Debug.WriteLine("Select attacking units, oddzielenie nadmiaru nie dziala prawidlowo");
                    }
                }
            }
            mainArmyList = copyOfMainList.ToList();
        }
        public void SelectDefendingUnits(ref List<LandUnit> mainArmyList, ref List<LandUnit> defendingUnits)
        {
            defendingUnits.Clear();
            mainArmyList.Sort((x, y) => y.Initiative.CompareTo(x.Initiative));
            List<LandUnit> copyOfMainList = mainArmyList.ToList();
            if (GetArmyCount(mainArmyList) <= 0) return;
            int maxUnits = GetArmyCount(mainArmyList) / 6;
            int portionOfArmy = 6;
            while (maxUnits <= 0 && portionOfArmy > 3)
            {
                portionOfArmy--;
                maxUnits = GetArmyCount(mainArmyList) / portionOfArmy;
            }
            if (maxUnits <= 0)
            {
                foreach (LandUnit unit in mainArmyList)
                {
                    unit.Initiative--;
                    if (unit.Initiative < 1) unit.Initiative = 1;
                }
                return;
            }
            foreach (LandUnit unit in mainArmyList)
            {
                if (unit.Type != "ChargeCavalry" && unit.Type != "RangerCavalry" && unit.NumberOf > 0 && unit.Initiative > 0)
                {
                    defendingUnits.Add(unit);
                }
                if (GetArmyCount(defendingUnits) > maxUnits)
                {
                    int numOfSplitOffUnits = GetArmyCount(defendingUnits) - maxUnits;
                    defendingUnits[defendingUnits.Count - 1].NumberOf -= numOfSplitOffUnits;
                    if (defendingUnits[defendingUnits.Count - 1].NumberOf <= 0) defendingUnits.RemoveAt(defendingUnits.Count - 1);
                    if (numOfSplitOffUnits > 0)
                    {
                        LandUnit copyOfTheLastLandunit = new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.MaxHealth, unit.Morale, unit.Speed, unit.Type, numOfSplitOffUnits);
                        copyOfMainList.Add(copyOfTheLastLandunit);
                        break;
                    }
                    else
                    {
                        Debug.WriteLine("Select defending units, oddzielenie nadmiaru nie dziala prawidlowo");

                    }
                    
                }
            }
            List<LandUnit> artillery = new List<LandUnit>();
            SelectBombardingUnits(ref defendingUnits, ref artillery, true);
            if(GetArmyCount(artillery) > 0)
            {
                SelectBombardingUnits(ref copyOfMainList, ref artillery, true);
                if(GetArmyCount(artillery) > 0)
                {
                    int numOfLastUnit = defendingUnits[defendingUnits.Count - 1].NumberOf;
                    if(numOfLastUnit > 5) numOfLastUnit = 5;
                    defendingUnits[defendingUnits.Count - 1].NumberOf -= numOfLastUnit;
                    if (defendingUnits[defendingUnits.Count - 1].NumberOf <= 0) defendingUnits.RemoveAt(defendingUnits.Count - 1);
                    if (numOfLastUnit > 0)
                    {
                        LandUnit copyOfTheLastLandunit = new LandUnit(defendingUnits[defendingUnits.Count - 1].Name, defendingUnits[defendingUnits.Count - 1].LongRange, defendingUnits[defendingUnits.Count - 1].MediumRange, defendingUnits[defendingUnits.Count - 1].LowRange, defendingUnits[defendingUnits.Count - 1].ShockAttack, defendingUnits[defendingUnits.Count - 1].Melee, defendingUnits[defendingUnits.Count - 1].ShockDef, defendingUnits[defendingUnits.Count - 1].ArtilleryDef, defendingUnits[defendingUnits.Count - 1].Initiative, defendingUnits[defendingUnits.Count - 1].MaxHealth, defendingUnits[defendingUnits.Count - 1].Morale, defendingUnits[defendingUnits.Count - 1].Speed, defendingUnits[defendingUnits.Count - 1].Type, numOfLastUnit);
                        copyOfMainList.Add(copyOfTheLastLandunit);
                    }
                    foreach (LandUnit unit in artillery)
                    {
                        if (unit.NumberOf > 0 && unit.Initiative > 0)
                        {
                            defendingUnits.Add(unit);
                        }
                        if (GetArmyCount(defendingUnits) > maxUnits)
                        {
                            int numOfSplitOffUnits = GetArmyCount(defendingUnits) - maxUnits;
                            defendingUnits[defendingUnits.Count - 1].NumberOf -= numOfSplitOffUnits;
                            if (defendingUnits[defendingUnits.Count - 1].NumberOf <= 0) defendingUnits.RemoveAt(defendingUnits.Count - 1);
                            if (numOfSplitOffUnits > 0)
                            {
                                LandUnit copyOfTheLastLandunit = new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.MaxHealth, unit.Morale, unit.Speed, unit.Type, numOfSplitOffUnits);
                                copyOfMainList.Add(copyOfTheLastLandunit);
                                break;
                            }
                            else
                            {
                                Debug.WriteLine("Select defending units, artillery part, oddzielenie nadmiaru nie dziala prawidlowo");

                            }

                        }
                    }
                }
            }
            if(GetArmyCount(defendingUnits) < maxUnits)
            {
                //wybierzemy tez kawalerie w takim przypadku
                foreach (LandUnit unit in mainArmyList)
                {
                    if ((unit.Type == "ChargeCavalry" || unit.Type == "RangerCavalry") && unit.NumberOf > 0 && unit.Initiative > 0)
                    {
                        defendingUnits.Add(unit);
                    }
                    if (GetArmyCount(defendingUnits) > maxUnits)
                    {
                        int numOfSplitOffUnits = GetArmyCount(defendingUnits) - maxUnits;
                        defendingUnits[defendingUnits.Count - 1].NumberOf -= numOfSplitOffUnits;
                        if (defendingUnits[defendingUnits.Count - 1].NumberOf <= 0) defendingUnits.RemoveAt(defendingUnits.Count - 1);
                        if (numOfSplitOffUnits > 0)
                        {
                            LandUnit copyOfTheLastLandunit = new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.MaxHealth, unit.Morale, unit.Speed, unit.Type, numOfSplitOffUnits);
                            copyOfMainList.Add(copyOfTheLastLandunit);
                            break;
                        }
                        else
                        {
                            Debug.WriteLine("Select defending units, cavalry, oddzielenie nadmiaru nie dziala prawidlowo");

                        }

                    }
                }
            }
            mainArmyList = copyOfMainList;
        }
        public void SelectBombardingUnits(ref List<LandUnit> mainArmyList, ref List<LandUnit> bombardingUnits, bool allowFieldGuns = false)
        {
            bombardingUnits.Clear();
            foreach (LandUnit unit in  mainArmyList)
            {
                if (unit.Type == "SiegeArtillery")
                {
                    bombardingUnits.Add(unit);
                }
                else if(unit.Type == "FieldGuns" && allowFieldGuns)
                {
                    bombardingUnits.Add(unit);
                }
            }
        }
        public void MergeLandUnits(ref List<LandUnit> armyList)
        {
            for (int i = 0; i < armyList.Count; i++)
            {
                for(int j = i + 1; j < armyList.Count; j++) 
                {
                    if (armyList[i].Name == armyList[j].Name && j != i && armyList[j].NumberOf > 0)
                    {
                        armyList[i].NumberOf += armyList[j].NumberOf;
                        armyList[j].NumberOf = 0;
                        armyList.RemoveAt(j);
                        j--;

                    }
                    else if(armyList[i].Name == armyList[j].Name && j != i && armyList[j].NumberOf <= 0)
                    {
                        armyList.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        // mozliwe opcje dla typu damage'u dla ponizszej funkcji
        public enum TypeOfDamage
        {
            LongRange,
            LongRangeRet,
            MidRange,
            MidRangeRet,
            LowRangeRet,
            LowRange,
            ChargeMelee,
            Melee,
            MeleeRet
        }
        private int GetCountForDealDamage(List<LandUnit> armyList)
        {
            int count = 0;
            foreach(LandUnit unit in armyList)
            {
                if (unit.NumberOf > 0) count++;
            }
            return count;
        }
        public void DealDamageToLandunits(ref List<LandUnit> attackerList, ref List<LandUnit> defenderList, TypeOfDamage damageType, bool isRiverBeingCrossed = false)
        {
            
            int attackerDamage = 0;
            int defenderDamage = 0;
            if(damageType == TypeOfDamage.LongRange)
            {
                //obliczanie damage
                foreach(LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0) attackerDamage += unit.LongRange;
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //zadanie obrazen
                foreach(LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - unit.ArtilleryDef);
                    unit.Morale -= attackerDamage - unit.ArtilleryDef;
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
            else if(damageType == TypeOfDamage.LongRangeRet)
            {
                //obliczanie damage
                foreach (LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0) attackerDamage += unit.LongRange;
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                attackerDamage /= 2;
                
                //zadanie obrazen
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - unit.ArtilleryDef);
                    unit.Morale -= attackerDamage - unit.ArtilleryDef;
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
                //obliczanie obrazen obroncow
                foreach (LandUnit unit in defenderList)
                {
                    if (unit.NumberOf > 0) defenderDamage += unit.LongRange;
                }
                if (GetCountForDealDamage(attackerList) > 0) attackerDamage /= GetCountForDealDamage(attackerList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //kontraatak
                foreach (LandUnit unit in attackerList)
                {
                    unit.Health -= (defenderDamage - unit.ArtilleryDef);
                    unit.Morale -= defenderDamage - unit.ArtilleryDef;
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
                // 2 czesc ataku atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - unit.ArtilleryDef);
                    unit.Morale -= attackerDamage - unit.ArtilleryDef;
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
            else if(damageType == TypeOfDamage.MidRange)
            {
                //obliczanie damage
                foreach (LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0) attackerDamage += unit.MediumRange;
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //zadanie obrazen
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - unit.ArtilleryDef);
                    unit.Morale -= attackerDamage - unit.ArtilleryDef;
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
            else if(damageType == TypeOfDamage.MidRangeRet)
            {
                //obliczanie damage
                foreach (LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0) attackerDamage += unit.MediumRange;
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                attackerDamage /= 2;

                //zadanie obrazen
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - unit.ArtilleryDef);
                    unit.Morale -= attackerDamage - unit.ArtilleryDef;
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
                //obliczanie obrazen obroncow
                foreach (LandUnit unit in defenderList)
                {
                    if (unit.NumberOf > 0) defenderDamage += unit.MediumRange;
                }
                if (GetCountForDealDamage(attackerList) > 0) attackerDamage /= GetCountForDealDamage(attackerList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //kontraatak
                foreach (LandUnit unit in attackerList)
                {
                    unit.Health -= (defenderDamage - unit.ArtilleryDef);
                    unit.Morale -= defenderDamage - unit.ArtilleryDef;
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
                // 2 czesc ataku atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - unit.ArtilleryDef);
                    unit.Morale -= attackerDamage - unit.ArtilleryDef;
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
            else if(damageType == TypeOfDamage.LowRange)
            {
                //obliczanie damage
                foreach (LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0) attackerDamage += unit.LowRange;
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //zadanie obrazen
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - (unit.ArtilleryDef / 2));
                    unit.Morale -= attackerDamage - (unit.ArtilleryDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
            else if(damageType == TypeOfDamage.LowRangeRet)
            {
                //obliczanie damage
                foreach (LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0) attackerDamage += unit.LowRange;
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                attackerDamage /= 2;

                //zadanie obrazen
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - (unit.ArtilleryDef / 2));
                    unit.Morale -= attackerDamage - (unit.ArtilleryDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
                //obliczanie obrazen obroncow
                foreach (LandUnit unit in defenderList)
                {
                    if (unit.NumberOf > 0) defenderDamage += unit.LowRange;
                }
                if (GetCountForDealDamage(attackerList) > 0) attackerDamage /= GetCountForDealDamage(attackerList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //kontraatak
                foreach (LandUnit unit in attackerList)
                {
                    unit.Health -= (defenderDamage - (unit.ArtilleryDef / 2));
                    unit.Morale -= defenderDamage - (unit.ArtilleryDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
                // 2 czesc ataku atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - (unit.ArtilleryDef / 2));
                    unit.Morale -= attackerDamage - (unit.ArtilleryDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
            else if(damageType == TypeOfDamage.Melee)
            {
                //obliczanie damage
                foreach (LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0) attackerDamage += unit.Melee;
                    if (unit.NumberOf > 0 && unit.ShockAttack > 0) attackerDamage += unit.ShockAttack / 2;
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //zadanie obrazen
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - (unit.ShockDef / 2));
                    unit.Morale -= attackerDamage - (unit.ShockDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
            else if(damageType == TypeOfDamage.MeleeRet)
            {
                //obliczanie damage
                foreach (LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0) attackerDamage += unit.Melee;
                    if (unit.NumberOf > 0 && unit.ShockAttack > 0) attackerDamage += unit.ShockAttack / 2;
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                foreach (LandUnit unit in defenderList)
                {
                    if (unit.NumberOf > 0) defenderDamage += unit.Melee;
                    if (unit.NumberOf > 0 && unit.ShockAttack > 0) defenderDamage += unit.ShockAttack / 2;
                }
                if (GetCountForDealDamage(attackerList) > 0) defenderDamage /= GetCountForDealDamage(attackerList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //zadanie obrazen
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - (unit.ShockDef / 2));
                    unit.Morale -= attackerDamage - (unit.ShockDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
                foreach (LandUnit unit in attackerList)
                {
                    unit.Health -= (attackerDamage - (unit.ShockDef / 2));
                    unit.Morale -= attackerDamage - (unit.ShockDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
            else if(damageType == TypeOfDamage.ChargeMelee)
            {
                
                //czy obroncy maja kawalerie szarszujaca
                bool defendersCanCharge = false;
                foreach (LandUnit unit in defenderList)
                {
                    if (unit.NumberOf > 0 && unit.Type == "ChargeCavalry")
                    {
                        defendersCanCharge = true;
                        break;
                    }
                }
                if (defendersCanCharge)
                {
                    //obliczanie damage
                    List<LandUnit> counterCharge = new List<LandUnit>();
                    foreach (LandUnit unit in defenderList)
                    {
                        if (unit.NumberOf > 0 && unit.Type == "ChargeCavalry")
                        {
                            counterCharge.Add(unit);
                            defenderDamage += unit.Melee + unit.ShockAttack;
                            if (unit.LowRange > 0) defenderDamage += unit.LowRange / 2;
                        }
                    }
                    if (GetCountForDealDamage(attackerList) > 0) defenderDamage /= GetCountForDealDamage(attackerList);
                    else
                    {
                        Debug.WriteLine("Deal damage, pusta armia!");
                        return;
                    }
                    foreach (LandUnit unit in defenderList)
                    {
                        if (unit.NumberOf > 0)
                        {
                            attackerDamage += unit.Melee + unit.ShockAttack;
                            if (unit.LowRange > 0) attackerDamage += unit.LowRange / 2;
                            if (isRiverBeingCrossed) attackerDamage -= unit.ShockAttack;
                        }
                    }
                    if (GetCountForDealDamage(counterCharge) > 0) attackerDamage /= GetCountForDealDamage(counterCharge);
                    else
                    {
                        Debug.WriteLine("Deal damage, pusta armia!");
                        return;
                    }
                    //zadawanie obrazen
                    foreach (LandUnit unit in counterCharge)
                    {
                        unit.Health -= (attackerDamage - unit.ShockDef);
                        unit.Morale -= attackerDamage - unit.ShockDef;
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.Morale -= 5;
                            unit.NumberOf--;
                        }
                    }
                    foreach (LandUnit unit in attackerList)
                    {
                        unit.Health -= (attackerDamage - unit.ShockDef);
                        unit.Morale -= attackerDamage - unit.ShockDef;
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.Morale -= 5;
                            unit.NumberOf--;
                        }
                    }
                }
                attackerDamage = 0;
                defenderDamage = 0;
                //obliczanie damage
                foreach (LandUnit unit in defenderList)
                {
                    if (unit.NumberOf > 0)
                    {
                        defenderDamage += unit.Melee + unit.ShockAttack;
                        if (unit.LowRange > 0) defenderDamage += unit.LowRange / 2;
                    }
                }
                if (GetCountForDealDamage(attackerList) > 0) defenderDamage /= GetCountForDealDamage(attackerList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                foreach (LandUnit unit in attackerList)
                {
                    if (unit.NumberOf > 0)
                    {
                        attackerDamage += unit.Melee + unit.ShockAttack;
                        if (unit.LowRange > 0) attackerDamage += unit.LowRange / 2;
                        if (isRiverBeingCrossed) attackerDamage -= unit.ShockAttack;
                    }
                }
                if (GetCountForDealDamage(defenderList) > 0) attackerDamage /= GetCountForDealDamage(defenderList);
                else
                {
                    Debug.WriteLine("Deal damage, pusta armia!");
                    return;
                }
                //zadawanie obrazen 
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= (attackerDamage - (unit.ShockDef / 2));
                    unit.Morale -= attackerDamage - (unit.ShockDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
                foreach (LandUnit unit in attackerList)
                {
                    unit.Health -= (attackerDamage - (unit.ShockDef / 2));
                    unit.Morale -= attackerDamage - (unit.ShockDef / 2);
                    while (unit.Health <= 0 && unit.NumberOf > 0)
                    {
                        unit.Health += unit.MaxHealth;
                        unit.Morale -= 5;
                        unit.NumberOf--;
                    }
                }
            }
        }
        public void RetreatInHaste(ref List<LandUnit> retreatingArmy, int portion = 2)
        {
            if (portion < 2) portion = 2;
            foreach(LandUnit unit in retreatingArmy)
            {
                Debug.WriteLine($"ReatreatInHaste:\nUnit health before {unit.Health}, numberof {unit.NumberOf}");
                unit.Health -= (unit.MaxHealth / 5) * unit.NumberOf;
                if (unit.Type == "SiegeArtillery") unit.Health -= (unit.MaxHealth / portion) * unit.NumberOf;
                else if (unit.Type == "FieldGuns") unit.Health -= (unit.MaxHealth / (portion + (portion / 2))) * unit.NumberOf;
                while (unit.Health <= 0 && unit.NumberOf > 0)
                {
                    unit.Health += unit.MaxHealth;
                    unit.Morale -= 5;
                    unit.NumberOf--;
                }
                if (unit.Morale <= 0) unit.Morale = 0;
                Debug.WriteLine($"Unit health after {unit.Health}, numberof {unit.NumberOf}");
            }
        }
        public void DestroyWeakUnits(ref List<LandUnit> armyList)
        {
            foreach (LandUnit unit in armyList)
            {
                if(unit.Health == unit.MaxHealth / 5 && unit.NumberOf > 0)
                {
                    unit.Health = unit.MaxHealth;
                    unit.NumberOf--;
                    unit.Morale -= 5;
                }
                if (unit.Morale <= 0) unit.Morale = 0;
            }
        }
        public void UpdateInitiative(ref List<LandUnit> armyList)
        {
            int percentPerPoint;
            foreach(LandUnit unit in armyList)
            {
                percentPerPoint = unit.MaxMorale / unit.MaxInitiative;
                unit.Initiative = unit.Morale / percentPerPoint;
            }
        }
        public void MoraleModification(ref List<LandUnit> armyList, int moraleModification, bool goBeyondLimit = false)
        {
            foreach (LandUnit unit in armyList)
            {
                unit.Morale += moraleModification;
                if (unit.Morale < 0 && !goBeyondLimit) unit.Morale = 0;
                if(unit.Morale > unit.MaxMorale && !goBeyondLimit) unit.Morale = unit.MaxMorale;
            }
            UpdateInitiative(ref armyList);
        }
        public int GetArmyMorale(List<LandUnit> armyList)
        {
            if (armyList.Count == 0) return 0;
            int avgMorale = 0;
            foreach (LandUnit unit in armyList)
            {
                avgMorale += unit.Morale;
            }
            avgMorale /= armyList.Count;
            return avgMorale;
        }
        public int GetArmySpeed(List<LandUnit> armyList)
        {
            if(armyList.Count == 0) return 0;
            int avgSpeed = 0;
            foreach(LandUnit unit in armyList)
            {
                avgSpeed += unit.Speed;
            }
            avgSpeed /= armyList.Count;
            return avgSpeed;
        }
        public int GetArmyInitiative(List<LandUnit> armyList)
        {
            if (armyList.Count == 0) return 0;
            int avgInitiative = 0;
            foreach(LandUnit unit in armyList)
            {
                avgInitiative += unit.Initiative;
            }
            avgInitiative /= armyList.Count;
            return avgInitiative;
        }
        public int GetArmyCount(List<LandUnit> armyList)
        {
            if (armyList.Count == 0) return 0;
            int count = 0;
            foreach(LandUnit unit in armyList)
            {
                count += unit.NumberOf;
            }
            return count;
        }
        void FillFleetList(List<CheckBox> ckList, List<TextBox> tbxList, ref List<Ship> fleetList)
        {
            fleetList.Clear();
            string? unitName;
            int numOfUnits;
            for (int i = 0; i < ckList.Count; i++)
            {
                unitName = Convert.ToString(ckList[i].Content);
                numOfUnits = Convert.ToInt32(tbxList[i].Text);
                switch (unitName)
                {
                    //startowe
                    case "Galera":
                        fleet1UnitsList.Add(new Ship(unitName, 1, 1, 100, 80, 5, 7, 5, 45, 0, 0, numOfUnits));
                        break;
                    case "Karak":
                        fleet1UnitsList.Add(new Ship(unitName, 2, 4, 140, 120, 3, 3, 25, 0, 0, 0, numOfUnits));
                        break;
                    case "Karawela":
                        fleet1UnitsList.Add(new Ship(unitName, 2, 3, 110, 100, 5, 4, 15, 5, 0, 0, numOfUnits));
                        break;
                    //epoka eksploracji
                    case "Galleon":
                        fleet1UnitsList.Add(new Ship(unitName, 2, 4, 160, 150, 3, 3, 35, 5, 0, 0, numOfUnits));
                        break;
                    //epoka ekspansji
                    case "Szkuner":
                        fleet1UnitsList.Add(new Ship(unitName, 1, 2, 95, 90, 5, 6, 20, 5, 0, 0, numOfUnits));
                        break;
                    case "Bryg":
                        fleet1UnitsList.Add(new Ship(unitName, 1, 2, 115, 105, 5, 6, 25, 5, 5, 0, numOfUnits));
                        break;
                    case "Fregata":
                        fleet1UnitsList.Add(new Ship(unitName, 2, 3, 140, 120, 5, 5, 45, 5, 5, 0, numOfUnits));
                        break;
                    case "Ciężka fregata":
                        fleet1UnitsList.Add(new Ship(unitName, 2, 3, 180, 145, 4, 4, 70, 5, 5, 10, numOfUnits));
                        break;
                    //epoka imperiow
                    case "Liniowiec":
                        fleet1UnitsList.Add(new Ship(unitName, 3, 4, 290, 250, 3, 3, 100, 10, 10, 20, numOfUnits));
                        break;
                    case "Eastindiaman":
                        fleet1UnitsList.Add(new Ship(unitName, 2, 3, 190, 140, 4, 3, 55, 5, 5, 0, numOfUnits));
                        break;
                    //epoka rewolucji
                    case "Opancerzona fregata":
                        fleet1UnitsList.Add(new Ship(unitName, 2, 3, 215, 120, 4, 4, 50, 10, 5, 0, numOfUnits));
                        break;
                    //unikalne
                    case "Azjatycki okręt":
                        fleet1UnitsList.Add(new Ship(unitName, 2, 3, 140, 120, 3, 3, 25, 5, 0, 0, numOfUnits));
                        break;
                }
            }
        }
        public bool AreAttackersCharging(List<LandUnit> armyList, bool checkForALaterCharge)
        {
            int forCharge = 0, againstCharge = 0;
            foreach (LandUnit unit in armyList)
            {
                if(unit.NumberOf > 0)
                {
                    if (unit.Type == "ChargeCavalry" || unit.Type == "MeleeInfantry")
                    {
                        forCharge++;
                    }
                    else if (unit.Type == "RangerInfantry" || unit.Type == "RangerCavalry")
                    {
                        againstCharge++;
                    }
                    else if (unit.Type == "LineInfantry" && checkForALaterCharge)
                    {
                        forCharge++;
                    }
                }
            }
            if (forCharge > againstCharge)
            {
                return true;
            }
            else if (forCharge < againstCharge)
            {
                return false;
            }
            else return true;
        }
        public string ConvertArmyToString(List<LandUnit> armyList, bool forDisplay, List<LandUnit>? originalArmyList = null)
        {
            string armyString = "";
            MergeLandUnits(ref armyList);
            if (!forDisplay)
            {
                foreach (LandUnit unit in armyList)
                {
                    armyString += $"{unit.Name}:{unit.NumberOf}\n";
                }
            }
            else
            {
                foreach (LandUnit unit in armyList)
                {
                    int losses = 0;
                    int ogUnitNum = unit.NumberOf;
                    if(originalArmyList != null)
                    {
                        foreach(LandUnit ogUnit in originalArmyList)
                        {
                            if (unit.Name == ogUnit.Name && unit.NumberOf < ogUnit.NumberOf)
                            {
                                Debug.WriteLine($"Unit num:{unit.NumberOf} | Og unit num:{ogUnit.NumberOf}");
                                losses = ogUnit.NumberOf - unit.NumberOf;
                                ogUnitNum = ogUnit.NumberOf;
                                break;
                            }
                            else losses = 0;
                        }
                    }
                    armyString += $"{unit.Name} | Pozostało:{unit.NumberOf} | Oryginalnie:{ogUnitNum} | Stracono:{losses}\n";
                }
            }
            return armyString;
        }
        void VerifyInputLists(ref List<CheckBox> ckList, ref List<TextBox> tbxList)
        {
            List<CheckBox> ckRemovalList = new List<CheckBox>();
            List<TextBox> tbxRemovalList = new List<TextBox>();
            int input;
            bool inputCorrect;
            for (int i = 0; i < ckList.Count; i++)
            {
                inputCorrect = int.TryParse(tbxList[i].Text.Trim(), out input);
                if (input <= 0 || ckList[i].IsChecked == false || !inputCorrect)
                {
                    ckList[i].IsChecked = false;
                    tbxList[i].IsEnabled = false;
                    ckRemovalList.Add(ckList[i]);
                    tbxRemovalList.Add(tbxList[i]);
                    Debug.WriteLine("Catched a false input");
                }
            }
            foreach(CheckBox ck in ckRemovalList)
            {
                if(ckList.Contains(ck)) ckList.Remove(ck);
            }
            foreach(TextBox tbx in tbxRemovalList)
            {
                if(tbxList.Contains(tbx)) tbxList.Remove(tbx);
            }
        }
        public void CopyArmyList(List<LandUnit> armyListToCopy, ref List<LandUnit> copiedArmyList) 
        {
            copiedArmyList.Clear();
            foreach(LandUnit unit in armyListToCopy)
            {
                copiedArmyList.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.MaxInitiative, unit.MaxHealth, unit.MaxMorale, unit.Speed, unit.Type, unit.NumberOf));
            }
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbBattleType.Text == "Bitwa lądowa")
            {
                VerifyInputLists(ref army1CheckBoxList, ref army1TextboxList);
                VerifyInputLists(ref army2CheckBoxList, ref army2TextboxList);
                ActivateStartButton();
                if (btnStart.IsEnabled)
                {
                    //tworzenie armii
                    FillArmyList(army1CheckBoxList, army1TextboxList, ref army1UnitsList);
                    FillArmyList(army2CheckBoxList, army2TextboxList, ref army2UnitsList);
                    //zapisanie oryginalnych armii
                    CopyArmyList(army1UnitsList, ref army1OgUnitsList);
                    CopyArmyList(army1UnitsList, ref army2OgUnitsList);
                    //teren 
                    switch (cbTerrainType.Text)
                    {
                        case "Łąki":
                            terrain = new TerrainType(cbTerrainType.Text, 2, 0, 2, 0);
                            break;
                        case "Wzgórza":
                            terrain = new TerrainType(cbTerrainType.Text, 10, 0, 6, 1);
                            break;
                        case "Las":
                            terrain = new TerrainType(cbTerrainType.Text, 10, 0, 8, 0);
                            break;
                        case "Leśne wzgórza":
                            terrain = new TerrainType(cbTerrainType.Text, 18, 0, 13, 1);
                            break;
                        case "Góry":
                            terrain = new TerrainType(cbTerrainType.Text, 17, 0, 13, 4);
                            break;
                        case "Bagno":
                            terrain = new TerrainType(cbTerrainType.Text, 2, 4, 2, 0);
                            break;
                        case "Dżungla":
                            terrain = new TerrainType(cbTerrainType.Text, 15, 4, 15, 0);
                            break;
                        case "Równina":
                            terrain = new TerrainType(cbTerrainType.Text, 0, 0, 1, 0);
                            break;
                        case "Pustynia":
                            terrain = new TerrainType(cbTerrainType.Text, 8, 2, 2, 1);
                            break;
                    }
                    // glowne dzialanie
                    ApplyTerrainEffects(ref army1UnitsList);
                    ApplyTerrainEffects(ref army2UnitsList);
                    List<LandUnit> actingUnitsFromArmy1 = new List<LandUnit>();
                    List<LandUnit> actingUnitsFromArmy2 = new List<LandUnit>();
                    //czyscimy listy wybierania
                    //to do
                    //bitwa
                    battleLog += "Bitwa sie rozpoczyna\n";
                    if (fortLevel == 0) 
                    {
                        //atak bez fortu
                        int army1CountBefore, army2CountBefore;
                        int army1Losses = 0, army2Losses = 0;
                        MoraleModification(ref army1UnitsList, 10, true);
                        int army1AvgInitiative, army2AvgInitiative;
                        int ticks = 0;
                        int maxTicks = 80 + GetArmyCount(army1UnitsList) + GetArmyCount(army2UnitsList);
                        while((!team1Win && !team2Win) && ticks <= maxTicks)
                        {
                            ticks++;
                            MoraleModification(ref actingUnitsFromArmy1, 5);
                            MoraleModification(ref actingUnitsFromArmy2, 5);
                            UpdateInitiative(ref army1UnitsList);
                            UpdateInitiative(ref army2UnitsList);
                            army1UnitsList.Sort((x, y) => y.Initiative.CompareTo(x.Initiative));
                            army2UnitsList.Sort((x, y) => y.Initiative.CompareTo(x.Initiative));
                            MergeLandUnits(ref army1UnitsList);
                            MergeLandUnits(ref army2UnitsList);
                            army1AvgInitiative = GetArmyInitiative(army1UnitsList);
                            if (isSkirmishAttack) army1AvgInitiative += 4;
                            army2AvgInitiative = GetArmyInitiative(army2UnitsList);
                            army1CountBefore = GetArmyCount(army1UnitsList);
                            army2CountBefore = GetArmyCount(army2UnitsList);
                            //warunki zakonczenia bitwy
                            //przewagi liczebne
                            if(GetArmyCount(army1UnitsList) > GetArmyCount(army2UnitsList) && GetArmyCount(army1UnitsList) - GetArmyCount(army2UnitsList) > 20)
                            {
                                //przewaga liczebna armii 1
                                battleLog += "Armia 2 wycofuje sie ze wzgledu na przewage liczebna armii 1\n";
                                SelectBombardingUnits(ref army1UnitsList, ref actingUnitsFromArmy1, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.LongRange);
                                team1Win = true;
                                break;
                            }
                            else if(GetArmyCount(army1UnitsList) < GetArmyCount(army2UnitsList) && GetArmyCount(army2UnitsList) - GetArmyCount(army1UnitsList) > 20)
                            {
                                //przewaga liczebna armii 2
                                battleLog += "Armia 1 wycofuje sie ze wzgledu na przewage liczebna armii 2\n";
                                SelectBombardingUnits(ref army2UnitsList, ref actingUnitsFromArmy2, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.LongRange);
                                team2Win = true;
                                break;
                            }
                            //straty w porownaniu
                            else if(army1Losses > army2Losses && army1Losses - army2Losses > 15)
                            {
                                //zbyt duze straty armii 1
                                battleLog += "Armia 1 wycofuje sie ze wzgledu przewazajace straty\n";
                                SelectBombardingUnits(ref army2UnitsList, ref actingUnitsFromArmy2, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.LongRange);
                                team2Win = true;
                                break;
                            }
                            else if (army1Losses < army2Losses && army2Losses - army1Losses > 15)
                            {
                                //zbyt duze straty armii 2
                                battleLog += "Armia 2 wycofuje sie ze wzgledu przewazajace straty\n";
                                SelectBombardingUnits(ref army1UnitsList, ref actingUnitsFromArmy1, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.LongRange);
                                team1Win = true;
                                break;
                            }
                            //morale
                            else if (GetArmyMorale(army1UnitsList) <= 20 && GetArmyMorale(army1UnitsList) > 10)
                            {
                                //zbyt male morale armii 1
                                battleLog += "Armia 1 wycofuje ze wzgledu an slabe morale\n";
                                SelectBombardingUnits(ref army2UnitsList, ref actingUnitsFromArmy2, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.LongRange);
                                RetreatInHaste(ref actingUnitsFromArmy1, 5);
                                team2Win = true;
                                break;
                            }
                            else if (GetArmyMorale(army2UnitsList) <= 20 && GetArmyMorale(army2UnitsList) > 10)
                            {
                                //zbyt male morale armii 2
                                battleLog += "Armia 2 wycofuje ze wzgledu an slabe morale\n";
                                SelectBombardingUnits(ref army1UnitsList, ref actingUnitsFromArmy1, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.LongRange);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.MidRange);
                                RetreatInHaste(ref actingUnitsFromArmy2, 5);
                                team1Win = true;
                                break;
                            }
                            //krytyczne morale
                            else if (GetArmyMorale(army1UnitsList) <= 10)
                            {
                                //zbyt male morale armii 1
                                battleLog += "Armia 1 zostala zlamana i ucieka w poplochu\n";
                                SelectBombardingUnits(ref army2UnitsList, ref actingUnitsFromArmy2, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.LongRange);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.MidRange);
                                RetreatInHaste(ref army1UnitsList);
                                team2Win = true;
                                break;
                            }
                            else if (GetArmyMorale(army2UnitsList) <= 10)
                            {
                                //zbyt male morale armii 2
                                battleLog += "Armia 2 zostala zlamana i ucieka w poplochu\n";
                                SelectBombardingUnits(ref army1UnitsList, ref actingUnitsFromArmy1, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.LongRange);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.MidRange);
                                RetreatInHaste(ref army2UnitsList);
                                team1Win = true;
                                break;
                            }
                            //zmasakrowanie armii
                            else if(GetArmyCount(army1UnitsList) <= 0)
                            {
                                battleLog += "Armia 1 zostala calkowicie wybita";
                                team2Win = true;
                                break;
                            }
                            else if (GetArmyCount(army2UnitsList) <= 0)
                            {
                                battleLog += "Armia 2 zostala calkowicie wybita";
                                team1Win = true;
                                break;
                            }
                            //maxticks
                            if (ticks > maxTicks)
                            {
                                battleLog += "Bitwa jest niedycująca!\n";
                                team1Win = true;
                                team2Win = true;
                                break;
                            }
                            //ataki
                            //przypadek rownej inicjatywe
                            if (army1AvgInitiative == army2AvgInitiative)
                            {
                                if (isSkirmishAttack) army1AvgInitiative++; // przy ataku, armia 1 ma pierwszenstwo
                                else //przy spotkaniu losujemy kto zaatakuje
                                {
                                    Random rnd = new Random();
                                    int num = rnd.Next(0, 2);
                                    if (num > 0) army1AvgInitiative++;
                                    else army2AvgInitiative++;
                                }
                            }
                            if (army1AvgInitiative > army2AvgInitiative)
                            {
                                battleLog += "Armia 1 ma inicjatywe i przechodzi do ataku\nNastepuje bombardowanie\n";
                                //armia 1 ma inicjatywe
                                //bombardowanie z obu stron
                                DealDamageToLandunits(ref army1UnitsList, ref army2UnitsList, TypeOfDamage.LongRangeRet);
                                //atak
                                SelectAttackingUnits(ref army1UnitsList, ref actingUnitsFromArmy1);
                                SelectDefendingUnits(ref army2UnitsList, ref actingUnitsFromArmy2);
                                bool chargeNow = AreAttackersCharging(actingUnitsFromArmy1, false), laterCharge = AreAttackersCharging(actingUnitsFromArmy1, true);
                                int distance = 30;
                                int attackerSpeed = GetArmySpeed(actingUnitsFromArmy1);
                                while(distance > 0 && distance - attackerSpeed >= attackerSpeed)
                                {
                                    if(distance > 15)
                                    {
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.MidRangeRet);
                                    }
                                    else
                                    {
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.LowRangeRet, team1SpecialCondition);
                                        if (!chargeNow) break;
                                    }
                                    distance -= attackerSpeed;
                                }
                                int stopper = 0, beforeCharge = 20;
                                bool chargeTaken = false;
                                while(stopper < 1500)
                                {
                                    UpdateInitiative(ref actingUnitsFromArmy1);
                                    UpdateInitiative(ref actingUnitsFromArmy2);
                                    stopper++;
                                    if (!chargeNow)
                                    {
                                        if (laterCharge && beforeCharge > 0)
                                        {
                                            if (GetArmyInitiative(actingUnitsFromArmy1) >= GetArmyInitiative(actingUnitsFromArmy2)) DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.LowRangeRet);
                                            else DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRangeRet);
                                            beforeCharge--;
                                        }
                                        else if(laterCharge && beforeCharge == 0)
                                        {
                                            laterCharge = false;
                                            chargeNow = true;
                                        }
                                        else
                                        {
                                            if (GetArmyInitiative(actingUnitsFromArmy1) >= GetArmyInitiative(actingUnitsFromArmy2)) DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.LowRangeRet);
                                            else DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRangeRet);
                                        }
                                    }
                                    else
                                    {
                                        if (!chargeTaken)
                                        {
                                            DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.ChargeMelee, team1SpecialCondition);
                                            chargeTaken = true;
                                        }
                                        else DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.MeleeRet);
                                    }
                                    //warunki wyjsciowe
                                    if(GetArmyCount(actingUnitsFromArmy1) > GetArmyCount(actingUnitsFromArmy2) && GetArmyCount(actingUnitsFromArmy1) - GetArmyCount(actingUnitsFromArmy2) >= 10)
                                    {
                                        battleLog += "Obrona armii 2 ucieka przez przewage liczebna wroga\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.LowRange);
                                        MoraleModification(ref army2UnitsList, -10);
                                        MoraleModification(ref army1UnitsList, 7);
                                        break;
                                    }
                                    else if (GetArmyCount(actingUnitsFromArmy2) > GetArmyCount(actingUnitsFromArmy1) && GetArmyCount(actingUnitsFromArmy2) - GetArmyCount(actingUnitsFromArmy1) >= 10)
                                    {
                                        battleLog += "Atakujacy armii 1 wycofuja sie ze wzgledu na przewage liczbena wroga\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRange);
                                        MoraleModification(ref army1UnitsList, -10);
                                        MoraleModification(ref army2UnitsList, 5);
                                        break;
                                    }
                                    else if(GetArmyCount(actingUnitsFromArmy1) == 0)
                                    {
                                        //atakujacy wybici
                                        battleLog += "Obrona armii 2 sie utrzymala, atakujacy zostali wybici\n";
                                        MoraleModification(ref army1UnitsList, -20);
                                        break;
                                    }
                                    else if(GetArmyCount(actingUnitsFromArmy2) == 0)
                                    {
                                        //broniacy wybici
                                        battleLog += "Obrona armii 2 zostala wybita w pien\n";
                                        MoraleModification(ref army2UnitsList, -15);
                                        break;
                                    }
                                    else if(GetArmyMorale(actingUnitsFromArmy1) <= 10)
                                    {
                                        //atakujacy uciekaja
                                        battleLog += "Obrona armii 2 sie utrzymala\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.Melee);
                                        RetreatInHaste(ref actingUnitsFromArmy1);
                                        DestroyWeakUnits(ref actingUnitsFromArmy1);
                                        MoraleModification(ref army1UnitsList, -12);
                                        break;
                                    }
                                    else if(GetArmyMorale(actingUnitsFromArmy2) <= 10)
                                    {
                                        //broniacy uciekaja uciekaja
                                        battleLog += "Obrona armii 2 zostala zlamana\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.Melee);
                                        RetreatInHaste(ref actingUnitsFromArmy2);
                                        DestroyWeakUnits(ref actingUnitsFromArmy2);
                                        MoraleModification(ref army2UnitsList, -20);
                                        MoraleModification(ref army1UnitsList, 5);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                battleLog += "Armia 2 ma inicjatywe i przechodzi do ataku\nNastepuje bombardowanie\n";
                                //armia 2 ma inicjatywe
                                //bombardowanie z obu stron
                                DealDamageToLandunits(ref army2UnitsList, ref army1UnitsList, TypeOfDamage.LongRangeRet);
                                //atak
                                SelectAttackingUnits(ref army1UnitsList, ref actingUnitsFromArmy1);
                                SelectDefendingUnits(ref army2UnitsList, ref actingUnitsFromArmy2);
                                bool chargeNow = AreAttackersCharging(actingUnitsFromArmy1, false), laterCharge = AreAttackersCharging(actingUnitsFromArmy1, true);
                                int distance = 30;
                                int attackerSpeed = GetArmySpeed(actingUnitsFromArmy2);
                                while (distance > 0 && distance - attackerSpeed >= attackerSpeed)
                                {
                                    if (distance > 15)
                                    {
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.MidRangeRet);
                                    }
                                    else
                                    {
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRangeRet, team1SpecialCondition);
                                    }
                                    distance -= attackerSpeed;
                                }
                                int stopper = 0, beforeCharge = 20;
                                bool chargeTaken = false;
                                while (stopper < 1500)
                                {
                                    UpdateInitiative(ref actingUnitsFromArmy1);
                                    UpdateInitiative(ref actingUnitsFromArmy2);
                                    stopper++;
                                    if (!chargeNow)
                                    {
                                        if (laterCharge && beforeCharge > 0)
                                        {
                                            if (GetArmyInitiative(actingUnitsFromArmy1) >= GetArmyInitiative(actingUnitsFromArmy2)) DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.LowRangeRet);
                                            else DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRangeRet);
                                            beforeCharge--;
                                        }
                                        else if (laterCharge && beforeCharge == 0)
                                        {
                                            laterCharge = false;
                                            chargeNow = true;
                                        }
                                        else
                                        {
                                            if (GetArmyInitiative(actingUnitsFromArmy1) >= GetArmyInitiative(actingUnitsFromArmy2)) DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.LowRangeRet);
                                            else DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRangeRet);
                                        }
                                    }
                                    else
                                    {
                                        if (!chargeTaken)
                                        {
                                            DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.ChargeMelee, team1SpecialCondition);
                                            chargeTaken = true;
                                        }
                                        else DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.MeleeRet);
                                    }
                                    //warunki wyjsciowe
                                    if (GetArmyCount(actingUnitsFromArmy2) > GetArmyCount(actingUnitsFromArmy1) && GetArmyCount(actingUnitsFromArmy2) - GetArmyCount(actingUnitsFromArmy1) >= 10)
                                    {
                                        battleLog += "Obrona armii 1 ucieka przez przewage liczebna wroga\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRange);
                                        MoraleModification(ref army1UnitsList, -10);
                                        MoraleModification(ref army2UnitsList, 7);
                                        break;
                                    }
                                    else if (GetArmyCount(actingUnitsFromArmy1) > GetArmyCount(actingUnitsFromArmy2) && GetArmyCount(actingUnitsFromArmy1) - GetArmyCount(actingUnitsFromArmy2) >= 10)
                                    {
                                        battleLog += "Atakujacy armii 2 wycofuja sie ze wzgledu na przewage liczbena wroga\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRange);
                                        MoraleModification(ref army2UnitsList, -10);
                                        MoraleModification(ref army1UnitsList, 5);
                                        break;
                                    }
                                    else if (GetArmyCount(actingUnitsFromArmy2) == 0)
                                    {
                                        //atakujacy uciekaja
                                        battleLog += "Obrona armii 1 sie utrzymala, atakujacy zostali wybici\n";
                                        MoraleModification(ref army2UnitsList, -20);
                                        break;
                                    }
                                    else if (GetArmyCount(actingUnitsFromArmy1) == 0)
                                    {
                                        //atakujacy uciekaja
                                        battleLog += "Obrona armii 1 zostala wybita w pien\n";
                                        MoraleModification(ref army1UnitsList, -20);
                                        break;
                                    }
                                    else if (GetArmyMorale(actingUnitsFromArmy2) <= 10)
                                    {
                                        //atakujacy uciekaja
                                        battleLog += "Obrona armii 1 sie utrzymala\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.Melee);
                                        RetreatInHaste(ref actingUnitsFromArmy2);
                                        DestroyWeakUnits(ref actingUnitsFromArmy2);
                                        MoraleModification(ref army2UnitsList, -15);
                                        break;
                                    }
                                    else if (GetArmyMorale(actingUnitsFromArmy1) <= 10)
                                    {
                                        //broniacy uciekaja uciekaja
                                        battleLog += "Obrona armii 1 zostala zlamana\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.Melee);
                                        RetreatInHaste(ref actingUnitsFromArmy1);
                                        DestroyWeakUnits(ref actingUnitsFromArmy1);
                                        MoraleModification(ref army1UnitsList, -12);
                                        MoraleModification(ref army2UnitsList, 5);
                                        break;
                                    }

                                }
                                //obliczanie strat
                                army1Losses += army1CountBefore - GetArmyCount(army1UnitsList);
                                army2Losses += army2CountBefore - GetArmyCount(army2UnitsList);
                            }
                        }
                    }
                    else
                    {
                        //atak na fort
                    }
                    //zakonczenia bitwy
                    if (team1Win && !team2Win)
                    {
                        battleLog += "Wygrywa armia 1!\n";
                        resultString = "Wygrywa armia 1!";
                    }
                    else if (team2Win && !team1Win)
                    {
                        battleLog += "Wygrywa armia 2!\n";
                        resultString = "Wygrywa armia 2!";
                    }
                    else if (team1Win && team2Win)
                    {
                        battleLog += "Remis!\n";
                        resultString = "Remis!";
                    }
                    DestroyWeakUnits(ref army1UnitsList);
                    DestroyWeakUnits(ref army2UnitsList);
                    team1StringList = ConvertArmyToString(army1UnitsList, false, null);
                    team1StringDisplayList = ConvertArmyToString(army1UnitsList, true, army1OgUnitsList);
                    team2StringList = ConvertArmyToString(army2UnitsList, false, null);
                    team2StringDisplayList = ConvertArmyToString(army2UnitsList, true, army2OgUnitsList);
                    ChangePageToResults(sender, e);
                }
            }
            else
            {
                VerifyInputLists(ref fleet1CheckBoxList, ref fleet1TextboxList);
                VerifyInputLists(ref fleet2CheckBoxList, ref fleet2TextboxList);
                ActivateStartButton();
                if (btnStart.IsEnabled)
                {
                    fleet1UnitsList.Clear();
                    fleet2UnitsList.Clear();
                    //dodanie jednostek do flot
                    FillFleetList(fleet1CheckBoxList, fleet1TextboxList, ref fleet1UnitsList);
                    FillFleetList(fleet2CheckBoxList, fleet2TextboxList, ref fleet2UnitsList);
                }

            }
        }

    }
}
