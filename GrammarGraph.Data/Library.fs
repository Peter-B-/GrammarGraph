namespace GrammarGraph.Data

open System.Reflection
open System.IO

module DataSets =
    type internal Marker = interface end

    type Diamond = 
        {
            Carat:float 
            Cut: string
            Color:string
            Clarity:string
            Depth:float
            Table:float
            Price:float
            X:float
            Y:float
            Z:float
        }


    let loadDataSetLines (name:string) = 
        let readLines (stream:Stream) = seq {
            use sr = new StreamReader (stream)
            while not sr.EndOfStream do
                yield sr.ReadLine ()
        }

        let t = typeof<Marker>.DeclaringType
        let assembly = Assembly.GetAssembly(t)

        let stream = assembly.GetManifestResourceStream($"GrammarGraph.Data.DataSets.{name}.csv")
        
        readLines stream
        // skip header line
        |> Seq.skip 1 

    let parseDiamond (parts:string array) =
        {
            Carat = System.Double.Parse(parts.[0], System.Globalization.CultureInfo.InvariantCulture)
            Cut = parts.[1]
            Color= parts.[2]
            Clarity = parts.[3]
            Depth = System.Double.Parse(parts.[4], System.Globalization.CultureInfo.InvariantCulture)
            Table = System.Double.Parse(parts.[5], System.Globalization.CultureInfo.InvariantCulture)
            Price = System.Double.Parse(parts.[6], System.Globalization.CultureInfo.InvariantCulture)
            X = System.Double.Parse(parts.[7], System.Globalization.CultureInfo.InvariantCulture)
            Y = System.Double.Parse(parts.[8], System.Globalization.CultureInfo.InvariantCulture)
            Z = System.Double.Parse(parts.[9], System.Globalization.CultureInfo.InvariantCulture)

        }

    let diamonds = 
        loadDataSetLines "diamonds"
        |> Seq.map (fun line -> 
            line.Split(',')
            |> Array.map (fun p -> p.Trim('"'))
            )
        |> Seq.map parseDiamond
        |> Seq.toList
            
            
