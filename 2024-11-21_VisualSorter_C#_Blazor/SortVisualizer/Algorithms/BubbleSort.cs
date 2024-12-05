using System.Drawing;
using System.Runtime.InteropServices;

namespace SortVisualizer.Algorithms
{
    public class BubbleSort : ISortable
    {
		public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
        {
            bool srtd = false;
            int unsortedUntil = array.Length - 1;
            while(!srtd) // Loop until we have not swapped any values - indicating the array is sorted fully
            {
                srtd = true;
                // Iterate until one before the end of the array,
                // Check the current value with the next value, swap if they are out of order.
                for (int i = 0; i < unsortedUntil; i++)
                {
                    if (array[i].CompareTo(array[i + 1]) > 0)
                    {
                        await Swap(i, i + 1, array, Repaint); // Every time a value is swapped, we want to continue to loop
                        srtd = false; // So set sorted to false.
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                }
                unsortedUntil--;
                // Each sorted item will gather at the right of the array, so these values do not need to be analyzed again.
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
