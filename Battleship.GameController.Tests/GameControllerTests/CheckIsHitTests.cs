namespace Battleship.GameController.Tests.GameControllerTests
{
    using System;
    using System.Linq;
    using Battleship.GameController.Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The game controller tests.
    /// </summary>
    [TestClass]
    public class GameControllerTests
    {
        private TestHelpers testHelpers = new TestHelpers();

        /// <summary>
        /// The should hit the ship.
        /// </summary>
        [TestMethod]
        public void ShouldHitTheShip()
        {
            var ships = GameController.InitializeShips().ToList();
            testHelpers.DeployFleet(ships);

            var inShipPos = testHelpers.GetInShipPosition(ships);

            var result = GameController.CheckIsHit(ships, inShipPos);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// The should not hit the ship.
        /// </summary>
        [TestMethod]
        public void ShouldNotHitTheShip()
        {
            var ships = GameController.InitializeShips().ToList();
            testHelpers.DeployFleet(ships);

            var notInShipPos = testHelpers.GetNotInShipPosition(ships);

            var result = GameController.CheckIsHit(ships, notInShipPos);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// The throw exception if positstion is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionIfPositstionIsNull()
        {
            GameController.CheckIsHit(GameController.InitializeShips(), null);
        }

        /// <summary>
        /// The throw exception if ship is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionIfShipIsNull()
        {
            GameController.CheckIsHit(null, new Position(Letters.H, 1));
        }

        [TestMethod]
        public void SimulatePlay()
        {
            bool isHit = false;
            var ships = GameController.InitializeShips().ToList();
            testHelpers.DeployFleet(ships);
            foreach (var ship in ships)
            {
                foreach(var pos in ship.Positions)
                  isHit = GameController.CheckIsHit(ships, pos);
            }

            var result = ships.Where(x => !x.IsSunk).Count();

            Assert.AreEqual(0, result);
        }
    }
}