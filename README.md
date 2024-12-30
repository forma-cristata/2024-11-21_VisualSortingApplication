# Visual Sorting Algorithm Demonstrator

## About

This project is a visual demonstration of various sorting algorithms implemented in C# using Blazor. The application provides a real-time visualization of how different sorting algorithms work, making it an excellent educational tool for understanding sorting mechanisms.

## Features

- Interactive visualization of sorting processes
- Multiple sorting algorithm implementations including:
    - Bubble Sort
    - Selection Sort
    - Insertion Sort
    - Quicksort
    - Mergesort
    - Cocktail Shaker Sort
    - Comb Sort
    - Gnome Sort
    - Odd-Even Sort
- Real-time display of sorting progress
- Algorithm complexity information (Best, Average, and Worst case)
- Ability to cancel sorting operations mid-process

## Technical Implementation

The project is structured around the ISortable interface, which each sorting algorithm implements. Key technical features include:

- Repaint delegate system for UI updates during sorting
- Configurable delay timing for visualization
- CancellationToken implementation for operation control
- Blazor-based user interface

## Code Structure

The main components of the project include:

- Algorithms directory containing nine distinct sorting algorithm implementations
- Index.razor.cs handling the main UI logic
- Algorithm enum for sorting algorithm selection
- SortingAlgorithmInstances array managing algorithm instantiation

## Running the Project

To run this project:

1. Ensure you have the .NET SDK installed
2. Clone the repository
3. Build and run the project using your preferred IDE or command line
4. Select a sorting algorithm from the dropdown
5. Click "Go!" to start the visualization
