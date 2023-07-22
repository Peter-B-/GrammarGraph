module GrammarGraph.Data.DataSets

open GrammarGraph.Data

let diamonds =
    Csv.load "diamonds" Diamonds.parse

let fuelEconomy =
    Csv.load "mpg" FuelEconomy.parse
    
    
