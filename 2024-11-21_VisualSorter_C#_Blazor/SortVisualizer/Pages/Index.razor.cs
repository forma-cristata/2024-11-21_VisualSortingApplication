using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SortVisualizer.Algorithms;
using System.Diagnostics;


namespace SortVisualizer.Pages
{
	public enum Algorithm
	{ 
		BubbleSort,
		SelectionSort,
		InsertionSort,
        Quicksort,
        Mergesort,
        OddEvenSort,
        CocktailShakerSort,
        GnomeSort,
        CombSort
    }

    public enum Complexity
    {
        O_1,        // Constant
        O_logN,     // Logarithmic
        O_N,        // Linear
        O_NlogN,    // Linearithmic
        O_N2,       // Quadratic
        O_N3,       // Cubic
        O_2N,       // Exponential
        O_NFact,     // Factorial
        O_N22P
    }

	public partial class Index : ComponentBase
	{
        #region Properties (you should not change these)
        [Inject]
        public IJSRuntime? JS { get; set; }

        private CancellationTokenSource? cancellationSource;
        private bool displayResults = false;

        private int[]? originalArray;//Do not modify this!
        #endregion

        public record AlgorithmDetails(
            Algorithm AlgorithmType, 
            Complexity BestCaseComplexity,
            Complexity AverageCaseComplexity,
            Complexity WorstCaseComplexity,
            ISortable AlgorithmInstance
            );

        //---------Properties for the UI--------------
        private int Seed { get; set; } = 42;        // Default to 42
        public int ArraySize { get; set; } = 600;   // Default to 600
        //--------------------------------------------

        // The array to sort (do not modify this)
        public int[] Array { get; set; } = null!;


        private int INSTRUCTIONS_UNTIL_REPAINT = 175;
        public int instructionsUntilRepaint;

        //The algorithm to use for sorting
        public Algorithm AlgorithmToUse { get; set; } = Algorithm.BubbleSort; //Default value

        public AlgorithmDetails[] SortingAlgorithmInstances = [
            new AlgorithmDetails(Algorithm.BubbleSort, Complexity.O_N, Complexity.O_N2, Complexity.O_N2, new BubbleSort()),
            new AlgorithmDetails(Algorithm.SelectionSort, Complexity.O_N2, Complexity.O_N2, Complexity.O_N2, new SelectionSort()),
            new AlgorithmDetails(Algorithm.InsertionSort, Complexity.O_N, Complexity.O_N2, Complexity.O_N2, new InsertionSort()),
            new AlgorithmDetails(Algorithm.Quicksort, Complexity.O_NlogN, Complexity.O_NlogN, Complexity.O_N2, new Quicksort()),
            new AlgorithmDetails(Algorithm.Mergesort, Complexity.O_NlogN, Complexity.O_NlogN, Complexity.O_NlogN, new Mergesort()),
            new AlgorithmDetails(Algorithm.OddEvenSort, Complexity.O_N, Complexity.O_N2, Complexity.O_N2, new OddEvenSort()),
            new AlgorithmDetails(Algorithm.CocktailShakerSort, Complexity.O_N, Complexity.O_N2, Complexity.O_N2, new CocktailShakerSort()),
            new AlgorithmDetails(Algorithm.GnomeSort, Complexity.O_N, Complexity.O_N2, Complexity.O_N2, new GnomeSort()), /*TODO These time complexities need tested...*/
            new AlgorithmDetails(Algorithm.CombSort, Complexity.O_NlogN, Complexity.O_N22P, Complexity.O_N2, new CombSort())
            ];

        public AlgorithmDetails? CurrentAlgorithm { get; set; }

