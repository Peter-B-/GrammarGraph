module GrammarGraph.Aes

open GrammarGraph.Model

let x a desc =
    let aes =
        { AesDesc.Aes = Aesthetic.X
          AesDesc.Expr = a }
        :: desc.Desc.Aes

    let d = { desc.Desc with Aes = aes }
    { desc with Desc = d }

let y a desc =
    let aesthecits =
        { AesDesc.Aes = Aesthetic.Y
          AesDesc.Expr = a }
        :: desc.Desc.Aes

    let d = { desc.Desc with Aes = aesthecits }
    { desc with Desc = d }
