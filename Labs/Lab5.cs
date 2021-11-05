using System;
using System.Diagnostics.Tracing;
using System.Threading;

namespace Lab5
{
    public class Queue
    {
        private string[] _array;
        private int _head;
        private int _tail;

        public Queue()
        {
            _array = new string[5];
        }

        public int Count { get; private set; }

        public string[] ToArray
        {
            get
            {
                var destinationArray = new string[Count];
                Array.Copy(_array, _head, destinationArray, 0, Count);
                return destinationArray;
            }
        }

        public string Pop()
        {
            if (Count == 0)
                return "**FIFO IS EMPTY**";

            var deletedItem = _array[_head];
            _array[_head] = "";
            _head = (_head + 1) % _array.Length;
            Count--;

            return deletedItem;
        }

        public void Push(string item)
        {
            if (Count == _array.Length)
                SetCapacity(_array.Length * 2);

            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            Count++;
        }

        private void SetCapacity(int capacity)
        {
            var destinationArray = new string[capacity];
            if (Count > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, destinationArray, 0, Count);
                }
                else
                {
                    Array.Copy(_array, _head, destinationArray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, destinationArray, _array.Length - _head, _tail);
                }
            }

            _array = destinationArray;

            _head = 0;
            _tail = Count == capacity ? 0 : Count;
        }
    }

    public class Menu
    {
        public void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("\n**MENU**\n");
            Console.WriteLine("1. init new FIFO");
            Console.WriteLine("2. print FIFO");
            Console.WriteLine("3. push item");
            Console.WriteLine("4. pop item");
            Console.WriteLine("5. exit\n");

            Console.ResetColor();
        }

        public void Init(Queue fifo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n**CREATING NEW FIFO**\n");
            Console.WriteLine("Input items. To finish - input keyword stop \n");

            while (true)
            {
                var item = Console.ReadLine();
                if (item == "stop")
                    break;
                fifo.Push(item);
            }

            Console.WriteLine("\nNew FIFO has been successfully created\n");
            Thread.Sleep(3000);
            Console.ResetColor();
        }

        public void Print(Queue fifo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n**PRINT FIFO**");
            var size = fifo.Count;
            if (size == 0)
            {
                Console.WriteLine("\n*---EMPTY---*");
                Thread.Sleep(3000);
                Console.ResetColor();
                return;
            }

            var queue = fifo.ToArray;
            Console.WriteLine("\n*---HEAD---*");

            foreach (var item in queue)
                Console.WriteLine(item);

            Console.WriteLine("*---TAIL---*\n");

            Thread.Sleep(3000);
            Console.ResetColor();
        }

        public void Push(Queue fifo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n**PUSH ITEM**\n");
            Console.WriteLine("Input item you wanna push\n");

            var item = Console.ReadLine();

            fifo.Push(item);

            Console.WriteLine("\n**SUCCESS**\n");
            Thread.Sleep(3000);
            Console.ResetColor();
        }

        public void Pop(Queue fifo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            var deletedItem = fifo.Pop();
            Console.WriteLine(deletedItem == "**FIFO IS EMPTY**"
                ? deletedItem
                : $"\n**ITEM '{deletedItem}' HAS BEEN POPPED**\n");

            Thread.Sleep(3000);
            Console.ResetColor();
        }
    }

    internal class Program
    {
        private static void Main()
        {
            var fifo = new Queue();
            var running = true;
            while (running)
            {
                var menu = new Menu();
                menu.ShowMenu();

                var option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        menu.Init(fifo);
                        break;
                    case 2:
                        menu.Print(fifo);
                        break;
                    case 3:
                        menu.Push(fifo);
                        break;
                    case 4:
                        menu.Pop(fifo);
                        break;
                    case 5:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("ERROR - INCORRECT OPTION");
                        break;
                }
            }
        }
    }
}