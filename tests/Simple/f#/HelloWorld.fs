module M

open System

let someFunction x y = x + y

[<EntryPoint>]
let main args = 
    Console.WriteLine("Hello world!")
    Console.WriteLine(someFunction 2 2)
    Console.WriteLine("<%END%>")
    0