        // Gets called when the user clicks the "Go!" button
        // There should be no need to modify this method but feel free to do so if needed
        private async Task Sort(CancellationToken cancellationToken)
        {
            try
            {
                CurrentAlgorithm = SortingAlgorithmInstances
                    .First(x => x.AlgorithmType == AlgorithmToUse);
                await CurrentAlgorithm.AlgorithmInstance.SortAsync(Array, RepaintScreen, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                StateHasChanged();
                if(await ValidateResults())
                {
                    displayResults = true;
                }
            }
            catch(OperationCanceledException)
            {
                Console.WriteLine("Sort was cancelled");
            }
            catch(Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                Console.Error.WriteLine(ex.Message);
            }
            finally
            {
                StateHasChanged();
            }
        }

		private void ChangeAlgorithmToRun(ChangeEventArgs e)
		{
			switch (e.Value!.ToString()!)
			{
                // TODO NOT WORKING WITH INSTTUCTIONS on startup, only when pages change
				case "bubble":
                    AlgorithmToUse = Algorithm.BubbleSort;
                    INSTRUCTIONS_UNTIL_REPAINT = 175;
                    break;
				case "select":
					AlgorithmToUse = Algorithm.SelectionSort;
					INSTRUCTIONS_UNTIL_REPAINT = 1;
					break;
				case "insert":
					AlgorithmToUse = Algorithm.InsertionSort;
                    INSTRUCTIONS_UNTIL_REPAINT = 1;
                    break;
                case "quick":
                    AlgorithmToUse = Algorithm.Quicksort;
                    INSTRUCTIONS_UNTIL_REPAINT = 2;
                    break;
                case "merge":
                    AlgorithmToUse = Algorithm.Mergesort;
                    INSTRUCTIONS_UNTIL_REPAINT = 10;
                    break;
                case "oddeven":
                    AlgorithmToUse = Algorithm.OddEvenSort;
                    INSTRUCTIONS_UNTIL_REPAINT = 80;
                    break;
                case "cocktail":
                    AlgorithmToUse = Algorithm.CocktailShakerSort;
                    INSTRUCTIONS_UNTIL_REPAINT = 76;
                    break;
                case "gnome":
                    AlgorithmToUse = Algorithm.GnomeSort;
                    INSTRUCTIONS_UNTIL_REPAINT = 160;
                    break;
                case "comb":
                    AlgorithmToUse = Algorithm.CombSort;
                    INSTRUCTIONS_UNTIL_REPAINT = 4;
                    break;
            }

            Console.WriteLine($"Algorithm to run: {AlgorithmToUse}");
		}

        #region Helper Methods (you should not change these)
        // ================== Helper Methods (DO NOT MODIFY) ==================

        private async void BeginSort()
        {
            displayResults = false;
            if (cancellationSource != null)
            {
                cancellationSource.Cancel();
                Reset();
            }

            cancellationSource = new CancellationTokenSource();
            instructionsUntilRepaint = INSTRUCTIONS_UNTIL_REPAINT;
            StateHasChanged();
            await Sort(cancellationSource.Token);
        }

        public void Reset()
        {
            displayResults = false;
            if (cancellationSource != null)
            {
                cancellationSource.Cancel();
            }

            Array = originalArray!.ToArray();
            StateHasChanged();
        }

        public async Task RepaintScreen(int? delay)
        {
            if(--instructionsUntilRepaint <= 0)
            {
                instructionsUntilRepaint = INSTRUCTIONS_UNTIL_REPAINT;
                StateHasChanged();
                await Task.Delay(delay ?? 2); // Default time is 2ms
            }
        }

        //Called when the component is initialized
        protected override void OnInitialized()
        {
            PerformArrayPrep();
        }

        private async Task ResizeOrRandomizeArray()
        {
            displayResults = false;
            if (cancellationSource != null)
            {
                cancellationSource.Cancel();
                Reset();
            }

            if (JS != null)
            {
                int elements = await JS.InvokeAsync<int>("getNumberOfElements");
                // Console.WriteLine(elements);
                ArraySize = elements;
            }

            PerformArrayPrep();
        }

        private void PerformArrayPrep()
        {
            //Populate the array with random numbers using LINQ
            Random rand = new Random(Seed);
            Array = Enumerable.Range(0, ArraySize)
                .Select(x => x = rand.Next(1, 1000))
                .ToArray();
            //Clone Array into originalArray
            originalArray = Array.ToArray();
        }

        private async Task<bool> ValidateResults()
        {
            Console.WriteLine("Validating results...");
            // Is Array sorted?
            List<int> tempArray = Array.ToList();
            tempArray.Sort();
            if (tempArray.SequenceEqual(Array))
            {
                Console.WriteLine("Array is sorted!");
                return true;
            }
            else
            {
                if (JS != null)
                {
                    await JS.InvokeVoidAsync("alert", "Array is not sorted!");
                }
                Console.WriteLine("Array is not sorted!");
                return false;
            }
        }

        public String[] Colors = ["#025B7F", "#025d80", "#025e81", "#026082", "#026284", "#026385", "#026486", "#026686", "#026787", "#026a89", "#026b8a", "#026d8c", "#026f8d", "#02708e", "#02728f", "#027390", "#027491", "#027692", "#027893", "#027a94", "#027b95", "#027d96", "#027e97", "#028097", "#028198",
                                  "#028298", "#038499", "#03869a", "#03879b", "#03899b", "#038b9c", "#048d9d", "#048e9e", "#04909e", "#04929f", "#0493a0", "#0594a0", "#0596a1", "#0598a1", "#0599a2", "#069ba3", "#089ca3", "#0a9ca3", "#0c9da3", "#0e9ea3", "#109ea4", "#129fa4", "#14a0a4", "#16a0a4", "#18a1a4",
                                  "#1aa2a5", "#1ca3a5", "#1ea4a5", "#20a4a5", "#22a5a5", "#24a6a6", "#26a6a6", "#28a7a6", "#2ba8a6", "#2da9a7", "#2faaa7", "#31aba7", "#33aba7", "#35aca7", "#37ada7", "#38ada8", "#3aaea8", "#3cafa8", "#3eafa8", "#40b0a8", "#42b1a8", "#44b2a8", "#45b2a9", "#47b3a9", "#49b4a9",
                                  "#4bb4a9", "#4db5a9", "#4fb6a9", "#51b7aa", "#53b7aa", "#54b8aa", "#55b9aa", "#57b9aa", "#58baaa", "#59bbaa", "#5bbbaa", "#5cbcaa", "#5dbdaa", "#5fbeaa", "#61beaa", "#62bfaa", "#63c0aa", "#65c0aa", "#66c1aa", "#67c2aa", "#69c2aa", "#6ac3aa", "#6cc4ab", "#6ec4ab", "#6fc5ab",
                                  "#70c6ab", "#73c7ab", "#77c9ab", "#7acaab", "#7cccab", "#7fcdab", "#82ceab", "#84d0ab", "#88d1ac", "#8bd2ac", "#8cd3ac", "#8dd4ac", "#8fd4ac", "#90d5ac", "#92d5ac", "#94d6ac", "#95d7ac", "#98d8ac", "#9bd9ac", "#9edbac", "#a1dcac", "#a5ddad", "#a8dfad", "#ace0ad", "#b0e2ad",
                                  "#b4e3ad", "#b7e4ad", "#b8e5ad", "#bae6ad", "#bce6ad", "#bde7ad", "#bfe8ae", "#c1e8ae", "#c3e9af", "#c5e9af", "#c7e9b0", "#c9eab0", "#cbeab1", "#ccebb1", "#ceebb1", "#d0ebb2", "#d1ecb2", "#d3ecb3", "#d5edb3", "#d7edb4", "#d9edb4", "#daeeb5", "#dceeb5", "#deefb6", "#e0efb6",
                                  "#e2f0b7", "#e4f0b7", "#e6f0b7", "#e8f1b8", "#eaf1b8", "#ebf2b9", "#edf2b9", "#eff2b9", "#f0f3ba", "#f2f3ba", "#f4f4bb", "#f6f4bb", "#f8f4bb", "#faf5bc", "#FCF5BC", "#fdf4bb", "#fdf4ba", "#fdf3b9", "#fdf2b8", "#fdf1b7", "#fdf0b6", "#fdefb5", "#fdeeb4", "#fdecb3", "#fdebb1",
                                  "#fde9af", "#fde8ae", "#fde7ad", "#fde6ac", "#fde5aa", "#fde4a8", "#fde3a7", "#fde2a6", "#fde1a5", "#fddfa3", "#fddda1", "#fddca0", "#fddb9f", "#fdda9e", "#fdd99d", "#fdd89c", "#fdd79b", "#fdd69a", "#fdd499", "#fdd298", "#fdd197", "#fdd096", "#fdcf95", "#fdcd93", "#fdcc92",
                                  "#fdcb91", "#fdca90", "#fdc88f", "#fdc68e", "#fdc48d", "#fdc28c", "#fdc08b", "#fdbe8a", "#fdbc89", "#fdba88", "#fdb888", "#fdb787", "#fdb686", "#fdb486", "#fdb385", "#fdb184", "#FDAF83", "#fdad83", "#fdab83", "#fda983", "#fca883", "#fca683", "#fca483", "#fba282", "#fba082",
                                  "#fb9e82", "#fb9c82", "#fa9b81", "#fa9981", "#fa9781", "#fa9581", "#f99481", "#f99281", "#f89181", "#f88f81", "#f88e82", "#f78c82", "#f78a82", "#f68982", "#f68782", "#f58683", "#f58483", "#f48383", "#f48183", "#f48084", "#f37e84", "#f37c84", "#f27b84", "#f27984", "#f17885"];
        private String ComplexityToHTML(Complexity complexity)
        {
            switch(complexity)
            {
                case Complexity.O_1:
                    return "O(1)";
                case Complexity.O_logN:
                    return "O(log n)";
                case Complexity.O_N:
                    return "O(n)";
                case Complexity.O_NlogN:
                    return "O(n log n)";
                case Complexity.O_N2:
                    return "O(n<sup>2</sup>)";
                case Complexity.O_N3:
                    return "O(n<sup>3</sup>)";
                case Complexity.O_2N:
                    return "O(2<sup>n</sup>)";
                case Complexity.O_NFact:
                    return "O(n!)";
                case Complexity.O_N22P:
                    return "O(n<sup>2</sup>/2p)";
                default:
                    return "Unknown";
            }
        }
        #endregion
    }
}
