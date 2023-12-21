namespace CellularAutomata

module ConwaysGameOfLifeModule =
  open CellularAutomataModule
  
  type State = | Alive | Dead with override this.ToString() = match this with | Alive -> "A" | Dead  -> "D"
  type Cache = { LiveNeighbors : int }

  type ConwaysGameOfLife = Board<State>

  let rules : Rule<State,Cache> array =
    [|
      fun state cache ->
        match state with
        // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
        | Alive when cache.LiveNeighbors < 2 -> Some Dead
        // Any live cell with two or three live neighbours lives on to the next generation.
        | Alive when (cache.LiveNeighbors = 2 || cache.LiveNeighbors = 3) -> Some Alive
        // Any live cell with more than three live neighbours dies, as if by overpopulation.
        | Alive when cache.LiveNeighbors > 3 -> Some Dead
        // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        | Dead when cache.LiveNeighbors = 3 -> Some Alive
        | _ -> None
    |]

  let getNextBoard board =
    let cacheGenerator x y board =
      {
        LiveNeighbors =
          getMooreNeighborhood 1 x y board
          |> Seq.filter (fun element -> element = Alive)
          |> Seq.length
      }
    CellularAutomataModule.getNextBoard cacheGenerator rules board