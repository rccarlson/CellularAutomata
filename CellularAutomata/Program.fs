module Program

open System
open CellularAutomata.ConwaysGameOfLifeModule
open System.Threading

[<EntryPoint>]
let main args =
  let rand = Random()
  let mutable board = ConwaysGameOfLife(90, 30, true, (fun _ _ -> if rand.NextDouble() < 0.5 then Alive else Dead))
  for i = 0 to 1000 do
    Console.SetCursorPosition(0,0)
    Console.WriteLine(board.ToString(function | Alive -> "O" | Dead -> " "))
    let percentAlive =
      let alive =
        [
          for x = 0 to board.Width-1 do
          for y = 0 to board.Height-1 do
            yield board.[x,y]
        ]
        |> Seq.filter (fun state -> state = Alive) |> Seq.length
      let total = board.Width * board.Height
      (float alive) / (float total)
    Console.Write($"{percentAlive*100.0:f0} percent alive")
    board <- getNextBoard board
    Thread.Sleep(1_000/10)
    ()
  0