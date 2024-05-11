using LaplaceSOR.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace LaplaceSOR.Solving
{
    public class LaplaceEquationSolver : ILaplaceEquationSolver
    {
        private int _height;
        private int _width;

        private int _processCount;
        private int _chunkHeight;

        private Barrier _barrier;

        private LaplaceCell[,] _grid;
        private double[,] _nextGrid;
        private double[] _maxdiffs;

        private double _maxdiff;

        public event Action? GridUpdated;

        public int Height { get { return _height; } }
        public int Width { get { return _width; } }
        public LaplaceCell[,] Grid {  get { return _grid; } }

        private LaplaceEquationSolver() { }
        public LaplaceEquationSolver(LaplaceCell[,] initialGrid) => LoadGrid(initialGrid);

        [MemberNotNull(nameof(_grid), nameof(_nextGrid), nameof(_barrier), nameof(_maxdiffs))]
        public void LoadGrid(LaplaceCell[,] initialGrid)
        {
            ArgumentNullException.ThrowIfNull(initialGrid, nameof(initialGrid));

            _height = initialGrid.GetLength(0);
            _width = initialGrid.GetLength(1);

            if (_height < 2 || _width < 2)
            {
                throw new NotSupportedException($"Height: {_height}, Width: {_width}");
            }

            _processCount = DefineProcessCount(_height);
            _chunkHeight = _height / _processCount;

            _maxdiffs = new double[_processCount];
            
            _grid = (LaplaceCell[,])initialGrid.Clone();
            _nextGrid = new double[_height, _width];

            _barrier = new Barrier(_processCount, _ => 
            {
                _maxdiff = _maxdiffs.Max();
                GridUpdated?.Invoke();
            });
        }
        public void Solve(double omega = 1.5, double eps = 0.01, double maxIterations = 0, int millisecondsTimeout = 0, CancellationToken cancellationToken = default)
        {
            _maxdiffs = new double[_processCount];

            Parallel.For(0, _processCount, (worker, loopState) =>
            {
                int startY = worker * _chunkHeight + (worker == 0 ? 1 : 0);
                int endY = startY + _chunkHeight + (worker == _processCount - 1 ? -1 : 0);

                int iter = 0;

                do
                {
                    var redmaxdiff = UpdateGrid(omega, startY, endY, isWorkingOnRedLaplaceCells: false);
                    var blackmaxdiff = UpdateGrid(omega, startY, endY, isWorkingOnRedLaplaceCells: true);

                    _maxdiffs[worker] = Math.Max(redmaxdiff, blackmaxdiff);

                    _barrier.SignalAndWait();

                    if (millisecondsTimeout > 0)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                } while (_maxdiff > eps && (maxIterations == 0 || ++iter < maxIterations));
            
                loopState.Break();
            });
        }

        private static int DefineProcessCount(int height)
        {
            int count = 1;

            for (int i = 2; i <= Environment.ProcessorCount; i++)
            {
                if (height % i == 0)
                {
                    if ((i - height / i) >= 0)
                    {
                        return i;
                    }

                    count = i;
                }
            }

            return count;
        }
        private double UpdateGrid(double omega, int startY, int endY, bool isWorkingOnRedLaplaceCells)
        {
            double maxdiff = 0;

            for (int y = startY; y < endY; y++)
            {
                int x = isWorkingOnRedLaplaceCells
                    ? (y % 2 == 0 ? 1 : 2)
                    : (y % 2 == 0 ? 2 : 1);

                for (; x < _width - 1; x += 2)
                {
                    var temp = _grid[y, x].Value;
                    _grid[y, x].Value = (1 - omega) * _grid[y, x].Value
                                        + omega * 0.25 * (_grid[y, x + 1].Value + _grid[y, x - 1].Value
                                                        + _grid[y + 1, x].Value + _grid[y - 1, x].Value);

                    maxdiff = Math.Max(maxdiff, Math.Abs(_grid[y, x].Value - temp));
                }
            }

            return maxdiff;
        }
    }
}
