using System.Diagnostics.Metrics;
using System.Drawing;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace SortVisualizer.Algorithms
{
    public class GnomeSort : ISortable
    {
        public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
        {
            // Stupid Sort!! Just like Insertion sort but avoids nested loops because it's too stupid to understand them
            for (int i = 0;  i < array.Length;)
            {
                if (i == 0 || array[i].CompareTo(array[i - 1]) >= 0) // If the item is in the right order, move over them.
                {
                    i++;
                }
                else // If the items are out of order
                {
                    await Swap(i, i-1, array, Repaint); // Put them in order and recheck the next swapped value against by decreasing the iteration
                    i--;
                }
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
        public static async Task Swap(int x, int y, int[] array, Func<int?, Task> Repaint)
        {
            (array[y], array[x]) = (array[x], array[y]);
            await Repaint(null);
        }
    }
}
