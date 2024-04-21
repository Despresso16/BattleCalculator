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
        public MainWindow()
        {
            InitializeComponent();
            SelectionPage selectionPage = new SelectionPage();
            selectionPage.btnClick += startBtnClicked;
            frame.Content = selectionPage;
        }
        private void startBtnClicked(object sender, EventArgs e)
        {
            frame.Content = new ResultPage();
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

        string Type;

        int NumberOf;

        public LandUnit(string UnitName, int UnitLongRange, int UnitMidRange, int UnitLowRange, int UnitShockAttack, int UnitMelee, int UnitShockDef, int UnitArtDef, int UnitInitiative, int UnitHealth, int UnitMorale, int UnitSpeed, string UnitType, int UnitNumberOf)
        {
            Name = UnitName;
            LongRange = UnitLongRange;
            MediumRange = UnitMidRange;
            LowRange = UnitLowRange;
            ShockAttack = UnitShockAttack + (UnitSpeed * 2);
            Melee = UnitMelee;
            ShockDef = UnitShockDef;
            ArtilleryDef = UnitArtDef;
            initiative = UnitInitiative;
            Health = UnitHealth;
            MaxMorale = UnitMorale;
            Morale = UnitMorale;
            Speed = UnitSpeed;
            Type = UnitType;
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

        int numberOf;

        public Ship(string shipName, int shipType, int shipHullShallow, int shipHull, int shipCrew, int shipSpeed, int shipManeuver, int shipBroadside, int shipFrontal, int shipStern, int shipMortars, int numberOfShips)
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
            numberOf = numberOfShips;
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
