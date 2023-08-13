module GrammarGraph.Plotly

open Plotly.NET
open GrammarGraph.Model

let getAes desiredAes (aes: AesDesc<_> list) =
    aes
    |> List.tryFind (fun x -> x.Aes = desiredAes )
    |> function
        | Some x -> x
        | None -> raiseGrammarGraphException $"Aes {desiredAes} is not specified"
        
let addPointLayer graph (layer : Layer<_>) =
    let layerAes = layer.Aes @ graph.Desc.Aes    
    let xAes = getAes X layerAes
    let yAes = getAes Y layerAes
    let xValues = graph.Data |> Seq.map xAes.Expr |> Seq.toArray
    let yValues = graph.Data |> Seq.map yAes.Expr |> Seq.toArray
    Chart.Point (xValues, yValues)
    
let layerToPlot graph (layer : Layer<_>) =
    match layer.Geom with
    | Point -> addPointLayer graph layer
    |> Chart.withXAxisStyle (TitleText = "TODO: get prop name")
    |> Chart.withYAxisStyle (TitleText = "TODO: get prop name")

let plot (graph: GrammarGraph<'a>) =
    graph.Desc.Layers
    |> List.map (layerToPlot graph)
    |> function
    | [] -> raiseGrammarGraphException "No layer specified"
    | ch -> Chart.combine ch
    |> Chart.withTitle typeof<'a>.Name
    