module Testing.ConwaysGameOfLifeTests

open NUnit.Framework
open CellularAutomata.ConwaysGameOfLifeModule
open Testing.CellularAutomataTests

[<SetUp>]
let SetUp() = ()

let A,D = Alive, Dead

[<Test>]
let HasRules () = Assert.That(CellularAutomata.ConwaysGameOfLifeModule.rules, Is.Not.Null)

[<Test>]
let Blinker () =
  let blinker =
    [
      [D;D;D;D;D]
      [D;D;A;D;D]
      [D;D;A;D;D]
      [D;D;A;D;D]
      [D;D;D;D;D]
    ]
    |> boardFromLists false
    |> getNextBoard
  Assert.That(blinker.ToString(), Is.EqualTo("""
DDDDD
DDDDD
DAAAD
DDDDD
DDDDD
  """.Trim()))

[<Test>]
let Toad () =
  let toad =
    [
      [D;D;D;D;D;D]
      [D;D;D;D;D;D]
      [D;D;A;A;A;D]
      [D;A;A;A;D;D]
      [D;D;D;D;D;D]
      [D;D;D;D;D;D]
    ]
    |> boardFromLists false
    |> getNextBoard
  Assert.That(toad.ToString(), Is.EqualTo("""
DDDDDD
DDDADD
DADDAD
DADDAD
DDADDD
DDDDDD
  """.Trim()))

[<Test>]
let Glider () =
  let gliderInitial =
    [
      [D;D;D;D;D]
      [D;D;A;D;D]
      [D;D;D;A;D]
      [D;A;A;A;D]
      [D;D;D;D;D]
    ]
    |> boardFromLists true

  let glider1 = gliderInitial |> getNextBoard
  Assert.That(glider1.ToString(), Is.EqualTo("""
DDDDD
DDDDD
DADAD
DDAAD
DDADD
  """.Trim()))

  /// gliders run on a 4-phase cycle. Advances by 4 cycles to complete a loop
  let advance4 glider = glider |> getNextBoard |> getNextBoard |> getNextBoard |> getNextBoard
  // advance by 5 loops to return to original position
  let gliderLooped = gliderInitial |> advance4 |> advance4 |> advance4 |> advance4 |> advance4

  Assert.That(gliderLooped.ToString(), Is.EqualTo(gliderInitial.ToString()))