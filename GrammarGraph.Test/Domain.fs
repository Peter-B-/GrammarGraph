module GrammarGraph.Test.Domain

type DataAccessExpression = { TODO: int }

type Aesthetic =
    | X
    | Y
    | Color
    | Size

type AesDesc =
    { Aes: Aesthetic
      Expr: DataAccessExpression }

type Statistics = { TODO: int }
type Geometry = { TODO: int }
type Position = { TODO: int }

type Layer =
    { Stats: Statistics list
      Aes: AesDesc list
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
    { Aes: AesDesc list
      Layers: Layer list
      Guides: Guide list
      Annotations: Annotation list
      Scales: Scales
      Facetting: Facetting
      Coordinates: Coordinates }

type GrammarGraph<'a> =
    { Data: seq<'a>
      Desc: GraphicsDescription<'a> }

module Grammar =
    let defaultGraphicsDescription =
        { Aes = []
          Layers = []
          Guides = []
          Annotations = []
          Scales = { xScale = Linear; yScale = Linear }
          Facetting = SingleGraph
          Coordinates = Carthesian }

    let grammarGraph data =
        { Data = data
          Desc = defaultGraphicsDescription }


//  mpg %>%
//    ggplot() +
//    aes(x = cyl, y = hwy) +
//    facet_wrap(. ~ trans) +
//    scale_x_log10() +
//    geom_point() +
//    geom_jitter(aes(colour = class))
//
//  mpg %>%
//    ggplot() +
//    aes(x = cyl, y = hwy, alpha = fl) +
//    facet_wrap(. ~ trans) +
//    scale_x_log10() +
//    geom_point(color = "red") +
//    geom_jitter(aes(colour = class))
