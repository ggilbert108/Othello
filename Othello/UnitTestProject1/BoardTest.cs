using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Othello.Game;

namespace UnitTestProject1
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void getMoves_test()
        {
            Board board = new Board();
            HashSet<Coord> moves = board.getMoves(Piece.Black);
            bool result = moves.Contains(new Coord(2, 2));
            Assert.IsTrue(result);
        }
    }
}
