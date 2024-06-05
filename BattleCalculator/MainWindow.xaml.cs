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
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleCalculator
{
    
    public partial class MainWindow : Window
    {

        SelectionPage selectionPage = new SelectionPage();
        public MainWindow()
        {
            InitializeComponent();
            selectionPage.btnClick += startBtnClicked;
            frame.Content = selectionPage;
        }
        private void startBtnClicked(object? sender, EventArgs e)
        {
            ResultPage resultPage = new ResultPage();
            resultPage.lbResult.Content = selectionPage.resultString;
            resultPage.tbxBattleLog.Text = selectionPage.battleLog;
            resultPage.tbxTeam1DisplayList.Text = selectionPage.team1StringDisplayList;
            resultPage.tbxTeam2DisplayList.Text = selectionPage.team2StringDisplayList;
            resultPage.tbxTeam1List.Text = selectionPage.team1StringList;
            resultPage.tbxTeam2List.Text = selectionPage.team2StringList;
            frame.Content = resultPage;
        }
    }
    public class LandUnit //jednostki ladowe
    {
        public string Name;

        public int LongRange;
        public int MediumRange;
        public int LowRange;

        public int ShockAttack;
        public int Melee;
        public int ShockDef;
        public int ArtilleryDef;

        public int Initiative;
        public int MaxInitiative;

        public int Health;
        public int MaxHealth;
        public int MaxMorale;
        public int Morale;
        public int Speed;

        public string Type;

        public int NumberOf;

        public LandUnit(string UnitName, int UnitLongRange, int UnitMidRange, int UnitLowRange, int UnitShockAttack, int UnitMelee, int UnitShockDef, int UnitArtDef, int UnitInitiative, int UnitHealth, int UnitMorale, int UnitSpeed, string UnitType, int UnitNumberOf)
        {
            this.Name = UnitName;
            this.LongRange = UnitLongRange;
            this.MediumRange = UnitMidRange;
            this.LowRange = UnitLowRange;
            this.ShockAttack = UnitShockAttack + (UnitSpeed * 2);
            this.Melee = UnitMelee;
            this.ShockDef = UnitShockDef;
            this.ArtilleryDef = UnitArtDef;
            this.Initiative = UnitInitiative;
            this.MaxInitiative = UnitInitiative;
            this.Health = UnitHealth;
            this.MaxHealth = UnitHealth;
            this.MaxMorale = UnitMorale;
            this.Morale = UnitMorale;
            this.Speed = UnitSpeed;
            this.Type = UnitType;
            this.NumberOf = UnitNumberOf;

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

        int numberOf;

        public Ship(string shipName, int shipType, int shipHullShallow, int shipHull, int shipCrew, int shipSpeed, int shipManeuver, int shipBroadside, int shipFrontal, int shipStern, int shipMortars, int numberOfShips)
        {
            this.name = shipName;
            this.type = shipType;
            this.size = shipHullShallow;
            this.hullHealth = shipHull;
            this.maxHull = shipHull;
            this.crew = shipCrew;
            this.maxCrew = shipCrew;
            this.speed = shipSpeed;
            this.maneuver = shipManeuver + shipSpeed;
            this.broadSideFirepower = shipBroadside;
            this.frontalFirepower = shipFrontal;
            this.sternFirepower = shipStern;
            this.mortarFirepower = shipMortars;
            this.numberOf = numberOfShips;
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

        public TerrainType(string TerrainName, int TerrainConcealment, int TerrainMud, int TerrainObst, int TerrainHighDif)
        {
            Name = TerrainName;
            Concealment = TerrainConcealment;
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
