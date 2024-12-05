using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace SortVisualizer.Algorithms
{
    public interface ISortable
    {
        Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken);
    }
}
