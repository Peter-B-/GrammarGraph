module GrammarGraph.Test.Domain

open System.Linq.Expressions

type Aesthetic =
    | X
    | Y
    | Color
    | Size

type AesDesc = { Aes: Aesthetic; Expr: Expression }

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

module Aes =
    let x a desc =
        let aesthecits =
            { AesDesc.Aes = Aesthetic.X
              AesDesc.Expr = a }
            :: desc.Desc.Aes

        let d = { desc.Desc with Aes = aesthecits }
        { desc with Desc = d }

    let y a desc =
        let aesthecits =
            { AesDesc.Aes = Aesthetic.X
              AesDesc.Expr = a }
            :: desc.Desc.Aes

        let d = { desc.Desc with Aes = aesthecits }
        { desc with Desc = d }

module Grammar =
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
