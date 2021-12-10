using System.Linq;

namespace Battleship.GameController
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Battleship.GameController.Contracts;

    /// <summary>
    ///     The game controller.
    /// </summary>
    public class GameController
    {
        public static List<Ship> myFleet;
        public static List<Ship> enemyFleet;
        private const ConsoleColor consoleColorWin = ConsoleColor.Yellow;
        private const ConsoleColor consoleColorLost = ConsoleColor.White;

        /// <summary>
        /// Checks the is hit.
        /// </summary>
        /// <param name="ships">
        /// The ships.
        /// </param>
        /// <param name="shot">
        /// The shot.
        /// </param>
        /// <returns>
        /// True if hit, else false
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// ships
        ///     or
        ///     shot
        /// </exception>
        public static bool CheckIsHit(IEnumerable<Ship> ships, Position shot)
        {
            foreach (var ship in ships)
            {
                foreach (var position in ship.Positions)
                {
                    if (position.Equals(shot))
                    {
                        position.Ship = ship;
                        position.WasHit = true;
                        shot.UpdateFrom(position);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     The initialize ships.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public static IEnumerable<Ship> InitializeShips()
        {
            return new List<Ship>()
                       {
                           new Ship() { Name = "Aircraft Carrier", Size = 5, Color = ConsoleColor.Blue },
                           new Ship() { Name = "Battleship", Size = 4, Color = ConsoleColor.Red },
                           new Ship() { Name = "Submarine", Size = 3, Color = ConsoleColor.Gray },
                           new Ship() { Name = "Destroyer", Size = 3, Color = ConsoleColor.Yellow },
                           new Ship() { Name = "Patrol Boat", Size = 2, Color = ConsoleColor.Green }
                       };
        }

        /// <summary>
        /// The is ships valid.
        /// </summary>
        /// <param name="ship">
        /// The ship.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsShipValid(Ship ship)
        {
            return ship.Positions.Count == ship.Size;
        }

        public static Position GetRandomPosition(int size)
        {
            var random = new Random();
            var letter = (Letters)random.Next(size);
            var number = random.Next(size);
            var position = new Position(letter, number, null);
            return position;
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

        public static void ShowWin()
        {
            Console.Clear();
            Console.Beep();

            WriteColor(consoleColorWin, @"|@@@@|     |####|","\n");
            WriteColor(consoleColorWin, @"|@@@@|     |####|", "\n");
            WriteColor(consoleColorWin, @"|@@@@|     |####|", "\n");
            WriteColor(consoleColorWin, @"\@@@@|     |####/", "\n");
            WriteColor(consoleColorWin, @" \@@@|     |###/", "\n");
            WriteColor(consoleColorWin, @"  `@@|_____|##'", "\n");
            WriteColor(consoleColorWin, @"       (O)", "\n");
            WriteColor(consoleColorWin, @"    .-'''''-.", "\n");
            WriteColor(consoleColorWin, @"  .'  * * *  `.", "\n");
            WriteColor(consoleColorWin, @" :  *       *  :", "\n");
            WriteColor(consoleColorWin, @":~ B A T T L E ~:", "\n");
            WriteColor(consoleColorWin, @": ~ A W A R D ~ :", "\n");
            WriteColor(consoleColorWin, @" :  *       *  :", "\n");
            WriteColor(consoleColorWin, @"  `.  * * *  .'", "\n");
            WriteColor(consoleColorWin, @"    `-.....-'", "\n", null);

            MissionImpossible();
        }

        public static  void ShowLost()
        {
            Console.Clear();
            Console.Beep();

            WriteColor(consoleColorLost, @"                 uuuuuuu", "\n");
            WriteColor(consoleColorLost, @"             uu$$$$$$$$$$$uu", "\n");
            WriteColor(consoleColorLost, @"          uu$$$$$$$$$$$$$$$$$uu", "\n");
            WriteColor(consoleColorLost, @"         u$$$$$$$$$$$$$$$$$$$$$u", "\n");
            WriteColor(consoleColorLost, @"        u$$$$$$$$$$$$$$$$$$$$$$$u", "\n");
            WriteColor(consoleColorLost, @"       u$$$$$$$$$$$$$$$$$$$$$$$$$u", "\n");
            WriteColor(consoleColorLost, @"       u$$$$$$$$$$$$$$$$$$$$$$$$$u", "\n");
            WriteColor(consoleColorLost, "       u$$$$$$\"   \"$$$\"   \"$$$$$$u", "\n");
            WriteColor(consoleColorLost, "       \"$$$$\"      u$u       $$$$\"","\n");
            WriteColor(consoleColorLost, @"        $$$u       u$u       u$$$", "\n");
            WriteColor(consoleColorLost, @"        $$$u      u$$$u      u$$$", "\n");
            WriteColor(consoleColorLost, "         \"$$$$uu$$$   $$$uu$$$$\"", "\n");
            WriteColor(consoleColorLost, "          \"$$$$$$$\"   \"$$$$$$$\"", "\n");
            WriteColor(consoleColorLost, @"            u$$$$$$$u$$$$$$$u", "\n");
            WriteColor(consoleColorLost, "             u$\"$\"$\"$\"$\"$\"$u", "\n");
            WriteColor(consoleColorLost, @"  uuu        $$u$ $ $ $ $u$$       uuu", "\n");
            WriteColor(consoleColorLost, @" u$$$$        $$$$$u$u$u$$$       u$$$$", "\n");
            WriteColor(consoleColorLost, "  $$$$$uu      \"$$$$$$$$$\"     uu$$$$$$\"", "\n");
            WriteColor(consoleColorLost, "u$$$$$$$$$$$uu    \"\"\"\"\"    uuuu$$$$$$$$$$\"","\n");
            WriteColor(consoleColorLost, "$$$$\"\"\"$$$$$$$$$$uuu   uu$$$$$$$$$\"\"\"$$$\"\"","\n");
            WriteColor(consoleColorLost, " \"\"\"      \"\"$$$$$$$$$$$uu \"\"$\"\"\"\"", "\n");
            WriteColor(consoleColorLost, @"           uuuu ""$$$$$$$$$$uuu", "\n");
            WriteColor(consoleColorLost, @"  u$$$uuu$$$$$$$$$uu ""$$$$$$$$$$$uuu$$$", "\n");
            WriteColor(consoleColorLost, "  $$$$$$$$$$\"\"\"\"           \"\"$$$$$$$$$$$\"","\n");
            WriteColor(consoleColorLost, "   \"$$$$$\"                      \"\"$$$$\"\"", "\n");
            WriteColor(consoleColorLost, "     $$$\"                         $$$$\"", "\n", null);

            StarWars();

        }

        private static void StarWars()
        {
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(698, 350);
            Console.Beep(523, 150);
            Console.Beep(415, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
        }

        static void MissionImpossible()
        {
            Console.Beep(784, 150);
            Thread.Sleep(300);
            Console.Beep(784, 150);
            Thread.Sleep(300);
            Console.Beep(932, 150);
            Thread.Sleep(150);
            Console.Beep(1047, 150);
            Thread.Sleep(150);
            Console.Beep(784, 150);
            Thread.Sleep(300);
            Console.Beep(784, 150);
            Thread.Sleep(300);
            Console.Beep(699, 150);
            Thread.Sleep(150);
            Console.Beep(740, 150);
            Thread.Sleep(150);
            Console.Beep(784, 150);
            Thread.Sleep(300);
            Console.Beep(784, 150);
            Thread.Sleep(300);
            Console.Beep(932, 150);
            Thread.Sleep(150);
            Console.Beep(1047, 150);
            Thread.Sleep(150);
            Console.Beep(784, 150);
            Thread.Sleep(300);
            Console.Beep(784, 150);
            Thread.Sleep(300);
            Console.Beep(699, 150);
            Thread.Sleep(150);
            Console.Beep(740, 150);
            Thread.Sleep(150);
            Console.Beep(932, 150);
            Console.Beep(784, 150);
            Console.Beep(587, 1200);
            Thread.Sleep(75);
            Console.Beep(932, 150);
            Console.Beep(784, 150);
            Console.Beep(554, 1200);
            Thread.Sleep(75);
            Console.Beep(932, 150);
            Console.Beep(784, 150);
            Console.Beep(523, 1200);
            Thread.Sleep(150);
            Console.Beep(466, 150);
            Console.Beep(523, 150);
        }
    }
}