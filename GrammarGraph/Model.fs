module GrammarGraph.Model

open System
open Microsoft.FSharp.Quotations

type PropAccess<'a> = 'a -> IConvertible

type Aesthetic =
    | X
    | Y
    | Color
    | Size

type AesDesc<'a> = { Aes: Aesthetic; Expr: Expr<('a -> IConvertible)> } //TODO: Use System.Linq.Expression or F# code quotations?
// https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression?view=net-7.0
// https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/code-quotations

type Statistics = { TODO: int }
type Geometry =
    | Point
    | Line
type Position = { TODO: int }

type Layer<'a> =
    { Stats: Statistics list
      Aes: AesDesc<'a> list
      Geom: Geometry
      Position: Position }

type Annotation = { TODO: int } // Is this the same or similar to Layer? https://ggplot2.tidyverse.org/reference/aes_position.html

type Guide = { TODO: int }

type Scale =
    | Linear
    | Reverse
    | Log10

type Scales = { xScale: Scale; yScale: Scale }
type Facetting = | SingleGraph
type Coordinates = | Carthesian

type GraphicsDescription<'a> =
    { Aes: AesDesc<'a> list
      Layers: Layer<'a> list
      Guides: Guide list
      Annotations: Annotation list
      Scales: Scales
      Facetting: Facetting
      Coordinates: Coordinates }

type GrammarGraph<'a> =
    { Data: seq<'a>
      Desc: GraphicsDescription<'a> }

type GrammarGraphException(message : string) =
    inherit Exception(message)
    
let raiseGrammarGraphException message = GrammarGraphException message |> raise
    