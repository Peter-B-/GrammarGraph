module GrammarGraph.Data.Csv

open System.IO
open System.Reflection

/// Used as a marker to identify this assembly
type internal Marker =
    interface
    end

let skipHeader stream = stream |> Seq.skip 1

let loadDataSetLines (name: string) =
    let readLines (stream: Stream) =
        seq {
            use sr = new StreamReader(stream)

            while not sr.EndOfStream do
                yield sr.ReadLine()
        }

    let t = typeof<Marker>.DeclaringType
    let assembly = Assembly.GetAssembly(t)

    let stream =
        assembly.GetManifestResourceStream($"GrammarGraph.Data.DataSets.{name}.csv")

    readLines stream |> skipHeader

let splitAndTrim (line: string) =
    line.Split(',') |> Array.map (fun p -> p.Trim('"'))

let load name parser =
    loadDataSetLines name |> Seq.map splitAndTrim |> Seq.map parser |> Seq.toList
