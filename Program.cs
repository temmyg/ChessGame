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
            var friendPieces = GetColoredPieces(color);
            var currPos = GetCombinedPosition(currRow, currCol);

            if (!friendPieces.ContainsKey(currPos) || friendPieces[currPos] != PieceType.Pawn)
            {
                throw new Exception("The current position of the Paw is not valid.");
            }

            if(currRow > 7 || currRow < 2)
            {
                return false;
            }

            var enemyPositions = (ICollection<int>)GetColoredPieces(1 - color).Keys;
            var allPositions = friendPieces.Keys.Concat(enemyPositions);

            var validNextPositions = new HashSet<int>();

            var nextPosition = GetCombinedPosition(currRow + 1, currCol);
            if (!allPositions.Contains(nextPosition))
            {
                validNextPositions.Add(nextPosition);
            }

            if(currRow == 2)
            {
                nextPosition = GetCombinedPosition(currRow + 2, currCol);

                if(!allPositions.Contains(nextPosition))
                {
                    validNextPositions.Add(nextPosition);
                }
            }

            if(currCol < 8)
            {
                nextPosition = GetCombinedPosition(currRow + 1, currCol + 1);
                if (enemyPositions.Contains(nextPosition))
                {
                    validNextPositions.Add(nextPosition);
                }
            }

            if(currCol > 1)
            {
                nextPosition = GetCombinedPosition(currRow + 1, currCol - 1);
                if (enemyPositions.Contains(nextPosition))
                {
                    validNextPositions.Add(nextPosition);
                }
            }

            if(validNextPositions.Contains(GetCombinedPosition(nextRow, nextCol)))
            {
                return true;
            }

            return false;
        }

        private void Initialize()
        {
            for (int i = 0; i < 8; i++)
            {
                whitePieces.Add(GetCombinedPosition(2, i+1), PieceType.Pawn);
                blackPieces.Add(GetCombinedPosition(9-2, i+1), PieceType.Pawn);
            }

            for (PieceType i = PieceType.Rook; i <= PieceType.King; i++)
            {
                int rowWhite = 1, rowBlack = 8;
                switch (i)
                {
                    case PieceType.Rook:
                        whitePieces.Add(GetCombinedPosition(rowWhite, 1), i);
                        whitePieces.Add(GetCombinedPosition(rowWhite, 9-1), i);
                        blackPieces.Add(GetCombinedPosition(rowBlack, 1), i);
                        blackPieces.Add(GetCombinedPosition(rowBlack, 9-1), i);
                        break;
                    case PieceType.Knight:
                        whitePieces.Add(GetCombinedPosition(rowWhite, 2), i);
                        whitePieces.Add(GetCombinedPosition(rowWhite, 9 - 2), i);
                        blackPieces.Add(GetCombinedPosition(rowBlack, 2), i);
                        blackPieces.Add(GetCombinedPosition(rowBlack, 9 - 2), i);
                        break;
                    case PieceType.Bishop:
                        whitePieces.Add(GetCombinedPosition(rowWhite, 3), i);
                        whitePieces.Add(GetCombinedPosition(rowWhite, 9 - 3), i);
                        blackPieces.Add(GetCombinedPosition(rowBlack, 3), i);
                        blackPieces.Add(GetCombinedPosition(rowBlack, 9 - 3), i);
                        break;
                    case PieceType.Queen:
                        whitePieces.Add(GetCombinedPosition(rowWhite, 4), i);
                        blackPieces.Add(GetCombinedPosition(rowBlack, 4), i);
                        break;
                    default:
                        whitePieces.Add(GetCombinedPosition(rowWhite, 5), i);
                        blackPieces.Add(GetCombinedPosition(rowBlack, 5), i);
                        break;
                }
            }
        }

        private Dictionary<int, PieceType> GetColoredPieces(PieceColor color)
        {
            return color == PieceColor.White ? whitePieces : blackPieces;
        }

        private int GetCombinedPosition(int row, int col) { return row * 10 + col; }
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
