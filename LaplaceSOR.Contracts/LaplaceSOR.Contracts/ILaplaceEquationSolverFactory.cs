namespace LaplaceSOR.Contracts
{
    /// <summary>
    /// Factory interface for creating instances of class implementing the Laplace equation solver.
    /// </summary>
    public interface ILaplaceEquationSolverFactory
    {
        /// <summary>
        /// Creates an instance of a class implementing the Laplace equation solver.
        /// </summary>
        /// <param name="height">The height of the grid for solving the equation.</param>
        /// <param name="width">The width of the grid for solving the equation.</param>
        /// <param name="processCount">The number of processes for parallelizing computations.</param>
        /// <returns>An instance of a class implementing the Laplace equation solver.</returns>
        abstract static ILaplaceEquationSolver CreateParallelSolver(int height, int width, int processCount);
    }
}
