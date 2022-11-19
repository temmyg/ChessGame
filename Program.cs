using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessPawn
{
    class Program
    {
        static void Main(string[] args)
        {
            var chess = new Chess();

            var canMove = chess.CanPawnMove(2, 2, 4, 3, PieceColor.White);
        }
    }

    public class Chess
    {
        // key: position; rowIndex: 1 to 8, colIndex: 1 to 8, position = rowIndex * 10 + colIndex
        Dictionary<int, PieceType> whitePieces = new Dictionary<int, PieceType>();
        Dictionary<int, PieceType> blackPieces = new Dictionary<int, PieceType>();

        public Chess()
        {
            Initialize();
        }

        /// <summary>
        /// from current row, column, can we move to next row, column
        /// </summary>
        /// <param name="currRow"></param>
        /// <param name="currCol"></param>
        /// <param name="nextRow"></param>
        /// <param name="nextCol"></param>
        /// <param name="color"></param>
        /// <returns>if can move</returns>
        public bool CanPawnMove(int currRow, int currCol, int nextRow, int nextCol, PieceColor color)
        {
            var mySidePieces = GetColoredPieces(color);
            var currPos = currRow * 10 + currCol;

            if (!mySidePieces.ContainsKey(currPos) || mySidePieces[currPos] != PieceType.Pawn)
            {
                throw new Exception("The current position of the Paw is not valid.");
            }

            var rowDistance = nextRow - currRow;

            // after above if check, this piece type can only be pawn and currRow can only be from 2 to 8
            if (currRow == 8 
                || !(rowDistance == 1 || rowDistance == 2)
                || (rowDistance == 2 && currRow != 2))
            {
                return false;
            }

            var possibleCols = new HashSet<int> { currCol };

            if (rowDistance == 1 && currCol < 8)
            {
                possibleCols.Add(currCol + 1);
            }

            if (rowDistance == 1 && currCol > 1)
            {
                possibleCols.Add(nextCol - 1);
            }

            if (!possibleCols.Contains(nextCol))
            {
                return false;
            }

            var isDiagonal = Math.Abs(nextCol - currCol) == 1;

            var enemyPositions = (ICollection<int>)GetColoredPieces(1 - color).Keys;
            var allPositions = mySidePieces.Keys.Concat(enemyPositions);

            var nextPosition = nextRow * 10 + nextCol;

            if (isDiagonal && !enemyPositions.Contains(nextPosition))
            {
                return false;
            }

            if (!isDiagonal && allPositions.Contains(nextPosition))
            {
                return false;
            }

            return true;
        }

        private void Initialize()
        {
            for (int i = 0; i < 8; i++)
            {
                whitePieces.Add(2 * 10 + i + 1, PieceType.Pawn);
                blackPieces.Add((9 - 2) * 10 + i + 1, PieceType.Pawn);
            }

            for (PieceType i = PieceType.Rook; i <= PieceType.King; i++)
            {
                int rowWhite = 1, rowBlack = 8;
                switch (i)
                {
                    case PieceType.Rook:
                        whitePieces.Add(rowWhite * 10 + 1, i);
                        whitePieces.Add(rowWhite * 10 + 9 - 1, i);
                        blackPieces.Add(rowBlack * 10 + 1, i);
                        blackPieces.Add(rowBlack * 10 + 9 - 1, i);
                        break;
                    case PieceType.Knight:
                        whitePieces.Add(rowWhite * 10 + 2, i);
                        whitePieces.Add(rowWhite * 10 + 9 - 2, i);
                        blackPieces.Add(rowBlack * 10 + 2, i);
                        blackPieces.Add(rowBlack * 10 + 9 - 2, i);
                        break;
                    case PieceType.Bishop:
                        whitePieces.Add(rowWhite * 10 + 3, i);
                        whitePieces.Add(rowWhite * 10 + 9 - 3, i);
                        blackPieces.Add(rowBlack * 10 + 3, i);
                        blackPieces.Add(rowBlack * 10 + 9 - 3, i);
                        break;
                    case PieceType.Queen:
                        whitePieces.Add(rowWhite * 10 + 4, i);
                        blackPieces.Add(rowBlack * 10 + 4, i);
                        break;
                    default:
                        whitePieces.Add(rowWhite * 10 + 5, i);
                        blackPieces.Add(rowBlack * 10 + 5, i);
                        break;
                }
            }
        }

        private Dictionary<int, PieceType> GetColoredPieces(PieceColor color)
        {
            return color == PieceColor.White ? whitePieces : blackPieces;
        }
    }

    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public enum PieceColor
    {
        White,
        Black
    }
}
