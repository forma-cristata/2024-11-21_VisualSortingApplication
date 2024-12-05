using System.Drawing;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace SortVisualizer.Algorithms
{
	public class InsertionSort : ISortable
	{
		public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			int size = array.Length;
			// For every item in the array, put it in the right spot on the left
			for(int i = 1; i < size; i++)
			{
				int temp = array[i]; // Removal of the current element
				int j = i - 1;
				for (; j >= 0 && array[j].CompareTo(temp) > 0; j--) // Complete comparisons
				{
					array[j + 1] = array[j]; // Shifts
					cancellationToken.ThrowIfCancellationRequested();
				}
				array[j + 1] = temp; // Insertions
				// This is all one long swap. to put the next item in the array into the correct position in the sorted portion on the left.
				await Repaint(null);
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
	}
}
