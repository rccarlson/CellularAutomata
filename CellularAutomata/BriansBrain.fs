namespace CellularAutomata

module BriansBrainModule =
  open CellularAutomataModule
  
  type State = | Alive | Dying | Dead with override this.ToString() = match this with | Alive -> "A" | Dying -> "X" | Dead  -> "D"
  type Cache = { LiveNeighbors : int }

  type BriansBrain = Board<State>

  let rules : Rule<State,Cache> array =
    [|
      fun state cache ->
        match state with
        // If a square is on, it turns off.
        | Alive -> Some Dying
        // When a square turns off, it can't turn on in the very next iteration.
        | Dying -> Some Dead
        // If a square is off, it turns on if exactly two neighboring squares are on. 
        | Dead when cache.LiveNeighbors = 2 -> Some Alive
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