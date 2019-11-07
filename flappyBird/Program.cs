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
    class Program
    {
        public static char[,] table = new char[100, 10];
        public static Random random = new Random();

        static void GenerateRandomMap()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    table[i, j] = '-';
                }
            }

            for (int i = 10; i < 100; i += 7)
            {
                int randomNumber = random.Next(0, 8);
                for (int j = 0; j < 10; j++)
                {
                    table[i, j] = (j == randomNumber || j == randomNumber + 1) ? '-' : '#';
                }
            }
        }

        static void PrintMap()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(table[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void SetPlayerToStart()
        {
            table[0, 5] = '@';
        }

        static Side[] GenerateRandomMoves()
        {
            Side[] moves = new Side[100];

            for(int i = 0; i < 100; i++)
            {
                moves[i] = random.Next(0, 2) == 0 ? Side.left : Side.right;

                //Console.Write(moves[i] == Side.left ? "left " : "right ");
            }
            return moves;
        }

        static int PlaySetOfMoves(Side[] moves)
        {
            int playerPosition = 5;

            for(int i = 0; i < moves.Length; i++)
            {
                table[i, playerPosition] = '-';
                try
                {
                    playerPosition += moves[i] == Side.left ? -1 : 1;
                    if (table[i, playerPosition] == '#')
                        return 1;

                    table[i, playerPosition] = '@';
                }
                catch
                {
                    return 1;
                }

                Console.WriteLine("*******************************************");
                PrintMap();
            }
            return 0;
        }

        static void Main(string[] args)
        {
            GenerateRandomMap();

            SetPlayerToStart();

            PrintMap();

            PlaySetOfMoves(GenerateRandomMoves());
        }
    }
}
