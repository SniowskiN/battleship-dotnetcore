
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
                Console.WriteLine();
                Console.WriteLine("Player, it's your turn");

                Position position;
                do
                {
                    Console.WriteLine("Enter coordinates for your shot :");
                    position = Position.ParsePosition(Console.ReadLine());

                    if (position != null)
                    {
                        break;
                    }
                    Console.WriteLine("Invalid position, try again.");
                } while (true);

                var isHit = GameController.CheckIsHit(enemyFleet, position);

                if (isHit)
                    ShowExplosion();

                WriteColor(isHit ? hitColor : missColor, isHit ? "Yeah ! Nice hit !" : "Miss", null, "\n");

                if (isHit && position.Ship != null && position.Ship.IsSunk)
                {
                    Console.WriteLine("Ship sunk");
                }

                if (enemyFleet.Where(x => !x.IsSunk).Count() == 0)
                {
                    Console.WriteLine("You win");
                    Console.WriteLine("Press any key to close window.");
                    Console.ReadKey(false);
                    break;
                }

                position = Position.GetRandomPosition();
                isHit = GameController.CheckIsHit(myFleet, position);
                Console.WriteLine();
                WriteColor(isHit ? hitColor : missColor, $"Computer shot in {position.Column}{position.Row} and ",
                            isHit ? "has hit your ship !" : "miss",null, "\n");
                if (isHit)
                    ShowExplosion();

                if (position.Ship != null && position.Ship.IsSunk)
                {
                    Console.WriteLine("Ship sunk");
                }

                if (myFleet.Where(x => !x.IsSunk).Count() == 0)
                {
                    Console.WriteLine("You Lost");
                    Console.WriteLine("Press any key to close window.");
                    Console.ReadKey(false);
                    break;
                }
            }
            while (true);
        }

        private static void ShowExplosion()
        {
            Console.Beep();

            Console.WriteLine(@"                \         .  ./");
            Console.WriteLine(@"              \      .:"";'.:..""   /");
            Console.WriteLine(@"                  (M^^.^~~:.'"").");
            Console.WriteLine(@"            -   (/  .    . . \ \)  -");
            Console.WriteLine(@"               ((| :. ~ ^  :. .|))");
            Console.WriteLine(@"            -   (\- |  \ /  |  /)  -");
            Console.WriteLine(@"                 -\  \     /  /-");
            Console.WriteLine(@"                   \  \   /  /");
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
                        Console.WriteLine("Invalid position, try again.");
                    } while (true);
                }
            }
        }

        private static void InitializeEnemyFleet()
        {
            enemyFleet = GameController.InitializeShips().ToList();

            enemyFleet[0].Positions.Add(new Position(Letters.B, 4, enemyFleet[0]));
            enemyFleet[0].Positions.Add(new Position(Letters.B, 5, enemyFleet[0]));
            enemyFleet[0].Positions.Add(new Position(Letters.B, 6, enemyFleet[0]));
            enemyFleet[0].Positions.Add(new Position(Letters.B, 7, enemyFleet[0]));
            enemyFleet[0].Positions.Add(new Position(Letters.B, 8, enemyFleet[0]));

            enemyFleet[1].Positions.Add(new Position(Letters.E, 6, enemyFleet[1]));
            enemyFleet[1].Positions.Add(new Position(Letters.E, 7, enemyFleet[1]));
            enemyFleet[1].Positions.Add(new Position(Letters.E, 8, enemyFleet[1]));
            enemyFleet[1].Positions.Add(new Position(Letters.E, 9, enemyFleet[1]));

            enemyFleet[2].Positions.Add(new Position(Letters.A, 2, enemyFleet[2]));
            enemyFleet[2].Positions.Add(new Position(Letters.B, 2, enemyFleet[2]));
            enemyFleet[2].Positions.Add(new Position(Letters.C, 2, enemyFleet[2]));

            enemyFleet[3].Positions.Add(new Position(Letters.E, 1, enemyFleet[3]));
            enemyFleet[3].Positions.Add(new Position(Letters.F, 1, enemyFleet[3]));
            enemyFleet[3].Positions.Add(new Position(Letters.G, 1, enemyFleet[3]));

            enemyFleet[4].Positions.Add(new Position(Letters.C, 5, enemyFleet[4]));
            enemyFleet[4].Positions.Add(new Position(Letters.C, 6, enemyFleet[4]));
        }
    }
}
