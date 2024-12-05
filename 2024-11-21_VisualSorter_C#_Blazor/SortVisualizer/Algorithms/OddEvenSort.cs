using System.Drawing;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace SortVisualizer.Algorithms
{
	public class OddEvenSort : ISortable
	{
		public async Task SortAsync(int[] array, Func<int?, Task> Repaint, CancellationToken cancellationToken)
		{
			int size = array.Length;
			for (bool sorted = false; !sorted; )
			{
				sorted = true;
				// Iterate through the odd values, checking them against the subsequent value
				for (int i = 1; i < size - 1; i += 2)
				{
					if (array[i].CompareTo(array[i + 1]) > 0)
					{
						await Swap(i, i + 1, array, Repaint);
						sorted = false;
					}
					cancellationToken.ThrowIfCancellationRequested();
				}
				// Iterate through the even values, checking them against the subsequent value
				for(int i = 0; i < size-1; i += 2)
				{
					if (array[i].CompareTo(array[i + 1]) > 0)
					{
						await Swap(i, i + 1, array, Repaint);
						sorted = false;
					}
					cancellationToken.ThrowIfCancellationRequested();
				}
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
        private static async Task Swap(int x, int y, int[] array, Func<int?, Task> Repaint)
        {
            (array[y], array[x]) = (array[x], array[y]);
			await Repaint(null);
        }
    }
}
