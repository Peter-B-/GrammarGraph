module GrammarGraph.Plotly

open System
open FSharp.Linq.RuntimeHelpers
open Microsoft.FSharp.Quotations.ExprShape
open Microsoft.FSharp.Quotations.Patterns

open Plotly.NET
open GrammarGraph.Model

let rec getPropertyName expression =
    match expression with
    | PropertyGet (_, propOrValInfo, _) ->
        propOrValInfo.Name
    | ShapeVar var -> var.Name
    | ShapeLambda (_, expr) -> getPropertyName expr
    | ShapeCombination(_, exprList) -> getPropertyName exprList.Head //TODO: can the list be longer?

let eval quotation = LeafExpressionConverter.EvaluateQuotation quotation

let getAes desiredAes (aes: AesDesc<_> list) =
    aes
    |> List.tryFind (fun x -> x.Aes = desiredAes)
    |> function
        | Some x -> x
        | None -> raiseGrammarGraphException $"Aes {desiredAes} is not specified"
        
let extractAes aes graph =
    let access = aes.Expr |> eval :?> 'a -> IConvertible
    let values = graph.Data |> Seq.map access |> Seq.toArray
    let name = getPropertyName aes.Expr
    (values, name)

let addPointLayer graph (layer: Layer<_>) =
    let layerAes = layer.Aes @ graph.Desc.Aes
    
    let xAes = getAes X layerAes
    let xValues, xName = extractAes xAes graph 

    let yAes = getAes Y layerAes
    let yValues, yName = extractAes yAes graph

    Chart.Point(xValues, yValues)
    |> Chart.withXAxisStyle (TitleText = xName)
    |> Chart.withYAxisStyle (TitleText = yName)
    
let addLineLayer graph (layer: Layer<_>) =
    let layerAes = layer.Aes @ graph.Desc.Aes
    
    let xAes = getAes X layerAes
    let xValues, xName = extractAes xAes graph 

    let yAes = getAes Y layerAes
    let yValues, yName = extractAes yAes graph

    Chart.Line(xValues, yValues)
    |> Chart.withXAxisStyle (TitleText = xName)
    |> Chart.withYAxisStyle (TitleText = yName)

let layerToPlot graph (layer: Layer<_>) =
    match layer.Geom with
    | Point -> addPointLayer graph layer
    | Line -> addLineLayer graph layer

let plot (graph: GrammarGraph<'a>) =
    graph.Desc.Layers
    |> List.map (layerToPlot graph)
    |> function
        | [] -> raiseGrammarGraphException "No layer specified"
        | ch -> Chart.combine ch
    |> Chart.withTitle typeof<'a>.Name
