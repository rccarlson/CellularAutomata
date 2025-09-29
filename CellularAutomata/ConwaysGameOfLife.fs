module CellularAutomataDemo.ConwaysGameOfLife

type State =
  | Alive
  | Dead

let rules state (neighborhood : State array) : State =
  let liveNeighbors = neighborhood |> Array.filter (fun element -> element = Alive) |> Array.length
  match state with
  // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
  | Alive when liveNeighbors < 2 -> Dead
  // Any live cell with more than three live neighbours dies, as if by overpopulation.
  | Alive when liveNeighbors > 3 -> Dead
  // Any live cell with two or three live neighbours lives on to the next generation.
  | Alive when liveNeighbors = 2 || liveNeighbors = 3 -> Alive
  // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
  | Dead when liveNeighbors = 3 -> Alive
  // All other dead cells stay dead.
  | Dead -> Dead
  // Unreachable state for completeness
  | Alive -> Alive

let stateToChar (state: State) : char =
  match state with
  | Alive -> 'O'
  | Dead  -> ' '

