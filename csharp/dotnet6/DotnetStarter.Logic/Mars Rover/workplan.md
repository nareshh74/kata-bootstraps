mars rover kata


data
 - grid size
 - no. of rovers
 - rover positions

behaviour
 - move
 - turn left, right


Grid
 - int length
 - int breadth
 - list<Rover>
 - IsRoverInValidPositionToMove(rover) -> bool
 - AddRover(Rover) -> grid w/o rovers means nothing in this context. so move this to ctor
 - MoverRover(rover)
 - GetRoverPosition(rover) -> Position
 - TurnRover(rover, direction) -> conceptually, grid contains rovers so, exposing all public APIs through grid makes sense. this way, client doesnt have to know about rover's behaviour.

Rover
 - Position
 - void Turn(direction)
 - void Move() 
 - -> needs to check if the move is valid
 - -> if we do this by calling the containing class grid's IsValid method, then move takes implicit dependency on grid
 - -> instead, we can expose the Move API in Grid and pass the rover as a parameter. this way client code can call grid.Move(rover) and so it's clear that both rover and grid are involved in the move operation
 - GetPosition() -> Position => using public getters and private setters

Position
 - int x
 - int y
 - char direction




tests
 - L shud turn left
 - R shud turn right
 - move shud move 1 point
 - out of bounds check
 - get position shud return correct position
 - shudnt move a rover that is not in the grid
 - sequence of steps