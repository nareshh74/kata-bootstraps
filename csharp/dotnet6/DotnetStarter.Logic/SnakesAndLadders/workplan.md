https://workat.tech/machine-coding/practice/snake-and-ladder-problem-zgtac9lxwntg

snakes and ladder

i/p => 
no. of snakes - start and end indices of each snake
no. of ladders - start and end indices of each ladder
no. of players - name of each player

o/p =>
print the move

data
 - board size
 - player name and position
 - snake and ladder positions
 - game status

behavior
 - dice gives random
 - snake and ladder rules
 - player turn rule
 - game won rule
 - beyond 100, invalid move rule

design

GameBuilder
 - public Build() => Game
 - public AddBoardObject(BoardObject)
     - board object is snake or ladder
 - public AddPlayer(Player)

Game
 - public Game(Board, Players[])
 - Board board
 - Players
 - Dice
 - public GiveTurnToNextPlayer()
    - delegates to Players.GiveTurnToNextPlayer()
 - public bool HasEnded()
 - public void MoveCurrentPlayer(int count)
    - reactive to dice roll
    - delegates to currentPlayer.move()
 - int GetFinalPosition(int position)
    - reactive to player positions
    - delegates to board.GetFinalPosition(position)
    - sets player position using SetPosition(position) API

Dice - static class
 - public int Roll()

Board
 - public Board(int size, BoardObject[])
 - size
 - BoardObject[] -> interface for snakes and ladders
 - internal int GetFinalPosition(int position)
    - looks like recursive function

BoardObject
 - public int Start
 - public int End
 - public static BoardObject CreateSnake(int start, int end)
 - public static BoardObject CreateLadder(int start, int end)

Snake
 - private Snake(Position start, Position end)

Ladder
 - private Ladder(Position start, Position end)

Players
 - public Players(Player[] players)
 - Player[] players
 - int currentPlayerIndex
 - public Player GetCurrentPlayer()
 - public void GiveTurnToNextPlayer()

Player
 - public Player(string name)
 - string Name
 - int Position - observed by Game
 - internal Move(count)
 - internal SetPosition(position)


***optional requirements***
 - The game is played with two dice instead of 1 and so the total dice value could be between 2 to 12 in a single move. 
    - DiceFactory, IDice, DiceV2
    
    or
    
    - DiceFactory, IDice, DiceDecorator
 - The board size can be customizable and can be taken as input before other input (snakes, ladders, players).
    - add WithBoardSize(int size) to GameBuilder and use it in Build()
 - In case of more than 2 players, the game continues until only one player is left.
     - strategy pattern for Game.HasEnded()
     - IGameEndedStrategy, AtleastOnePlayerWonStrategy, AtMostOnePlayerLeftStrategy
     - set in Game's ctor based on player count
     - both strategies need Players to be injected
 - On getting a 6, you get another turn and on getting 3 consecutive 6s, all the three of those get cancelled.
     - DiceFactory, IDice, DiceDecorator
 - On starting the application, the snakes and ladders should be created programmatically without any user input, keeping in mind the constraints mentioned in rules.
    - not clear on the requirement