namespace Symbolic.Expressions
open System
open Microsoft.FSharp.Math
module Ast =

    type Expr =
        | Num of BigNum
        | Var of String
        | Neg of Expr
        | Add of Expr * Expr
        | Sub of Expr * Expr
        | Mul of Expr * Expr
        | Div of Expr * Expr
        | Sin of Expr
        | Cos of Expr
        | Exp of Expr * Int32

        override this.ToString() = 
            match this with
            | Num n      -> n.ToString()
            | Var x      -> x
            | Neg e      -> String.Format("-({0})", e)
            | Add (n, m) -> String.Format("({0} + {1})", n, m)
            | Sub (n, m) -> String.Format("({0} - {1})", n, m)
            | Mul (n, m) -> String.Format("({0} * {1})", n, m)
            | Div (n, m) -> String.Format("({0} / {1})", n, m)
            | Sin e      -> String.Format("sin({0})", e)
            | Cos e      -> String.Format("cos({0})", e)
            | Exp (e, p) -> String.Format("{0}^{1}", e, p)

    let rec simp = function
        | Num n -> Num n
        | Var x -> Var x
        | Neg (Num n) -> Num -n
        | Neg (Add (x, y)) -> Add (Neg x |> simp, Neg y |> simp) |> simp
        | Neg (Div (x, y)) -> Div (Neg x |> simp, y |> simp) |> simp
        | Neg (Mul (x, y)) -> Mul (Neg x |> simp, y |> simp) |> simp
        | Neg e -> Neg (simp e)
        | Add (Num n, Num m) -> Num (n + m)
        | Add (Num n, e) when n = 0N -> e |> simp
        | Add (e, Num n) -> Add (Num n, e) |> simp
        | Add (Num n, Add (Num m, e)) -> Add(Num (n + m), e) |> simp
        | Add (Num n, e) -> 
            match simp e with
            | e' when e' = e -> Add (Num n, e')
            | e' -> Add(Num n, e') |> simp
        | Add (x, y) -> 
            match simp x, simp y with
            | x', y' when x' = x && y' = y -> Add (x', y')
            | x', y' -> Add (x', y') |> simp
        | Sub (x, y) -> Add (x, Neg y) |> simp
        | Div (x, y) when x = y -> Num 1N
        | Div (Num x, _) when x = 0N -> Num 0N
        | Div (Num n, Num m) -> Num (n / m)
        | Div (a, Div(b, c)) -> Div(Mul (a, c), b) |> simp
        | Div (Div(a, b), c) -> Div(a, Mul (b, c)) |> simp
        | Mul (Num n, Num m) -> Num (n * m)
        | Mul (Num n, Add (Num m, e)) -> Add(Num (n * m), Mul (Num n, e)) |> simp
        | Mul (e, Num n) -> Mul (Num n, e) |> simp
        | Mul (Num n, e) -> 
            if n = 0N
            then Num 0N
            elif n = 1N
            then e |> simp
            else match simp e with
                 | e' when e' = e -> Mul (Num n, e')
                 | e' -> Mul (Num n, e') |> simp
        | Mul (a, Div(b, c)) | Mul (Div (a, c), b) -> Div (Mul (a, b), c) |> simp
        | Exp (_, n) when n = 0 -> Num 1N
        | Exp (Num n, m) -> Num (BigNum.PowN(n, m))
        | Exp (e, m) -> 
            match simp e with
            | e' when e' = e -> Exp (e', m)
            | e' -> Exp (e', m) |> simp
        | e -> e