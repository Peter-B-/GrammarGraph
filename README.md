# GrammarGraph

This repository is about investigating how [The Grammar of Graphics](https://www.amazon.com/Grammar-Graphics-Statistics-Computing/dp/0387245448/ref=as_li_ss_tl) can be implemented in a statically typed language, like F# or C#.

## Motivation

[Leland Wilkinson](https://en.wikipedia.org/wiki/Leland_Wilkinson) describes in his book [The Grammar of Graphics](https://www.amazon.com/Grammar-Graphics-Statistics-Computing/dp/0387245448/ref=as_li_ss_tl) a declarative approach to creating graphics or charts from data. [Hadley Wickham](https://hadley.nz/) created an implementation of this in the [ggplot2](https://ggplot2.tidyverse.org/) package for R. Learning ggplot2 changes the way one thinks about graphics. The declarative  syntax of this approach gives you a very potent tool for analyzing and visualizing data, without manually splitting data into arrays or handling indices in for-loops.

I spent quite some time thinking about how one would implement the Grammar of Graphics in a statically typed language. I plan to investigate some ideas in this project.

Any ideas, comments or contributions are highly appreciated. Let us see where the journey leads to.

## Data

You need one important thing to test and demonstrate a chart engine and that is data. ggolpt2 does an ingenious and simple thing: It ships some datasets that are used throughout the documentation. 

I started by creating a `GrammarGraph.Data` assembly that contains the [Diamonds](https://ggplot2.tidyverse.org/reference/diamonds.html#format) and the [MPG](https://ggplot2.tidyverse.org/reference/mpg.html#format) datasets.
I'm trying to figure out the proper way to cite these data sources. I hope for now the links will do.

## Design considerations

This section contains a collection of the design goals for this library. I think [ggplot2](https://ggplot2.tidyverse.org/reference/index.html) has a good syntax and a large user base familiar with it, so I would like to stick to that, where possible. I will also try to use the same vocabulary for the various concepts.

Qutations in this section directly come from the [ggplot2 documentation](https://ggplot2.tidyverse.org/reference/index.html).

### Implementation language

I want to implement the library in eighter F# or C#

#### F#
- great support for generating a DSL
- great API in [Plotly.NET](https://plotly.net)
- great data science community

#### C#
- larger user base


### Charting backend

GrammarGraph is supposed to provide a high level API for describing charts or graphics. But it does not make sense to implement all the details of a plotting framework from scretch. It makes sense to render the charts using an established plotting library like [Plotly.NET](https://plotly.net) or [OxyPlot](https://github.com/oxyplot/oxyplot).

### Elements of a chart
In ggplot a chart of graph is composed from a list of (mostly optional) elements. 

- Layers
  - Geoms
  - Stats
  - Position adjustmend
  - Annotations 
- Aesthetics
- Scales
- Facetting
- Coordinate systems
- Themes

### Aestethics
> Aesthetic mappings describe how variables in the data are mapped to visual properties (aesthetics) of geoms. 

#### Level of application
> Aesthetic mappings can be set in ggplot() and in individual layers.

Setting aesthetics on a plot:
```r
ggplot(mtcars) + 
  aes(x = wt, y = mpg) +
  geom_point()
```

Setting aesthetics on a layer:
```r
ggplot(mtcars) + 
  geom_point(aes(x = wt, y = mpg))
```

Combining plot and layer aesthetics:
```r
ggplot(mtcars) + 
  aes(x = wt, y = mpg) +
  geom_point(aes(color = cyl)) +
  geom_line()
```
Both line and point use the shared `x` and `y` aesthetics, but `color` is only applied to point.

#### Functions in aesthetics
Aesthetics can contain functions or operations on data:
```r
ggplot(mtcars) + 
  aes(x = wt * wt, y = mpg) + 
  geom_point()
```
Here `x` is mapped to `wt * wt`. It might be argied that this kind of transformation should happen before the data is passed on to the visualization stage, but for other transformations, the division is not so obvious:
```r
ggplot(mtcars) + 
  aes(x = wt, y = mpg, color = factor(cyl)) +
  geom_point()
```
Here the number of cylinders `cyl` is converted to a factor, which instructs ggplot to treat it as a categorial variable. This seems like a valid transformation within the visualization stage.

#### Extnsibility
ggplot allows to specify any aesthetics. The individual layers decide if and how to react to them. Unused aesthetics are ignored:
```r
ggplot(mtcars) + 
  aes(x = wt, y = mpg, unknown = cyl) +
  geom_point()
```
This is necessary so the framework can be extended with new geoms.

#### Overwriting aesthetics
Aesthetics can be overwritten in the layer with "static values". This is used for explicitly setting color or size: 

```r
# set color and size
ggplot(mtcars) + 
  aes(x = wt, y = mpg) +
  geom_point(color = "red", size = 2)

# same result, overwriting size aesthetics
ggplot(mtcars) + 
  aes(x = wt, y = mpg, size = wt) +
  geom_point(color = "red", size = 2)

```

### Statistics

Statistics can be applied to variables before plotting. This allows to directly create bar charts or histograms from data without explicitly calculating statistics.

```r
ggplot(diamonds) +
  aes(x = price) +
  geom_histogram()
```

In this simple case one could argue, that the accumulation of values into bins should happen before the vizualisation stage. However this combination proves very powerfull, when combined with faceting or grouping into colors:

```r
ggplot(diamonds) +
  aes(x = price, fill = clarity) +
  facet_wrap(.~ cut) +
  geom_histogram(binwidth = 1000)
```

Many geoms have predefined statistics, that can be overwritten.

It is also possible to use other return values from the stat function directly in plotting. This is an advanced topic, but enables some very powerfull applications like:

```r
# To make it easier to compare distributions with very different counts,
# put density on the y axis instead of the default count
ggplot(diamonds) +
  aes(price, after_stat(density), colour = cut) +
  geom_freqpoly(binwidth = 500, linewidth = 1.5)
```

### Faceting

### Factors

R has a datatype [Factor](https://www.rdocumentation.org/packages/base/versions/3.6.2/topics/factor), which you can think of as a dynamic enum.

A factor has a list of possible values, named levels, and each instance of a factor has one of those. I think factors are an important datatype when
working with charts. They have some properties you cannot easily achieve with enums or strings.

Factors
- can be created at runtime from the data source, which enums can not.
- have an order of levels, which strings have not. 
- convey that variables should be interpreted as categorical. (`x = Cylinders` vs. `x = factor(Cylinders)`)
- have a list of all possible values attached to every instance. So even subsets of a dataset contain the information on the complete list of levels. 

I think that at some point such a data structure would be a valuable addition for a charting framework. I'm not aware of an implementation of this concept in .Net.

### Color palette

The charting library should come with a preset of color palettes for both continuous and categorial data. It should be easy for the user to use custom palette.

Colors are applied as a scale (similar to use `log10` on x-axis).

```r
ggplot(faithfuld, aes(waiting, eruptions, fill = density)) +
  geom_tile() +
  scale_fill_continuous(type = "gradient")
```

## Data representation

ggplot creates a representation of the grafic in an `gg, ggplot` [S3 object](http://adv-r.had.co.nz/S3.html). But, R being R, this is not really obvious to the untrained observer looking at the source code.

However, one can create plots, store them in variables and use [RStudio's](https://posit.co/download/rstudio-desktop/) variable viewer to analyze the resulting structure:

```r
plot <- 
  ggplot(mtcars) + 
    aes(x = wt, y = mpg, size = wt) +
    geom_point(color = "red", size = 2)
```
This leads to plot being

![Data structure in RStudio](Docs/RStudio%20plot%20variable.png)

The class contains
 - data
 - layers
 - scales
 - mapping
 - theme
 - coordinates
 - faced
 - plot_env
 - labels


## Try out

This repository contains two tryout-files, one for [LINQPad](https://www.linqpad.net) and one for [Polyglot Notebooks](https://code.visualstudio.com/docs/languages/polyglot). Both reference the local build of the repository and can be used to try the library.
