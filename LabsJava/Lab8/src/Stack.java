import java.util.Arrays;

public class Stack<T> {
    Node<T> head;
    int count;
    private IllegalArgumentException err411 = new IllegalArgumentException("Error 411: Stack is empty");

    public boolean IsEmpty() {
        return count == 0;
    }

    public int Count() {
        return count;
    }

    public void Push(T item) {
        Node<T> node = new Node<T>(item);
        node.next = head;
        head = node;
        count++;
    }

    public T Pop() throws IllegalArgumentException {
        if (IsEmpty())
            throw err411;
        Node<T> temp = head;
        head = head.next;
        count--;
        return temp.value;
    }

    public Stack<T> Reverse() {
        Stack<T> copy = new Stack<T>();
        Node<T> node = head;
        for (; node != null; node = node.next)
            copy.Push(node.value);
        return copy;
    }

    public String[] ToArray() {
        @SuppressWarnings("unchecked")
        T[] arr = (T[]) new Object[count];
        int i = 0;
        Node<T> node = head;
        for (; node != null; node = node.next, i++)
            arr[i] = node.value;

        String[] stringArray = Arrays.copyOf(arr, arr.length, String[].class);
        return stringArray;
    }

    public T Peek() throws IllegalArgumentException {
        if (IsEmpty())
            throw err411;
        return head.value;
    }

}