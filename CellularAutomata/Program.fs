module CellularAutomataDemo.Program

open System
open CellularAutomataDemo.CellularAutomataModule
open System.Threading

let runSimulation toChar rules getNeighborhood (board: Board<'TState>) =
  let mutable board = board
  for generation in 0 .. 1000 do
    Console.SetCursorPosition(0,0)
    printBoard board toChar
    Console.WriteLine($"Generation {generation}")

    let getNeighborhood = getNeighborhood board
    board <- getNextBoard board rules getNeighborhood

    Thread.Sleep 100

[<EntryPoint>]
let main args =
  let width, height = 100, 40
  match Array.tryHead args with
  | None
  | Some "conway" ->
    createBoard width height
      (fun _x _y ->
        match Random.Shared.NextSingle() < 0.5f with
        | true -> ConwaysGameOfLife.State.Alive
        | false -> ConwaysGameOfLife.State.Dead
      )
    |> runSimulation
      ConwaysGameOfLife.stateToChar
      ConwaysGameOfLife.rules
      ConwaysGameOfLife.getNeighborhood
  | Some "brain"
  | Some "brian" ->
    createBoard width height
      (fun _x _y ->
        match Random.Shared.NextSingle() < 0.5f with
        | true -> BriansBrain.State.Alive
        | false -> BriansBrain.State.Dead
      )
    |> runSimulation
      BriansBrain.stateToChar
      BriansBrain.rules
      BriansBrain.getNeighborhood

  | Some arg -> Console.Error.WriteLine $"Argument {arg} is not valid"

  0

