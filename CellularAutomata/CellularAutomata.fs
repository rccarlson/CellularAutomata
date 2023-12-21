namespace CellularAutomata

module CellularAutomataModule =
  open System

  type Board<'TState>(width, height, wrapAround, initializer) =
    let state : 'TState array2d = Array2D.init width height initializer
    member __.Width = width
    member __.Height = height
    member __.WrapAround = wrapAround
    member __.Item
      with get(x, y) = state.[x, y]
      and set(x, y) value = state.[x, y] <- value
    member __.ToString(toString) =
      [|
        for y in 0..(height-1) do
          for x in 0..(width-1) do
            yield toString state.[x,y]
          if y <> (height-1) then
            yield Environment.NewLine
      |]
      |> String.concat ""
    override this.ToString() = this.ToString(fun s -> s.ToString())

  /// Given a neighborhood and current state, optionally return a new state
  type Rule<'TState,'TCache> = 'TState -> 'TCache -> 'TState option

  let private getNeighborhood allowDxDy manhattanDistance x y (board: Board<'TState>) =
    let width, height = board.Width, board.Height
    [|
      for dx in -manhattanDistance .. manhattanDistance do
      for dy in -manhattanDistance .. manhattanDistance do
        if dx = 0 && dy = 0 || (allowDxDy dx dy |> not) then () else
        let nx, ny =
          if board.WrapAround then
            ((x + dx) % width + width) % width,
            ((y + dy) % height + height) % height
          else
            x + dx,
            y + dy
        if nx >= 0 && nx < width && ny >= 0 && ny < height then
          yield board.[nx, ny]
    |]

  let getMooreNeighborhood manhattanDistance x y board =
    getNeighborhood (fun dx dy -> true) manhattanDistance x y board

  let getVonNeumannNeighborhood manhattanDistance x y board =
    getNeighborhood (fun dx dy -> abs(dx) + abs(dy) <= manhattanDistance) manhattanDistance x y board

  /// Creates the next board based on the provided rules.
  /// Uses a cache to calculate relevant values only once for each cell
  let getNextBoard (cacheGenerator) (rules:Rule<'TState,'TCache> seq) (board:Board<'TState>) =
    let getNextState x y =
      let cache = cacheGenerator x y board
      let currentValue = board.[x,y]
      rules
      |> Seq.map (fun rule -> rule currentValue cache) |> Seq.tryPick id
      |> Option.defaultValue currentValue
    Board(board.Width, board.Height, board.WrapAround, getNextState)
