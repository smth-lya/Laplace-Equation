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
        LaplaceCell[,] Grid { get; }

        /// <summary>
        /// Solves the Laplace equation numerically using the Successive Over-Relaxation (SOR) method.
        /// </summary>
        /// <param name="omega">The relaxation parameter (omega) used in the SOR method (default is 1.5).</param>
        /// <param name="eps">The desired precision (epsilon) of the solution (default is 0.01).</param>
        /// <param name="maxIterations">The maximum number of iterations (optional).</param>
        /// <param name="millisecondsTimeout">The timeout value in milliseconds (optional, default is 0).</param>
        /// /// <param name="cancellationToken">The cancellation token (optional).</param>
        void Solve(double omega = 1.5, double eps = 0.01, double maxIterations = 0, int millisecondsTimeout = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Loads the grid with the specified cell values.
        /// </summary>
        /// <param name="grid">The grid of cells to load.</param>
        void LoadGrid(LaplaceCell[,] grid);
    }
}
