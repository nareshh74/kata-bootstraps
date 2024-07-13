bowling rules

 - 1 game has 10 rounds
 - 1 round has score - determined by 2 rolls
    - score = no. of pins in both rolls + bonus
    - bonus = if strike:
                 - bonus += next 2 rounds scores
              else if spare:
                 - bonus += next 1 round score
 - 1 roll - player can knock 10 pins



Game
 - Round[10]
 - Roll(int)
 - GetScore() -> int


Round
 - Roll[] - upto 2
 - GetScore() -> int 

TenthRound : Round
 - Roll[] - upto 3

Roll
 - Pins