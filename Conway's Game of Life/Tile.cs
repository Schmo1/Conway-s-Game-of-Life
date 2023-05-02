using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Diagnostics.Eventing.Reader;

namespace Conway_s_Game_of_Life
{
    internal class Tile
    {
        private static readonly SolidColorBrush tileColor = new(Color.FromRgb(0, 186, 0));

        public uint AliveNeighbours { 
            get 
            {
                uint aliveNeighbours = 0; 
                NeighbourTiles.ForEach((neighbour) =>{ if (neighbour.Alive) aliveNeighbours++; });
                return aliveNeighbours;
            } 
        }

        private bool _aliveNextGen = false;

        public bool AliveNextGen 
        { 
            get { return _aliveNextGen; } 
            set { _aliveNextGen = value; } 
        }

        private bool _alive = false;

        public bool Alive
        {
            get { return _alive; }
            set
            {
                if (value)
                    RTile.Fill = tileColor;
                else
                    RTile.Fill = Brushes.Black;

                _alive = value;
            }
        }

        public int Row { get; private set; }
        public int Column { get; private set; }

        public List<Tile> NeighbourTiles { get; set; }

        public Rectangle RTile { get; set; }
       

        public Tile(int row, int column)
        {
            Row = row;
            Column = column;
        }        

        public void GenerateNextGeneration()
        {

            //1. Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            if(AliveNeighbours < 2){ AliveNextGen = false; }

            //2. Any live cell with two or three live neighbours lives on to the next generation.
            else if(AliveNeighbours == 2 ) { AliveNextGen = Alive; }

            //3. Any live cell with more than three live neighbours dies, as if by overpopulation.
            else if(AliveNeighbours > 3) { AliveNextGen = false; }

            //4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
            else if(AliveNeighbours == 3) { AliveNextGen = true; }
        }


    }
}
