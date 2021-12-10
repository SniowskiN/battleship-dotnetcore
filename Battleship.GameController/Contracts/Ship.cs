using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Battleship.GameController.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The ship.
    /// </summary>
    public class Ship
    {
        private bool isPlaced;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class.
        /// </summary>
        public Ship()
        {
            Positions = new List<Position>();
            availablePositions = new List<Position>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        public List<Position> Positions { get; set; }

        /// <summary>
        /// The color of the ship
        /// </summary>
        public ConsoleColor Color { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Indicating that ship sunk
        /// </summary>
        public bool IsSunk { get; set; }

        

        private List<Position> availablePositions;
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add position.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public bool AddPosition(string input)
        {
            var position = Position.ParsePosition(input);
            if (position == null)
                return false;

            if (Positions.Contains(position))
                return false;

            position.Ship = this;
            Positions.Add(position);
            return true;

            //if (Positions.Count == 0 || availablePositions.Any(x => x == position))
            //{


            //    var row = position.Row;
            //    var column = (int)position.Column;

            //    var tmpPos = new Position();

            //    if(row - 1 > 0)
            //        tmpPos = new Position((Letters)column, row);


            //    if(row + 1 < 9)
            //        tmpPos = new Position(position.Column, position.Row +1));



            //    return true;
            //}

            //return false;
        }

        public bool IsPlaced
        {
            get { return isPlaced; }
            set
            {
                if (value.Equals(isPlaced)) return;
                isPlaced = value;
            }
        }
        #endregion

        public override bool Equals(object obj)
        {
            var ship = obj as Ship;
            if (ship == null)
            {
                return false;
            }

            return ship.Name == Name && ship.Size == Size;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Size.GetHashCode();
        }
    }
}