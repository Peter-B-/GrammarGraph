module GrammarGraph.Geom

open GrammarGraph.Model

let point graph =
    let newPointLayer =
        { Stats = []
          Aes = []
          Geom = Point
          Position = { TODO = -1 } }

    { graph with Desc = { graph.Desc with Layers = newPointLayer :: graph.Desc.Layers } }
