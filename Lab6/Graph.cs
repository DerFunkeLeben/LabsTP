using System;
using System.Collections.Generic;
using System.Text;

namespace Lab6
{
	abstract class Graph
	{
		Stack<char> currentPath_stack = new Stack<char>();
		protected const int N = 26;
		protected const int ASCII_SHIFT = 65;
		public Graph(string verts, string edges)
		{
			FromString(verts + ';' + edges);
		}
		
		public virtual string[] FindAllPaths(char start, char finish, int[,] AdjacencyMatrix)
        {
			Stack<string> allPaths = new Stack<string>();
			bool[] visits = new bool[N];
			for (int i = 0; i < N; i++) visits[i] = false;

			currentPath_stack.Push(start);
			visits[start - ASCII_SHIFT] = true;
			int startSearch = 0;

			while (!currentPath_stack.IsEmpty())
			{
				char curr = (char)(currentPath_stack.Peek() - ASCII_SHIFT);
				char next = FindConnectable(curr, visits, startSearch);

				if (next == finish)
				{
					string path = GetPath(currentPath_stack, finish);
					allPaths.Push(path);
				}
				else if (next != '!')
				{
					currentPath_stack.Push(next);
					visits[next - ASCII_SHIFT] = true;
					startSearch = 0;
				}

				if (next == '!' || next == finish)
				{
					startSearch = curr + 1;
					visits[curr] = false;
					currentPath_stack.Pop();
				}
			}
			return allPaths.ToArray();
		}
		
		private string GetPath(Stack<char> stack, char finish)
        {
			string path = "";
			Stack<char> temp = stack.Reverse();
			for (int i = 0; !temp.IsEmpty(); i++)
				path += temp.Pop();
			path += finish;
			return path;
		}
		
		public abstract char FindConnectable(char curr, bool[] visits, int startSearch);
		
		public abstract string[] FindValidPaths(string[] allPaths, char[] AppropriateVertexes);

		public abstract void FromString(string data);
	}

	class GraphMatrix : Graph
	{
		public int[,] AdjacencyMatrix = new int[N, N];
		char[] perVertexes;
		public GraphMatrix(string verts, string edges) : base(verts, edges) {
			perVertexes = GetPeriphericVertexes();
		}

		public char[] PerVertexes
        {
			get { return perVertexes; }
        }
		public void Clear()
		{
			for (int i = 0; i < N; i++)
				for (int j = 0; j < N; j++)
					AdjacencyMatrix[i, j] = -1;
		}

		public override void FromString(string data)
		{
			Clear();
			string[] temp = data.Split(';');
			string vertexesData = temp[0];
			string edgesData = temp[1];

			char[] separators = new char[] { ' ', '\r', '\n', ',' };
			string[] vertexes = vertexesData.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			string[] edges = edgesData.Split(separators, StringSplitOptions.RemoveEmptyEntries);


			for (int i = 0; i < vertexes.Length; i++)
				for (int j = 0; j < vertexes.Length; j++)
				{
					int row = vertexes[i][0] - ASCII_SHIFT;
					int col = vertexes[j][0] - ASCII_SHIFT;
					
					AdjacencyMatrix[row, col] = 0;
					AdjacencyMatrix[col, row] = 0;
				}

			foreach (var edge in edges)
			{
				int start = edge[0] - ASCII_SHIFT;
				int finish = edge[1] - ASCII_SHIFT;

				AdjacencyMatrix[start, finish] = 1;
				AdjacencyMatrix[finish, start] = 1;
			}
		}

		public override string[] FindAllPaths(char start, char finish, int[,] adj = null)
		{
			string[] allPaths = base.FindAllPaths(start, finish, AdjacencyMatrix);
			return FindValidPaths(allPaths, perVertexes);
			//return allPaths;
		}

		private int[,] GetDistMatrix()
        {
			int[,] DistMatrix = new int[N, N];
			Array.Copy(AdjacencyMatrix, DistMatrix, N * N);

			for (int k = 0; k < N; ++k)
				for (int i = 0; i < N; ++i)
					if (DistMatrix[k, i] == 0 && i != k) 
						DistMatrix[k, i] = N + 1;

			for (int k = 0; k < N; ++k)
				for (int i = 0; i < N; ++i)
					for (int j = 0; j < N; ++j)
						if (DistMatrix[i, k] != -1 && DistMatrix[k, j] != -1 && DistMatrix[i, j] != -1)
							DistMatrix[i, j] = Math.Min(DistMatrix[i, j],
													DistMatrix[i, k] + DistMatrix[k, j]);

			return DistMatrix;
		}

		private int[] GetEccentricity()
        {
			int[,] DistMatrix = GetDistMatrix();
			int[] ecc = new int[N];
			
			for (int i = 0; i < N; ++i)
            {
				int max = 0;
				for (int j = 0; j < N; ++j)
					if (DistMatrix[i, j] > max && DistMatrix[i, j] <= N)
						max = DistMatrix[i, j];
				ecc[i] = max;
			}

			return ecc;

		}

		private char[] GetPeriphericVertexes()
		{
			int[] ecc = GetEccentricity();
			int max = 0;
			int PerVertexesCount = 1;
			for (int j = 0; j < N; ++j)
			{
				if (ecc[j] > max)
				{
					max = ecc[j];
					PerVertexesCount = 1;
				}
				else if (ecc[j] == max)
					PerVertexesCount++;
			}

			char[] PerVertexes = new char[PerVertexesCount];
			int index = 0;

			for (int j = 0; j < N; ++j)
				if (ecc[j] == max)
				{
					PerVertexes[index] = (char)(j + ASCII_SHIFT);
					index++;
				}
			return PerVertexes;
		}
					
		public override char FindConnectable(char curr, bool[] visits, int startSearch)
		{
			for (int i = startSearch; i < N; i++)
			{
				if (AdjacencyMatrix[curr, i] != 1) continue;
				if (visits[i]) continue;
				return (char)(i + ASCII_SHIFT);
			}
			return '!';
		}

		public override string[] FindValidPaths(string[] allPaths, char[] PerVertexes)
		{
			Stack<string> PathsWithCondition = new Stack<string>();

			foreach (string path in allPaths)
				foreach (char perVertex in PerVertexes)
					if (path.Contains(perVertex))
					{
						PathsWithCondition.Push(path);
						break;
					}

			return PathsWithCondition.ToArray();
		}
	}
}
