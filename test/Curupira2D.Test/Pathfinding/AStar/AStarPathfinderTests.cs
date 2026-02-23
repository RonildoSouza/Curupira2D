using Curupira2D.AI.Extensions;
using Curupira2D.AI.Pathfinding.AStar;
using Curupira2D.AI.Pathfinding.Graphs;
using System.Drawing;

namespace Curupira2D.Test.Pathfinding.AStar
{
    public class AStarPathfinderTests
    {
        [Theory(DisplayName = "Find path with locked goal")]
        [Trait("FindPath", "AStarPathfinder")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnFoundPathFalse_When_LockedGoal(bool allowDiagonalSearch)
        {
            // ___________
            // . . . # . .
            // . S . # . .
            // . . . # G .
            // ___________

            // Arrage
            var start = new Point(1, 1);
            var goal = new Point(4, 2);
            var gridGraph = new GridGraph(6, 3, allowDiagonalSearch)
            {
                Walls = [new(3, 0), new(3, 1), new(3, 2)]
            };

            // Act
            var path = AStarPathfinder.FindPath(gridGraph, start, goal);
            gridGraph.WriteLine(start, goal, path, showPath: true);

            // Assert
            Assert.NotNull(path);
            Assert.False(path.FoundPath);
            Assert.NotEmpty(path.CameFrom);
            Assert.Empty(path.Edges);
            Assert.NotEmpty(path.CostSoFar);
            Assert.True(path.DurationCostSoFar > 0);
        }

        [Theory(DisplayName = "Find path with unlocked goal and without walls on grid")]
        [Trait("FindPath", "AStarPathfinder")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnFoundPathTrue_When_UnlockedGoal_And_WithoutWalls(bool allowDiagonalSearch)
        {
            // ___________
            // . . . . . .
            // . S . . . .
            // . . . . G .
            // ___________

            // Arrage
            var start = new Point(1, 1);
            var goal = new Point(4, 2);
            var gridGraph = new GridGraph(6, 3, allowDiagonalSearch);

            // Act
            var path = AStarPathfinder.FindPath(gridGraph, start, goal);
            gridGraph.WriteLine(start, goal, path, showPath: true);

            // Assert
            Assert.NotNull(path);
            Assert.True(path.FoundPath);
            Assert.NotEmpty(path.CameFrom);
            Assert.NotEmpty(path.Edges);
            Assert.NotEmpty(path.CostSoFar);
            Assert.True(path.DurationCostSoFar > 0);
        }

        [Theory(DisplayName = "Find path with unlocked goal and with walls on grid")]
        [Trait("FindPath", "AStarPathfinder")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnFoundPathTrue_When_UnlockedGoal_And_WithWalls(bool allowDiagonalSearch)
        {
            // ___________
            // . . . # . .
            // . S . # . .
            // . . . . G .
            // ___________

            // Arrage
            var start = new Point(1, 1);
            var goal = new Point(4, 2);
            var gridGraph = new GridGraph(6, 3, allowDiagonalSearch)
            {
                Walls = [new(3, 0), new(3, 1)]
            };

            // Act
            var path = AStarPathfinder.FindPath(gridGraph, start, goal);
            gridGraph.WriteLine(start, goal, path, showPath: true);

            // Assert
            Assert.NotNull(path);
            Assert.True(path.FoundPath);
            Assert.NotEmpty(path.CameFrom);
            Assert.NotEmpty(path.Edges);
            Assert.NotEmpty(path.CostSoFar);
            Assert.True(path.DurationCostSoFar > 0);
        }

        [Theory(DisplayName = "Find path with unlocked goal and with walls on big grid")]
        [Trait("FindPath", "AStarPathfinder")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnFoundPathTrue_When_UnlockedGoal_And_WithWalls_BigGrid(bool allowDiagonalSearch)
        {
            // ___________
            // . . . # G .
            // . S . # . .
            // . . . # . .
            // . . . # # .
            // . . . # . .
            // . . . # . .
            // . . . . . .
            // . . . . . .
            // ___________

            // Arrage
            var start = new Point(10, 10);
            var goal = new Point(40, 0);

            var walls = Enumerable
                .Range(0, 21)
                .Select(i => new Point(30, i))
                .ToHashSet();

            walls.Add(new(31, 3));
            walls.Add(new(32, 3));
            walls.Add(new(33, 3));
            walls.Add(new(34, 3));

            var gridGraph = new GridGraph(60, 30, allowDiagonalSearch)
            {
                Walls = walls
            };

            // Act
            var path = AStarPathfinder.FindPath(gridGraph, start, goal);
            gridGraph.WriteLine(start, goal, path, showPath: true);

            // Assert
            Assert.NotNull(path);
            Assert.True(path.FoundPath);
            Assert.NotEmpty(path.CameFrom);
            Assert.NotEmpty(path.Edges);
            Assert.NotEmpty(path.CostSoFar);
            Assert.True(path.DurationCostSoFar > 0);
        }
    }
}