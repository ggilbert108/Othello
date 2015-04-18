using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Auxillary
{
    static class Util
    {
        public static Random random = new Random();

        public static LinkedList<int> getShuffledConsecutive(int n)
        {
            LinkedList<int> a = new LinkedList<int>();
            for (int i = 0; i < n; i++)
            {
                a.AddLast(i);
            }
            shuffle(a);
            return a;
        }

        public static void shuffle<T>(LinkedList<T> list)
        {
            T[] array = list.ToArray();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                int j = random.Next(i);
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            list.Clear();
            for (int i = 0; i < array.Length; i++)
            {
                list.AddLast(array[i]);
            }
        }
    }
}
