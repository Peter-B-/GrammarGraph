module GrammarGraph.Aes

open GrammarGraph.Model

let add aes access desc =
    let aes =
        { AesDesc.Aes = aes
          AesDesc.Expr = access }
        :: desc.Desc.Aes

    let d = { desc.Desc with Aes = aes }
    { desc with Desc = d }
    
let x access desc = add X access desc

let y access desc = add Y access desc
