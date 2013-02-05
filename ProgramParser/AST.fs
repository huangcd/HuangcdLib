// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

namespace Symbolic.Expressions

open System

type Expr =
    | Val   of String
    | Int   of Int32
    | Plus  of Expr * Expr
    | Minus of Expr * Expr
    | Times of Expr * Expr

type Stmt = 
    | Assign        of string * Expr
    | While         of Expr * Stmt
    | Seq           of Stmt list
    | IfThen        of Expr * Stmt
    | IfThenElse    of Expr * Stmt * Stmt
    | Print         of Expr

type Prog = Program of Stmt list