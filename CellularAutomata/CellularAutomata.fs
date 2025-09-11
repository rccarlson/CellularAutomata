module CellularAutomataDemo.CellularAutomataModule

open System
open System.Text

/// A 2D board of states
type Board<'TState> = 'TState array2d

/// Given a neighborhood and current state, return a new state
type RuleSet<'TState,'TNeighborhood> = 'TState -> 'TNeighborhood -> 'TState

let createBoard = Array2D.init

/// Creates the next board based on the provided rule and neighborhood function.
let getNextBoard
  (board:Board<'TState>)
  (rules: RuleSet<'TState,'TNeighborhood>)
  (getNeighborhood: int -> int -> 'TNeighborhood)
  : Board<'TState> =
    let width, height = Array2D.length1 board, Array2D.length2 board
    let getNextState x y : 'TState =
      let currentValue : 'TState = board.[x,y]
      let neighborhood : 'TNeighborhood = getNeighborhood x y
      rules currentValue neighborhood
    createBoard width height getNextState

let printBoard (board:Board<'TState>) (toChar : 'TState -> char) =
  let width, height = Array2D.length1 board, Array2D.length2 board
  let sb = StringBuilder()
  for y in 0 .. height - 1 do
    for x in 0 .. width - 1 do
      sb.Append(toChar board[x,y]) |> ignore
    sb.AppendLine() |> ignore
  Console.Write(sb.ToString())

  
/// Gets the Moore neighborhood (8 surrounding cells) of a cell at (x,y)
let getMooreNeighborhood (board:Board<'TState>) x y : 'TState array =
  let width, height = board.GetLength(0), board.GetLength(1)
  [|
    for dx in -1 .. 1 do
      for dy in -1 .. 1 do
        if dx = 0 && dy = 0 then () else
        let nx, ny =
          // allow wraparound
          ((x + dx) % width + width) % width,
          ((y + dy) % height + height) % height
        if nx >= 0 && nx < width && ny >= 0 && ny < height then
          yield board.[nx, ny]
  |]