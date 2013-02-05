module Program
open Symbolic.Expressions.Ast
open Symbolic.Expressions.Lexer
open Microsoft.FSharp.Text.Lexing
open Symbolic.Expressions.Parser
open System.Text

[<EntryPoint>]
let main args =
    let getBuffer (text:string) = text |> Encoding.UTF8.GetBytes |> LexBuffer<byte>.FromBytes
    let x = "3x"
    try
        let buf = getBuffer x
        let y = parseExpr parseToken buf
        printfn "%O" (simp y)
    with e -> printfn "failed with:\n %O" e.StackTrace
    0
