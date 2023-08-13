module GrammarGraph.Graph

open GrammarGraph.Model

let defaultGraphicsDescription =
    { Aes = []
      Layers = []
      Guides = []
      Annotations = []
      Scales = { xScale = Linear; yScale = Linear }
      Facetting = SingleGraph
      Coordinates = Carthesian }

let create data =
    { Data = data
      Desc = defaultGraphicsDescription }
    
let plot graph = Plotly.plot graph
