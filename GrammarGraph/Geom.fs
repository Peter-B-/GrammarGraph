module GrammarGraph.Geom

open GrammarGraph.Model

let add geom graph =
    let newLayer =
        { Stats = []
          Aes = []
          Geom = geom
          Position = { TODO = -1 } }

    { graph with Desc = { graph.Desc with Layers = newLayer :: graph.Desc.Layers } }
    
let point graph = add Point graph
let line graph = add Line graph

