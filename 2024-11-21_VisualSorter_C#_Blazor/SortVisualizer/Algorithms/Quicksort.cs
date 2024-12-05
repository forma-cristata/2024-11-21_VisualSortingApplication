using System.Collections.Concurrent;
using System.Drawing;

namespace SortVisualizer.Algorithms
{
	public class Quicksort : ISortable
	{
		public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			await QuickSort(0, array.Length - 1, array, Repaint, cancellationToken);
		}
		private static async Task QuickSort(int leftIndex, int rightIndex, int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			if (rightIndex <= leftIndex) return; // Base Case

			// Cut the array in half
			int pivotIndex = await Partition(leftIndex, rightIndex, array, Repaint, cancellationToken);
			cancellationToken.ThrowIfCancellationRequested();
			
			// We do not need to include the pivot. 
			// Recursively sort the left subarray
			await QuickSort(leftIndex, pivotIndex - 1, array, Repaint, cancellationToken);
			
			// Recursively sort the right subarray
			await QuickSort(pivotIndex + 1, rightIndex, array, Repaint, cancellationToken);
			cancellationToken.ThrowIfCancellationRequested();
		}

		private static async Task<int> Partition(int leftPointer, int rightPointer, int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			int pivotIndex = rightPointer; // Define the pivot index
			int pivot = array[pivotIndex]; // Value of the pivot
			rightPointer--; // Pivot shouldn't be included in the current partition
			for (; true; leftPointer++)
			{
				// Check the left pointer against the pivot
				while (array[leftPointer].CompareTo(pivot) < 0)
				{
					leftPointer++;
					cancellationToken.ThrowIfCancellationRequested();
				}
				// And the right pointer
				while (rightPointer >= 0
						&& array[rightPointer].CompareTo(pivot) > 0)
				{
					rightPointer--;
					cancellationToken.ThrowIfCancellationRequested();
				}
				// We have reached eachother
				if (leftPointer >= rightPointer) break;

				await Swap(leftPointer, rightPointer, array, Repaint);
			}
			array[pivotIndex] = array[leftPointer];
			array[leftPointer] = pivot;
			return leftPointer;
			// Returns the position of the pivot after it has been moved
		}
		// Partitions job is just to set the pivot's correct position
		public static async Task Swap(int x, int y, int[] array, Func<int?, Task> Repaint)
		{
            (array[y], array[x]) = (array[x], array[y]);
            await Repaint(null);
		}
	}
}
