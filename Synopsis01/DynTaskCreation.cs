using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopsis01
{
   public class DynTaskCreation
    {
        private static int THRESHOLD = 100;

        static public void MergeSort_Divide(int[] numbers, int left, int right, int depthRemaining)
        {
            int mid;
            if (left - right <= THRESHOLD)
            {
                InsertionSort(numbers, left, right);
            }
            else
            {
                mid = (right + left) / 2;
                if (depthRemaining > 0)
                {
                    try
                    {
                        var t1 = Task.Factory.StartNew(
                            () => MergeSort_Divide(numbers, left, mid, depthRemaining - 1),
                            TaskCreationOptions.AttachedToParent);
                        var t2 = Task.Factory.StartNew(
                            () => MergeSort_Divide(numbers, (mid + 1), right, depthRemaining - 1),
                            TaskCreationOptions.AttachedToParent);
                        Task.WaitAll(t1, t2);
                    }
                    catch (AggregateException ae)
                    {
                        foreach (var e in ae.InnerExceptions)
                        {
                            // Handle the custom exception.
                            if (e is CustomException)
                            {
                                Console.WriteLine(e.Message);
                            }
                            // Rethrow any other exception.
                            else
                            {
                                throw;
                            }
                        }
                    }

                }
                else
                {
                    try
                    {
                        MergeSort_Divide(numbers, left, mid, 0);
                        MergeSort_Divide(numbers, (mid + 1), right, 0);
                    }
                    catch (AggregateException ae)
                    {
                        foreach (var e in ae.InnerExceptions)
                        {
                            // Handle the custom exception.
                            if (e is CustomException)
                            {
                                Console.WriteLine(e.Message);
                            }
                            // Rethrow any other exception.
                            else
                            {
                                throw;
                            }
                        }
                    }
                }


                MergeSort_Conquer(numbers, left, (mid + 1), right);
            }
        }

        static public void MergeSort_Conquer(int[] numbers, int left, int mid, int right)
        {
            int[] temp = new int[numbers.Length];
            int i, eol, num, pos;

            eol = (mid - 1);
            pos = left;
            num = (right - left + 1);

            while ((left <= eol) && (mid <= right))
            {
                if (numbers[left] <= numbers[mid])
                    temp[pos++] = numbers[left++];
                else
                    temp[pos++] = numbers[mid++];
            }

            while (left <= eol)
                temp[pos++] = numbers[left++];

            while (mid <= right)
                temp[pos++] = numbers[mid++];

            for (i = 0; i < num; i++)
            {
                numbers[right] = temp[right];
                right--;
            }
        }

        
        static public void ProcessCalc_MergeSort(int[] numbers, int left, int right)
        {
            // ,2) + 4) if you want to do 16x the processor count, depending on system.
            MergeSort_Divide(numbers, left, right, (int)Math.Log(Environment.ProcessorCount));
        }


        static public void InsertionSort(int[] numarray, int start, int end)
        {
            for (int i = start + 1; i < end; i++)
            {
                int val = numarray[i];

                int j = i - 1;
                while (j >= 0 && val < numarray[j])
                {
                    numarray[j + 1] = numarray[j];
                    j--;

                }
                numarray[j + 1] = val;
            }
        }

        private static void Parallel_QuickSort(int[] array, int from, int to, int depthRemaining)
        {
            if (to - from <= THRESHOLD)
            {
                InsertionSort(array, from, to);
            }
            else
            {
                int pivot = from + (to - from) / 2;
                pivot = Partition(array, from, to, pivot);

                if (depthRemaining > 0)
                {
                    try
                    {
                        Parallel.Invoke(
                          () => Parallel_QuickSort(array, from, pivot, depthRemaining - 1),
                          () => Parallel_QuickSort(array, pivot + 1, to, depthRemaining - 1));
                    }
                    catch (AggregateException ae)
                    {
                        foreach (var e in ae.InnerExceptions)
                        {
                            // Handle the custom exception.
                            if (e is CustomException)
                            {
                                Console.WriteLine(e.Message);
                            }
                            // Rethrow any other exception.
                            else
                            {
                                throw;
                            }
                        }
                    }
                }

                else
                {
                    try
                    {

                        Parallel_QuickSort(array, from, pivot, 0);
                        Parallel_QuickSort(array, pivot + 1, to, 0);
                    }
                    catch (AggregateException ae)
                    {
                        foreach (var e in ae.InnerExceptions)
                        {
                            // Handle the custom exception.
                            if (e is CustomException)
                            {
                                Console.WriteLine(e.Message);
                            }
                            // Rethrow any other exception.
                            else
                            {
                                throw;
                            }
                        }
                    }
                }

            }
        }

        private static int Partition(int[] input, int low, int high, int pivot)
        {
            pivot = 0;
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (input[j] <= pivot)
                {
                    i++;
                    swap(input, i, j);
                }
            }
            swap(input, i + 1, high);
            return i + 1;
        }

        private static void ProcessCalc_QuickSort(int[] input, int low, int high)
        {
            // ,2) + 4) if you want to do 16x the processor count, depending on system.
            Parallel_QuickSort(input, low, high, (int)Math.Log(Environment.ProcessorCount));
        }

        private static void swap(int[] ar, int a, int b)
        {

            int temp = ar[a];
            ar[a] = ar[b];
            ar[b] = temp;

        }

        public class CustomException : Exception
        {
            public CustomException(String message) : base(message)
            { }
        }

        static void Main(string[] args)
        {
            Random randNum = new Random();
            int max = randNum.Next(1000, 1000);
            int[] random = new int[max];
            Console.WriteLine("Nr of elements: " + max);
            Statistics(max);

            Console.ReadLine();
        }

        public static void Statistics(int max)
        {
            int[] random = GenerateRandom(max);
            int[] nearly = GenerateNearly(max);
            int[] few = GenerateFewUnique(max);
            int[] reversed = GenerateReversed(max);

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Input:   \tRand\tNear\tFew\tRev");

            Console.Write("Mergesort:\t");
            int[] merge = random;
            stopwatch.Start();
            ProcessCalc_MergeSort(merge, 0, max - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            merge = nearly;
            stopwatch.Start();
            ProcessCalc_MergeSort(merge, 0, max - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            merge = few;
            stopwatch.Start();
            ProcessCalc_MergeSort(merge, 0, max - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            merge = reversed;
            stopwatch.Start();
            ProcessCalc_MergeSort(merge, 0, max - 1);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();


            Console.Write("Quicksort:\t");
            int[] oquick = random;
            stopwatch.Start();
            ProcessCalc_QuickSort(oquick, 0, oquick.Length - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            oquick = nearly;
            stopwatch.Start();
            ProcessCalc_QuickSort(oquick, 0, oquick.Length - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            oquick = few;
            stopwatch.Start();
            ProcessCalc_QuickSort(oquick, 0, oquick.Length - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            oquick = reversed;
            stopwatch.Start();
            ProcessCalc_QuickSort(oquick, 0, oquick.Length - 1);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

        }

        public static int[] GenerateRandom(int size)
        {
            int min = 0;
            int max = 500;
            int[] test2 = new int[size];

            Random randNum = new Random();
            for (int i = 0; i < test2.Length; i++)
            {
                test2[i] = randNum.Next(min, max);
            }
            return test2;
        }

        public static int[] GenerateNearly(int size)
        {
            Random randNum = new Random();
            int[] arr = new int[size];
            for (int i = 0; i < size; ++i)
            {
                if (i % 4 == 0)
                {
                    arr[i] = randNum.Next(0, 500);
                }
                else
                {
                    arr[i] = i;
                }

            }
            return arr;
        }

        public static int[] GenerateFewUnique(int size)
        {
            Random randNum = new Random();
            int[] arr = new int[size];
            for (int i = 0; i < size; ++i)
            {
                arr[i] = randNum.Next(0, size / 4);
            }
            return arr;
        }
        public static int[] GenerateReversed(int size)
        {
            int[] arr = new int[size];
            for (int i = 0; i < size; ++i)
            {
                arr[i] = size - i;
            }
            return arr;
        }
    }
}
