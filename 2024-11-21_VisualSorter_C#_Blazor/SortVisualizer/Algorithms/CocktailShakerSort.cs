using System.Drawing;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace SortVisualizer.Algorithms
{
	public class CocktailShakerSort : ISortable
	{
		public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			// Similar to bubble sort, but bidirectional. 
			// First bubble sort.  Then bubble sort from the opposite direction
			int size = array.Length;
			bool sorted = false;
			int unsortedUntil = size - 1;
			// Here is the bubble sort, values gather on the right
			for (int start = 0; !sorted; start++) // I came up with this for loop and I like it
			{
				sorted = true;
				for (int i = start; i < unsortedUntil; i++)
				{
					if (array[i].CompareTo(array[i + 1]) > 0)
					{
						await Swap(i, i + 1, array, Repaint);
						sorted = false;
					}
					cancellationToken.ThrowIfCancellationRequested();
				}
				if (sorted) break;
				// Early exit optimization - check if the array is sorted before changing direction
				sorted = true;
				unsortedUntil--;
				// Reverse bubble sort. Start at end of unsorted portion and iterate towards the index of 0
				// sorted values gather on the left
				for (int i = unsortedUntil - 1; i >= start; i--)
				{
					if (array[i].CompareTo(array[i + 1]) > 0)
					{
						await Swap(i, i + 1, array, Repaint);
						sorted = false;
					}
					cancellationToken.ThrowIfCancellationRequested();
				}
			}
			cancellationToken.ThrowIfCancellationRequested();
		}
		public static async Task Swap(int x, int y, int[] array, Func<int?, Task> Repaint)
		{
            (array[y], array[x]) = (array[x], array[y]);
			await Repaint(null);
        }
    }
}