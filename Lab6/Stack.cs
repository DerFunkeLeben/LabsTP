using System;
using System.Collections.Generic;
using System.Text;

namespace Lab6
{
    class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }
    class Stack<T>
    {
        Node<T> head;
        int count;
        private InvalidOperationException err411 = new InvalidOperationException("Error 411: Stack is empty");
        public bool IsEmpty() => count == 0;
        public int Count() => count;
        public void Push(T item)
        {
            Node<T> node = new Node<T>(item);
            node.Next = head;
            head = node;
            count++;
        }
        public T Pop()
        {
            if (IsEmpty())
                throw err411;
            Node<T> temp = head;
            head = head.Next;
            count--;
            return temp.Data;
        }
        public T Peek() => IsEmpty() ? throw err411 : head.Data;
        public Stack<T> Reverse()
        {
            Stack<T> copy = new Stack<T>();
            Node<T> node = head;
            for (; node != null; node = node.Next)
                copy.Push(node.Data);
            return copy;
        }
        public T[] ToArray()
        {
            T[] arr = new T[count];
            int i = 0;
            Node<T> node = head;
            for(; node != null; node = node.Next, i++)
                arr[i] = node.Data;
            return arr;
        }
    }
}
