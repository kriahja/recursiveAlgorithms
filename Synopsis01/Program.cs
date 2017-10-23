using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopsis01
{
    class Program
    {
        static public void MainMerge(int[] numbers, int left, int mid, int right)
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

        static public void SortMerge(int[] numbers, int left, int right, int depthRemaining)
        {
            int mid;

            if (right > left)
            {
                mid = (right + left) / 2;
                if(depthRemaining > 0)
                {
                    Parallel.Invoke(
                    () => SortMerge(numbers, left, mid, depthRemaining - 1), 
                    () => SortMerge(numbers, (mid + 1), right, depthRemaining - 1));
                }
                else
                {
                    SortMerge(numbers, left, mid, 0);
                    SortMerge(numbers, (mid + 1), right, 0);
                }
                

                MainMerge(numbers, left, (mid + 1), right);
            }
        }
        static public void SortMerge(int[] numbers, int left, int right)
        {
            SortMerge(numbers, left, right, (int)Math.Log(Environment.ProcessorCount, 2)+4);
        }

        static public void InsertSort(int[] numarray, int max)
        {
            for (int i = 1; i < max; i++)
            {
                int j = i;
                while (j > 0)
                {
                    if (numarray[j - 1] > numarray[j])
                    {
                        int temp = numarray[j - 1];
                        numarray[j - 1] = numarray[j];
                        numarray[j] = temp;
                        j--;
                    }
                    else
                        break;
                }
            }
        }

        public static void insertionSortRecursive(int[] arr, int n)
        {
            // Base case
            if (n <= 1)
                return;

            // Sort first n-1 elements
            insertionSortRecursive(arr, n - 1);

            // Insert last element at its correct position
            // in sorted array.
            int last = arr[n - 1];
            int j = n - 2;

            /* Move elements of arr[0..i-1], that are
              greater than key, to one position ahead
              of their current position */
            while (j >= 0 && arr[j] > last)
            {
                arr[j + 1] = arr[j];
                j--;
            }
            arr[j + 1] = last;
        }

        public static void BubbleSort(int[] number)
        {
            bool flag = true;
            int temp;
            int numLength = number.Length;
            //sorting an array
            for (int i = 1; (i <= (numLength - 1)) && flag; i++)
            {
                flag = false;
                for (int j = 0; j < (numLength - 1); j++)
                {
                    if (number[j + 1] < number[j])
                    {
                        temp = number[j];
                        number[j] = number[j + 1];
                        number[j + 1] = temp;
                        flag = true;
                    }
                }
            }
        }

        public static void RecursiveBubbleSort(int[] arr, int n)
        {
            // Base case
            if (n == 1)
                return;

            // One pass of bubble sort. After
            // this pass, the largest element
            // is moved (or bubbled) to end.
            for (int i = 0; i < n - 1; i++)
                if (arr[i] > arr[i + 1])
                {
                    int temp = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = temp;
                }

            // Largest element is fixed,
            // recur for remaining array
            RecursiveBubbleSort(arr, n - 1);
        }

        private static void Quicksort(int[] input, int low, int high, int depthRemaining)
        {
            int pivot_loc = 0;

            if (low < high)
            {
                pivot_loc = partition(input, low, high);
                if (depthRemaining > 0)
                {
                    //Tasks are created dynamically with recursive call;
                    //if the array is large, many tasks might be created.
                    Parallel.Invoke(
                    () => Quicksort(input, low, pivot_loc - 1, depthRemaining - 1),
                    () => Quicksort(input, pivot_loc + 1, high, depthRemaining - 1));
                }
                else
                {
                    Quicksort(input, low, pivot_loc - 1, 0);
                    Quicksort(input, pivot_loc + 1, high, 0);
                }
            }
        }
        private static void Quicksort(int[] input, int low, int high)
        {
            Quicksort(input, low, high, (int)Math.Log(Environment.ProcessorCount));
        }

        private static int partition(int[] input, int low, int high)
        {
            int pivot = input[high];
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



        private static void swap(int[] ar, int a, int b)
        {
            int temp = ar[a];
            ar[a] = ar[b];
            ar[b] = temp;
        }

        public static int[] CountingSort(int[] array)
        {
            int[] sortedArray = new int[array.Length];

            // find smallest and largest value
            int minVal = array[0];
            int maxVal = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < minVal) minVal = array[i];
                else if (array[i] > maxVal) maxVal = array[i];
            }

            // init array of frequencies
            int[] counts = new int[maxVal - minVal + 1];

            // init the frequencies
            for (int i = 0; i < array.Length; i++)
            {
                counts[array[i] - minVal]++;
            }

            // recalculate
            counts[0]--;
            for (int i = 1; i < counts.Length; i++)
            {
                counts[i] = counts[i] + counts[i - 1];
            }

            // Sort the array
            for (int i = array.Length - 1; i >= 0; i--)
            {
                sortedArray[counts[array[i] - minVal]--] = array[i];
            }

            return sortedArray;
        }

        public static void heapify(int[] arr, int n, int i)
        {
            int largest = i;  // Initialize largest as root
            int l = 2 * i + 1;  // left = 2*i + 1
            int r = 2 * i + 2;  // right = 2*i + 2

            // If left child is larger than root
            if (l < n && arr[l] > arr[largest])
                largest = l;

            // If right child is larger than largest so far
            if (r < n && arr[r] > arr[largest])
                largest = r;

            // If largest is not root
            if (largest != i)
            {
                int swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;

                // Recursively heapify the affected sub-tree
                heapify(arr, n, largest);
            }
        }

        // main function to do heap sort
        public static void heapSort(int[] arr, int n)
        {
            // Build heap (rearrange array)
            for (int i = n / 2 - 1; i >= 0; i--)
                heapify(arr, n, i);

            // One by one extract an element from heap
            for (int i = n - 1; i >= 0; i--)
            {
                // Move current root to end
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;

                // call max heapify on the reduced heap
                heapify(arr, i, 0);
            }
        }

        static void Main(string[] args)
        {
            Random randNum = new Random();
            int max = randNum.Next(20, 50);
            int[] random = new int[max];
            Console.WriteLine("Nr of elements: " + max);
            Statistics(max);

            /*random = GenerateRandom(max);
            for (int i = 0; i < max; ++i)
            {
                Console.WriteLine(random[i]);
            }

            int[] numbers = new int[max];
            numbers = random;
           
            int[] merge = numbers;
            Console.WriteLine("\nMergeSort By Recursive Method:");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SortMerge(merge, 0, max - 1);
            stopwatch.Stop();
            for (int i = 0; i < max; i++)
                Console.WriteLine(merge[i]);
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            stopwatch.Reset();

            int[] insertion = numbers;
            stopwatch.Start();
            InsertSort(insertion, max);
            stopwatch.Stop();
            Console.Write("\nInsertionSort:\n");
            for (int i = 0; i < max; i++)
            { 
                Console.Write(insertion[i]);
                Console.Write("\n");
            }
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            stopwatch.Reset();

            int[] recinsertion = numbers;
            stopwatch.Start();
            insertionSortRecursive(recinsertion, max);
            stopwatch.Stop();
            Console.Write("\nRecursiveInsertionSort:\n");
            for (int i = 0; i < max; i++)
            {
                Console.Write(recinsertion[i]);
                Console.Write("\n");
            }
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            stopwatch.Reset();

            int[] bubble = numbers;
            stopwatch.Start();
            BubbleSort(bubble);
            stopwatch.Stop();
            Console.WriteLine("BubbleSort:");
            for(int i = 0; i < max; i++)
            {
                Console.WriteLine(bubble[i]);
            }
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            stopwatch.Reset();

            int[] recbubble = numbers;
            stopwatch.Start();
            RecursiveBubbleSort(recbubble, max);
            stopwatch.Stop();
            Console.WriteLine("RecursiveBubbleSort:");
            for (int i = 0; i < max; i++)
            {
                Console.WriteLine(recbubble[i]);
            }
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            stopwatch.Reset();

            int[] quick = numbers;
            Console.WriteLine("QuickSort By Recursive Method");
            stopwatch.Start();
            Quicksort(quick, 0, quick.Length - 1);
            stopwatch.Stop();
            for (int i = 0; i < max; i++)
                Console.WriteLine(quick[i]);
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            stopwatch.Reset();

            Console.WriteLine("CountingSort:");
            stopwatch.Start();
            int[] counting = CountingSort(numbers);
            stopwatch.Stop();
            for (int i = 0; i < max; i++)
                Console.WriteLine(counting[i]);
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            stopwatch.Reset();

            Console.WriteLine("HeapSort:");
            stopwatch.Start();
            int[] heap = numbers;

            int n = heap.Length;

            heapSort(heap, n);
            for (int i = 0; i < max; i++)
                Console.WriteLine(heap[i]);
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            stopwatch.Reset();

            int[] reversed = GenerateReversed(max);
            for(int i = 0; i < max; i++)
            {
                Console.WriteLine(reversed[i]);
            }

            int[] nearly = GenerateNearly(max);
            for (int i = 0; i < max; i++)
            {
                Console.WriteLine(nearly[i]);
            }

            int[] few = GenerateFewUnique(max);
            for (int i = 0; i < max; i++)
            {
                Console.WriteLine(few[i]);
            }

            */

            Console.ReadLine();
        }

        public static void Statistics(int max)
        {
            int[] random = GenerateRandom(max);
            int[] nearly = GenerateNearly(max);
            int[] few = GenerateFewUnique(max);
            int[] reversed = GenerateReversed(max);

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Iput:   \tRand\tNear\tFew\tRev");

            Console.Write("Merge Sort:\t");
            int[] merge = random;
            stopwatch.Start();
            SortMerge(merge, 0, max - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            merge = nearly;
            stopwatch.Start();
            SortMerge(merge, 0, max - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            merge = few;
            stopwatch.Start();
            SortMerge(merge, 0, max - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            merge = reversed;
            stopwatch.Start();
            SortMerge(merge, 0, max - 1);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();


            Console.Write("Insertion:\t");
            int[] ins = random;
            stopwatch.Start();
            InsertSort(ins, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            ins = nearly;
            stopwatch.Start();
            InsertSort(ins, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            ins = few;
            stopwatch.Start();
            InsertSort(ins, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            ins = reversed;
            stopwatch.Start();
            InsertSort(ins, max);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();


            Console.Write("RecInsert:\t");
            int[] recins = random;
            stopwatch.Start();
            insertionSortRecursive(recins, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            recins = nearly;
            stopwatch.Start();
            insertionSortRecursive(recins, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            recins = few;
            stopwatch.Start();
            insertionSortRecursive(recins, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            recins = reversed;
            stopwatch.Start();
            insertionSortRecursive(recins, max);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            Console.Write("BubbleSort:\t");
            int[] bubble = random;
            stopwatch.Start();
            BubbleSort(bubble);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            bubble = nearly;
            stopwatch.Start();
            BubbleSort(bubble);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            bubble = few;
            stopwatch.Start();
            BubbleSort(bubble);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            bubble = reversed;
            stopwatch.Start();
            BubbleSort(bubble);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            Console.Write("RecBubble:\t");
            int[] recb = random;
            stopwatch.Start();
            RecursiveBubbleSort(recb, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            recb = nearly;
            stopwatch.Start();
            RecursiveBubbleSort(recb, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            recb = few;
            stopwatch.Start();
            RecursiveBubbleSort(recb, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            recb = reversed;
            stopwatch.Start();
            RecursiveBubbleSort(recb, max);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            Console.Write("QuickSort:\t");
            int[] quick = random;
            stopwatch.Start();
            Quicksort(quick, 0, quick.Length - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            quick = nearly;
            stopwatch.Start();
            Quicksort(quick, 0, quick.Length - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            quick = few;
            stopwatch.Start();
            Quicksort(quick, 0, quick.Length - 1);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            quick = reversed;
            stopwatch.Start();
            Quicksort(quick, 0, quick.Length - 1);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            Console.Write("HeapSort:\t");
            int[] heap = random;
            stopwatch.Start();
            heapSort(heap, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            heap = nearly;
            stopwatch.Start();
            heapSort(heap, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            heap = few;
            stopwatch.Start();
            heapSort(heap, max);
            stopwatch.Stop();
            Console.Write(stopwatch.Elapsed.Ticks + "\t");
            stopwatch.Reset();

            heap = reversed;
            stopwatch.Start();
            heapSort(heap, max);
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
            for(int i = 0; i < size; ++i)
            {
                if(i % 4 == 0)
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
            for(int i = 0; i < size; ++i)
            {
                arr[i] = randNum.Next(0, size / 4);
            }
            return arr;
        }
        public static int[] GenerateReversed(int size)
        {
            int[] arr = new int[size];
            for(int i = 0; i < size; ++i)
            {
                arr[i] = size - i;
            }
            return arr;
        }
    }
}
