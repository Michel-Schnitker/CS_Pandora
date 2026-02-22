using System;
using System.Collections.Generic;
using System.Text;

namespace PandoraWorld_CS.Pandora.Board
{
    public class Position
    {

        private int x;
        private int y;

        /// <summary>
        /// TODO:
        /// </summary>
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public int SetRandomX(int max)
        {
            x = Random.Shared.Next(0, max);

            return X;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public int SetRandomY(int max)
        {
            y = Random.Shared.Next(0, max);

            return Y;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public Position SetRandom(int max)
        {
            SetRandomX(max);
            SetRandomY(max);

            return this;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public Position Copy()
        {
            return new Position(x, y);
        }
    }
}
