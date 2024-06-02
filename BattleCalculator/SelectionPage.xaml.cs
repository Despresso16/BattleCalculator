using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        public string battleLog = "";
        int fortLevel = 0;
        bool isSkirmishAttack = false;
        bool isBattleLand = false;
        bool team1SpecialCondition = false;
        bool team2SpecialCondition = false;
        bool team1Win = false, team2Win = false;
        TerrainType terrain = new TerrainType("filler", 0, 0, 0, 0);
        SeaType[] seaTypes = new SeaType[1];
        List<LandUnit> army1UnitsList = new List<LandUnit>();
        List<LandUnit> army2UnitsList = new List<LandUnit>();
        List<Ship> fleet1UnitsList = new List<Ship>();
        List<Ship> fleet2UnitsList = new List<Ship>();


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
                gridFortLevel.IsEnabled = true;
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
        //zaznaczanie jednostek i wyznaczanie ich ilosci
        void SelectAnUnit(CheckBox ckUnit, TextBox tbxUnitNum, ref List<CheckBox> ckList, ref List<TextBox> tbxList)
        {
            if (ckUnit.IsChecked == true)
            {
                tbxUnitNum.IsEnabled = true;
                ckList.Add(ckUnit);
                tbxList.Add(tbxUnitNum);

            }
            else
            {
                tbxUnitNum.IsEnabled = false;
                ckList.Remove(ckUnit);
                tbxList.Remove(tbxUnitNum);
            }
            ActivateStartButton();
        }
        void VerifyNumberOfUnits(TextBox tbxUnitNum)
        {
            int numOfUnit = 0;
            bool tbxIsNumber = int.TryParse(tbxUnitNum.Text, out numOfUnit);
            if (!tbxIsNumber || numOfUnit <= 0)
            {
                tbxUnitNum.Text = "0";
            }
        }
        //dla armii 1
        private void ckArmy1Pikemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Pikemen, tbxArmy1Pikemen, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Pikemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Pikemen);
        }
        private void ckArmy1Arquebusiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Arquebusiers, tbxArmy1Arquebusiers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Arquebusiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Arquebusiers);
        }

        private void ckArmy1Archers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Archers, tbxArmy1Archers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Archers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Archers);
        }

        private void ckArmy1Crossbowmen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Crossbowmen, tbxArmy1Crossbowmen, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Crossbowmen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Crossbowmen);
        }

        private void ckArmy1Knights_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Knights, tbxArmy1Knights, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Knights_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Knights);
        }
        private void ckArmy1Horsemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Horsemen, tbxArmy1Horsemen, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Horsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Horsemen);
        }

        private void ckArmy1Bombard_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Bombard, tbxArmy1Bombard, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Bombard_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Bombard);
        }

        private void ckArmy1PikeShotArq_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1PikeShotArq, tbxArmy1PikeShotArq, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1PikeShotArq_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1PikeShotArq);
        }

        private void ckArmy1HeavyHussars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1HeavyHussars, tbxArmy1HeavyHussars, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1HeavyHussars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1HeavyHussars);
        }

        private void ckArmy1Cossacks_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Cossacks, tbxArmy1Cossacks, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Cossacks_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Cossacks);
        }

        private void ckArmy1Reiters_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Reiters, tbxArmy1Reiters, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Reiters_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Reiters);
        }
        private void ckArmy1FieldCannon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1FieldCannon, tbxArmy1FieldCannon, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1FieldCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1FieldCannon);
        }

        private void ckArmy1HeavyCannon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1HeavyCannon, tbxArmy1HeavyCannon, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1HeavyCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1HeavyCannon);
        }

        private void ckArmy1PikeShotMusk_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1PikeShotMusk, tbxArmy1PikeShotMusk, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1PikeShotMusk_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1PikeShotMusk);
        }

        private void ckArmy1EarlyFusiliers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1EarlyFusiliers, tbxArmy1EarlyFusiliers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1EarlyFusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1EarlyFusiliers);
        }

        private void ckArmy1EarlyCuirassier_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1EarlyCuirassier, tbxArmy1EarlyCuirassier, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1EarlyCuirassier_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1EarlyCuirassier);
        }

        private void ckArmy1Harquebusers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Harquebusers, tbxArmy1Harquebusers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Harquebusers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Harquebusers);
        }

        private void ckArmy1Lancers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Lancers, tbxArmy1Lancers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Lancers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Lancers);
        }

        private void ckArmy1SiegeHowitzer_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1SiegeHowitzer, tbxArmy1SiegeHowitzer, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1SiegeHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1SiegeHowitzer);
        }

        private void ckArmy1Fusiliers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Fusiliers, tbxArmy1Fusiliers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Fusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Fusiliers);
        }
        private void ckArmy1Grenadiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Grenadiers, tbxArmy1Grenadiers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Grenadiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Grenadiers);
        }
        private void ckArmy1Militia_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Militia, tbxArmy1Militia, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Militia_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Militia);
        }

        private void ckArmy1CarbineCav_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1CarbineCav, tbxArmy1CarbineCav, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1CarbineCav_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1CarbineCav);
        }

        private void ckArmy1Dragoons_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Dragoons, tbxArmy1Dragoons, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Dragoons_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Dragoons);
        }

        private void ckArmy1Hussars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Hussars, tbxArmy1Hussars, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Hussars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Hussars);
        }

        private void ckArmy1Cuiraissiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Cuiraissiers, tbxArmy1Cuiraissiers, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Cuiraissiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Cuiraissiers);
        }

        private void ckArmy1FieldGun_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1FieldGun, tbxArmy1FieldGun, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1FieldGun_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1FieldGun);
        }

        private void ckArmy1Mortars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1Mortars, tbxArmy1Mortars, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1Mortars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1Mortars);
        }

        private void ckArmy1LightInfantry_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1LightInfantry, tbxArmy1LightInfantry, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1LightInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1LightInfantry);
        }

        private void ckArmy1LineInfantry_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1LineInfantry, tbxArmy1LineInfantry, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1LineInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1LineInfantry);
        }

        private void ckArmy1FieldHowitzer_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1FieldHowitzer, tbxArmy1FieldHowitzer, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1FieldHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1FieldHowitzer);
        }

        private void ckArmy1TribalWarriors_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1TribalWarriors, tbxArmy1TribalWarriors, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1TribalWarriors_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1TribalWarriors);
        }

        private void ckArmy1TribalRanger_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1TribalRanger, tbxArmy1TribalRanger, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1TribalRanger_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1TribalRanger);
        }

        private void ckArmy1TribalHorsemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1TribalHorsemen, tbxArmy1TribalHorsemen, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void tbxArmy1TribalHorsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1TribalHorsemen);
        }
        private void tbxArmy1HorseArcher_LostFocus(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy1HorseArcher, tbxArmy1HorseArcher, ref army1CheckBoxList, ref army1TextboxList);
        }

        private void ckArmy1HorseArcher_Click(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy1HorseArcher);
        }
        // dla floty 1
        private void ckFleet1Carrack_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1Carrack, tbxFleet1Carrack, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Carrack_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1Carrack);
        }

        private void ckFleet1Caravel_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1Caravel, tbxFleet1Caravel, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Caravel_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1Caravel);
        }

        private void ckFleet1Galley_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1Galley, tbxFleet1Galley, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Galley_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1Galley);
        }

        private void ckFleet1Galleon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1Galleon, tbxFleet1Galleon, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Galleon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1Galleon);
        }

        private void ckFleet1Schooner_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1Schooner, tbxFleet1Schooner, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Schooner_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1Schooner);
        }

        private void ckFleet1Brig_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1Brig, tbxFleet1Brig, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Brig_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1Brig);
        }

        private void ckFleet1Frigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1Frigate, tbxFleet1Frigate, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Frigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1Frigate);
        }

        private void ckFleet1GreatFrigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1GreatFrigate, tbxFleet1GreatFrigate, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1GreatFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1GreatFrigate);
        }

        private void ckFleet1ShipOfLine_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1ShipOfLine, tbxFleet1ShipOfLine, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1ShipOfLine_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1ShipOfLine);
        }

        private void ckFleet1Eastindiaman_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1Eastindiaman, tbxFleet1Eastindiaman, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1Eastindiaman_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1Eastindiaman);
        }

        private void ckFleet1ArmoredFrigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1ArmoredFrigate, tbxFleet1ArmoredFrigate, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1ArmoredFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1ArmoredFrigate);
        }

        private void ckFleet1AsianShip_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet1AsianShip, tbxFleet1AsianShip, ref fleet1CheckBoxList, ref fleet1TextboxList);
        }

        private void tbxFleet1AsianShip_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet1AsianShip);
        }
        // dla armii 1
        private void ckArmy2Pikemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Pikemen, tbxArmy2Pikemen, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Pikemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Pikemen);
        }
        private void ckArmy2Arquebusiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Arquebusiers, tbxArmy2Arquebusiers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Arquebusiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Arquebusiers);
        }

        private void ckArmy2Archers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Archers, tbxArmy2Archers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Archers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Archers);
        }

        private void ckArmy2Crossbowmen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Crossbowmen, tbxArmy2Crossbowmen, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Crossbowmen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Crossbowmen);
        }

        private void ckArmy2Knights_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Knights, tbxArmy2Knights, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Knights_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Knights);
        }
        private void ckArmy2Horsemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Horsemen, tbxArmy2Horsemen, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Horsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Horsemen);
        }

        private void ckArmy2Bombard_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Bombard, tbxArmy2Bombard, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Bombard_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Bombard);
        }

        private void ckArmy2PikeShotArq_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2PikeShotArq, tbxArmy2PikeShotArq, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2PikeShotArq_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2PikeShotArq);
        }

        private void ckArmy2HeavyHussars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2HeavyHussars, tbxArmy2HeavyHussars, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2HeavyHussars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2HeavyHussars);
        }

        private void ckArmy2Cossacks_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Cossacks, tbxArmy2Cossacks, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Cossacks_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Cossacks);
        }

        private void ckArmy2Reiters_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Reiters, tbxArmy2Reiters, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Reiters_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Reiters);
        }
        private void ckArmy2FieldCannon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2FieldCannon, tbxArmy2FieldCannon, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2FieldCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2FieldCannon);
        }

        private void ckArmy2HeavyCannon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2HeavyCannon, tbxArmy2HeavyCannon, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2HeavyCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2HeavyCannon);
        }

        private void ckArmy2PikeShotMusk_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2PikeShotMusk, tbxArmy2PikeShotMusk, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2PikeShotMusk_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2PikeShotMusk);
        }

        private void ckArmy2EarlyFusiliers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2EarlyFusiliers, tbxArmy2EarlyFusiliers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2EarlyFusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2EarlyFusiliers);
        }

        private void ckArmy2EarlyCuirassier_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2EarlyCuirassier, tbxArmy2EarlyCuirassier, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2EarlyCuirassier_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2EarlyCuirassier);
        }

        private void ckArmy2Harquebusers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Harquebusers, tbxArmy2Harquebusers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Harquebusers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Harquebusers);
        }

        private void ckArmy2Lancers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Lancers, tbxArmy2Lancers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Lancers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Lancers);
        }

        private void ckArmy2SiegeHowitzer_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2SiegeHowitzer, tbxArmy2SiegeHowitzer, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2SiegeHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2SiegeHowitzer);
        }

        private void ckArmy2Fusiliers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Fusiliers, tbxArmy2Fusiliers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Fusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Fusiliers);
        }
        private void ckArmy2Grenadiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Grenadiers, tbxArmy2Grenadiers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Grenadiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Grenadiers);
        }

        private void ckArmy2Militia_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Militia, tbxArmy2Militia, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Militia_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Militia);
        }

        private void ckArmy2CarbineCav_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2CarbineCav, tbxArmy2CarbineCav, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2CarbineCav_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2CarbineCav);
        }

        private void ckArmy2Dragoons_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Dragoons, tbxArmy2Dragoons, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Dragoons_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Dragoons);
        }

        private void ckArmy2Hussars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Hussars, tbxArmy2Hussars, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Hussars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Hussars);
        }

        private void ckArmy2Cuiraissiers_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Cuiraissiers, tbxArmy2Cuiraissiers, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Cuiraissiers_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Cuiraissiers);
        }

        private void ckArmy2FieldGun_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2FieldGun, tbxArmy2FieldGun, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2FieldGun_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2FieldGun);
        }

        private void ckArmy2Mortars_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2Mortars, tbxArmy2Mortars, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2Mortars_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2Mortars);
        }

        private void ckArmy2LightInfantry_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2LightInfantry, tbxArmy2LightInfantry, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2LightInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2LightInfantry);
        }

        private void ckArmy2LineInfantry_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2LineInfantry, tbxArmy2LineInfantry, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2LineInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2LineInfantry);
        }

        private void ckArmy2FieldHowitzer_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2FieldHowitzer, tbxArmy2FieldHowitzer, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2FieldHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2FieldHowitzer);
        }

        private void ckArmy2TribalWarriors_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2TribalWarriors, tbxArmy2TribalWarriors, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2TribalWarriors_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2TribalWarriors);
        }

        private void ckArmy2TribalRanger_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2TribalRanger, tbxArmy2TribalRanger, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2TribalRanger_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2TribalRanger);
        }

        private void ckArmy2TribalHorsemen_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2TribalHorsemen, tbxArmy2TribalHorsemen, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2TribalHorsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2TribalHorsemen);
        }
        private void ckArmy2HorseArcher_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckArmy2HorseArcher, tbxArmy2HorseArcher, ref army2CheckBoxList, ref army2TextboxList);
        }

        private void tbxArmy2HorseArcher_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxArmy2HorseArcher);
        }
        // flota 2
        private void ckFleet2Carrack_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2Carrack, tbxFleet2Carrack, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Carrack_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2Carrack);
        }
        private void ckFleet2Caravel_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2Caravel, tbxFleet2Caravel, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Caravel_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2Caravel);
        }
        private void ckFleet2Galley_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2Galley, tbxFleet2Galley, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Galley_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2Galley);
        }

        private void ckFleet2Galleon_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2Galleon, tbxFleet2Galleon, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Galleon_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2Galleon);
        }

        private void ckFleet2Schooner_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2Schooner, tbxFleet2Schooner, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Schooner_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2Schooner);
        }

        private void ckFleet2Brig_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2Brig, tbxFleet2Brig, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Brig_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2Brig);
        }

        private void ckFleet2Frigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2Frigate, tbxFleet2Frigate, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Frigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2Frigate);
        }

        private void ckFleet2GreatFrigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2GreatFrigate, tbxFleet2GreatFrigate, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2GreatFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2GreatFrigate);
        }

        private void ckFleet2ShipOfLine_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2ShipOfLine, tbxFleet2ShipOfLine, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }
        private void tbxFleet2ShipOfLine_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2ShipOfLine);
        }

        private void ckFleet2Eastindiaman_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2Eastindiaman, tbxFleet2Eastindiaman, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2Eastindiaman_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2Eastindiaman);
        }

        private void ckFleet2ArmoredFrigate_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2ArmoredFrigate, tbxFleet2ArmoredFrigate, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2ArmoredFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2ArmoredFrigate);
        }

        private void ckFleet2AsianShip_Click(object sender, RoutedEventArgs e)
        {
            SelectAnUnit(ckFleet2AsianShip, tbxFleet2AsianShip, ref fleet2CheckBoxList, ref fleet2TextboxList);
        }

        private void tbxFleet2AsianShip_LostFocus(object sender, RoutedEventArgs e)
        {
            VerifyNumberOfUnits(tbxFleet2AsianShip);
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
                unitName = Convert.ToString(ckList[i].Content);
                numOfUnits = Convert.ToInt32(tbxList[i].Text);
                switch (unitName)
                {
                    //jednostki startowe
                    case "Pikinierzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 5, 15, 30, 0, 4, 170, 100, 4, "MeleeInfantry", numOfUnits));
                        break;
                    case "Arkebuzerzy":
                        armyList.Add(new LandUnit(unitName, 0, 10, 20, 0, 8, 0, 0, 3, 160, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Łucznicy":
                        armyList.Add(new LandUnit(unitName, 0, 13, 15, 0, 8, 0, 0, 3, 150, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Kusznicy":
                        armyList.Add(new LandUnit(unitName, 0, 11, 17, 0, 8, 0, 5, 3, 160, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Rycerze":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 25, 23, 5, 10, 6, 200, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Konnica":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 15, 19, 5, 10, 6, 165, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Bombarda":
                        armyList.Add(new LandUnit(unitName, 12, 10, 0, 0, 4, 0, 10, 1, 150, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka eksploracji
                    case "Piki i arkebuzerzy":
                        armyList.Add(new LandUnit(unitName, 0, 10, 20, 5, 16, 30, 0, 4, 180, 100, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Ciężcy husarze":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 30, 25, 5, 10, 6, 190, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Reiterzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 14, 10, 18, 5, 10, 6, 200, 110, 6, "RangerCavalry", numOfUnits));
                        break;
                    case "Armata polowa":
                        armyList.Add(new LandUnit(unitName, 0, 22, 38, 0, 4, 0, 10, 2, 150, 100, 2, "FieldGuns", numOfUnits));
                        break;
                    case "Ciężka armata":
                        armyList.Add(new LandUnit(unitName, 20, 24, 12, 0, 4, 0, 10, 1, 150, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka ekspansji
                    case "Piki i muszkieterzy":
                        armyList.Add(new LandUnit(unitName, 0, 17, 28, 5, 16, 30, 0, 4, 180, 100, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Wcześni fusilierzy":
                        armyList.Add(new LandUnit(unitName, 0, 25, 34, 0, 13, 0, 10, 4, 160, 110, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Wcześni kirasjerzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 18, 26, 5, 10, 6, 185, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Harkebuzerzy":
                        armyList.Add(new LandUnit(unitName, 0, 12, 23, 10, 18, 5, 10, 6, 170, 100, 6, "RangerCavalry", numOfUnits));
                        break;
                    case "Lansjerzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 23, 26, 5, 10, 6, 170, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Haubica oblężnicza":
                        armyList.Add(new LandUnit(unitName, 28, 34, 14, 0, 4, 0, 10, 1, 150, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka imperiow
                    case "Fusilierzy":
                        armyList.Add(new LandUnit(unitName, 0, 28, 36, 13, 20, 23, 15, 4, 180, 100, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Grenadierzy":
                        armyList.Add(new LandUnit(unitName, 0, 28, 44, 14, 26, 24, 15, 4, 180, 110, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Milicja":
                        armyList.Add(new LandUnit(unitName, 0, 25, 32, 7, 18, 5, 18, 4, 150, 80, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Karbinerzy":
                        armyList.Add(new LandUnit(unitName, 0, 24, 30, 10, 19, 5, 10, 6, 170, 100, 6, "RangerCavalry", numOfUnits));
                        break;
                    case "Dragoni":
                        armyList.Add(new LandUnit(unitName, 0, 28, 36, 11, 20, 5, 14, 4, 170, 100, 6, "MobileRangerInfantry", numOfUnits));
                        break;
                    case "Huzarzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 17, 27, 5, 14, 6, 170, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Kirasjerzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 18, 29, 5, 14, 6, 185, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Działa polowe":
                        armyList.Add(new LandUnit(unitName, 0, 34, 51, 0, 6, 0, 12, 2, 150, 100, 2, "FieldGuns", numOfUnits));
                        break;
                    case "Moździerze":
                        armyList.Add(new LandUnit(unitName, 35, 32, 0, 0, 4, 0, 12, 1, 150, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka rewolucji
                    case "Lekka piechota":
                        armyList.Add(new LandUnit(unitName, 0, 30, 39, 10, 22, 9, 23, 4, 170, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Piechota liniowa":
                        armyList.Add(new LandUnit(unitName, 0, 31, 42, 13, 22, 15, 15, 4, 180, 100, 4, "LineInfantry", numOfUnits));
                        break;
                    case "Haubice polowe":
                        armyList.Add(new LandUnit(unitName, 23, 42, 54, 0, 6, 0, 12, 2, 150, 100, 2, "FieldGuns", numOfUnits));
                        break;
                    //jednostki unikalne
                    case "Kozacy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 10, 15, 25, 3, 10, 6, 160, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Tubylcy wojownicy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 5, 10, 0, 0, 4, 125, 80, 4, "MeleeInfantry", numOfUnits));
                        break;
                    case "Tubylcy strzelcy":
                        armyList.Add(new LandUnit(unitName, 0, 5, 13, 0, 9, 0, 0, 4, 120, 80, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Konnica tubylców":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 12, 13, 0, 5, 6, 130, 90, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Łucznicy konni":
                        armyList.Add(new LandUnit(unitName, 0, 13, 15, 5, 18, 0, 5, 4, 160, 100, 6, "RangerInfantry", numOfUnits));
                        break;
                }
            }
        }
        public void SelectAttackingUnits(ref List<LandUnit> mainArmyList, ref List<LandUnit> attackingUnits)
        {
            attackingUnits.Clear();
            int maxUnitCount= 0;
            foreach(LandUnit unit in mainArmyList)
            {
                maxUnitCount += unit.NumberOf;
            }
            maxUnitCount /= 6;
            int numOfAttackers = 0;
            int i = 0;
            bool quitLoop = false;
            while( numOfAttackers < maxUnitCount )
            {
                foreach(LandUnit unit in mainArmyList)
                {
                    if (!attackingUnits.Contains(unit) && unit.Name != "SiegeArtillery" && unit.Name != "FieldGuns" && unit.Initiative > 1 && unit.NumberOf > 0)
                    {
                        attackingUnits.Add(unit);
                        numOfAttackers += unit.NumberOf;
                    }
                    else if (i >= mainArmyList.Count - 1)
                    {
                        quitLoop = true;
                        break;
                    }
                    if ( numOfAttackers > maxUnitCount)
                    {
                        //odzielamy liczbe jednostek nieuzywanych
                        mainArmyList.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.Health, unit.Morale, unit.Speed, unit.Type, numOfAttackers - maxUnitCount));
                        unit.NumberOf -= numOfAttackers - maxUnitCount;
                        numOfAttackers = maxUnitCount;
                        break;
                    }
                    i++;
                }
                if (numOfAttackers >= maxUnitCount || quitLoop)
                {
                    break;
                }
            }
        }
        public void SelectDefendingUnits(ref List<LandUnit> mainArmyList, ref List<LandUnit> defendingUnits)
        {
            defendingUnits.Clear();
            int maxUnitCount = 0;
            bool armyHasCannons = false, defendersHaveCannons = false;
            foreach (LandUnit unit in mainArmyList)
            {
                maxUnitCount += unit.NumberOf;
                if(unit.Type == "SiegeArtillery" || unit.Type == "FieldGun")
                {
                    armyHasCannons = true;
                }
            }
            maxUnitCount /= 6;
            int numOfDefenders = 0;
            int i = 0;
            bool quitLoop = false;
            while (numOfDefenders < maxUnitCount)
            {
                foreach (LandUnit unit in mainArmyList)
                {
                    if (!defendingUnits.Contains(unit) && unit.Initiative > 0 && unit.NumberOf > 0 && unit.Name == "ChargeCavalry" && unit.Name == "RangerCavalry")
                    {
                        defendingUnits.Add(unit);
                        numOfDefenders += unit.NumberOf;
                    }
                    else if (i >= mainArmyList.Count - 1)
                    {
                        quitLoop = true;
                        break;
                    }
                    if (numOfDefenders > maxUnitCount)
                    {
                        //odzielamy liczbe jednostek nieuzywanych
                        mainArmyList.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.Health, unit.Morale, unit.Speed, unit.Type, numOfDefenders - maxUnitCount));
                        unit.NumberOf -= numOfDefenders - maxUnitCount;
                        numOfDefenders = maxUnitCount;
                        break;
                    }
                    i++;
                }
                if (numOfDefenders >= maxUnitCount || quitLoop)
                {
                    break;
                }
            }
            if(numOfDefenders < maxUnitCount) // jesli nie ma wystarczajaco duzo obroncow wybierze tez kawalerie
            {
                quitLoop = false;
                i = 0;
                while (numOfDefenders < maxUnitCount)
                {
                    foreach (LandUnit unit in mainArmyList)
                    {
                        if (!defendingUnits.Contains(unit) && unit.Initiative > 0 && unit.NumberOf > 0)
                        {
                            defendingUnits.Add(unit);
                            numOfDefenders += unit.NumberOf;
                        }
                        else if (i >= mainArmyList.Count - 1)
                        {
                            quitLoop = true;
                            break;
                        }
                        if (numOfDefenders > maxUnitCount)
                        {
                            //odzielamy liczbe jednostek nieuzywanych
                            mainArmyList.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.Health, unit.Morale, unit.Speed, unit.Type, numOfDefenders - maxUnitCount));
                            unit.NumberOf -= numOfDefenders - maxUnitCount;
                            numOfDefenders = maxUnitCount;
                            break;
                        }
                        i++;
                    }
                    if (numOfDefenders >= maxUnitCount || quitLoop)
                    {
                        break;
                    }
                }
            }
            if (armyHasCannons && !defendersHaveCannons)
            {
                LandUnit lastlyAddedUnitCopyUnit = defendingUnits[defendingUnits.Count - 1];
                int maxMinusLastUnit = 5;
                while (true)
                {
                    if (maxMinusLastUnit >= lastlyAddedUnitCopyUnit.NumberOf)
                    {
                        maxMinusLastUnit--;
                    }
                    else
                    {
                        if (maxMinusLastUnit <= 0)
                        {
                            defendingUnits.RemoveAt(defendingUnits.Count - 1);
                            maxMinusLastUnit = 1;
                        }
                        break;
                    }
                }
                mainArmyList.Add(new LandUnit(lastlyAddedUnitCopyUnit.Name, lastlyAddedUnitCopyUnit.LongRange, lastlyAddedUnitCopyUnit.MediumRange, lastlyAddedUnitCopyUnit.LowRange, lastlyAddedUnitCopyUnit.ShockAttack, lastlyAddedUnitCopyUnit.Melee, lastlyAddedUnitCopyUnit.ShockDef, lastlyAddedUnitCopyUnit.ArtilleryDef, lastlyAddedUnitCopyUnit.Initiative, lastlyAddedUnitCopyUnit.Health, lastlyAddedUnitCopyUnit.Morale, lastlyAddedUnitCopyUnit.Speed, lastlyAddedUnitCopyUnit.Type, lastlyAddedUnitCopyUnit.NumberOf - (lastlyAddedUnitCopyUnit.NumberOf - maxMinusLastUnit)));
                lastlyAddedUnitCopyUnit.NumberOf -= numOfDefenders - maxMinusLastUnit;
                numOfDefenders -= maxMinusLastUnit;
                i = 0;
                quitLoop = false;
                while(true)
                {
                    foreach (LandUnit unit in mainArmyList)
                    {
                        if (!defendingUnits.Contains(unit) && unit.Initiative > 0 && unit.NumberOf > 0 && (unit.Name == "SiegeArtillery" || unit.Name == "FieldGuns"))
                        {
                            defendingUnits.Add(unit);
                            numOfDefenders += unit.NumberOf;
                        }
                        else if (i >= mainArmyList.Count - 1)
                        {
                            quitLoop = true;
                            break;
                        }
                        if (numOfDefenders > maxUnitCount)
                        {
                            //odzielamy liczbe jednostek nieuzywanych
                            mainArmyList.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.Health, unit.Morale, unit.Speed, unit.Type, numOfDefenders - maxUnitCount));
                            unit.NumberOf -= numOfDefenders - maxUnitCount;
                            numOfDefenders = maxUnitCount;
                            break;
                        }
                        i++;
                        if (numOfDefenders >= maxUnitCount || quitLoop)
                        {
                            break;
                        }
                    }
                }
            }
        }
        public void SelectBombardingUnits(ref List<LandUnit> mainArmyList, ref List<LandUnit> bombardingUnits, bool allowFieldGuns = false)
        {
            bombardingUnits.Clear();
            foreach (LandUnit unit in  mainArmyList)
            {
                if (allowFieldGuns && (unit.Type == "SiegeArtillery" || unit.Type == "FieldGuns")) 
                {
                    bombardingUnits.Add(unit);
                }
                else if(unit.Type == "SiegeArtillery")
                {
                    bombardingUnits.Add(unit);
                }
            }
        }
        public void MergeLandUnits(ref List<LandUnit> armyList)
        {
            for (int i = 0; i < armyList.Count; i++)
            {
                for(int j = 0; j < armyList.Count; j++) 
                {
                    if (armyList[i].Name == armyList[j].Name && j != i)
                    {
                        armyList[i].NumberOf += armyList[j].NumberOf;
                        armyList.RemoveAt(j);
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
        public void DealDamageToLandunits(ref List<LandUnit> attackerList, ref List<LandUnit> defenderList, TypeOfDamage typeOfdamage, bool isRiverBeingCrossed = false)
        {
            string damageType = typeOfdamage.ToString();
            int attackerDamage = 0;
            int defenderDamage = 0;
            List<LandUnit> attackers = new List<LandUnit>(attackerList);
            if (damageType == "ChargeMelee" && isRiverBeingCrossed)
            {
                attackers.Clear();
                int i = 0;
                foreach (LandUnit unit in attackerList)
                {
                    attackers.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack - 10, unit.Melee - 5, unit.ShockDef, unit.ArtilleryDef, unit.Initiative, unit.Health, unit.Morale, unit.Speed, unit.Type, unit.NumberOf));
                    if (attackers[i].ShockAttack < 0) attackers[i].ShockAttack = 0;
                    if (attackers[i].Melee < 3) attackers[i].Melee = 3;
                    i++;
                }
            }
            if (damageType == "LongRange")
            {
                foreach(LandUnit unit in attackers)
                {
                    attackerDamage += unit.LongRange;
                }
                if(attackerDamage > 0) attackerDamage /= defenderList.Count;
                foreach(LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if(unit.Health <= 0)
                    {
                        while(unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
            }
            else if(damageType == "LongRangeRet")
            {
                foreach (LandUnit unit in attackers)
                {
                    attackerDamage += unit.LongRange;
                }
                if (attackerDamage > 0) attackerDamage /= defenderList.Count;
                if (attackerDamage > 0) attackerDamage /= 2;
                //1 polowa ostrzalu atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
                //ostrzal broniacvych sie
                foreach (LandUnit unit in defenderList)
                {
                    defenderDamage += unit.LongRange;
                }
                if (defenderDamage > 0) defenderDamage /= attackers.Count;
                foreach (LandUnit unit in attackers)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
                //2 polowa ostrzalu atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
            }
            else if (damageType == "MidRange")
            {
                foreach (LandUnit unit in attackers)
                {
                    attackerDamage += unit.MediumRange;
                }
                if (attackerDamage > 0) attackerDamage /= defenderList.Count;
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if(unit.Morale <= 0) unit.Morale = 0;
                }
            }
            else if(damageType == "MidRangeRet")
            {
                foreach (LandUnit unit in attackers)
                {
                    attackerDamage += unit.MediumRange;
                }
                if (attackerDamage > 0) attackerDamage /= defenderList.Count;
                if (attackerDamage > 0) attackerDamage /= 2;
                //1 polowa ostrzalu atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
                //ostrzal broniacvych sie
                foreach (LandUnit unit in defenderList)
                {
                    defenderDamage += unit.MediumRange;
                }
                if (defenderDamage > 0) defenderDamage /= attackers.Count;
                foreach (LandUnit unit in attackers)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
                //2 polowa ostrzalu atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
            }
            else if(damageType == "LowRange")
            {
                foreach (LandUnit unit in attackers)
                {
                    attackerDamage += unit.LowRange;
                }
                if (attackerDamage > 0) attackerDamage /= defenderList.Count;
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
            }
            else if(damageType == "LowRangeRet")
            {
                foreach (LandUnit unit in attackers)
                {
                    attackerDamage += unit.LowRange;
                }
                if (attackerDamage > 0) attackerDamage /= defenderList.Count;
                if (attackerDamage > 0) attackerDamage /= 2;
                //1 polowa ostrzalu atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
                //ostrzal broniacvych sie
                foreach (LandUnit unit in defenderList)
                {
                    defenderDamage += unit.LowRange;
                }
                if (defenderDamage > 0) defenderDamage /= attackers.Count;
                foreach (LandUnit unit in attackers)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
                //2 polowa ostrzalu atakujacych
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
            }
            else if(damageType == "ChargeMelee")
            {
                foreach (LandUnit unit in attackers)
                {
                    attackerDamage += unit.Melee + unit.ShockAttack;
                    if (unit.LowRange > 0) attackerDamage += unit.LowRange / 2;
                }
                defenderList.Sort((x, y) => y.ShockDef.CompareTo(x.ShockDef)); // sortowanie broniacych sie po obronie przeciw szarże
                List<LandUnit> defensiveCharge = new List<LandUnit>();
                foreach (LandUnit unit in defenderList)
                {
                    if(unit.Type == "ChargeCavalry")
                    {
                        defensiveCharge.Add(unit);
                        defenderDamage += unit.Melee + unit.ShockAttack;
                        if(unit.LowRange > 0) defenderDamage += unit.LowRange / 2;
                    }
                }
                if(defensiveCharge.Count > 0)
                {
                    if(attackers.Count > 0) defenderDamage /= attackers.Count;
                    int localAttackerDamage = attackerDamage / defensiveCharge.Count;
                    for (int i = 0; i < defensiveCharge.Count; i++)
                    {
                        defensiveCharge[i].Health -= localAttackerDamage - defensiveCharge[i].ShockDef;
                        defensiveCharge[i].Morale -= (localAttackerDamage - defensiveCharge[i].ShockDef) / 2;
                        if (defensiveCharge[i].Health <= 0)
                        {
                            while(defensiveCharge[i].Health <= 0 && defensiveCharge[i].NumberOf > 0)
                            {
                                defensiveCharge[i].Health += defensiveCharge[i].MaxHealth;
                                defensiveCharge[i].NumberOf--;
                                defensiveCharge[i].Morale -= 5;
                            }
                        }
                        if (defensiveCharge[i].Morale < 0) defensiveCharge[i].Morale = 0;
                        if (i < attackers.Count)
                        {
                            attackers[i].Health -= defenderDamage; // zada tylko czesci armii ale moze byc
                            attackers[i].Morale -= (defenderDamage - attackers[i].ShockDef) / 2;
                            if (attackers[i].Health <= 0)
                            {
                                while (attackers[i].Health <= 0 && attackers[i].NumberOf > 0)
                                {
                                    attackers[i].Health += attackers[i].MaxHealth;
                                    attackers[i].NumberOf--;
                                    attackers[i].Morale -= 5;
                                }
                            }
                            if (attackers[i].Morale < 0) attackers[i].Morale = 0;
                        }
                    }
                }
                List<LandUnit> frontDefense = new List<LandUnit>();
                int frontDefenseCount = 0;
                defenderDamage = 0;
                foreach (LandUnit unit in defenderList)
                {
                    if (unit.Type == "MeleeInfantry" || unit.Type == "LineInfantry")
                    {
                        frontDefense.Add(unit);
                        defenderDamage += unit.Melee + unit.LowRange;
                        if (unit.ShockDef > 0) defenderDamage += unit.ShockDef / 2;
                        frontDefenseCount += unit.NumberOf;
                    }
                }
                if(frontDefenseCount > defenderList.Count / 3)
                {
                    attackerDamage /= frontDefenseCount;
                    foreach (LandUnit unit in frontDefense)
                    {
                        unit.Health -= attackerDamage - unit.ShockDef;
                        unit.Morale -= (attackerDamage - unit.ShockDef) / 2;
                        if (unit.Health <= 0)
                        {
                            while (unit.Health <= 0 && unit.NumberOf > 0)
                            {
                                unit.Health += unit.MaxHealth;
                                unit.NumberOf--;
                                unit.Morale -= 5;
                            }
                        }
                        if (unit.Morale <= 0) unit.Morale = 0;
                    }
                    foreach (LandUnit unit in attackers)
                    {
                        unit.Health -= defenderDamage - unit.ShockDef;
                        unit.Morale -= (defenderDamage - unit.ShockDef) / 2;
                        if (unit.Health <= 0)
                        {
                            while (unit.Health <= 0 && unit.NumberOf > 0)
                            {
                                unit.Health += unit.MaxHealth;
                                unit.NumberOf--;
                                unit.Morale -= 5;
                            }
                        }
                        if (unit.Morale <= 0) unit.Morale = 0;
                    }
                }
                else
                {
                    defenderDamage = 0;
                    if (defenderList.Count > 0) attackerDamage /= defenderList.Count;
                    foreach (LandUnit unit in defenderList)
                    {
                        defenderDamage += unit.Melee + unit.LowRange;
                        if (unit.ShockDef > 0) defenderDamage += unit.ShockDef / 2;
                        frontDefenseCount += unit.NumberOf;
                    }
                    foreach (LandUnit unit in defenderList)
                    {
                        unit.Health -= attackerDamage - unit.ShockDef;
                        unit.Morale -= (attackerDamage - unit.ShockDef) / 2;
                        if (unit.Health <= 0)
                        {
                            while (unit.Health <= 0 && unit.NumberOf > 0)
                            {
                                unit.Health += unit.MaxHealth;
                                unit.NumberOf--;
                                unit.Morale -= 5;
                            }
                        }
                        if (unit.Morale <= 0) unit.Morale = 0;
                    }
                    foreach (LandUnit unit in attackers)
                    {
                        unit.Health -= defenderDamage - unit.ShockDef;
                        unit.Morale -= (defenderDamage - unit.ShockDef) / 2;
                        if (unit.Health <= 0)
                        {
                            while (unit.Health <= 0 && unit.NumberOf > 0)
                            {
                                unit.Health += unit.MaxHealth;
                                unit.NumberOf--;
                                unit.Morale -= 5;
                            }
                        }
                        if (unit.Morale <= 0) unit.Morale = 0;
                    }
                }
            }
            else if(damageType == "MeleeRet")
            {
                foreach(LandUnit unit in attackers)
                {
                    attackerDamage += unit.Melee;
                    if(unit.LowRange > 0) attackerDamage += unit.LowRange / 3;
                    if (unit.ShockAttack > 0) attackerDamage += unit.ShockAttack / 3;
                }
                attackerDamage /= defenderList.Count;
                foreach (LandUnit unit in defenderList)
                {
                    defenderDamage += unit.Melee;
                    if (unit.LowRange > 0) defenderDamage += unit.LowRange / 3;
                    if (unit.ShockAttack > 0) defenderDamage += unit.ShockAttack / 3;
                }
                defenderDamage /= attackers.Count;
                foreach (LandUnit unit in defenderList)
                {
                    unit.Health -= attackerDamage - unit.ShockDef;
                    unit.Morale -= (attackerDamage - unit.ShockDef) / 2;
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
                foreach (LandUnit unit in attackers)
                {
                    unit.Health -= defenderDamage - unit.ShockDef;
                    unit.Morale -= (defenderDamage - unit.ShockDef) / 2;
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
            }
            else if (damageType == "Melee")
            {
                foreach (LandUnit unit in attackers)
                {
                    attackerDamage += unit.Melee;
                    if (unit.ShockAttack > 0) attackerDamage += unit.ShockAttack / 3;
                    if (unit.LowRange > 0) attackerDamage += unit.LowRange / 3;
                }
                if (attackerDamage > 0) attackerDamage /= defenderList.Count;
                foreach (LandUnit unit in defenderList)
                {
                    if (attackerDamage - unit.ArtilleryDef > 0)
                    {
                        unit.Health -= attackerDamage - unit.ArtilleryDef;
                        unit.Morale -= (attackerDamage - unit.ArtilleryDef) / 2;
                    }
                    if (unit.Health <= 0)
                    {
                        while (unit.Health <= 0 && unit.NumberOf > 0)
                        {
                            unit.Health += unit.MaxHealth;
                            unit.NumberOf--;
                            unit.Morale -= 5;
                        }
                    }
                    if (unit.Morale <= 0) unit.Morale = 0;
                }
            }
        }
        public void RetreatInHaste(ref List<LandUnit> retreatingArmy)
        {
            List<LandUnit> artilleryList = new List<LandUnit>();
            SelectBombardingUnits(ref retreatingArmy, ref artilleryList);
            foreach (LandUnit unit in artilleryList)
            {
                if(unit.Type == "SiegeArtillery") unit.Health -= unit.MaxHealth / 2;
                else if (unit.Type == "FieldGuns") unit.Health -= unit.MaxHealth / 4;
                while(unit.Health < 0 && unit.NumberOf > 0)
                {
                    unit.Health += unit.MaxHealth;
                    unit.NumberOf--;
                }
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

        void VerifyInputLists(ref List<CheckBox> ckList, ref List<TextBox> tbxList)
        {
            int input;
            List<int> indexesToBeDeleted = new List<int>();
            for (int i = 0; i < ckList.Count; i++)
            {
                input = Convert.ToInt32(tbxList[i].Text);
                if (input <= 0)
                {
                    ckList[i].IsChecked = false;
                    tbxList[i].IsEnabled = false;
                    
                }
            }
            foreach(int index  in indexesToBeDeleted)
            {
                ckList.RemoveAt(index);
                tbxList.RemoveAt(index);
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
                    int army1AvgMorale = GetArmyMorale(army1UnitsList), army2AvgMorale = GetArmyMorale(army2UnitsList);
                    List<LandUnit> actingUnitsFromArmy1 = new List<LandUnit>();
                    List<LandUnit> actingUnitsFromArmy2 = new List<LandUnit>();
                    
                    if(fortLevel == 0) 
                    {
                        //atak bez fortu
                        int army1CountBefore, army2CountBefore;
                        int army1Losses = 0, army2Losses = 0;
                        MoraleModification(ref army1UnitsList, 10, true);
                        int army1AvgInitiative, army2AvgInitiative;
                        battleLog += "Bitwa sie rozpoczyna\n";
                        while(!team1Win && !team2Win)
                        {
                            MoraleModification(ref actingUnitsFromArmy1, 5);
                            MoraleModification(ref actingUnitsFromArmy2, 5);
                            UpdateInitiative(ref army1UnitsList);
                            UpdateInitiative(ref army2UnitsList);
                            army1AvgInitiative = GetArmyInitiative(army1UnitsList);
                            if (isSkirmishAttack) army1AvgInitiative += 4;
                            army2AvgInitiative = GetArmyInitiative(army2UnitsList);
                            army1CountBefore = GetArmyCount(army1UnitsList);
                            army2CountBefore = GetArmyCount(army2UnitsList);
                            //warunki zakonczenia bitwy
                            //przewagi liczebne
                            if(GetArmyCount(army1UnitsList) > GetArmyCount(army2UnitsList) && GetArmyCount(army1UnitsList) - GetArmyCount(army2UnitsList) > 30)
                            {
                                //przewaga liczebna armii 1
                                battleLog += "Armia 2 wycofuje sie ze wzgledu na przewage liczebna armii 1";
                                SelectBombardingUnits(ref army1UnitsList, ref actingUnitsFromArmy1, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.LongRange);
                                team1Win = true;
                                break;
                            }
                            else if(GetArmyCount(army1UnitsList) < GetArmyCount(army2UnitsList) && GetArmyCount(army2UnitsList) - GetArmyCount(army1UnitsList) > 30)
                            {
                                //przewaga liczebna armii 2
                                battleLog += "Armia 1 wycofuje sie ze wzgledu na przewage liczebna armii 2";
                                SelectBombardingUnits(ref army2UnitsList, ref actingUnitsFromArmy2, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.LongRange);
                                team2Win = true;
                                break;
                            }
                            //straty w porownaniu
                            else if(army1Losses > army2Losses && army1Losses - army2Losses > 20)
                            {
                                //zbyt duze straty armii 1
                                battleLog += "Armia 1 wycofuje sie ze wzgledu przewazajace straty";
                                SelectBombardingUnits(ref army2UnitsList, ref actingUnitsFromArmy2, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.LongRange);
                                team2Win = true;
                                break;
                            }
                            else if (army1Losses < army2Losses && army2Losses - army1Losses > 20)
                            {
                                //zbyt duze straty armii 2
                                battleLog += "Armia 2 wycofuje sie ze wzgledu przewazajace straty";
                                SelectBombardingUnits(ref army1UnitsList, ref actingUnitsFromArmy1, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.LongRange);
                                team1Win = true;
                                break;
                            }
                            //morale
                            else if(GetArmyMorale(army1UnitsList) < 20 && GetArmyMorale(army1UnitsList) > 10)
                            {
                                //zbyt male morale armii 1
                                battleLog += "Armia 1 wycofuje ze wzgledu an slabe morale";
                                SelectBombardingUnits(ref army2UnitsList, ref actingUnitsFromArmy2, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.LongRange);
                                team2Win = true;
                                break;
                            }
                            else if (GetArmyMorale(army2UnitsList) < 20 && GetArmyMorale(army2UnitsList) > 10)
                            {
                                //zbyt male morale armii 2
                                battleLog += "Armia 2 wycofuje ze wzgledu an slabe morale";
                                SelectBombardingUnits(ref army1UnitsList, ref actingUnitsFromArmy1, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.LongRange);
                                team1Win = true;
                                break;
                            }
                            //krytyczne morale
                            else if (GetArmyMorale(army1UnitsList) <= 10)
                            {
                                //zbyt male morale armii 1
                                battleLog += "Armia 1 zostala zlamana i ucieka w poplochu";
                                SelectBombardingUnits(ref army2UnitsList, ref actingUnitsFromArmy2, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref army1UnitsList, TypeOfDamage.LongRange);
                                RetreatInHaste(ref army1UnitsList);
                                team2Win = true;
                                break;
                            }
                            else if (GetArmyMorale(army2UnitsList) <= 10)
                            {
                                //zbyt male morale armii 2
                                battleLog += "Armia 2 zostala zlamana i ucieka w poplochu";
                                SelectBombardingUnits(ref army1UnitsList, ref actingUnitsFromArmy1, true);
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref army2UnitsList, TypeOfDamage.LongRange);
                                RetreatInHaste(ref army2UnitsList);
                                team1Win = true;
                                break;
                            }
                            //ataki
                            if(army1AvgInitiative == army2AvgInitiative)
                            {
                                if (isSkirmishAttack) army1AvgInitiative++;
                                else
                                {
                                    Random rnd = new Random();
                                    int num = rnd.Next(0, 2);
                                    if (num > 0) army1AvgInitiative++;
                                    else army2AvgInitiative++;
                                }
                            }
                            if (army1AvgInitiative > army2AvgInitiative)
                            {
                                battleLog += "Armia 1 ma inicjatywe i przechodzi do ataku\n Nastepuje bombardowanie\n";
                                //armia 1 ma inicjatywe
                                //bombardowanie z obu stron
                                DealDamageToLandunits(ref army1UnitsList, ref army2UnitsList, TypeOfDamage.LongRangeRet);
                                //atak
                                SelectAttackingUnits(ref army1UnitsList, ref actingUnitsFromArmy1);
                                SelectDefendingUnits(ref army2UnitsList, ref actingUnitsFromArmy2);
                                int distance = 20;
                                int attackerSpeed = GetArmySpeed(actingUnitsFromArmy1);
                                while(distance > 0 && distance - attackerSpeed >= attackerSpeed)
                                {
                                    if(distance > 12)
                                    {
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.MidRange);
                                    }
                                    else
                                    {
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.LowRangeRet, team1SpecialCondition);
                                    }
                                    distance -= attackerSpeed;
                                }
                                DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.ChargeMelee, team1SpecialCondition);
                                while(true)
                                {
                                    if (GetArmyInitiative(actingUnitsFromArmy1) > GetArmyInitiative(actingUnitsFromArmy2)) DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.MeleeRet);
                                    else DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.MeleeRet);
                                    //warunki wyjsciowe
                                    if(GetArmyCount(actingUnitsFromArmy1) - GetArmyCount(actingUnitsFromArmy2) >= 20)
                                    {
                                        battleLog += "Obrona armii 2 ucieka przez przewage liczebna wroga\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.LowRange);
                                        MoraleModification(ref army2UnitsList, -12);
                                        MoraleModification(ref army1UnitsList, 7);
                                        break;
                                    }
                                    else if (GetArmyCount(actingUnitsFromArmy2) - GetArmyCount(actingUnitsFromArmy1) >= 20)
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
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.Melee);
                                        MoraleModification(ref army1UnitsList, -15);
                                        break;
                                    }
                                    else if(GetArmyCount(actingUnitsFromArmy2) == 0)
                                    {
                                        //broniacy wybici
                                        battleLog += "Obrona armii 2 zostala wybita w pien\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.Melee);
                                        MoraleModification(ref army2UnitsList, -15);
                                        break;
                                    }
                                    else if(GetArmyMorale(actingUnitsFromArmy1) <= 10)
                                    {
                                        //atakujacy uciekaja
                                        battleLog += "Obrona armii 2 sie utrzymala\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.Melee);
                                        MoraleModification(ref army1UnitsList, -10);
                                        break;
                                    }
                                    else if(GetArmyMorale(actingUnitsFromArmy2) <= 10)
                                    {
                                        //broniacy uciekaja uciekaja
                                        battleLog += "Obrona armii 2 zostala zlamana\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.Melee);
                                        MoraleModification(ref army2UnitsList, -10);
                                        MoraleModification(ref army1UnitsList, 5);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                battleLog += "Armia 2 ma inicjatywe i przechodzi do ataku\n Nastepuje bombardowanie\n";
                                //armia 2 ma inicjatywe
                                //bombardowanie z obu stron
                                DealDamageToLandunits(ref army2UnitsList, ref army1UnitsList, TypeOfDamage.LongRangeRet);
                                //atak
                                SelectAttackingUnits(ref army1UnitsList, ref actingUnitsFromArmy1);
                                SelectDefendingUnits(ref army2UnitsList, ref actingUnitsFromArmy2);
                                int distance = 20;
                                int attackerSpeed = GetArmySpeed(actingUnitsFromArmy2);
                                while (distance > 0 && distance - attackerSpeed >= attackerSpeed)
                                {
                                    if (distance > 12)
                                    {
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.MidRange);
                                    }
                                    else
                                    {
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRangeRet, team1SpecialCondition);
                                    }
                                    distance -= attackerSpeed;
                                }
                                DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.ChargeMelee, team1SpecialCondition);
                                while (true)
                                {
                                    if (GetArmyInitiative(actingUnitsFromArmy2) > GetArmyInitiative(actingUnitsFromArmy1)) DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.MeleeRet);
                                    else DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.MeleeRet);
                                    //warunki wyjsciowe
                                    if (GetArmyCount(actingUnitsFromArmy2) - GetArmyCount(actingUnitsFromArmy1) >= 20)
                                    {
                                        battleLog += "Obrona armii 1 ucieka przez przewage liczebna wroga\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.LowRange);
                                        MoraleModification(ref army1UnitsList, -12);
                                        MoraleModification(ref army2UnitsList, 7);
                                        break;
                                    }
                                    else if (GetArmyCount(actingUnitsFromArmy1) - GetArmyCount(actingUnitsFromArmy2) >= 20)
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
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.Melee);
                                        MoraleModification(ref army2UnitsList, -15);
                                        break;
                                    }
                                    else if (GetArmyCount(actingUnitsFromArmy1) == 0)
                                    {
                                        //atakujacy uciekaja
                                        battleLog += "Obrona armii 1 zostala wybita w pien\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.Melee);
                                        MoraleModification(ref army1UnitsList, -15);
                                        break;
                                    }
                                    else if (GetArmyMorale(actingUnitsFromArmy2) <= 10)
                                    {
                                        //atakujacy uciekaja
                                        battleLog += "Obrona armii 1 sie utrzymala\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy2, ref actingUnitsFromArmy1, TypeOfDamage.Melee);
                                        MoraleModification(ref army2UnitsList, -10);
                                        break;
                                    }
                                    else if (GetArmyMorale(actingUnitsFromArmy1) <= 10)
                                    {
                                        //broniacy uciekaja uciekaja
                                        battleLog += "Obrona armii 1 zostala zlamana\n";
                                        DealDamageToLandunits(ref actingUnitsFromArmy1, ref actingUnitsFromArmy2, TypeOfDamage.Melee);
                                        MoraleModification(ref army1UnitsList, -10);
                                        MoraleModification(ref army2UnitsList, 5);
                                        break;
                                    }

                                }
                                //obliczanie strat
                                army1Losses += army1CountBefore - GetArmyCount(army1UnitsList);
                                army2Losses += army2CountBefore - GetArmyCount(army2UnitsList);
                            }
                        }
                        //zakonczenia bitwy

                    }
                    else
                    {
                        //atak na fort
                    }
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
