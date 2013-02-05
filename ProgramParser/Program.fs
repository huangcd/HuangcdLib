module Program
open Symbolic.Expressions
open Symbolic.Expressions.Lexer
open Symbolic.Expressions.Parser
open System.Text

let parseText (text:string) = 
    let lexbuf = Lexing.LexBuffer<byte>.FromBytes (Encoding.UTF8.GetBytes text)
    start token lexbuf

let sample = "  accum := 0; \n\
                while counter do \n\
                begin \n\
                counter := counter - 1; \n\
                accum := accum + counter \n\
                accum := 0; \n\
                end; \n\
                print accum"

[<EntryPoint>]
let main argv = 
    let prog = parseText sample
    0 // return an integer exit code