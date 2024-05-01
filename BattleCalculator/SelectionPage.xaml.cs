using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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


        private void changePageToResults(object sender, RoutedEventArgs e)
        {
            btnClick?.Invoke(this, EventArgs.Empty);
        }

        int fortLevel = 0;
        bool isSkirmishAttack = false;
        bool isBattleLand = false;
        bool army1SpecialCondition = false;
        bool army2SpecialCondition = false;
        TerrainType[] terrain = new TerrainType[1];
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
        void activateStartButton()
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
            activateStartButton();
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
            activateStartButton();
        }
        private void cbTerrainType_LostFocus(object sender, RoutedEventArgs e)
        {
            activateStartButton();
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
        void dropdownBtn(ref bool isBtnSelected, StackPanel stackpanel)
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
            dropdownBtn(ref startUnits1, stpStartUnits1);
        }
        private void btnExplorationEra1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref explorationUnits1, stpExplorationEra1);
        }
        private void btnExpansionEra1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref expansionUnits1, stpExpansionEra1);
        }
        private void btnEmpireEra1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref empireUnits1, stpEmpireEra1);
        }
        private void btnRevolutionEra1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref revUnits1, stpRevolutionEra1);
        }
        private void btnUniqueUnitsEra1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref uniqueUnits1, stpUniqueUnits1);
        }

        // rozwijanie epok dla armii 2
        private void btnStartUnits2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref startUnits2, stpStartUnits2);
        }
        private void btnExplorationEra2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref explorationUnits2, stpExplorationEra2);
        }
        private void btnExpansionEra2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref expansionUnits2, stpExpansionEra2);
        }
        private void btnEmpireEra2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref empireUnits2, stpEmpireEra2);
        }
        private void btnRevolutionEra2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref revUnits2, stpRevolutionEra2);
        }
        private void btnUniqueUnitsEra2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref uniqueUnits2 , stpUniqueUnits2);
        }
        // rozwijanie epok dla floty 1
        private void btnStartShips1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref startShips1 , stpStartShips1);
        }
        private void btnExplorationEraShips1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref explorationShips1 , stpExplorationEraShips1);
        }

        private void btnExpansionEraShips1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref expansionShips1 , stpExpansionEraShips1);
        }
        private void btnEmpireEraShips1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref empireShips1 , stpEmpireEraShips1);
        }
        private void btnRevolutionEraShips1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref revShips1, stpRevolutionEraShips1);
        }
        private void btnUniqueUnitsShipsEra1_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref uniqueShips1, stpUniqueShips1);
        }
        //rozwijanie epok dla floty 2
        private void btnStartShips2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref startShips2 , stpStartShips2);
        }
        private void btnExplorationEraShips2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref explorationShips2 , stpExplorationEraShips2);
        }
        private void btnExpansionEraShips2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref expansionShips2 , stpExpansionEraShips2);
        }
        private void btnEmpireEraShips2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref empireShips2 , stpEmpireEraShips2);
        }
        private void btnRevolutionEraShips2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref revShips2, stpRevolutionEraShips2);
        }
        private void btnUniqueUnitsShipsEra2_Click(object sender, RoutedEventArgs e)
        {
            dropdownBtn(ref uniqueShips2, stpUniqueShips2);
        }
        //zaznaczanie jednostek i wyznaczanie ich ilosci
        void selectAnUnit(CheckBox ckUnit, TextBox tbxUnitNum, List<CheckBox> ckList, List<TextBox> tbxList)
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
            activateStartButton();
        }
        void verifyNumberOfUnits(TextBox tbxUnitNum)
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
            selectAnUnit(ckArmy1Pikemen, tbxArmy1Pikemen, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Pikemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Pikemen);
        }
        private void ckArmy1Arquebusiers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Arquebusiers, tbxArmy1Arquebusiers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Arquebusiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Arquebusiers);
        }

        private void ckArmy1Archers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Archers, tbxArmy1Archers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Archers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Archers);
        }

        private void ckArmy1Crossbowmen_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Crossbowmen, tbxArmy1Crossbowmen, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Crossbowmen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Crossbowmen);
        }

        private void ckArmy1Knights_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Knights, tbxArmy1Knights, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Knights_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Knights);
        }
        private void ckArmy1Horsemen_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Horsemen, tbxArmy1Horsemen, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Horsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Horsemen);
        }

        private void ckArmy1Bombard_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Bombard, tbxArmy1Bombard, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Bombard_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Bombard);
        }

        private void ckArmy1PikeShotArq_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1PikeShotArq, tbxArmy1PikeShotArq, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1PikeShotArq_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1PikeShotArq);
        }

        private void ckArmy1HeavyHussars_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1HeavyHussars, tbxArmy1HeavyHussars, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1HeavyHussars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1HeavyHussars);
        }

        private void ckArmy1Cossacks_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Cossacks, tbxArmy1Cossacks, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Cossacks_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Cossacks);
        }

        private void ckArmy1Reiters_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Reiters, tbxArmy1Reiters, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Reiters_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Reiters);
        }
        private void ckArmy1FieldCannon_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1FieldCannon, tbxArmy1FieldCannon, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1FieldCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1FieldCannon);
        }

        private void ckArmy1HeavyCannon_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1HeavyCannon, tbxArmy1HeavyCannon, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1HeavyCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1HeavyCannon);
        }

        private void ckArmy1PikeShotMusk_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1PikeShotMusk, tbxArmy1PikeShotMusk, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1PikeShotMusk_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1PikeShotMusk);
        }

        private void ckArmy1EarlyFusiliers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1EarlyFusiliers, tbxArmy1EarlyFusiliers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1EarlyFusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1EarlyFusiliers);
        }

        private void ckArmy1EarlyCuirassier_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1EarlyCuirassier, tbxArmy1EarlyCuirassier, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1EarlyCuirassier_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1EarlyCuirassier);
        }

        private void ckArmy1Harquebusers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Harquebusers, tbxArmy1Harquebusers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Harquebusers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Harquebusers);
        }

        private void ckArmy1Lancers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Lancers, tbxArmy1Lancers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Lancers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Lancers);
        }

        private void ckArmy1SiegeHowitzer_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1SiegeHowitzer, tbxArmy1SiegeHowitzer, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1SiegeHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1SiegeHowitzer);
        }

        private void ckArmy1Fusiliers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Fusiliers, tbxArmy1Fusiliers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Fusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Fusiliers);
        }
        private void ckArmy1Grenadiers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Grenadiers, tbxArmy1Grenadiers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Grenadiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Grenadiers);
        }
        private void ckArmy1Militia_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Militia, tbxArmy1Militia, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Militia_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Militia);
        }

        private void ckArmy1CarbineCav_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1CarbineCav, tbxArmy1CarbineCav, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1CarbineCav_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1CarbineCav);
        }

        private void ckArmy1Dragoons_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Dragoons, tbxArmy1Dragoons, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Dragoons_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Dragoons);
        }

        private void ckArmy1Hussars_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Hussars, tbxArmy1Hussars, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Hussars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Hussars);
        }

        private void ckArmy1Cuiraissiers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Cuiraissiers, tbxArmy1Cuiraissiers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Cuiraissiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Cuiraissiers);
        }

        private void ckArmy1FieldGun_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1FieldGun, tbxArmy1FieldGun, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1FieldGun_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1FieldGun);
        }

        private void ckArmy1Mortars_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1Mortars, tbxArmy1Mortars, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Mortars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1Mortars);
        }

        private void ckArmy1LightInfantry_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1LightInfantry, tbxArmy1LightInfantry, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1LightInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1LightInfantry);
        }

        private void ckArmy1LineInfantry_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1LineInfantry, tbxArmy1LineInfantry, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1LineInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1LineInfantry);
        }

        private void ckArmy1FieldHowitzer_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1FieldHowitzer, tbxArmy1FieldHowitzer, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1FieldHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1FieldHowitzer);
        }

        private void ckArmy1TribalWarriors_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1TribalWarriors, tbxArmy1TribalWarriors, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1TribalWarriors_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1TribalWarriors);
        }

        private void ckArmy1TribalRanger_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1TribalRanger, tbxArmy1TribalRanger, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1TribalRanger_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1TribalRanger);
        }

        private void ckArmy1TribalHorsemen_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1TribalHorsemen, tbxArmy1TribalHorsemen, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1TribalHorsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1TribalHorsemen);
        }
        private void tbxArmy1HorseArcher_LostFocus(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy1HorseArcher, tbxArmy1HorseArcher, army1CheckBoxList, army1TextboxList);
        }

        private void ckArmy1HorseArcher_Click(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy1HorseArcher);
        }
        // dla floty 1
        private void ckFleet1Carrack_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1Carrack, tbxFleet1Carrack, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Carrack_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1Carrack);
        }

        private void ckFleet1Caravel_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1Caravel, tbxFleet1Caravel, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Caravel_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1Caravel);
        }

        private void ckFleet1Galley_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1Galley, tbxFleet1Galley, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Galley_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1Galley);
        }

        private void ckFleet1Galleon_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1Galleon, tbxFleet1Galleon, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Galleon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1Galleon);
        }

        private void ckFleet1Schooner_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1Schooner, tbxFleet1Schooner, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Schooner_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1Schooner);
        }

        private void ckFleet1Brig_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1Brig, tbxFleet1Brig, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Brig_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1Brig);
        }

        private void ckFleet1Frigate_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1Frigate, tbxFleet1Frigate, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Frigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1Frigate);
        }

        private void ckFleet1GreatFrigate_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1GreatFrigate, tbxFleet1GreatFrigate, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1GreatFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1GreatFrigate);
        }

        private void ckFleet1ShipOfLine_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1ShipOfLine, tbxFleet1ShipOfLine, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1ShipOfLine_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1ShipOfLine);
        }

        private void ckFleet1Eastindiaman_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1Eastindiaman, tbxFleet1Eastindiaman, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Eastindiaman_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1Eastindiaman);
        }

        private void ckFleet1ArmoredFrigate_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1ArmoredFrigate, tbxFleet1ArmoredFrigate, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1ArmoredFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1ArmoredFrigate);
        }

        private void ckFleet1AsianShip_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet1AsianShip, tbxFleet1AsianShip, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1AsianShip_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet1AsianShip);
        }
        // dla armii 1
        private void ckArmy2Pikemen_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Pikemen, tbxArmy2Pikemen, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Pikemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Pikemen);
        }
        private void ckArmy2Arquebusiers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Arquebusiers, tbxArmy2Arquebusiers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Arquebusiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Arquebusiers);
        }

        private void ckArmy2Archers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Archers, tbxArmy2Archers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Archers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Archers);
        }

        private void ckArmy2Crossbowmen_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Crossbowmen, tbxArmy2Crossbowmen, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Crossbowmen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Crossbowmen);
        }

        private void ckArmy2Knights_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Knights, tbxArmy2Knights, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Knights_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Knights);
        }
        private void ckArmy2Horsemen_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Horsemen, tbxArmy2Horsemen, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Horsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Horsemen);
        }

        private void ckArmy2Bombard_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Bombard, tbxArmy2Bombard, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Bombard_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Bombard);
        }

        private void ckArmy2PikeShotArq_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2PikeShotArq, tbxArmy2PikeShotArq, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2PikeShotArq_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2PikeShotArq);
        }

        private void ckArmy2HeavyHussars_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2HeavyHussars, tbxArmy2HeavyHussars, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2HeavyHussars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2HeavyHussars);
        }

        private void ckArmy2Cossacks_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Cossacks, tbxArmy2Cossacks, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Cossacks_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Cossacks);
        }

        private void ckArmy2Reiters_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Reiters, tbxArmy2Reiters, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Reiters_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Reiters);
        }
        private void ckArmy2FieldCannon_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2FieldCannon, tbxArmy2FieldCannon, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2FieldCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2FieldCannon);
        }

        private void ckArmy2HeavyCannon_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2HeavyCannon, tbxArmy2HeavyCannon, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2HeavyCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2HeavyCannon);
        }

        private void ckArmy2PikeShotMusk_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2PikeShotMusk, tbxArmy2PikeShotMusk, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2PikeShotMusk_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2PikeShotMusk);
        }

        private void ckArmy2EarlyFusiliers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2EarlyFusiliers, tbxArmy2EarlyFusiliers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2EarlyFusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2EarlyFusiliers);
        }

        private void ckArmy2EarlyCuirassier_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2EarlyCuirassier, tbxArmy2EarlyCuirassier, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2EarlyCuirassier_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2EarlyCuirassier);
        }

        private void ckArmy2Harquebusers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Harquebusers, tbxArmy2Harquebusers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Harquebusers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Harquebusers);
        }

        private void ckArmy2Lancers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Lancers, tbxArmy2Lancers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Lancers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Lancers);
        }

        private void ckArmy2SiegeHowitzer_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2SiegeHowitzer, tbxArmy2SiegeHowitzer, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2SiegeHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2SiegeHowitzer);
        }

        private void ckArmy2Fusiliers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Fusiliers, tbxArmy2Fusiliers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Fusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Fusiliers);
        }
        private void ckArmy2Grenadiers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Grenadiers, tbxArmy2Grenadiers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Grenadiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Grenadiers);
        }

        private void ckArmy2Militia_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Militia, tbxArmy2Militia, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Militia_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Militia);
        }

        private void ckArmy2CarbineCav_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2CarbineCav, tbxArmy2CarbineCav, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2CarbineCav_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2CarbineCav);
        }

        private void ckArmy2Dragoons_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Dragoons, tbxArmy2Dragoons, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Dragoons_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Dragoons);
        }

        private void ckArmy2Hussars_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Hussars, tbxArmy2Hussars, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Hussars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Hussars);
        }

        private void ckArmy2Cuiraissiers_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Cuiraissiers, tbxArmy2Cuiraissiers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Cuiraissiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Cuiraissiers);
        }

        private void ckArmy2FieldGun_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2FieldGun, tbxArmy2FieldGun, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2FieldGun_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2FieldGun);
        }

        private void ckArmy2Mortars_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2Mortars, tbxArmy2Mortars, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Mortars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2Mortars);
        }

        private void ckArmy2LightInfantry_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2LightInfantry, tbxArmy2LightInfantry, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2LightInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2LightInfantry);
        }

        private void ckArmy2LineInfantry_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2LineInfantry, tbxArmy2LineInfantry, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2LineInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2LineInfantry);
        }

        private void ckArmy2FieldHowitzer_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2FieldHowitzer, tbxArmy2FieldHowitzer, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2FieldHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2FieldHowitzer);
        }

        private void ckArmy2TribalWarriors_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2TribalWarriors, tbxArmy2TribalWarriors, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2TribalWarriors_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2TribalWarriors);
        }

        private void ckArmy2TribalRanger_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2TribalRanger, tbxArmy2TribalRanger, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2TribalRanger_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2TribalRanger);
        }

        private void ckArmy2TribalHorsemen_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2TribalHorsemen, tbxArmy2TribalHorsemen, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2TribalHorsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2TribalHorsemen);
        }
        private void ckArmy2HorseArcher_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckArmy2HorseArcher, tbxArmy2HorseArcher, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2HorseArcher_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxArmy2HorseArcher);
        }
        // flota 2
        private void ckFleet2Carrack_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2Carrack, tbxFleet2Carrack, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Carrack_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2Carrack);
        }
        private void ckFleet2Caravel_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2Caravel, tbxFleet2Caravel, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Caravel_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2Caravel);
        }
        private void ckFleet2Galley_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2Galley, tbxFleet2Galley, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Galley_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2Galley);
        }

        private void ckFleet2Galleon_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2Galleon, tbxFleet2Galleon, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Galleon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2Galleon);
        }

        private void ckFleet2Schooner_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2Schooner, tbxFleet2Schooner, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Schooner_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2Schooner);
        }

        private void ckFleet2Brig_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2Brig, tbxFleet2Brig, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Brig_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2Brig);
        }

        private void ckFleet2Frigate_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2Frigate, tbxFleet2Frigate, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Frigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2Frigate);
        }

        private void ckFleet2GreatFrigate_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2GreatFrigate, tbxFleet2GreatFrigate, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2GreatFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2GreatFrigate);
        }

        private void ckFleet2ShipOfLine_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2ShipOfLine, tbxFleet2ShipOfLine, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2ShipOfLine_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2ShipOfLine);
        }

        private void ckFleet2Eastindiaman_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2Eastindiaman, tbxFleet2Eastindiaman, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Eastindiaman_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2Eastindiaman);
        }

        private void ckFleet2ArmoredFrigate_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2ArmoredFrigate, tbxFleet2ArmoredFrigate, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2ArmoredFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2ArmoredFrigate);
        }

        private void ckFleet2AsianShip_Click(object sender, RoutedEventArgs e)
        {
            selectAnUnit(ckFleet2AsianShip, tbxFleet2AsianShip, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2AsianShip_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyNumberOfUnits(tbxFleet2AsianShip);
        }
        // wszystko po kliknieciu start
        void applyTerrainEffects(ref List<LandUnit> armyList)
        {
            foreach (LandUnit unit in armyList)
            {
                unit.Speed = unit.Speed - terrain[0].Mud - terrain[0].HighDiff;
                if (unit.Type == "SiegeArtillery" && unit.Speed < 1)
                {
                    unit.Speed = 1;
                }
                else if (unit.Speed < 2)
                {
                    unit.Speed = 2;
                }
                unit.ArtilleryDef += terrain[0].Concealment;
                if(unit.MediumRange > 0)
                {
                    unit.MediumRange -= terrain[0].HighDiff;
                    if (unit.MediumRange < 0) unit.MediumRange = 5;
                }
                if (unit.LongRange > 0)
                {
                    unit.LongRange -= terrain[0].HighDiff;
                    if (unit.LongRange < 0) unit.LongRange = 4;
                }
                if(unit.LowRange > 0)
                {
                    unit.LowRange -= terrain[0].Obstacles;
                    if (unit.LowRange < 0) unit.LowRange = 6;
                }
            }
        }
        void fillArmyList(List<CheckBox> ckList, List<TextBox> tbxList, ref List<LandUnit> armyList)
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
                        armyList.Add(new LandUnit(unitName, 0, 10, 20, 0, 8, 0, 0, 4, 160, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Łucznicy":
                        armyList.Add(new LandUnit(unitName, 0, 13, 15, 0, 8, 0, 0, 4, 150, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Kusznicy":
                        armyList.Add(new LandUnit(unitName, 0, 11, 17, 0, 8, 0, 5, 4, 160, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Rycerze":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 25, 22, 5, 10, 6, 200, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Konnica":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 15, 20, 5, 10, 6, 165, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Bombarda":
                        armyList.Add(new LandUnit(unitName, 12, 10, 0, 0, 4, 0, 10, 1, 150, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka eksploracji
                    case "Piki i arkebuzerzy":
                        armyList.Add(new LandUnit(unitName, 0, 10, 20, 5, 16, 30, 0, 4, 180, 100, 4, "RangerInfantry", numOfUnits));
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
                        armyList.Add(new LandUnit(unitName, 0, 17, 28, 5, 16, 30, 0, 4, 180, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Wcześni fusilierzy":
                        armyList.Add(new LandUnit(unitName, 0, 25, 34, 0, 13, 0, 10, 4, 160, 110, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Wcześni kirasjerzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 16, 25, 5, 10, 6, 185, 110, 6, "ChargeCavalry", numOfUnits));
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
                        armyList.Add(new LandUnit(unitName, 0, 28, 36, 8, 20, 15, 15, 4, 180, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Grenadierzy":
                        armyList.Add(new LandUnit(unitName, 0, 28, 44, 12, 26, 17, 15, 4, 180, 110, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Milicja":
                        armyList.Add(new LandUnit(unitName, 0, 25, 32, 4, 18, 5, 18, 4, 150, 80, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Karbinerzy":
                        armyList.Add(new LandUnit(unitName, 0, 24, 30, 10, 19, 5, 10, 6, 170, 100, 6, "RangerCavalry", numOfUnits));
                        break;
                    case "Dragoni":
                        armyList.Add(new LandUnit(unitName, 0, 28, 36, 8, 20, 5, 14, 4, 170, 100, 6, "MobileRangerInfantry", numOfUnits));
                        break;
                    case "Huzarzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 16, 27, 5, 14, 6, 170, 100, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Kirasjerzy":
                        armyList.Add(new LandUnit(unitName, 0, 0, 0, 16, 29, 5, 14, 6, 185, 110, 6, "ChargeCavalry", numOfUnits));
                        break;
                    case "Działa polowe":
                        armyList.Add(new LandUnit(unitName, 0, 34, 51, 0, 6, 0, 12, 2, 150, 100, 2, "FieldGuns", numOfUnits));
                        break;
                    case "Moździerze":
                        armyList.Add(new LandUnit(unitName, 35, 32, 0, 0, 4, 0, 12, 1, 150, 100, 1, "SiegeArtillery", numOfUnits));
                        break;
                    //epoka rewolucji
                    case "Lekka piechota":
                        armyList.Add(new LandUnit(unitName, 0, 30, 39, 5, 22, 9, 23, 4, 170, 100, 4, "RangerInfantry", numOfUnits));
                        break;
                    case "Piechota liniowa":
                        armyList.Add(new LandUnit(unitName, 0, 31, 42, 8, 22, 15, 15, 4, 180, 100, 4, "RangerInfantry", numOfUnits));
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
        void selectAttackingUnits(ref List<LandUnit> mainArmyList, ref List<LandUnit> attackingUnits)
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
            while( numOfAttackers < maxUnitCount )
            {
                foreach(LandUnit unit in mainArmyList)
                {
                    if (!attackingUnits.Contains(unit) && unit.Name != "SiegeArtillery" && unit.Name != "FieldGuns" && unit.initiative > 1 && unit.NumberOf > 0)
                    {
                        attackingUnits.Add(unit);
                        numOfAttackers += unit.NumberOf;
                    }
                    else if (i >= mainArmyList.Count - 1)
                    {
                        numOfAttackers = maxUnitCount++;//zakonczenie poszukiwania jednostek atakujacych ze wzgledu na brak jednostek spelniajacyh warunki
                        break;
                    }
                    if ( numOfAttackers > maxUnitCount)
                    {
                        //odzielamy liczbe jednostek nieuzywanych
                        mainArmyList.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.initiative, unit.Health, unit.Morale, unit.Speed, unit.Type, numOfAttackers - maxUnitCount));
                        unit.NumberOf -= numOfAttackers - maxUnitCount;
                        numOfAttackers = maxUnitCount;
                        break;
                    }
                    i++;
                }
                if (numOfAttackers >= maxUnitCount)
                {
                    break;
                }
            }
        }
        void selectDefendingUnits(ref List<LandUnit> mainArmyList, ref List<LandUnit> defendingUnits)
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
            while (numOfDefenders < maxUnitCount)
            {
                foreach (LandUnit unit in mainArmyList)
                {
                    if (!defendingUnits.Contains(unit) && unit.initiative > 0 && unit.NumberOf > 0)
                    {
                        defendingUnits.Add(unit);
                        numOfDefenders += unit.NumberOf;
                    }
                    else if (i >= mainArmyList.Count - 1)
                    {
                        numOfDefenders = maxUnitCount++;//zakonczenie poszukiwania jednostek atakujacych ze wzgledu na brak jednostek spelniajacyh warunki
                        break;
                    }
                    if (numOfDefenders > maxUnitCount)
                    {
                        //odzielamy liczbe jednostek nieuzywanych
                        mainArmyList.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.initiative, unit.Health, unit.Morale, unit.Speed, unit.Type, numOfDefenders - maxUnitCount));
                        unit.NumberOf -= numOfDefenders - maxUnitCount;
                        numOfDefenders = maxUnitCount;
                        break;
                    }
                    i++;
                }
                if(armyHasCannons && !defendersHaveCannons)
                {
                    LandUnit lastlyAddedUnitCopyUnit = defendingUnits[defendingUnits.Count - 1];
                    int maxMinusLastUnit = 5;
                    while (true)
                    {
                        if(maxMinusLastUnit >= lastlyAddedUnitCopyUnit.NumberOf)
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
                    mainArmyList.Add(new LandUnit(lastlyAddedUnitCopyUnit.Name, lastlyAddedUnitCopyUnit.LongRange, lastlyAddedUnitCopyUnit.MediumRange, lastlyAddedUnitCopyUnit.LowRange, lastlyAddedUnitCopyUnit.ShockAttack, lastlyAddedUnitCopyUnit.Melee, lastlyAddedUnitCopyUnit.ShockDef, lastlyAddedUnitCopyUnit.ArtilleryDef, lastlyAddedUnitCopyUnit.initiative, lastlyAddedUnitCopyUnit.Health, lastlyAddedUnitCopyUnit.Morale, lastlyAddedUnitCopyUnit.Speed, lastlyAddedUnitCopyUnit.Type, lastlyAddedUnitCopyUnit.NumberOf - (lastlyAddedUnitCopyUnit.NumberOf - maxMinusLastUnit)));
                    lastlyAddedUnitCopyUnit.NumberOf -= numOfDefenders - maxMinusLastUnit;
                    numOfDefenders -= maxMinusLastUnit;
                    i = 0;
                    foreach (LandUnit unit in mainArmyList)
                    {
                        if (!defendingUnits.Contains(unit) && unit.initiative > 0 && unit.NumberOf > 0 && (unit.Name == "SiegeArtillery" || unit.Name == "FieldGuns"))
                        {
                            defendingUnits.Add(unit);
                            numOfDefenders += unit.NumberOf;
                        }
                        else if (i >= mainArmyList.Count - 1)
                        {
                            numOfDefenders = maxUnitCount++;//zakonczenie poszukiwania jednostek atakujacych ze wzgledu na brak jednostek spelniajacyh warunki
                            break;
                        }
                        if (numOfDefenders > maxUnitCount)
                        {
                            //odzielamy liczbe jednostek nieuzywanych
                            mainArmyList.Add(new LandUnit(unit.Name, unit.LongRange, unit.MediumRange, unit.LowRange, unit.ShockAttack, unit.Melee, unit.ShockDef, unit.ArtilleryDef, unit.initiative, unit.Health, unit.Morale, unit.Speed, unit.Type, numOfDefenders - maxUnitCount));
                            unit.NumberOf -= numOfDefenders - maxUnitCount;
                            numOfDefenders = maxUnitCount;
                            break;
                        }
                        i++;
                    }
                }
                if (numOfDefenders >= maxUnitCount)
                {
                    break;
                }
            }
        }
        void selectBombardingUnits(ref List<LandUnit> mainArmyList, List<LandUnit> bombardingUnits, bool allowFieldGuns)
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
        void fillFleetList(List<CheckBox> ckList, List<TextBox> tbxList, ref List<Ship> fleetList)
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

        void verifyInputLists(ref List<CheckBox> ckList, ref List<TextBox> tbxList)
        {
            int input;
            for (int i = 0; i < ckList.Count; i++)
            {
                input = Convert.ToInt32(tbxList[i].Text);
                if (input <= 0)
                {
                    ckList[i].IsChecked = false;
                    tbxList[i].IsEnabled = false;
                    ckList.RemoveAt(i);
                    tbxList.RemoveAt(i);
                }
            }
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbBattleType.Text == "Bitwa lądowa")
            {
                verifyInputLists(ref army1CheckBoxList, ref army1TextboxList);
                verifyInputLists(ref army2CheckBoxList, ref army2TextboxList);
                activateStartButton();
                if (btnStart.IsEnabled)
                {
                    //tworzenie armii
                    fillArmyList(army1CheckBoxList, army1TextboxList, ref army1UnitsList);
                    fillArmyList(army2CheckBoxList, army2TextboxList, ref army2UnitsList);
                    //teren 
                    switch (cbTerrainType.Text)
                    {
                        case "Łąki":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 2, 0, 2, 0);
                            break;
                        case "Wzgórza":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 10, 0, 6, 1);
                            break;
                        case "Las":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 10, 0, 8, 0);
                            break;
                        case "Leśne wzgórza":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 18, 0, 13, 1);
                            break;
                        case "Góry":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 17, 0, 13, 4);
                            break;
                        case "Bagno":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 2, 4, 2, 0);
                            break;
                        case "Dżungla":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 15, 4, 15, 0);
                            break;
                        case "Równina":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 0, 0, 1, 0);
                            break;
                        case "Pustynia":
                            terrain[0] = new TerrainType(cbTerrainType.Text, 8, 2, 2, 1);
                            break;
                    }
                    // glowne dzialanie
                    applyTerrainEffects(ref army1UnitsList);
                    applyTerrainEffects(ref army2UnitsList);
                    changePageToResults(sender, e);
                }
            }
            else
            {
                verifyInputLists(ref fleet1CheckBoxList, ref fleet1TextboxList);
                verifyInputLists(ref fleet2CheckBoxList, ref fleet2TextboxList);
                activateStartButton();
                if (btnStart.IsEnabled)
                {
                    fleet1UnitsList.Clear();
                    fleet2UnitsList.Clear();
                    //dodanie jednostek do flot
                    fillFleetList(fleet1CheckBoxList, fleet1TextboxList, ref fleet1UnitsList);
                    fillFleetList(fleet2CheckBoxList, fleet2TextboxList, ref fleet2UnitsList);
                }

            }
        }

    }
    
}
