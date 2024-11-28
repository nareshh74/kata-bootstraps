using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotnetStarter.Logic.SnakesAndLadders
{
    public class GameBuilder
    {
        private List<BoardObject> _boardObjects;
        private List<Player> _players;

        public GameBuilder()
        {
            this._boardObjects = new();
            this._players = new();
        }

        public void AddBoardObject(BoardObject boardObject)
        {
            this._boardObjects.Add(boardObject);
        }

        public void AddPlayer(Player player)
        {
            this._players.Add(player);
        }

        public Game Build()
        {
            var players = new Players(this._players);
            var board = new Board(100, this._boardObjects);
            return new Game(board, players);
        }
    }

    public class Player
    {
        private readonly string _name;
        private int _position;

        public Player(string name)
        {
            this._name = name;
            this._position = 0;
        }

        public int Position => this._position;

        public void MoveToPosition(int position)
        {
            this._position = position;
        }

        public override string ToString()
        {
            return this._name;
        }
    }

    internal static class Dice
    {
        /*
         *Gaurav rolled a 6 and moved from 0 to 6
           Sagar rolled a 1 and moved from 0 to 1
           Gaurav rolled a 6 and moved from 6 to 12
           Sagar rolled a 4 and moved from 1 to 5
           Gaurav rolled a 4 and moved from 12 to 16
           Sagar rolled a 6 and moved from 5 to 11
           Gaurav rolled a 5 and moved from 16 to 21
           Sagar rolled a 4 and moved from 11 to 15
           Gaurav rolled a 1 and moved from 21 to 22
           Sagar rolled a 6 and moved from 15 to 21
           Gaurav rolled a 6 and moved from 22 to 28
           Sagar rolled a 2 and moved from 21 to 23
           Gaurav rolled a 6 and moved from 28 to 34
           Sagar rolled a 6 and moved from 23 to 29
           Gaurav rolled a 5 and moved from 34 to 39==
           Sagar rolled a 2 and moved from 29 to 31
           Gaurav rolled a 2 and moved from 39 to 20
           Sagar rolled a 5 and moved from 31 to 36
           Gaurav rolled a 3 and moved from 20 to 23
           Sagar rolled a 5 and moved from 36 to 20
           Gaurav rolled a 6 and moved from 23 to 29
           Sagar rolled a 3 and moved from 20 to 23==
           Gaurav rolled a 2 and moved from 29 to 31
           Sagar rolled a 3 and moved from 23 to 26
           Gaurav rolled a 3 and moved from 31 to 34
           Sagar rolled a 5 and moved from 26 to 31
           Gaurav rolled a 3 and moved from 34 to 37
           Sagar rolled a 4 and moved from 31 to 35
           Gaurav rolled a 2 and moved from 37 to 39
           Sagar rolled a 5 and moved from 35 to 40==
           Gaurav rolled a 2 and moved from 39 to 20
           Sagar rolled a 5 and moved from 40 to 45
           Gaurav rolled a 2 and moved from 20 to 22
           Sagar rolled a 6 and moved from 45 to 68
           Gaurav rolled a 3 and moved from 22 to 25
           Sagar rolled a 3 and moved from 68 to 91
           Gaurav rolled a 5 and moved from 25 to 30==
           Sagar rolled a 2 and moved from 91 to 73
           Gaurav rolled a 5 and moved from 30 to 35
           Sagar rolled a 6 and moved from 73 to 79
           Gaurav rolled a 5 and moved from 35 to 40
           Sagar rolled a 1 and moved from 79 to 80
           Gaurav rolled a 4 and moved from 40 to 44
           Sagar rolled a 2 and moved from 80 to 82
           Gaurav rolled a 5 and moved from 44 to 9==
           Sagar rolled a 4 and moved from 82 to 86
           Gaurav rolled a 1 and moved from 9 to 32
           Sagar rolled a 6 and moved from 86 to 92
           Gaurav rolled a 3 and moved from 32 to 35
           Sagar rolled a 4 and moved from 92 to 96
           Gaurav rolled a 1 and moved from 35 to 36
           Sagar rolled a 1 and moved from 96 to 97
           Gaurav rolled a 1 and moved from 36 to 37
           Sagar rolled a 5 and moved from 97 to 97
           Gaurav rolled a 6 and moved from 36 to 42
           Sagar rolled a 3 and moved from 97 to 100
         */
        private static readonly Queue<int> Rolls = new(new List<int>()
        {
            6, 1, 6, 4, 4, 6, 5, 4, 1, 6, 6, 2, 6, 6, 5, 2, 2, 5, 3, 5, 6, 3, 2, 3, 3, 5, 3, 4, 2, 5, 2, 5, 2, 6, 3, 3, 5, 2, 5, 6, 5, 1, 4, 2, 5, 4, 1, 6, 3, 4, 1, 1, 1, 5, 6, 3
        });

        public static int Roll()
        {
            return Dice.Rolls.Dequeue();
        }
    }

    public class Game
    {
        private readonly Board _board;
        private readonly Players _players;

        public Game(Board board, Players players)
        {
            this._board = board;
            this._players = players;
        }

        private bool HasEnded()
        {
            return this._players.Any(player => player.Position == this._board.Size);
        }

        public void Simulate()
        {
            while (!this.HasEnded())
            {
                var count = Dice.Roll();
                this.MoveCurrentPlayer(count);
                this._players.GiveTurnToNextPlayer();
            }
        }

        private void MoveCurrentPlayer(int count)
        {
            var currentPosition = this._players.Current.Position;
            var newPosition = this.GetFinalPosition(count + currentPosition);
            if (currentPosition + count <= this._board.Size)
            {
                this._players.Current.MoveToPosition(newPosition);
            }
            else
            {
                newPosition = currentPosition;
            }

            Console.WriteLine($"{this._players.Current} rolled a {count} and moved from {currentPosition} to {newPosition}");
            if(newPosition == this._board.Size)
            {
                Console.WriteLine($"{this._players.Current} wins the game");
            }
        }

        private int GetFinalPosition(int position)
        {
            return this._board.GetFinalPosition(position);
        }
    }

    public class Players : IEnumerable<Player>
    {
        private readonly List<Player> _players;
        private int _currentPlayerIndex;

        public Players(List<Player> players)
        {
            this._players = players;
            this._currentPlayerIndex = 0;
        }

        public Player Current => this._players[this._currentPlayerIndex];

        public void GiveTurnToNextPlayer()
        {
            this._currentPlayerIndex = (this._currentPlayerIndex + 1) % this._players.Count;
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return this._players.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class Board
    {
        private readonly int _size;
        private readonly List<BoardObject> _boardObjects;

        public Board(int size, List<BoardObject> boardObjects)
        {
            this._size = size;
            this._boardObjects = boardObjects;
        }

        public int Size => this._size;

        public int GetFinalPosition(int position)
        {
            foreach (var boardObject in this._boardObjects)
            {
                if (position == boardObject.Start)
                {
                    return this.GetFinalPosition(boardObject.End);
                }
            }
            return position;
        }
    }

    public abstract class BoardObject
    {
        public int Start { get; }
        public int End { get; }

        protected BoardObject(int start, int end)
        {
            this.Start = start;
            this.End = end;
        }

        public static BoardObject CreateSnake(int start, int end)
        {
            return new Snake(start, end);
        }

        public static BoardObject CreateLadder(int start, int end)
        {
            return new Ladder(start, end);
        }
    }

    public class Ladder : BoardObject
    {
        public Ladder(int start, int end) : base(start, end)
        {
        }
    }

    public class Snake : BoardObject
    {
        public Snake(int start, int end) : base(start, end)
        {
        }
    }
}
