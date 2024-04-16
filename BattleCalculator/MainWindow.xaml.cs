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
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            switch (cbTerrainType.Text)
            {
                case "Łąki":
                    TerrainType Laki = new TerrainType(cbTerrainType.Text, 1, 1, 1, 1);
                    terrain[0] = Laki;
                    break;
                case "Wzgórza":
                    TerrainType Wzgorza = new TerrainType(cbTerrainType.Text, 2, 1, 1, 3);
                    terrain[0] = Wzgorza;
                    break;
                case "Las":
                    TerrainType Las = new TerrainType(cbTerrainType.Text, 4, 1, 3, 1);
                    terrain[0] = Las;
                    break;
                case "Leśne wzgórza":
                    TerrainType LesneWzgorza = new TerrainType(cbTerrainType.Text, 5, 1, 3, 3);
                    terrain[0] = LesneWzgorza;
                    break;
                case "Góry":
                    TerrainType Gory = new TerrainType(cbTerrainType.Text, 3, 1, 2, 6);
                    terrain[0] = Gory;
                    break;
                case "Bagno":
                    TerrainType Bagno = new TerrainType(cbTerrainType.Text, 1, 5, 3, 1);
                    terrain[0] = Bagno;
                    break;
                case "Dżungla":
                    TerrainType Dzungla = new TerrainType(cbTerrainType.Text, 6, 4, 4, 1);
                    terrain[0] = Dzungla;
                    break;
                case "Równina":
                    TerrainType Rownina = new TerrainType(cbTerrainType.Text, 1, 1, 1, 1);
                    terrain[0] = Rownina;
                    break;
                case "Pustynia":
                    TerrainType Pustynia = new TerrainType(cbTerrainType.Text, 1, 2, 1, 2);
                    terrain[0] = Pustynia;
                    break;
            }
            activateStartButton();
        }
        // rozwijanie epok dla armii 1
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
        private void btnStartUnits1_Click(object sender, RoutedEventArgs e)
        {
            if (!startUnits1)
            {
                scvStartUnits1.Visibility = Visibility.Visible;
                startUnits1 = true;
            }
            else
            {
                scvStartUnits1.Visibility = Visibility.Collapsed;
                startUnits1 = false;
            }
        }
        private void btnExplorationEra1_Click(object sender, RoutedEventArgs e)
        {
            if (!explorationUnits1)
            {
                scvExplorationEra1.Visibility = Visibility.Visible;
                explorationUnits1 = true;
            }
            else
            {
                scvExplorationEra1.Visibility = Visibility.Collapsed;
                explorationUnits1 = false;
            }
        }
        private void btnExpansionEra1_Click(object sender, RoutedEventArgs e)
        {
            if (!expansionUnits1)
            {
                scvExpansionEra1.Visibility = Visibility.Visible;
                expansionUnits1 = true;
            }
            else
            {
                scvExpansionEra1.Visibility = Visibility.Collapsed;
                expansionUnits1 = false;
            }
        }
        private void btnEmpireEra1_Click(object sender, RoutedEventArgs e)
        {
            if (!empireUnits1)
            {
                scvEmpireEra1.Visibility = Visibility.Visible;
                empireUnits1 = true;
            }
            else
            {
                scvEmpireEra1.Visibility = Visibility.Collapsed;
                empireUnits1 = false;
            }
        }
        private void btnRevolutionEra1_Click(object sender, RoutedEventArgs e)
        {
            if (!revUnits1)
            {
                scvRevolutionEra1.Visibility = Visibility.Visible;
                revUnits1 = true;
            }
            else
            {
                scvRevolutionEra1.Visibility = Visibility.Collapsed;
                revUnits1 = false;
            }
        }
        private void btnUniqueUnitsEra1_Click(object sender, RoutedEventArgs e)
        {
            if (!uniqueUnits1)
            {
                scvUniqueUnitsEra1.Visibility = Visibility.Visible;
                uniqueUnits1 = true;
            }
            else
            {
                scvUniqueUnitsEra1.Visibility = Visibility.Collapsed;
                uniqueUnits1 = false;
            }
        }

        // rozwijanie epok dla armii 2
        private void btnStartUnits2_Click(object sender, RoutedEventArgs e)
        {
            if (!startUnits2)
            {
                scvStartUnits2.Visibility = Visibility.Visible;
                startUnits2 = true;
            }
            else
            {
                scvStartUnits2.Visibility = Visibility.Collapsed;
                startUnits2 = false;
            }
        }
        private void btnExplorationEra2_Click(object sender, RoutedEventArgs e)
        {
            if (!explorationUnits2)
            {
                scvExplorationEra2.Visibility = Visibility.Visible;
                explorationUnits2 = true;
            }
            else
            {
                scvExplorationEra2.Visibility = Visibility.Collapsed;
                explorationUnits2 = false;
            }
        }
        private void btnExpansionEra2_Click(object sender, RoutedEventArgs e)
        {
            if (!expansionUnits2)
            {
                scvExpansionEra2.Visibility = Visibility.Visible;
                expansionUnits2 = true;
            }
            else
            {
                scvExpansionEra2.Visibility = Visibility.Collapsed;
                expansionUnits2 = false;
            }
        }
        private void btnEmpireEra2_Click(object sender, RoutedEventArgs e)
        {
            if (!empireUnits2)
            {
                scvEmpireEra2.Visibility = Visibility.Visible;
                empireUnits2 = true;
            }
            else
            {
                scvEmpireEra2.Visibility = Visibility.Collapsed;
                empireUnits2 = false;
            }
        }
        private void btnRevolutionEra2_Click(object sender, RoutedEventArgs e)
        {
            if (!revUnits2)
            {
                scvRevolutionEra2.Visibility = Visibility.Visible;
                revUnits2 = true;
            }
            else
            {
                scvRevolutionEra2.Visibility = Visibility.Collapsed;
                revUnits2 = false;
            }
        }
        private void btnUniqueUnitsEra2_Click(object sender, RoutedEventArgs e)
        {
            if (!uniqueUnits2)
            {
                scvUniqueUnitsEra2.Visibility = Visibility.Visible;
                uniqueUnits2 = true;
            }
            else
            {
                scvUniqueUnitsEra2.Visibility = Visibility.Collapsed;
                uniqueUnits2 = false;
            }
        }
        // rozwijanie epok dla floty 1
        private void btnStartShips1_Click(object sender, RoutedEventArgs e)
        {
            if (!startShips1)
            {
                scvStartShips1.Visibility = Visibility.Visible;
                startShips1 = true;
            }
            else
            {
                scvStartShips1.Visibility = Visibility.Collapsed;
                startShips1 = false;
            }
        }
        private void btnExplorationEraShips1_Click(object sender, RoutedEventArgs e)
        {
            if (!explorationShips1)
            {
                scvExplorationEraShips1.Visibility = Visibility.Visible;
                explorationShips1 = true;
            }
            else
            {
                scvExplorationEraShips1.Visibility = Visibility.Collapsed;
                explorationShips1 = false;
            }
        }

        private void btnExpansionEraShips1_Click(object sender, RoutedEventArgs e)
        {
            if (!expansionShips1)
            {
                scvExpansionEraShips1.Visibility = Visibility.Visible;
                expansionShips1 = true;
            }
            else
            {
                scvExpansionEraShips1.Visibility = Visibility.Collapsed;
                expansionShips1 = false;
            }
        }
        private void btnEmpireEraShips1_Click(object sender, RoutedEventArgs e)
        {
            if (!empireShips1)
            {
                scvEmpireEraShips1.Visibility = Visibility.Visible;
                empireShips1 = true;
            }
            else
            {
                scvEmpireEraShips1.Visibility = Visibility.Collapsed;
                empireShips1 = false;
            }
        }
        private void btnRevolutionEraShips1_Click(object sender, RoutedEventArgs e)
        {
            if (!revShips1)
            {
                scvRevolutionEraShips1.Visibility = Visibility.Visible;
                revShips1 = true;
            }
            else
            {
                scvRevolutionEraShips1.Visibility = Visibility.Collapsed;
                revShips1 = false;
            }
        }
        private void btnUniqueUnitsShipsEra1_Click(object sender, RoutedEventArgs e)
        {
            if (!uniqueShips1)
            {
                scvUniqueShips1.Visibility = Visibility.Visible;
                uniqueShips1 = true;
            }
            else
            {
                scvUniqueShips1.Visibility = Visibility.Collapsed;
                uniqueShips1 = false;
            }
        }
        //rozwijanie epok dla floty 2
        private void btnStartShips2_Click(object sender, RoutedEventArgs e)
        {
            if (!startShips2)
            {
                scvStartShips2.Visibility = Visibility.Visible;
                startShips2 = true;
            }
            else
            {
                scvStartShips2.Visibility = Visibility.Collapsed;
                startShips2 = false;
            }
        }
        private void btnExplorationEraShips2_Click(object sender, RoutedEventArgs e)
        {
            if (!explorationShips2)
            {
                scvExplorationEraShips2.Visibility = Visibility.Visible;
                explorationShips2 = true;
            }
            else
            {
                scvExplorationEraShips2.Visibility = Visibility.Collapsed;
                explorationShips2 = false;
            }
        }
        private void btnExpansionEraShips2_Click(object sender, RoutedEventArgs e)
        {
            if (!expansionShips2)
            {
                scvExpansionEraShips2.Visibility = Visibility.Visible;
                expansionShips2 = true;
            }
            else
            {
                scvExpansionEraShips2.Visibility = Visibility.Collapsed;
                expansionShips2 = false;
            }
        }
        private void btnEmpireEraShips2_Click(object sender, RoutedEventArgs e)
        {
            if (!empireShips2)
            {
                scvEmpireEraShips2.Visibility = Visibility.Visible;
                empireShips2 = true;
            }
            else
            {
                scvEmpireEraShips2.Visibility = Visibility.Collapsed;
                empireShips2 = false;
            }
        }
        private void btnRevolutionEraShips2_Click(object sender, RoutedEventArgs e)
        {
            if (!revShips2)
            {
                scvRevolutionEraShips2.Visibility = Visibility.Visible;
                revShips2 = true;
            }
            else
            {
                scvRevolutionEraShips2.Visibility = Visibility.Collapsed;
                revShips2 = false;
            }
        }
        private void btnUniqueUnitsShipsEra2_Click(object sender, RoutedEventArgs e)
        {
            if (!uniqueShips2)
            {
                scvUniqueShips2.Visibility = Visibility.Visible;
                uniqueShips2 = true;
            }
            else
            {
                scvUniqueShips2.Visibility = Visibility.Collapsed;
                uniqueShips2 = false;
            }
        }
        //zaznaczanie jednostek i wyznaczanie ich ilosci
        void addUnitToArmy(CheckBox ckUnit, TextBox tbxUnitNum, List<CheckBox> ckList, List<TextBox> tbxList)
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
        void verifyUnit(TextBox tbxUnitNum)
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
            addUnitToArmy(ckArmy1Pikemen, tbxArmy1Pikemen, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Pikemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Pikemen);
        }
        private void ckArmy1Arquebusiers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Arquebusiers, tbxArmy1Arquebusiers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Arquebusiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Arquebusiers);
        }

        private void ckArmy1Archers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Archers, tbxArmy1Archers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Archers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Archers);
        }

        private void ckArmy1Crossbowmen_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Crossbowmen, tbxArmy1Crossbowmen, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Crossbowmen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Crossbowmen);
        }

        private void ckArmy1Knights_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Knights, tbxArmy1Knights, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Knights_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Knights);
        }
        private void ckArmy1Horsemen_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Horsemen, tbxArmy1Horsemen, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Horsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Horsemen);
        }

        private void ckArmy1Bombard_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Bombard, tbxArmy1Bombard, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Bombard_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Bombard);
        }

        private void ckArmy1PikeShotArq_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1PikeShotArq, tbxArmy1PikeShotArq, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1PikeShotArq_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1PikeShotArq);
        }

        private void ckArmy1HeavyHussars_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1HeavyHussars, tbxArmy1HeavyHussars, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1HeavyHussars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1HeavyHussars);
        }

        private void ckArmy1Cossacks_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Cossacks, tbxArmy1Cossacks, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Cossacks_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Cossacks);
        }

        private void ckArmy1Reiters_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Reiters, tbxArmy1Reiters, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Reiters_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Reiters);
        }

        private void ckArmy1Tarabanas_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Tarabanas, tbxArmy1Tarabanas, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Tarabanas_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Tarabanas);
        }

        private void ckArmy1FieldCannon_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1FieldCannon, tbxArmy1FieldCannon, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1FieldCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1FieldCannon);
        }

        private void ckArmy1HeavyCannon_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1HeavyCannon, tbxArmy1HeavyCannon, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1HeavyCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1HeavyCannon);
        }

        private void ckArmy1PikeShotMusk_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1PikeShotMusk, tbxArmy1PikeShotMusk, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1PikeShotMusk_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1PikeShotMusk);
        }

        private void ckArmy1EarlyFusiliers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1EarlyFusiliers, tbxArmy1EarlyFusiliers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1EarlyFusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1EarlyFusiliers);
        }

        private void ckArmy1EarlyCuirassier_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1EarlyCuirassier, tbxArmy1EarlyCuirassier, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1EarlyCuirassier_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1EarlyCuirassier);
        }

        private void ckArmy1Harquebusers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Harquebusers, tbxArmy1Harquebusers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Harquebusers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Harquebusers);
        }

        private void ckArmy1Lancers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Lancers, tbxArmy1Lancers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Lancers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Lancers);
        }

        private void ckArmy1SiegeHowitzer_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1SiegeHowitzer, tbxArmy1SiegeHowitzer, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1SiegeHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1SiegeHowitzer);
        }

        private void ckArmy1Fusiliers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Fusiliers, tbxArmy1Fusiliers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Fusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Fusiliers);
        }

        private void ckArmy1Militia_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Militia, tbxArmy1Militia, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Militia_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Militia);
        }

        private void ckArmy1CarbineCav_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1CarbineCav, tbxArmy1CarbineCav, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1CarbineCav_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1CarbineCav);
        }

        private void ckArmy1Dragoons_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Dragoons, tbxArmy1Dragoons, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Dragoons_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Dragoons);
        }

        private void ckArmy1Hussars_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Hussars, tbxArmy1Hussars, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Hussars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Hussars);
        }

        private void ckArmy1Cuiraissiers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Cuiraissiers, tbxArmy1Cuiraissiers, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Cuiraissiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Cuiraissiers);
        }

        private void ckArmy1FieldGun_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1FieldGun, tbxArmy1FieldGun, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1FieldGun_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1FieldGun);
        }

        private void ckArmy1Mortars_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1Mortars, tbxArmy1Mortars, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1Mortars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1Mortars);
        }

        private void ckArmy1LightInfantry_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1LightInfantry, tbxArmy1LightInfantry, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1LightInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1LightInfantry);
        }

        private void ckArmy1LineInfantry_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1LineInfantry, tbxArmy1LineInfantry, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1LineInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1LineInfantry);
        }

        private void ckArmy1FieldHowitzer_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1FieldHowitzer, tbxArmy1FieldHowitzer, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1FieldHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1FieldHowitzer);
        }

        private void ckArmy1TribalWarriors_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1TribalWarriors, tbxArmy1TribalWarriors, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1TribalWarriors_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1TribalWarriors);
        }

        private void ckArmy1TribalRanger_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1TribalRanger, tbxArmy1TribalRanger, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1TribalRanger_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1TribalRanger);
        }

        private void ckArmy1TribalHorsemen_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy1TribalHorsemen, tbxArmy1TribalHorsemen, army1CheckBoxList, army1TextboxList);
        }

        private void tbxArmy1TribalHorsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy1TribalHorsemen);
        }
        // dla floty 1
        private void ckFleet1Carrack_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1Carrack, tbxFleet1Carrack, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Carrack_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1Carrack);
        }

        private void ckFleet1Caravel_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1Caravel, tbxFleet1Caravel, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Caravel_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1Caravel);
        }

        private void ckFleet1Galley_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1Galley, tbxFleet1Galley, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Galley_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1Galley);
        }

        private void ckFleet1Galleon_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1Galleon, tbxFleet1Galleon, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Galleon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1Galleon);
        }

        private void ckFleet1Schooner_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1Schooner, tbxFleet1Schooner, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Schooner_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1Schooner);
        }

        private void ckFleet1Brig_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1Brig, tbxFleet1Brig, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Brig_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1Brig);
        }

        private void ckFleet1Frigate_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1Frigate, tbxFleet1Frigate, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Frigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1Frigate);
        }

        private void ckFleet1GreatFrigate_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1GreatFrigate, tbxFleet1GreatFrigate, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1GreatFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1GreatFrigate);
        }

        private void ckFleet1ShipOfLine_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1ShipOfLine, tbxFleet1ShipOfLine, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1ShipOfLine_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1ShipOfLine);
        }

        private void ckFleet1Eastindiaman_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1Eastindiaman, tbxFleet1Eastindiaman, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1Eastindiaman_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1Eastindiaman);
        }

        private void ckFleet1ArmoredFrigate_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1ArmoredFrigate, tbxFleet1ArmoredFrigate, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1ArmoredFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1ArmoredFrigate);
        }

        private void ckFleet1AsianShip_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet1AsianShip, tbxFleet1AsianShip, fleet1CheckBoxList, fleet1TextboxList);
        }

        private void tbxFleet1AsianShip_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet1AsianShip);
        }
        // dla armii 1
        private void ckArmy2Pikemen_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Pikemen, tbxArmy2Pikemen, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Pikemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Pikemen);
        }
        private void ckArmy2Arquebusiers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Arquebusiers, tbxArmy2Arquebusiers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Arquebusiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Arquebusiers);
        }

        private void ckArmy2Archers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Archers, tbxArmy2Archers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Archers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Archers);
        }

        private void ckArmy2Crossbowmen_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Crossbowmen, tbxArmy2Crossbowmen, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Crossbowmen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Crossbowmen);
        }

        private void ckArmy2Knights_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Knights, tbxArmy2Knights, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Knights_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Knights);
        }
        private void ckArmy2Horsemen_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Horsemen, tbxArmy2Horsemen, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Horsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Horsemen);
        }

        private void ckArmy2Bombard_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Bombard, tbxArmy2Bombard, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Bombard_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Bombard);
        }

        private void ckArmy2PikeShotArq_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2PikeShotArq, tbxArmy2PikeShotArq, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2PikeShotArq_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2PikeShotArq);
        }

        private void ckArmy2HeavyHussars_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2HeavyHussars, tbxArmy2HeavyHussars, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2HeavyHussars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2HeavyHussars);
        }

        private void ckArmy2Cossacks_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Cossacks, tbxArmy2Cossacks, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Cossacks_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Cossacks);
        }

        private void ckArmy2Reiters_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Reiters, tbxArmy2Reiters, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Reiters_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Reiters);
        }

        private void ckArmy2Tarabanas_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Tarabanas, tbxArmy2Tarabanas, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Tarabanas_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Tarabanas);
        }

        private void ckArmy2FieldCannon_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2FieldCannon, tbxArmy2FieldCannon, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2FieldCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2FieldCannon);
        }

        private void ckArmy2HeavyCannon_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2HeavyCannon, tbxArmy2HeavyCannon, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2HeavyCannon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2HeavyCannon);
        }

        private void ckArmy2PikeShotMusk_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2PikeShotMusk, tbxArmy2PikeShotMusk, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2PikeShotMusk_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2PikeShotMusk);
        }

        private void ckArmy2EarlyFusiliers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2EarlyFusiliers, tbxArmy2EarlyFusiliers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2EarlyFusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2EarlyFusiliers);
        }

        private void ckArmy2EarlyCuirassier_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2EarlyCuirassier, tbxArmy2EarlyCuirassier, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2EarlyCuirassier_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2EarlyCuirassier);
        }

        private void ckArmy2Harquebusers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Harquebusers, tbxArmy2Harquebusers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Harquebusers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Harquebusers);
        }

        private void ckArmy2Lancers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Lancers, tbxArmy2Lancers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Lancers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Lancers);
        }

        private void ckArmy2SiegeHowitzer_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2SiegeHowitzer, tbxArmy2SiegeHowitzer, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2SiegeHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2SiegeHowitzer);
        }

        private void ckArmy2Fusiliers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Fusiliers, tbxArmy2Fusiliers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Fusiliers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Fusiliers);
        }

        private void ckArmy2Militia_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Militia, tbxArmy2Militia, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Militia_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Militia);
        }

        private void ckArmy2CarbineCav_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2CarbineCav, tbxArmy2CarbineCav, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2CarbineCav_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2CarbineCav);
        }

        private void ckArmy2Dragoons_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Dragoons, tbxArmy2Dragoons, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Dragoons_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Dragoons);
        }

        private void ckArmy2Hussars_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Hussars, tbxArmy2Hussars, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Hussars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Hussars);
        }

        private void ckArmy2Cuiraissiers_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Cuiraissiers, tbxArmy2Cuiraissiers, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Cuiraissiers_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Cuiraissiers);
        }

        private void ckArmy2FieldGun_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2FieldGun, tbxArmy2FieldGun, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2FieldGun_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2FieldGun);
        }

        private void ckArmy2Mortars_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2Mortars, tbxArmy2Mortars, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2Mortars_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2Mortars);
        }

        private void ckArmy2LightInfantry_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2LightInfantry, tbxArmy2LightInfantry, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2LightInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2LightInfantry);
        }

        private void ckArmy2LineInfantry_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2LineInfantry, tbxArmy2LineInfantry, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2LineInfantry_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2LineInfantry);
        }

        private void ckArmy2FieldHowitzer_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2FieldHowitzer, tbxArmy2FieldHowitzer, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2FieldHowitzer_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2FieldHowitzer);
        }

        private void ckArmy2TribalWarriors_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2TribalWarriors, tbxArmy2TribalWarriors, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2TribalWarriors_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2TribalWarriors);
        }

        private void ckArmy2TribalRanger_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2TribalRanger, tbxArmy2TribalRanger, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2TribalRanger_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2TribalRanger);
        }

        private void ckArmy2TribalHorsemen_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckArmy2TribalHorsemen, tbxArmy2TribalHorsemen, army2CheckBoxList, army2TextboxList);
        }

        private void tbxArmy2TribalHorsemen_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxArmy2TribalHorsemen);
        }
        // flota 2
        private void ckFleet2Carrack_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2Carrack, tbxFleet2Carrack, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Carrack_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2Carrack);
        }



        private void ckFleet2Caravel_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2Caravel, tbxFleet2Caravel, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Caravel_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2Caravel);
        }

        private void ckFleet2Galley_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2Galley, tbxFleet2Galley, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Galley_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2Galley);
        }

        private void ckFleet2Galleon_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2Galleon, tbxFleet2Galleon, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Galleon_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2Galleon);
        }

        private void ckFleet2Schooner_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2Schooner, tbxFleet2Schooner, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Schooner_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2Schooner);
        }

        private void ckFleet2Brig_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2Brig, tbxFleet2Brig, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Brig_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2Brig);
        }

        private void ckFleet2Frigate_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2Frigate, tbxFleet2Frigate, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Frigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2Frigate);
        }

        private void ckFleet2GreatFrigate_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2GreatFrigate, tbxFleet2GreatFrigate, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2GreatFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2GreatFrigate);
        }

        private void ckFleet2ShipOfLine_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2ShipOfLine, tbxFleet2ShipOfLine, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2ShipOfLine_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2ShipOfLine);
        }

        private void ckFleet2Eastindiaman_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2Eastindiaman, tbxFleet2Eastindiaman, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2Eastindiaman_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2Eastindiaman);
        }

        private void ckFleet2ArmoredFrigate_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2ArmoredFrigate, tbxFleet2ArmoredFrigate, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2ArmoredFrigate_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2ArmoredFrigate);
        }

        private void ckFleet2AsianShip_Click(object sender, RoutedEventArgs e)
        {
            addUnitToArmy(ckFleet2AsianShip, tbxFleet2AsianShip, fleet2CheckBoxList, fleet2TextboxList);
        }

        private void tbxFleet2AsianShip_LostFocus(object sender, RoutedEventArgs e)
        {
            verifyUnit(tbxFleet2AsianShip);
        }
        // wszystko po kliknieciu start
        void verifyInputLists(List<CheckBox> ckList, List<TextBox> tbxList)
        {
            string input;
            for (int i = 0; i < ckList.Count; i++)
            {
                input = tbxList[i].Text;
                if (Convert.ToInt32(input) <= 0)
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
                verifyInputLists(army1CheckBoxList, army1TextboxList);
                verifyInputLists(army2CheckBoxList, army2TextboxList);
                activateStartButton();
                string unitName;
                int numOfUnits;
                if (btnStart.IsEnabled)
                {
                    for (int i = 0; i < army1CheckBoxList.Count; i++)
                    {
                        unitName = Convert.ToString(army1CheckBoxList[i].Content);
                        numOfUnits = Convert.ToInt32(army1TextboxList[i].Text);
                        switch (army1CheckBoxList[i].Content)
                        {
                            case "Pikinierzy":
                                LandUnit Pikemen1 = new LandUnit(unitName, 0, 0, 0, 5, 25, 25, 0, 4, 170, 100, 4, "Infantry", numOfUnits);
                                army1UnitsList.Add(Pikemen1);
                                break;

                        }
                    }

                }

            }
            else
            {
                verifyInputLists(fleet1CheckBoxList, fleet1TextboxList);
                verifyInputLists(fleet2CheckBoxList, fleet2TextboxList);
                activateStartButton();
            }
            // glowne dzialanie
            if (btnStart.IsEnabled)
            {
                mainPage.Visibility = Visibility.Collapsed;
                resultPage.Visibility = Visibility.Visible;
            }
        }
    }
    public class LandUnit //jednostki ladowe
    {
        string Name;

        int LongRange;
        int MediumRange;
        int LowRange;

        int ShockAttack;
        int Melee;
        int ShockDef;
        int ArtilleryDef;

        int initiative;

        int Health;
        int MaxMorale;
        int Morale;
        int Speed;

        string Tactic;

        int NumberOf;

        public LandUnit(string UnitName, int UnitLongRange, int UnitMedRange, int UnitLowRange, int UnitShockAttack, int UnitMelee, int UnitShockDef, int UnitArtDef, int UnitInitiative, int UnitHealth, int UnitMorale, int UnitSpeed, string UnitTactic, int UnitNumberOf)
        {
            Name = UnitName;
            LongRange = UnitLongRange;
            MediumRange = UnitMedRange;
            LowRange = UnitLowRange;
            ShockAttack = UnitShockAttack;
            Melee = UnitMelee;
            ShockDef = UnitShockDef;
            ArtilleryDef = UnitArtDef;
            initiative = UnitInitiative * 10;
            Health = UnitHealth;
            MaxMorale = UnitMorale;
            Morale = UnitMorale;
            Speed = UnitSpeed;
            Tactic = UnitTactic;
            NumberOf = UnitNumberOf;

        }

    }
    class Ship //statki
    {
        string name;

        int size;
        int type;

        int hullHealth;
        int maxHull;
        int crew;
        int maxCrew;

        int speed;
        int maneuver;

        int broadSideFirepower;
        int frontalFirepower;
        int sternFirepower;
        int mortarFirepower;

        public Ship(string shipName, int shipType, int shipHullShallow, int shipHull, int shipCrew, int shipSpeed, int shipManeuver, int shipBroadside, int shipFrontal, int shipStern, int shipMortars)
        {
            name = shipName;
            type = shipType;
            size = shipHullShallow;
            hullHealth = shipHull;
            maxHull = shipHull;
            crew = shipCrew;
            maxCrew = shipCrew;
            speed = shipSpeed;
            maneuver = shipManeuver + shipSpeed;
            broadSideFirepower = shipBroadside;
            frontalFirepower = shipFrontal;
            sternFirepower = shipStern;
            mortarFirepower = shipMortars;
        }
    }
    public class TerrainType //typy terenu
    {
        public string Name;
        //efekty
        public int Concealment; //ukrycie
        public int Mud; //bloto, grzaskosc terenu
        public int Obstacles; // przeszkody 
        public int HighDiff; //zmiany terenu

        public TerrainType(string TerrainName, int TerrainConc, int TerrainMud, int TerrainObst, int TerrainHighDif)
        {
            Name = TerrainName;
            Concealment = TerrainConc;
            Mud = TerrainMud;
            Obstacles = TerrainObst;
            HighDiff = TerrainHighDif;

        }
    }
    public class FortType
    {
        public int level;
        public int wallLevel;
        public int firePower;
        public FortType(int fortlevel, int fortWallLevel, int fortFirePower)
        {
            level = fortlevel;
            wallLevel = fortWallLevel;
            firePower = fortFirePower;
        }
    }
    public class SeaType //typy morza
    {
        public string Name;
        //efekty
        public int Wind;
        public SeaType(string SeaName, int SeaWind)
        {
            Name = SeaName;
            Wind = SeaWind;
        }
    }
}
