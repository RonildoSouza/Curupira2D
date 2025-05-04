using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Curupira2D.AI.Pathfinding.AStar;
using Curupira2D.AI.Pathfinding.Graphs;
using System.Drawing;
using Curupira2D.AI.Pathfinding.BreadthFirst;
using Curupira2D.AI.Extensions;

namespace Curupira2D.Test.Pathfinding.BreadthFirst
{
    public class BreadthFirstPathfinderTests
    {
        [Theory(DisplayName = "Find path with locked goal")]
        [Trait("FindPath", "BreadthFirstPathfinder")]
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
            var path = BreadthFirstPathfinder.FindPath(gridGraph, start, goal);
            gridGraph.WriteLine(start, goal, path, showPath: true);

            // Assert
            Assert.NotNull(path);
            Assert.False(path.FoundPath);
            Assert.NotEmpty(path.CameFrom);
            Assert.Empty(path.Edges);
            Assert.Empty(path.CostSoFar);
            Assert.Equal(0, path.DurationCostSoFar);
        }

        [Theory(DisplayName = "Find path with unlocked goal and without walls on grid")]
        [Trait("FindPath", "BreadthFirstPathfinder")]
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
            var path = BreadthFirstPathfinder.FindPath(gridGraph, start, goal);
            gridGraph.WriteLine(start, goal, path, showPath: true);

            // Assert
            Assert.NotNull(path);
            Assert.True(path.FoundPath);
            Assert.NotEmpty(path.CameFrom);
            Assert.NotEmpty(path.Edges);
            Assert.Empty(path.CostSoFar);
            Assert.Equal(0, path.DurationCostSoFar);
        }

        [Theory(DisplayName = "Find path with unlocked goal and with walls on grid")]
        [Trait("FindPath", "BreadthFirstPathfinder")]
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
            var path = BreadthFirstPathfinder.FindPath(gridGraph, start, goal);
            gridGraph.WriteLine(start, goal, path, showPath: true);

            // Assert
            Assert.NotNull(path);
            Assert.True(path.FoundPath);
            Assert.NotEmpty(path.CameFrom);
            Assert.NotEmpty(path.Edges);
            Assert.Empty(path.CostSoFar);
            Assert.Equal(0, path.DurationCostSoFar);
        }
    }
}