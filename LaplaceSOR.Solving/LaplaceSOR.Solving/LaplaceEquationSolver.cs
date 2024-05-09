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

        private double[,] _grid;
        private double[,] _nextGrid;
        private double[] _maxdiffs;

        private double _maxdiff;

        public event Action? GridUpdated;

        public int Height { get { return _height; } }
        public int Width { get { return _width; } }
        public double[,] Grid {  get { return _grid; } }

        //private LaplaceEquationSolver() { }
        public LaplaceEquationSolver(double[,] initialGrid) => LoadGrid(initialGrid);

        [MemberNotNull(nameof(_grid), nameof(_nextGrid), nameof(_barrier), nameof(_maxdiffs))]
        public void LoadGrid(double[,] initialGrid)
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
            
            _grid = (double[,])initialGrid.Clone();
            _nextGrid = new double[_height, _width];

            _barrier = new Barrier(_processCount, _ => 
            {
                _maxdiff = _maxdiffs.Max();
                GridUpdated?.Invoke();
            });
        }
        public void Solve(double omega = 1.5, double eps = 0.01, double? maxIterations = null, int? millisecondsTimeout = 10)
        {
            _maxdiffs = new double[_processCount];

            Parallel.For(0, _processCount, (worker, loopState) =>
            {
                int startY = worker * _chunkHeight + (worker == 0 ? 1 : 0);
                int endY = startY + _chunkHeight + (worker == _processCount - 1 ? -1 : 0);

                int iter = 0;

                do
                {
                    var redmaxdiff = UpdateGrid(omega, startY, endY, isWorkingOnRedCells: false);
                    var blackmaxdiff = UpdateGrid(omega, startY, endY, isWorkingOnRedCells: true);

                    _maxdiffs[worker] = Math.Max(redmaxdiff, blackmaxdiff);

                    _barrier.SignalAndWait();

                    if (millisecondsTimeout != null)
                    {
                        Thread.Sleep((int)millisecondsTimeout);
                    }

                } while (_maxdiff > eps && (maxIterations == null || ++iter < maxIterations));
            });
        }
        public void SolveSequentially(double omega = 1.5, double eps = 0.01, double? maxIterations = null, int? millisecondsTimeout = 10)
        {
            _maxdiffs = new double[_processCount];
            
            int startY = 1;
            int endY = _height - 1;

            int iter = 0;

            do
            {
                var redmaxdiff = UpdateGrid(omega, startY, endY, isWorkingOnRedCells: false);
                var blackmaxdiff = UpdateGrid(omega, startY, endY, isWorkingOnRedCells: true);

                _maxdiff = Math.Max(redmaxdiff, blackmaxdiff);

                GridUpdated?.Invoke();

                if (millisecondsTimeout != null)
                {
                    Thread.Sleep((int)millisecondsTimeout);
                }

            } while (_maxdiff > eps && (maxIterations == null || ++iter < maxIterations));
        }

        public void ResetGrid(double boundaryValue = 100)
        {
             _grid = new double[_height, _width];

            for (int y = 0; y < _height; y++)
            {
                _grid[y, 0] = boundaryValue;
                _grid[y, _width - 1] = boundaryValue;
            }

            for (int x = 0; x < _width; x++)
            {
                _grid[0, x] = boundaryValue;
                _grid[_height - 1, x] = boundaryValue;
            }
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
        private double UpdateGrid(double omega, int startY, int endY, bool isWorkingOnRedCells)
        {
            double maxdiff = 0;

            for (int y = startY; y < endY; y++)
            {
                int x = isWorkingOnRedCells
                    ? (y % 2 == 0 ? 1 : 2)
                    : (y % 2 == 0 ? 2 : 1);

                for (; x < _width - 1; x += 2)
                {
                    var temp = _grid[y, x];
                    _grid[y, x] = (1 - omega) * _grid[y, x]
                                + omega * 0.25 * (_grid[y, x + 1] + _grid[y, x - 1]
                                                + _grid[y + 1, x] + _grid[y - 1, x]);

                    maxdiff = Math.Max(maxdiff, Math.Abs(_grid[y, x] - temp));
                }
            }

            return maxdiff;
        }
    }
}
