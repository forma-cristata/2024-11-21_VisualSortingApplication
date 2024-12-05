using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;

namespace SortVisualizer.Algorithms
{
	public class Mergesort : ISortable
	{
		public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			int size = array.Length;
			int right = size - 1;
		
			await MergeSort(0, size - 1, array, Repaint, cancellationToken);
		}
		private static async Task MergeSort(int left, int right, int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
            if(left.CompareTo(right) >= 0)
            {
				return;
            }
            // This will chop it down to the smallest value
            int middle = (left + right) / 2;
			cancellationToken.ThrowIfCancellationRequested();
			
			await MergeSort(left, middle, array, Repaint, cancellationToken);
			
			// Bring the array down to base case
			await MergeSort(middle + 1, right, array, Repaint, cancellationToken);
			
			//Begin merging the arrays back together
			await Merge(left, middle, right, array, Repaint, cancellationToken);
			
			cancellationToken.ThrowIfCancellationRequested();
		}
		private static async Task Merge(int left, int middle, int right, int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			int n1 = middle - left + 1; // size of left sub array
			
			int n2 = right - middle; // size of right sub array

			int[] leftArray = new int[n1];
			int[] rightArray = new int[n2];

			// Copy the contents of the left side of the array into the temporary array
			for (int i = 0; i < n1; i++)
			{
				leftArray[i] = array[left + i];
				cancellationToken.ThrowIfCancellationRequested();
			}
			for (int i = 0; i < n2; i++)
			{
				rightArray[i] = array[middle + 1 + i];
				cancellationToken.ThrowIfCancellationRequested();
			}

			// Merge the temporary arrays back into one whole array
			int leftIndex = 0;
			int rightIndex = 0;
			int mergeIndex = left;
			for (; leftIndex.CompareTo(n1) < 0 && rightIndex.CompareTo(n2) < 0; mergeIndex++)
			{
				// Compare the contents of each subarray starting at the left most indices
				if (leftArray[leftIndex].CompareTo(rightArray[rightIndex]) < 0)
				{
					array[mergeIndex] = leftArray[leftIndex];
					await Repaint(null);
					leftIndex++;
				}
				// The element in the right sub array is smaller
				else
				{
					array[mergeIndex] = rightArray[rightIndex];
					await Repaint(null);
					rightIndex++;
				}
				cancellationToken.ThrowIfCancellationRequested();
			}

			// Copy the remaining elements of the left subarray into the merged array
			while (leftIndex < n1)
			{
				array[mergeIndex] = leftArray[leftIndex];
				await Repaint(null);
				leftIndex++;
				mergeIndex++;
				cancellationToken.ThrowIfCancellationRequested();
			}
			while (rightIndex < n2)
			{
				array[mergeIndex] = rightArray[rightIndex];
				await Repaint(null);
				rightIndex++;
				mergeIndex++;
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
	}
}
