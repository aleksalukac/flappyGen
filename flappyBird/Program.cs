using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flappyBird
{
    enum Side
    {
        left = -1,
        right = 1
    }

    class Entity
    {
        public const int tableSize = 1000;
        public List<Side> Moves = new List<Side>(tableSize);
    }

    class Program
    {
        public static Random random = new Random();
        public static DateTime localDate = DateTime.Now;
        public const int numberOfGenerations = 100;
        public const int oldPopulation = 5;
        public const int newRandom = 95;
        public const int generationSize = 100;
        public const int tableSize = 1000;

        static char[,] GenerateRandomMap()
        {
            char[,] table = new char[tableSize, 10];

            for (int i = 0; i < tableSize; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    table[i, j] = random.Next(0, 8) == 1 && (i % 10 == 5 || i % 10 == 4) ? '#' : '-';
                }
            }

            for (int i = 10; i < tableSize; i += 10)
            {
                int randomNumber = random.Next(0, 8);
                for (int j = 0; j < 10; j++)
                {
                    table[i, j] = (j == randomNumber || j == randomNumber + 1) ? '-' : '#';
                }
            }

            return table;
        }

        static void PrintMap(ref char[,] table)
        {
            for (int i = 0; i < tableSize; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //Console.Write(table[i, j]);
                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(@"C:\Users\Aleksa\source\repos\flappyBird\results\map_" + localDate.ToString("yyyy-dd-M--HH-mm-ss") + ".txt", true))
                    {
                        file.Write(table[i, j]);
                    }
                }
                //Console.WriteLine();
                using (System.IO.StreamWriter file =
                       new System.IO.StreamWriter(@"C:\Users\Aleksa\source\repos\flappyBird\results\map_" + localDate.ToString("yyyy-dd-M--HH-mm-ss") + ".txt", true))
                {
                    file.WriteLine();
                }
            }
        }

        static void SetPlayerToStart(ref char[,] table)
        {
            table[0, 5] = '@';
        }

        static Entity GenerateRandomMovesFromBest(Entity best, int bestScore)
        {
            if (bestScore == -1)
                return GenerateRandomMoves();

            int randomBackMove = random.Next(2, 10);

            Entity entity = new Entity();

            for (int i = 0; i < tableSize; i++)
            {
                if (i < bestScore - randomBackMove)
                {
                    entity.Moves.Add(best.Moves[i]);
                }
                else
                    entity.Moves.Add(random.Next(0, 2) == 0 ? Side.left : Side.right);

                //Console.Write(moves[i] == Side.left ? "left " : "right ");
            }
            return entity;
        }

        static Entity GenerateRandomMoves()
        {
            Entity entity = new Entity();

            for(int i = 0; i < tableSize; i++)
            {
                entity.Moves.Add(random.Next(0, 2) == 0 ? Side.left : Side.right);

                //Console.Write(moves[i] == Side.left ? "left " : "right ");
            }
            return entity;
        }

        static int PlaySetOfMoves(Entity entity, char[,] table)
        {
            char[,] table2 = (char[,])table.Clone();
            int playerPosition = 5;

            for(int i = 0; i < entity.Moves.Count; i++)
            {
                //table2[i, playerPosition] = '-';
                try
                {
                    playerPosition += entity.Moves[i] == Side.left ? -1 : 1;
                    if (table2[i, playerPosition] == '#')
                        return i;

                    table2[i, playerPosition] = '@';
                }
                catch
                {
                    return i;
                }

                //Console.WriteLine("*******************************************");
                //PrintMap(ref table);
            }
            return 0;
        }

        static void PlayAndPrint(Entity entity, char[,] table, int numberOfMoves, int generation)
        {
            char[,] table2 = (char[,])table.Clone();
            int playerPosition = 5;

            for (int i = 0; i < entity.Moves.Count; i++)
            {
                //table2[i, playerPosition] = '-';
                try
                {
                    playerPosition += entity.Moves[i] == Side.left ? -1 : 1;
                    if (table2[i, playerPosition] == '#')
                        break;

                    table2[i, playerPosition] = '@';
                }
                catch
                {
                    break;
                }
                //Console.WriteLine("*******************************************");
                //PrintMap(ref table);
            }
            //Console.WriteLine("*******************************************");
            using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(@"C:\Users\Aleksa\source\repos\flappyBird\results\map_" + localDate.ToString("yyyy-dd-M--HH-mm-ss") + ".txt", true))
            {
                file.Write("*****************\n result: " + numberOfMoves + " generation: " + generation + "\n\n");
            }
            PrintMap(ref table2);
        }

        static Side[] FindBestMoves(char[,] table)
        {
            Entity[,] generation = new Entity[numberOfGenerations, generationSize];

            int bestScore = -1;
            int besti = -1, bestj = -1;
            int generationNumber = 0;

            for(int i = 0; i < numberOfGenerations; i++)
            {
                if (bestScore > tableSize * 0.95)
                    break;

                for(int j = 0; j < generationSize; j++)
                {
                    for (int k = 0; k < tableSize; k++)
                    {
                        if(bestScore == -1)
                            generation[i, j] = GenerateRandomMoves();
                        else
                            generation[i, j] = GenerateRandomMovesFromBest(generation[besti,bestj], bestScore);
                    }

                    int score = PlaySetOfMoves(generation[i, j], table);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        besti = i;
                        bestj = j;
                    }
                }
                Console.WriteLine("Generation: " + i + " best score: " + bestScore);
                generationNumber = i;
            }

            PlayAndPrint(generation[besti, bestj], table, bestScore, generationNumber);


            return null;
        }

        static void Main(string[] args)
        {
            char[,] table = GenerateRandomMap();

            SetPlayerToStart(ref table);

            PrintMap(ref table);

            FindBestMoves(table);

            PlaySetOfMoves(GenerateRandomMoves(), table);
        }
    }
}
