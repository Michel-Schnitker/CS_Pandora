using System;
using System.Collections.Generic;
using PandoraWorld_CS.Pandora.Board;
using PandoraWorld_CS.Pandora.LifeForm;

namespace PandoraWorld_CS.Pandora
{
    /// <summary>
    /// Represents statistics for a single game round.
    /// </summary>
    public readonly record struct RoundStatistics(
        int RoundNumber,
        int FishCount,
        int SharkCount,
        int PlantCellCount,
        long TotalPlantPopulation,
        DateTime Timestamp
    );

    /// <summary>
    /// Ring buffer implementation for storing game statistics.
    /// Uses MainWindow as mediator - triggered after each round.
    /// </summary>
    public class Statistics
    {
        private readonly RoundStatistics[] _buffer;
        private readonly int _capacity;
        private int _head;
        private int _count;
        private int _currentRound;

        public int Count => _count;
        public int Capacity => _capacity;
        public int CurrentRound => _currentRound;

        /// <summary>
        /// Creates a new Statistics instance with a ring buffer of specified capacity.
        /// </summary>
        /// <param name="capacity">Maximum number of rounds to store (default: 1000).</param>
        public Statistics(int capacity = 1000)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 1);
            
            _capacity = capacity;
            _buffer = new RoundStatistics[capacity];
            _head = 0;
            _count = 0;
            _currentRound = 0;
        }

        /// <summary>
        /// Records statistics from the current game board state.
        /// Called by MainWindow after each round.
        /// </summary>
        /// <param name="gameBoard">The game board to analyze.</param>
        public void RecordRound(GameBoard gameBoard)
        {
            ArgumentNullException.ThrowIfNull(gameBoard);

            var (fishCount, sharkCount, plantCellCount, totalPlants) = CountEntities(gameBoard);

            _currentRound++;
            
            var stats = new RoundStatistics(
                RoundNumber: _currentRound,
                FishCount: fishCount,
                SharkCount: sharkCount,
                PlantCellCount: plantCellCount,
                TotalPlantPopulation: totalPlants,
                Timestamp: DateTime.Now
            );

            // Ring buffer write
            _buffer[_head] = stats;
            _head = (_head + 1) % _capacity;
            
            if (_count < _capacity)
                _count++;
        }

        /// <summary>
        /// Counts all entities on the game board.
        /// </summary>
        private static (int Fish, int Sharks, int PlantCells, long TotalPlants) CountEntities(GameBoard gameBoard)
        {
            int fishCount = 0;
            int sharkCount = 0;
            int plantCellCount = 0;
            long totalPlantPopulation = 0;

            int size = gameBoard.Size;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var cell = gameBoard.Cells[x, y];

                    switch (cell.lifeFormOnCell)
                    {
                        case Fish:
                            fishCount++;
                            break;
                        case Shark:
                            sharkCount++;
                            break;
                    }

                    if (cell.plantPopulation > 0)
                    {
                        plantCellCount++;
                        totalPlantPopulation += cell.plantPopulation;
                    }
                }
            }

            return (fishCount, sharkCount, plantCellCount, totalPlantPopulation);
        }

        /// <summary>
        /// Gets the most recent statistics entry.
        /// </summary>
        public RoundStatistics? GetLatest()
        {
            if (_count == 0)
                return null;

            int latestIndex = (_head - 1 + _capacity) % _capacity;
            return _buffer[latestIndex];
        }

        /// <summary>
        /// Gets statistics for a specific round number (if still in buffer).
        /// </summary>
        public RoundStatistics? GetByRound(int roundNumber)
        {
            foreach (var stat in GetAll())
            {
                if (stat.RoundNumber == roundNumber)
                    return stat;
            }
            return null;
        }

        /// <summary>
        /// Gets all stored statistics in chronological order (oldest first).
        /// </summary>
        public IEnumerable<RoundStatistics> GetAll()
        {
            if (_count == 0)
                yield break;

            int start = _count < _capacity ? 0 : _head;
            
            for (int i = 0; i < _count; i++)
            {
                int index = (start + i) % _capacity;
                yield return _buffer[index];
            }
        }

        /// <summary>
        /// Gets the last N statistics entries (most recent last).
        /// </summary>
        public IEnumerable<RoundStatistics> GetLast(int count)
        {
            if (_count == 0 || count <= 0)
                yield break;

            int actualCount = Math.Min(count, _count);
            int start = (_head - actualCount + _capacity) % _capacity;

            for (int i = 0; i < actualCount; i++)
            {
                int index = (start + i) % _capacity;
                yield return _buffer[index];
            }
        }

        /// <summary>
        /// Resets all statistics.
        /// </summary>
        public void Reset()
        {
            _head = 0;
            _count = 0;
            _currentRound = 0;
            Array.Clear(_buffer);
        }
    }
}
