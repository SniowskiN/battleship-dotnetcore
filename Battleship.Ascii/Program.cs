
namespace Battleship.Ascii
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Battleship.GameController;
    using Battleship.GameController.Contracts;

    public class Program
    {
        private static List<Ship> myFleet;
        private static List<Ship> enemyFleet;
        private static List<Position> myShots;
        private static List<Position> enemyShots;

        private const ConsoleColor myColor = ConsoleColor.DarkGreen;
        private const ConsoleColor enemyColor = ConsoleColor.DarkYellow;

        private const ConsoleColor hitColor = ConsoleColor.Red;
        private const ConsoleColor missColor = ConsoleColor.Blue;

        static void Main()
        {
            Console.Title = "Battleship";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            Console.WriteLine("                                     |__");
            Console.WriteLine(@"                                     |\/");
            Console.WriteLine("                                     ---");
            Console.WriteLine("                                     / | [");
            Console.WriteLine("                              !      | |||");
            Console.WriteLine("                            _/|     _/|-++'");
            Console.WriteLine("                        +  +--|    |--|--|_ |-");
            Console.WriteLine(@"                     { /|__|  |/\__|  |--- |||__/");
            Console.WriteLine(@"                    +---------------___[}-_===_.'____                 /\");
            Console.WriteLine(@"                ____`-' ||___-{]_| _[}-  |     |_[___\==--            \/   _");
            Console.WriteLine(@" __..._____--==/___]_|__|_____________________________[___\==--____,------' .7");
            Console.WriteLine(@"|                        Welcome to Battleship                         BB-61/");
            Console.WriteLine(@" \_________________________________________________________________________|");
            Console.WriteLine();

            InitializeGame();

            StartGame();
        }

        private static void StartGame()
        {
            Console.Clear();
            Console.WriteLine("                  __");
            Console.WriteLine(@"                 /  \");
            Console.WriteLine("           .-.  |    |");
            Console.WriteLine(@"   *    _.-'  \  \__/");
            Console.WriteLine(@"    \.-'       \");
            Console.WriteLine("   /          _/");
            Console.WriteLine(@"  |      _  /""");
            Console.WriteLine(@"  |     /_\'");
            Console.WriteLine(@"   \    \_/");
            Console.WriteLine(@"    """"""""");

            do
            {
                WriteColor(myColor, "\n*****************************************\n", null);
                WriteColor(myColor,"Player, it's your turn\n", null);
                WriteColor(myColor, "*****************************************\n\n", null);

                Position position;
                do
                {
                    WriteColor(myColor, "Enter coordinates for your shot : ", null);
                    position = Position.ParsePosition(Console.ReadLine());

                    if (position != null)
                    {
                        if (myShots.Contains(position))
                        {
                            WriteColor(myColor, "You've already shot that position. Try another.\n");
                            continue;
                        }

                        myShots.Add(position);
                        Console.WriteLine();
                        break;
                    }
                    WriteColor(myColor, "Invalid position, try again.\n", null);
                } while (true);

                var isHit = GameController.CheckIsHit(enemyFleet, position);

                if (isHit)
                    ShowExplosion();

                WriteColor(isHit ? hitColor : missColor, isHit ? "Yeah ! Nice hit !" : "Miss", null, "\n\n");

                if (isHit && position.Ship != null && position.Ship.IsSunk)
                {
                    WriteColor(enemyColor, $"Enemy ship sunk ({position.Ship.Name} {new string('o',position.Ship.Size)})", null, "\n");
                    ShowStatus(enemyFleet, enemyColor);
                }

                if (enemyFleet.Where(x => !x.IsSunk).Count() == 0)
                {
                    WriteColor(myColor,"*******************************************\n\n",null);
                    Console.WriteLine("You are the winner!");
                    Console.WriteLine("Press any key to close window.");
                    Console.ReadKey(false);
                    break;
                }

                WriteColor(enemyColor, "\n*****************************************\n", null);
                WriteColor(enemyColor, "Now it's Computer's turn\n", null);
                WriteColor(enemyColor, "*****************************************\n\n", null);

                do
                {
                    position = Position.GetRandomPosition();
                } while (enemyShots.Contains(position));

                enemyShots.Add(position);
                isHit = GameController.CheckIsHit(myFleet, position);

                WriteColor(isHit ? hitColor : missColor, $"Computer shot in {position.Column}{position.Row} and ",
                            isHit ? "has hit your ship !" : "miss",null, "\n");
                if (isHit)
                    ShowExplosion();

                if (position.Ship != null && position.Ship.IsSunk)
                {
                    WriteColor(myColor, "Ship sunk\n\n", null);
                    ShowStatus(myFleet, myColor);
                }

                if (myFleet.Where(x => !x.IsSunk).Count() == 0)
                {
                    WriteColor(enemyColor, "*******************************************", null, "\n");
                    Console.WriteLine("You lost!");
                    Console.WriteLine("Press any key to close window.");
                    Console.ReadKey(false);
                    break;
                }
            }
            while (true);
        }

        private static void ShowStatus(List<Ship> fleet, ConsoleColor color)
        {
            WriteColor(color, "\nShips sunk by now:\n");
            foreach (var ship in fleet.Where(x => x.IsSunk))
            {
                WriteColor("\t", ship.Color, ship.Name.PadRight(18), "\t", new string('o',ship.Size), null, "\n");
            }

            WriteColor(color, "\nStill alive:\n");
            foreach (var ship in fleet.Where(x => !x.IsSunk))
            {
                WriteColor("\t", ship.Color, ship.Name.PadRight(18), "\t", new string('o', ship.Size), null, "\n");
            }

        }

        private static void ShowExplosion()
        {
            Console.Beep();
            Console.WriteLine("\n");
            WriteColor(ConsoleColor.Red, @"                \         .  ./","\n");
            WriteColor(ConsoleColor.Red, @"              \      .:"";'.:..""   /", "\n");
            WriteColor(ConsoleColor.Red, @"                  (M^^.^~~:.'"").", "\n");
            WriteColor(ConsoleColor.Red, @"            -   (/  .    . . \ \)  -", "\n");
            WriteColor(ConsoleColor.Red, @"               ((| :. ~ ^  :. .|))", "\n");
            WriteColor(ConsoleColor.Red, @"            -   (\- |  \ /  |  /)  -", "\n");
            WriteColor(ConsoleColor.Red, @"                 -\  \     /  /-", "\n");
            WriteColor(ConsoleColor.Red, @"                   \  \   /  /", null, "\n\n");
        }

        public static void WriteColor(params object[] prm)
        {
            foreach (var param in prm)
                if (param == null)
                    Console.ResetColor();
                else if (param is ConsoleColor)
                    Console.ForegroundColor = (ConsoleColor)param;
                else
                    Console.Write(param.ToString());
        }

        private static void InitializeGame()
        {
            InitializeMyFleet();

            InitializeEnemyFleet();
        }

        private static void InitializeMyFleet()
        {
            myShots = new List<Position>();
            myFleet = GameController.InitializeShips().ToList();
            Console.WriteLine("Please position your fleet (Game board size is from A to H and 1 to 8) :");

            foreach (var ship in myFleet)
            {
                Console.WriteLine();
                Console.WriteLine("Please enter the positions for the {0} (size: {1})", ship.Name, ship.Size);
                for (var i = 1; i <= ship.Size; i++)
                {
                    do
                    {
                        Console.WriteLine("Enter position {0} of {1} (i.e A3):", i, ship.Size);

                        if (ship.AddPosition(Console.ReadLine()))
                        {
                            break;
                        }
                        Console.WriteLine("Invalid, or already placed position, try again.");
                    } while (true);
                }
            }
        }

        private static void InitializeEnemyFleet()
        {
            enemyShots = new List<Position>();

            Random r = new Random();
            int rInt = r.Next(1, 5);

            enemyFleet = GameController.InitializeShips().ToList();

            switch (rInt)
            {
                case 1:

                    enemyFleet[0].Positions.Add(new Position(Letters.B, 4, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.B, 5, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.B, 6, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.B, 7, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.B, 8, enemyFleet[0]));

                    enemyFleet[1].Positions.Add(new Position(Letters.E, 5, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.E, 6, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.E, 7, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.E, 8, enemyFleet[1]));

                    enemyFleet[2].Positions.Add(new Position(Letters.A, 2, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.B, 2, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.C, 2, enemyFleet[2]));

                    enemyFleet[3].Positions.Add(new Position(Letters.E, 1, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.F, 1, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.G, 1, enemyFleet[3]));

                    enemyFleet[4].Positions.Add(new Position(Letters.C, 5, enemyFleet[4]));
                    enemyFleet[4].Positions.Add(new Position(Letters.C, 6, enemyFleet[4]));
                    break;

                case 2:

                    enemyFleet[0].Positions.Add(new Position(Letters.A, 4, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.A, 5, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.A, 6, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.A, 7, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.A, 8, enemyFleet[0]));

                    enemyFleet[1].Positions.Add(new Position(Letters.F, 3, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.F, 4, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.F, 5, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.F, 6, enemyFleet[1]));

                    enemyFleet[2].Positions.Add(new Position(Letters.B, 1, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.C, 1, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.D, 1, enemyFleet[2]));

                    enemyFleet[3].Positions.Add(new Position(Letters.F, 1, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.G, 1, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.H, 1, enemyFleet[3]));

                    enemyFleet[4].Positions.Add(new Position(Letters.D, 5, enemyFleet[4]));
                    enemyFleet[4].Positions.Add(new Position(Letters.D, 6, enemyFleet[4]));
                    break;

                case 3:

                    enemyFleet[0].Positions.Add(new Position(Letters.A, 5, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.B, 5, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.C, 5, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.D, 5, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.E, 5, enemyFleet[0]));

                    enemyFleet[1].Positions.Add(new Position(Letters.G, 1, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.G, 2, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.G, 3, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.G, 4, enemyFleet[1]));

                    enemyFleet[2].Positions.Add(new Position(Letters.A, 1, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.A, 2, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.A, 3, enemyFleet[2]));

                    enemyFleet[3].Positions.Add(new Position(Letters.C, 1, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.C, 2, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.C, 3, enemyFleet[3]));

                    enemyFleet[4].Positions.Add(new Position(Letters.A, 7, enemyFleet[4]));
                    enemyFleet[4].Positions.Add(new Position(Letters.B, 7, enemyFleet[4]));
                    break;

                case 4:

                    enemyFleet[0].Positions.Add(new Position(Letters.A, 1, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.B, 1, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.C, 1, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.D, 1, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.E, 1, enemyFleet[0]));

                    enemyFleet[1].Positions.Add(new Position(Letters.G, 5, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.G, 6, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.G, 7, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.G, 8, enemyFleet[1]));

                    enemyFleet[2].Positions.Add(new Position(Letters.E, 6, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.E, 7, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.E, 8, enemyFleet[2]));

                    enemyFleet[3].Positions.Add(new Position(Letters.C, 6, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.C, 7, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.C, 8, enemyFleet[3]));

                    enemyFleet[4].Positions.Add(new Position(Letters.A, 7, enemyFleet[4]));
                    enemyFleet[4].Positions.Add(new Position(Letters.A, 8, enemyFleet[4]));
                    break;

                case 5:

                    enemyFleet[0].Positions.Add(new Position(Letters.H, 1, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.H, 2, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.H, 3, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.H, 4, enemyFleet[0]));
                    enemyFleet[0].Positions.Add(new Position(Letters.H, 5, enemyFleet[0]));

                    enemyFleet[1].Positions.Add(new Position(Letters.C, 2, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.C, 3, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.C, 4, enemyFleet[1]));
                    enemyFleet[1].Positions.Add(new Position(Letters.C, 5, enemyFleet[1]));

                    enemyFleet[2].Positions.Add(new Position(Letters.D, 8, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.E, 8, enemyFleet[2]));
                    enemyFleet[2].Positions.Add(new Position(Letters.F, 8, enemyFleet[2]));

                    enemyFleet[3].Positions.Add(new Position(Letters.E, 1, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.F, 1, enemyFleet[3]));
                    enemyFleet[3].Positions.Add(new Position(Letters.G, 1, enemyFleet[3]));

                    enemyFleet[4].Positions.Add(new Position(Letters.H, 7, enemyFleet[4]));
                    enemyFleet[4].Positions.Add(new Position(Letters.H, 8, enemyFleet[4]));
                    break;
            }
        }

    }
}
