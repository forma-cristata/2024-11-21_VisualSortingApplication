using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.Metrics;
using System.Drawing;
using Microsoft.AspNetCore.Components.Web.Virtualization;


namespace SortVisualizer.Algorithms
{
    public class CombSort : ISortable
    {
        public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
        {
            // Just like bubble sort, but checks items a certain distance away from eachother according to math
            int size = array.Length;
            double shrinkFactor = 1.3;

            for (bool sorted = false;  !sorted;) // While not sorted
            {
                size = (int)(size / shrinkFactor); // Define the gap 
                if (size <= 1) // If there is no gap left, we have checked all elements and the array is most likely sorted (early exit)
                {
                    sorted = true;
                    size = 1;
                }
                else if (size == 9 || size == 10) size = 11; // Math stuff


                for(int i=0; (i + size) < array.Length; i++) // Iterate until the current item plus the gap is out of range, increasing the current item each iteration
                {
                    if (array[i].CompareTo(array[i + size]) > 0) // Compare the current item with the item a gap length ahead of it and swap as necessary
                    {
                        await Swap(i, (i + size), array, Repaint);
                        sorted = false; // Don't exit looping until we never swap
                    }
                    cancellationToken.ThrowIfCancellationRequested();
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
