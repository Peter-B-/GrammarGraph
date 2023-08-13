module GrammarGraph.Grammar

open GrammarGraph.Model

let defaultGraphicsDescription =
    { Aes = []
      Layers = []
      Guides = []
      Annotations = []
      Scales = { xScale = Linear; yScale = Linear }
      Facetting = SingleGraph
      Coordinates = Carthesian }

let graph data =
    { Data = data
      Desc = defaultGraphicsDescription }
