using System.Drawing;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace SortVisualizer.Algorithms
{
	public class SelectionSort : ISortable
	{
		private static async Task Swap(int x, int y, int[] array, Func<int?, Task> Repaint)
		{
            (array[y], array[x]) = (array[x], array[y]);
			await Repaint(null);
        }
        public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			int size = array.Length;
			// Iterate until one before the end of the array
			for(int i = 0; i < size - 1; i++)
			{
				// Set the first element as the minimum index
				int minIndex = i;
				for (int j = i + 1; j < size; j++)// Iterate from current position to the end of the array comparing the mininum with them
				{
					if (array[j].CompareTo(array[minIndex]) < 0)
					{
						minIndex = j;
					}
					cancellationToken.ThrowIfCancellationRequested();
				}
				if (minIndex != i)
				{
					await Swap(i, minIndex, array, Repaint); // swap the miniumum with the first element
					// Gather sorted values on the left
				}
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
	}
}
