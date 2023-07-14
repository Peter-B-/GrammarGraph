namespace GrammarGraph.Data

open System.Reflection
open FSharp.Data

module DataSets =
    type internal Marker = interface end

    let loadDataSet name = 
        let t = typeof<Marker>.DeclaringType
        let assembly = Assembly.GetAssembly(t)

        let stream = assembly.GetManifestResourceStream($"GrammarGraph.Data.DataSets.{name}.csv")
        use reader = new System.IO.StreamReader(stream)
        reader.ReadToEnd()


    let diamonds = 
        let csv = loadDataSet "diamonds"
        let file = CsvFile.Parse( csv)
        file.Rows
        |> Seq.map (fun row -> row.["carat"].AsFloat())
            
            
