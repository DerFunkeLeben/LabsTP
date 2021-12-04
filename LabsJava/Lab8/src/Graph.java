public abstract class Graph {

    Stack<Character> currentPath_stack = new Stack<Character>();
    protected final int N = 26;
    protected final int ASCII_SHIFT = 65;

    public Graph(String verts, String edges) {
        FromString(verts + ';' + edges);
    }

    public String[] FindAllPaths(char start, char finish, int[][] AdjacencyMatrix) {
        Stack<String> allPaths = new Stack<String>();
        boolean[] visits = new boolean[N];
        for (int i = 0; i < N; i++)
            visits[i] = false;

        currentPath_stack.Push(start);
        visits[start - ASCII_SHIFT] = true;
        int startSearch = 0;

        while (!currentPath_stack.IsEmpty()) {
            
            int curr = currentPath_stack.Peek() - ASCII_SHIFT;
            char next = FindConnectable(curr, visits, startSearch);

            if (next == finish) {
                String path = GetPath(currentPath_stack, finish);
                allPaths.Push(path);
            } else if (next != '!') {
                currentPath_stack.Push(next);
                visits[next - ASCII_SHIFT] = true;
                startSearch = 0;
            }

            if (next == '!' || next == finish) {
                startSearch = curr + 1;
                visits[curr] = false;
                currentPath_stack.Pop();
            }
        }
        return allPaths.ToArray();
    }

    private String GetPath(Stack<Character> stack, char finish) {
        String path = "";
        Stack<Character> temp = stack.Reverse();
        while (!temp.IsEmpty())
            path += temp.Pop();
        path += finish;
        return path;
    }

    public abstract char FindConnectable(int curr, boolean[] visits, int startSearch);

    public abstract String[] FindValidPaths(String[] allPaths, char[] AppropriateVertexes);

    public abstract void FromString(String data);

}
