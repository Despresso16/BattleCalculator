namespace BattleCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string battleType, engage, terrain, sea;
                Team Team1 = new Team(1);
                Team Team2 = new Team(2);

                Console.WriteLine("Witaj w kalkulatorze!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Podaj typ bitwy [Ladowa, Morska, Atak portu]: ");
                battleType = Console.ReadLine();
                switch (battleType.ToUpper())
                {
                    case "LADOWA":
                        // jednostki ladowe
                        //LandUnit Pikinierzy = new LandUnit("Pikinierzy", 0, 0, 0, 1, 10, 15, 60, 100, 2, "Pikemen");
                        //LandUnit Kusznicy = new LandUnit("Kusznicy", 0, 10, 15, 0, 3, 0, 50, 100, 2, "Ranged Infantry");
                        //LandUnit Lucznicy = new LandUnit("Lucznicy", 0, 11, 13, 0, 3, 0, 40, 100, 2, "Ranged Infantry");
                        //LandUnit Rycerze = new LandUnit("Rycerze", 0, 0, 0, 10, 13, 5, 70, 100, 4, "Charge Cavalry");
                        //LandUnit Konnica = new LandUnit("Konnica", 0, 0, 0, 6, 11, 2, 45, 100, 5, "Charge Cavalry");


                        break;
                    case "MORSKA":
                        Console.WriteLine("mmm");

                        break;
                    case "ATAK PORTU":
                        Console.WriteLine("pppp");
                        break;
                    default:
                        Console.WriteLine("Wystapil blad, wprowadz dostepny typ bitwy.");
                        break;
                }
            }

        }

        //statystyki jednostek, terenu etc.
        private class LandUnit //jednostki ladowe
        {
            public string Name;

            public int LongRange;
            public int MediumRange;
            public int LowRange;

            public int ShockAttack;
            public int Melee;
            public int CavalryDef;

            public int Health;
            public int Morale;
            public int Speed;

            public string Tactic;

            public LandUnit(string UnitName, int UnitLongRange, int UnitMedRange, int UnitLowRange, int UnitShockAttack, int UnitMelee, int UnitCavDef, int UnitHealth, int UnitMorale, int UnitSpeed, string UnitTactic)
            {
                Name = UnitName;
                LongRange = UnitLongRange;
                MediumRange = UnitMedRange;
                LowRange = UnitLowRange;
                ShockAttack = UnitShockAttack;
                Melee = UnitMelee;
                CavalryDef = UnitCavDef;
                Health = UnitHealth;
                Morale = UnitMorale;
                Speed = UnitSpeed;
                Tactic = UnitTactic;

            }

        }

        private class Ship //statki
        {
            public string Name;
            public int LongRange;
            public int MediumRange;
            public int LowRange;
            public int Crew;
            public int CarryingCapacitity;
            public int Health;
            public int Morale;
            public int Speed;
            public string Design;
            public string Tactic;

            public Ship(string ShipName, int ShipLongRange, int ShipMedRange, int ShipLowRange, int ShipCrew, int ShipCC, int ShipHealth, int ShipMorale, int ShipSpeed, string ShipDesign, string ShipTactic)
            {
                Name = ShipName;
                LongRange = ShipLongRange;
                MediumRange = ShipMedRange;
                LowRange = ShipLowRange;
                Crew = ShipCrew;
                CarryingCapacitity = ShipCC;
                Health = ShipHealth;
                Morale = ShipMorale;
                Speed = ShipSpeed;
                Design = ShipDesign;
                Tactic = ShipTactic;

            }
        }

        private class TerrainType //typy terenu na ladzie
        {
            public string Name;
            //efekty
            public int Concealment; //ukrycie
            public int Mud; //bloto, grzaskosc terenu
            public int HighDiff; //zmiany terenu

            public TerrainType(string TerrainName, int TerrainConc, int TerrainMud, int TerreinHighDif)
            {
                Name = TerrainName;
                Concealment = TerrainConc;
                Mud = TerrainMud;
                HighDiff = TerreinHighDif;

            }
        }

        private class SeaType //typy morza
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

        private class Team // dwie strony
        {
            public int Num;
            public Team(int TeamNum)
            {
                Num = TeamNum;
            }

        }
    }

}
