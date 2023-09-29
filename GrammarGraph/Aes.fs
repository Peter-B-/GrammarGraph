namespace GrammarGraph

open GrammarGraph.Model

module Aes =
    let add aes access desc =
        let aes =
            { AesDesc.Aes = aes
              AesDesc.Expr = access }
            :: desc.Desc.Aes

        let d = { desc.Desc with Aes = aes }
        { desc with Desc = d }

type Aes =

    static member x([<ReflectedDefinition(true)>] access) = Aes.add X access

    static member y([<ReflectedDefinition(true)>] access) = Aes.add Y access
