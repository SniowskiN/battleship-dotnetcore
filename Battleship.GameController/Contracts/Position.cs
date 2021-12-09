using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.GameController.Contracts
{
    /// <summary>
    ///     The position.
    /// </summary>
    public class Position
    {
        private bool _wasHit;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="column">
        /// The column.
        /// </param>
        /// <param name="row">
        /// The row.
        /// </param>
        public Position(Letters column, int row, Ship ship = null)
        {
            Column = column;
            Row = row;
            Ship = ship;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Position" /> class.
        /// </summary>
        public Position()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the column.
        /// </summary>
        public Letters Column { get; set; }

        /// <summary>
        ///     Gets or sets the row.
        /// </summary>
        public int Row { get; set; }
        public bool WasHit { get => _wasHit;
            set
            {
                _wasHit = value;
                if (Ship.Positions.Where(x => x._wasHit).Count() == Ship.Size)
                {
                    Ship.IsSunk = true;
                }
            }
        }

        public Ship Ship;
        #endregion

        #region Public Methods and Operators

        public static Position ParsePosition(string input)
        {
            if (input.Length != 2)
                return null;

            int number;
            Letters letter;

            if (!Enum.TryParse(input.ToUpper().Substring(0, 1), out letter) ||
                !int.TryParse(input.Substring(1, 1), out number))
                return null;

            if (number > 8)
                return null;

            return new Position(letter, number);
        }

        public static Position GetRandomPosition()
        {
            int rows = 8;
            int lines = 8;

            var random = new Random();
            var letter = (Letters)random.Next(lines);
            var number = random.Next(rows);
            var position = new Position(letter, number);
            return position;
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var position = obj as Position;
            if (position == null)
            {
                return false;
            }

            return position.Column == Column && position.Row == Row;
        }

        public override int GetHashCode()
        {
            return Column.GetHashCode() + Row.GetHashCode() ;
        }
        #endregion

    }
}