module Testing.CellularAutomataTests

open NUnit.Framework
open CellularAutomata.CellularAutomataModule
open System

let boardFromLists wrapAround (l : 'T list list) =
  let width = l |> Seq.map (fun row -> row.Length) |> Seq.max
  let height = l.Length
  let getPoint x y = l[y][x]
  Board(width, height, wrapAround, getPoint)

let assertEqualUnordered expected actual = Assert.AreEqual((expected |> Seq.sort), (actual |> Seq.sort))
let boardValues = 
  [
    [  1;  2;  3;  4 ]
    [  5;  6;  7;  8 ]
    [  9; 10; 11; 12 ]
    [ 13; 14; 15; 16 ]
    [ 17; 18; 19; 20 ]
  ]

[<TestCase(0,0, [|2;6;5;8;4;17;18;20|] )>]
[<TestCase(1,1, [|1;2;3;7;11;10;9;5|] )>]
[<TestCase(1,0, [|1;5;6;7;3;17;18;19|] )>]
[<TestCase(3,4, [|19;15;16;3;4;13;17;1|] )>]
let MooreNeighborhoodWrapAroundCases x y (expected: int array) =
  let wrapAroundBoard = boardValues |> boardFromLists true
  assertEqualUnordered expected (getMooreNeighborhood 1 x y wrapAroundBoard)

[<TestCase(0,0, [|5;6;2|] )>]
[<TestCase(1,1, [|1;2;3;7;11;10;9;5|] )>]
[<TestCase(1,0, [|1;5;6;7;3|] )>]
[<TestCase(3,4, [|19;15;16|] )>]
let MooreNeighborhoodNoWrapAroundCases x y (expected: int array) =
  let noWrapAroundBoard = boardValues |> boardFromLists false
  assertEqualUnordered expected (getMooreNeighborhood 1 x y noWrapAroundBoard)

[<TestCase(0,0, [|5;2;4;17|] )>]
[<TestCase(1,1, [|2;5;7;10|] )>]
[<TestCase(1,0, [|1;6;3;18|] )>]
[<TestCase(3,4, [|16;19;4;17|] )>]
let VonNeumannNeighborhoodWrapAroundCases x y (expected : int array) =
  let wrapAroundBoard = boardValues |> boardFromLists true
  assertEqualUnordered expected (getVonNeumannNeighborhood 1 x y wrapAroundBoard)

[<TestCase(0,0, [|5;2|] )>]
[<TestCase(1,1, [|2;5;7;10|] )>]
[<TestCase(1,0, [|1;6;3|] )>]
[<TestCase(3,4, [|16;19|] )>]
let VonNeumannNeighborhoodNoWrapAroundCases x y (expected : int array) =
  let noWrapAroundBoard = boardValues |> boardFromLists false
  assertEqualUnordered expected (getVonNeumannNeighborhood 1 x y noWrapAroundBoard)

[<Test>]
let BoardToString () =
  let wrapAroundBoard = boardValues |> boardFromLists true
  Assert.That(wrapAroundBoard.ToString(), Is.EqualTo($"1234{Environment.NewLine}5678{Environment.NewLine}9101112{Environment.NewLine}13141516{Environment.NewLine}17181920"))