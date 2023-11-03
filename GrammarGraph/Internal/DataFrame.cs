using System.Collections.Immutable;
using GrammarGraph.Extensions;
using GrammarGraph.Render;
using JetBrains.Annotations;

namespace GrammarGraph.Internal;

public record DataFrame(
    ImmutableDictionary<AestheticsId, DataColumn> Columns,
    ImmutableArray<Panel> Panels,
    ImmutableArray<Group> Groups) : IDataColumnContainer
{
    public DataColumn this[AestheticsId aestheticsId] => Columns[aestheticsId];

    public IReadOnlyList<PanelGroupData> Group()
    {
        var groupedIndices = new HashSet<PanelGroupIndices>(PanelGroupIndices.PanelGroupComparer);
        for (var idx = 0; idx < Panels.Length; idx++)
        {
            var group = Groups[idx];
            var panel = Panels[idx];

            var groupedIndex = groupedIndices.GetOrAdd(new PanelGroupIndices(panel, group));
            groupedIndex.Add(idx);
        }

        return
            groupedIndices
                .Select(gi => new PanelGroupData(
                            gi.Panel,
                            gi.Group,
                            GetSubset(gi.GetIndices())))
                .ToList();
    }

    public static DataFrame Merge(List<PanelGroupData> panelGroups)
    {
        if (panelGroups.Count == 0)
            return new DataFrame(
                ImmutableDictionary<AestheticsId, DataColumn>.Empty,
                ImmutableArray<Panel>.Empty,
                ImmutableArray<Group>.Empty);

        var itemCount = panelGroups.Sum(pg => pg.Columns.First().Value.Length);

        var panelBuilder = ImmutableArray.CreateBuilder<Panel>(itemCount);
        var groupBuilder = ImmutableArray.CreateBuilder<Group>(itemCount);

        foreach (var pg in panelGroups)
            for (var i = 0; i < pg.Columns.First().Value.Length; i++)
            {
                panelBuilder.Add(pg.Panel);
                groupBuilder.Add(pg.Group);
            }

        var aesthetics = panelGroups.First().Columns.Keys;
        var columns = aesthetics
            .ToImmutableDictionary(
                id => id,
                id => DataColumnFactory.Merge(panelGroups.Select(pg => pg.Columns[id]))
            );

        return new DataFrame(columns, panelBuilder.MoveToImmutable(), groupBuilder.MoveToImmutable());
    }

    private ImmutableDictionary<AestheticsId, DataColumn> GetSubset(ImmutableArray<int> indices) =>
        Columns.ToImmutableDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Subset(indices)
        );
}

internal record PanelGroupIndices(Panel Panel, Group Group)
{
    private readonly ImmutableArray<int>.Builder indices = ImmutableArray.CreateBuilder<int>();

    public static IEqualityComparer<PanelGroupIndices> PanelGroupComparer { get; } = new PanelGroupEqualityComparer();

    public void Add(int idx)
    {
        indices.Add(idx);
    }

    public ImmutableArray<int> GetIndices() => indices.ToImmutableArray();

    private sealed class PanelGroupEqualityComparer : IEqualityComparer<PanelGroupIndices>
    {
        public bool Equals(PanelGroupIndices? x, PanelGroupIndices? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            if (ReferenceEquals(x.Panel, y.Panel) && ReferenceEquals(x.Group, y.Group)) return true;
            return x.Panel.Equals(y.Panel) && x.Group.Equals(y.Group);
        }

        public int GetHashCode(PanelGroupIndices obj) =>
            HashCode.Combine(obj.Panel, obj.Group);
    }
}

public record PanelGroupData(Panel Panel, Group Group, ImmutableDictionary<AestheticsId, DataColumn> Columns) : IDataColumnContainer
{
    public DataColumn this[AestheticsId aestheticsId] => Columns[aestheticsId];
}

[NoReorder]
public class DataFrameItem(DataFrame dataFrame, int index)
{
    public int Index => index;

    public Panel Panel => dataFrame.Panels[Index];
    public Group Group => dataFrame.Groups[Index];
}

public record AestheticsFactor(AestheticsId Aesthetics, Factor Factor)
{
    public override string ToString() => $"{Aesthetics}: {Factor}";

    private string ToDump() => ToString();
}

public record AestheticsFactorColumn(AestheticsId Aesthetics, FactorColumn FactorColumn)
{
    public override string ToString() => $"{Aesthetics}: {FactorColumn}";

    private string ToDump() => ToString();
}

public record Group(ImmutableArray<AestheticsFactor> Identifiers)
{
    public static Group Default => new(ImmutableArray<AestheticsFactor>.Empty);

    public bool MatchesIndices(int[] indices)
    {
        if (indices.Length != Identifiers.Length)
            throw new ArgumentException($"{nameof(indices)} must be of same length as {nameof(Identifiers)}.", nameof(indices));

        for (var idx = 0; idx < Identifiers.Length; idx++)
            if (indices[idx] != Identifiers[idx].Factor.Index)
                return false;

        return true;
    }

    public override string ToString()
    {
        if (Identifiers.IsEmpty) return "default";

        return Identifiers
            .Select(kvp => $"{kvp.Aesthetics}: {kvp.Factor.Value}")
            .JoinStrings("; ");
    }

    private string ToDump() => ToString();
}

public interface IDataColumnContainer
{
    ImmutableDictionary<AestheticsId, DataColumn> Columns { get; }
}
