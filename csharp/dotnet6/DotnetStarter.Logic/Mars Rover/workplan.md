mars rover kata


data
    grid size
    no. of rovers
    rover positions

behaviour
    move
    turn left, right


Grid
    int length
    int breadth
    list<Rover>
    IsValid(Position) -> bool
    AddRover(Rover)

Rover
    Position
    void Turn(direction)
    void Move()
    GetPosition() -> Position => using public getters and private setters

Position
    int x
    int y
    char direction




tests
 - L shud turn left
 - R shud turn right
 - move shud move 1 point
 - out of bounds check
 - get position shud return correct position
 - sequence of steps