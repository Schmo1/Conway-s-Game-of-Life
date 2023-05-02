using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Conway_s_Game_of_Life
{
    internal class Generation
    {
        public readonly Tile[,] tiles;
        public readonly int rowLenght;
        public readonly int columnLenght;
        public uint Probability { get; set; } = 50;

        public bool NextGenerationCalculated = false;

        public Generation(int rowLenght, int columnLenght)
        {
            this.rowLenght = rowLenght;
            this.columnLenght = columnLenght;
            tiles = GenerateGeneration(rowLenght, columnLenght);
                        
        }



        public void ResetGeneration()
        {
            ForEach((tile) => { tile.Alive = false; tile.AliveNextGen = false; });
        }

        public void SetNextGeneration()
        {
            
            CalculateNextGeneration();
            

            ForEach((tile) =>
            {
                tile.Alive = tile.AliveNextGen;
            });
        }

        public void CalculateNextGeneration()
        {
            NextGenerationCalculated = false;

            ForEach((tile) =>
            {
                tile.GenerateNextGeneration();
            });

            NextGenerationCalculated = true;
        }

        public void SetRandomGeneration()
        {
            Random rn = new(DateTime.Now.Millisecond);
            ForEach(tile => tile.Alive = rn.Next(101) < Probability);
        }




        public void ForEach(Action<Tile> action)
        {            
            //row
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                //column
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    action(tiles[row,col]);
                }

            }
        }


        private static Tile[,] GenerateGeneration(int rowlenght, int colLenght)
        {

            Tile[,] newGeneration = new Tile[rowlenght, colLenght]; //Empty array 

            for (int row = 0; row < rowlenght; row++)
            {
                for (int column = 0; column < colLenght; column++)
                {
                    newGeneration[row, column] = new Tile(row, column); //commit the neighbours
                }
            }


            for (int row = 0; row < rowlenght; row++)
            {
                for (int column = 0; column < colLenght; column++)
                {
                    List<Tile> tiles = new() //Set neighbourtiles in a list
            {
                        newGeneration[row,CalculateIndexOverOrUnderflow(column - 1,colLenght)], //Left
                        newGeneration[CalculateIndexOverOrUnderflow(row - 1, rowlenght), CalculateIndexOverOrUnderflow(column - 1, colLenght)], //LeftAbove
                        newGeneration[CalculateIndexOverOrUnderflow(row - 1, rowlenght), column], //Above
                        newGeneration[CalculateIndexOverOrUnderflow((row - 1), rowlenght), CalculateIndexOverOrUnderflow((column + 1), colLenght)], //AboveRight
                        newGeneration[row, CalculateIndexOverOrUnderflow((column + 1), colLenght)], //Right
                        newGeneration[CalculateIndexOverOrUnderflow((row + 1), rowlenght), CalculateIndexOverOrUnderflow(column + 1, colLenght)], //BelowRight
                        newGeneration[CalculateIndexOverOrUnderflow((row + 1), rowlenght), column], //Below
                        newGeneration[CalculateIndexOverOrUnderflow((row + 1), rowlenght), CalculateIndexOverOrUnderflow(column - 1, colLenght)], //BelowLeft
                    };

                    newGeneration[row, column].NeighbourTiles = tiles;
                }
            }

            return newGeneration;
        }

        private static int CalculateIndexOverOrUnderflow(int index, int indexMax)
        {
            if (index > indexMax - 1) return 0;
            else if (index < 0) return indexMax - 1;

            return index;
        }
    }
}
