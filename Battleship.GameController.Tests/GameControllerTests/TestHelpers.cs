using Battleship.GameController.Contracts;
using System;
using System.Collections.Generic;

namespace Battleship.GameController.Tests.GameControllerTests
{
    public class TestHelpers
    {
        public Position GetInShipPosition(List<Ship> fleet)
        {
            return fleet[0].Positions[0];
        }

        public Position GetNotInShipPosition(List<Ship> fleet)
        {
            Position position = new Position();
            bool notfound = true;

            foreach (int i in Enum.GetValues(typeof(Letters)))
            {
                for (int j = 0; j<=8; j++)
                {
                    position = new Position((Letters)i, j);

                    foreach (var ship in fleet)
                    {
                        if (ship.Positions.Contains(position))
                        {
                            notfound = false;
                            break;
                        }
                        notfound = true;
                    }

                    if (notfound)
                        return position;
                }
            }
            return position;
        }
        public void DeployFleet(List<Ship> fleet)
        {
            fleet[0].Positions.Add(new Position(Letters.A, 1, fleet[0]));
            fleet[0].Positions.Add(new Position(Letters.A, 2, fleet[0]));
            fleet[0].Positions.Add(new Position(Letters.A, 3, fleet[0]));
            fleet[0].Positions.Add(new Position(Letters.A, 4, fleet[0]));
            fleet[0].Positions.Add(new Position(Letters.A, 5, fleet[0]));

            fleet[1].Positions.Add(new Position(Letters.C, 1, fleet[1]));
            fleet[1].Positions.Add(new Position(Letters.C, 2, fleet[1]));
            fleet[1].Positions.Add(new Position(Letters.C, 3, fleet[1]));
            fleet[1].Positions.Add(new Position(Letters.C, 4, fleet[1]));

            fleet[2].Positions.Add(new Position(Letters.E, 1, fleet[2]));
            fleet[2].Positions.Add(new Position(Letters.E, 2, fleet[2]));
            fleet[2].Positions.Add(new Position(Letters.E, 3, fleet[2]));

            fleet[3].Positions.Add(new Position(Letters.G, 1, fleet[3]));
            fleet[3].Positions.Add(new Position(Letters.G, 2, fleet[3]));
            fleet[3].Positions.Add(new Position(Letters.G, 3, fleet[3]));

            fleet[4].Positions.Add(new Position(Letters.G, 5, fleet[4]));
            fleet[4].Positions.Add(new Position(Letters.G, 6, fleet[4]));
        }
    }
}
