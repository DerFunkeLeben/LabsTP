class GraphMatrix extends Graph
	{
		public int[][] AdjacencyMatrix;
		char[] perVertexes;
		public GraphMatrix(String verts, String edges) {
            super(verts, edges);
			perVertexes = GetPeriphericVertexes();
		}

		public char[] getPerVertexes()
        {
			return perVertexes;
        }
		public void Clear()
		{
			AdjacencyMatrix = new int[N][N];
			for (int i = 0; i < N; i++)
				for (int j = 0; j < N; j++)
					AdjacencyMatrix[i][j] = -1;
		}

		public void FromString(String data)
		{
			Clear();
			String[] temp = data.split(";");
			String vertexesData = temp[0];
			String edgesData = temp[1];

			// char[] separators = new char[] { ' ', '\r', '\n', ',' };

			String[] vertexes = vertexesData.split(" ");
			String[] edges = edgesData.split(" ");


			for (int i = 0; i < vertexes.length; i++)
				for (int j = 0; j < vertexes.length; j++)
				{
					int row = vertexes[i].charAt(0) - ASCII_SHIFT;
					int col = vertexes[j].charAt(0) - ASCII_SHIFT;
					
					AdjacencyMatrix[row][col] = 0;
					AdjacencyMatrix[col][row] = 0;
				}

			for (String edge : edges)
			{
				int start = edge.charAt(0) - ASCII_SHIFT;
				int finish = edge.charAt(1) - ASCII_SHIFT;

				AdjacencyMatrix[start][finish] = 1;
				AdjacencyMatrix[finish][start] = 1;
			}
		}

		public String[] FindAllPaths(char start, char finish, int[][] adj)
		{
			String[] allPaths = super.FindAllPaths(start, finish, AdjacencyMatrix);
			return FindValidPaths(allPaths, perVertexes);
			//return allPaths;
		}

		public String[] FindAllPaths(char start, char finish)
		{
			return FindAllPaths(start, finish, null);
		}

		private int[][] GetDistMatrix()
        {
			int[][] DistMatrix = AdjacencyMatrix.clone();

			for (int k = 0; k < N; ++k)
				for (int i = 0; i < N; ++i)
					if (DistMatrix[k][i] == 0 && i != k) 
						DistMatrix[k][i] = N + 1;

			for (int k = 0; k < N; ++k)
				for (int i = 0; i < N; ++i)
					for (int j = 0; j < N; ++j)
						if (DistMatrix[i][k] != -1 && DistMatrix[k][j] != -1 && DistMatrix[i][j] != -1)
							DistMatrix[i][j] = Math.min(DistMatrix[i][j],
													DistMatrix[i][k] + DistMatrix[k][j]);

			return DistMatrix;
		}

		private int[] GetEccentricity()
        {
			int[][] DistMatrix = GetDistMatrix();
			int[] ecc = new int[N];
			
			for (int i = 0; i < N; ++i)
            {
				int max = 0;
				for (int j = 0; j < N; ++j)
					if (DistMatrix[i][j] > max && DistMatrix[i][j] <= N)
						max = DistMatrix[i][j];
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
					
		public char FindConnectable(int curr, boolean[] visits, int startSearch)
		{
			for (int i = startSearch; i < N; i++)
			{
				if (AdjacencyMatrix[curr][i] != 1) continue;
				if (visits[i]) continue;
				return (char)(i + ASCII_SHIFT);
			}
			return '!';
		}

		public String[] FindValidPaths(String[] allPaths, char[] PerVertexes)
		{
			Stack<String> PathsWithCondition = new Stack<String>();

			for (String path : allPaths) {
				for (char perVertex : PerVertexes) 
					if (path.indexOf(perVertex)!= -1)
						{
							PathsWithCondition.Push(path);
							break;
						}
				
			}
			
			return PathsWithCondition.ToArray();
		}
	}