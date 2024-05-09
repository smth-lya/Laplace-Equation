namespace LaplaceSOR.Contracts
{
    /// <summary>
    /// Interfaces for solving Laplace equation.
    /// </summary>
    public interface ILaplaceEquationSolver
    {
        /// <summary>
        /// Event that occurs after the grid updated.
        /// </summary>
        event Action GridUpdated;

        /// <summary>
        /// Height of the grid.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Width of the grid.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Grid containing the solution to the Laplace equation.
        /// </summary>
        double[,] Grid { get; }

        /// <summary>
        /// Solves the Laplace equation numerically using the Successive Over-Relaxation (SOR) method.
        /// </summary>
        /// <param name="omega">The relaxation parameter (omega) used in the SOR method (default is 1.5).</param>
        /// <param name="eps">The desired precision (epsilon) of the solution (default is 0.01).</param>
        public void Solve(double omega = 1.5, double eps = 0.01, double? maxIterations = null, int? millisecondsTimeout = 10);

        void LoadGrid(double[,] grid);
    }
}
