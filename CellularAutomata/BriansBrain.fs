module CellularAutomataDemo.BriansBrain

open CellularAutomataModule
  
type State =
  | Alive
  | Dying
  | Dead

let rules state (neighborhood : State array) : State =
  let liveNeighbors = neighborhood |> Array.filter (fun element -> element = Alive) |> Array.length
  match state with
  // If a square is on, it turns off.
  | Alive -> Dying
  // When a square turns off, it can't turn on in the very next iteration.
  | Dying -> Dead
  // If a square is off, it turns on if exactly two neighboring squares are on. 
  | Dead when liveNeighbors = 2 -> Alive
  | Dead -> Dead

let getNeighborhood = getMooreNeighborhood

let stateToChar (state: State) : char =
  match state with
  | Alive -> 'O'
  | Dying -> 'X'
  | Dead  -> ' '