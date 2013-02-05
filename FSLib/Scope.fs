namespace FSLib.Scope
    open System
    open System.Linq
    open System.Net
    open System.Text
    open System.IO
    open System.Text.RegularExpressions

    module Scope =
        //let digit = ['0'-'9']
        type Identifier = 
            | Text of String

        type ColumnType = 
            | Int
            | UInt
            | Long
            | ULong
            | Short
            | UShort
            | Binary
            | String
            | Single
            | Double

        type Column = {
            Name: String
            Type: ColumnType option
        }

        type ColumnList = Column list

        let output (id:Identifier) = 
            let text = match id with
                        | Text t -> t
            printfn "hello world %s" text
            0

