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

Game
 - public Game(Board, Players[])
 - Board
 - Player[]
 - Dice
 - current turn Player
 - NextTurn()
 - public HasEnded() -> bool
 - MoveCurrentPlayer(int) 
    - reactive to dice roll
    - delegates to current player.move()
 - GetFinalPosition(position) -> position
    - reactive to player positions
    - delegates to board.GetFinalPosition(position)

Dice
 - public Roll() -> int - observed by Game

Board
 - size
 - BoardObjects[] -> interface for snakes and ladders
 - internal GetFinalPosition(position) -> position
    - looks like recursive function

Snake
 - (start and end positions)[]
 - GetFinalPosition(position) -> position

Ladder
 - (start and end positions)[]
 - GetFinalPosition(position) -> position

Player
 - Name
 - Position - observed by Game
 - internal Move(count)
 - internal SetPosition(Position)

Position
 - x
 - y
