# GrammarGraph

This repository is about investigating how [The Grammar of Graphics](https://www.amazon.com/Grammar-Graphics-Statistics-Computing/dp/0387245448/ref=as_li_ss_tl) can be implemented in a statically typed language, like F# or C#.

## Motivation

[Leland Wilkinson](https://en.wikipedia.org/wiki/Leland_Wilkinson) describes in his book [The Grammar of Graphics](https://www.amazon.com/Grammar-Graphics-Statistics-Computing/dp/0387245448/ref=as_li_ss_tl) a declarative approach to creating graphics or charts from data. [Hadley Wickham](https://hadley.nz/) created an implementation of this in the [ggplot2](https://ggplot2.tidyverse.org/) package for R. Learning ggplot2 changes the way one thinks about graphics. The declarative  syntax of this approach gives you a very potent tool for analyzing and visualizing data, without splitting data into arrays or handling indices in for-loops.

I spent quite some time thinking about how one would implement the Grammar of Graphics in a statically typed language. I plan to investigate some ideas in this project.

Any ideas or contributions are highly appreciated. Let us see where the journey leads to.

## Data

You need one important thing to test and demonstrate a chart engine and that is data. ggolpt2 does an ingenious and simple thing: It ships some datasets that are used throughout the documentation. 

I started by creating a `GrammarGraph.Data` assembly that contains the [Diamonds](https://ggplot2.tidyverse.org/reference/diamonds.html#format) and the [MPG](https://ggplot2.tidyverse.org/reference/mpg.html#format) datasets.
I'm trying to figure out the proper way to cite these data sources. I hope for now the links will do.

## Try out

This repository contains two tryout-files, one for [LINQPad](https://www.linqpad.net) and one for [Polyglot Notebooks](https://code.visualstudio.com/docs/languages/polyglot). Both reference the local build of the repository and cana be used to try the library.
