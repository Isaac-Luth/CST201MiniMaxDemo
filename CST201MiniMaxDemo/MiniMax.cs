﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CST201MiniMaxDemo.Board;
using Point = CST201MiniMaxDemo.Board.Point;
using CellState = CST201MiniMaxDemo.Board.CellState;
using WinnerStatus = CST201MiniMaxDemo.Board.WinnerStatus;
using System.Runtime.InteropServices;
using static System.Formats.Asn1.AsnWriter;

namespace CST201MiniMaxDemo
{
    public static class MiniMax
    {
        private static List<Observer> _observers = new List<Observer>();

        public static void AddObserver(Observer observer)
        {
            _observers.Add(observer);
        }

        private const int Min = int.MinValue;
        private const int Max = int.MaxValue;

        internal static (Point, int) GetBestMove(Board board, CellState turn = CellState.Computer)
        {
            Point[] moves = GetValidMoves(board);
            Point bestMove = new Point();
            int bestScore;

            if (turn == CellState.Computer)
            {
                bestScore = Min;
            }
            else
            {
                bestScore = Max;
            }

            WinnerStatus status = board.GetWinnerStatus();
            if (status == WinnerStatus.Computer)
            {
                return (new Point(-1, -1), Max);
            }
            else if (status == WinnerStatus.Tie)
            {
                return (new Point(-1, -1), 0);
            }
            else if (status == WinnerStatus.Player)
            {
                return (new Point(-1, -1), Min);
            }

            foreach (var move in moves)
            {
                board.MakeMove(move, turn);
                int score = GetBestMove(board, FlipTurn(turn)).Item2;
                OutputToObservers(board);
                board.UndoMove(move);

                if (turn == CellState.Computer)
                {
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = move;
                    }
                }
                else
                {
                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestMove = move;
                    }
                }
            }

            return (bestMove, bestScore);
        }

        private static void OutputToObservers(Board board)
        {
            foreach(var observer in _observers)
            {
                observer.Observe(board);
            }
        }

        private static CellState FlipTurn(CellState state)
        {
            if (state == CellState.Computer) return CellState.Player;
            else return CellState.Computer;
        }

        private static Point[] GetValidMoves(Board board)
        {
            Point[] validMoves = new Point[board.MaxMoves - board.MovesMade];
            int index = 0;

            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.Grid[row, col] == CellState.Empty)
                    {
                        validMoves[index] = new Point(row, col);
                        index++;
                    }
                }
            }

            return validMoves;
        }

    }
}
