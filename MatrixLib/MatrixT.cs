namespace MatrixLib
{
    using System.Threading.Tasks;

    public class MatrixT<T>
    {
        public static bool Paral { get; set; }

        public T[,] Elements { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public int Length { get; set; }

        public T this[int i, int j] //индексатор
        {
            get
            {
                return Elements[i, j];
            }
            set
            {
                Elements[i, j] = value;
            }
        }

        public MatrixT(int rows, int cols)
        {
            this.Rows = rows;
            this.Cols = cols;
            Elements = new T[this.Rows, this.Cols];
            Length = rows * cols;
        }

        public MatrixT(T[,] _elements)
        {
            this.Elements = _elements;

            this.Rows = this.Elements.GetLength(0);
            this.Cols = this.Elements.GetLength(1);
        }

        // Перегружаем бинарный оператор +
        public static MatrixT<T> operator +(MatrixT<T> A, MatrixT<T> B)
        {
            if (Paral)
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.Elements.GetLength(0), A.Elements.GetLength(1)]);
                Parallel.For(0, A.Elements.GetLength(0), (i) =>
                {
                    for (int j = 0; j < A.Elements.GetLength(1); j++)
                    {
                        ans.Elements[i, j] = (dynamic)(A.Elements[i, j]) + (dynamic)(B.Elements[i, j]);
                    }
                });

                return ans;
            }
            else
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.Elements.GetLength(0), A.Elements.GetLength(1)]);
                for (int i = 0; i < A.Elements.GetLength(0); i++)
                {
                    for (int j = 0; j < A.Elements.GetLength(1); j++)
                    {
                        ans.Elements[i, j] = (dynamic)(A.Elements[i, j]) + (dynamic)(B.Elements[i, j]);
                    }
                }

                return ans;
            }
        }

        // Перегружаем бинарный оператор *
        public static MatrixT<T> operator *(MatrixT<T> A, MatrixT<T> B)
        {
            if (Paral)
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.Elements.GetLength(0), B.Elements.GetLength(1)]);
                Parallel.For(0, A.Elements.GetLength(0), (i) =>
                {
                    for (int j = 0; j < B.Elements.GetLength(1); j++)
                    {
                        ans.Elements[i, j] = (dynamic)0;
                        for (int k = 0; k < A.Elements.GetLength(1); k++)
                        {
                            ans.Elements[i, j] += (dynamic)A.Elements[i, k] * (dynamic)B.Elements[k, j];
                        }
                    }
                });

                return ans;
            }
            else
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.Elements.GetLength(0), B.Elements.GetLength(1)]);
                for (int i = 0; i < A.Elements.GetLength(0); i++)
                {
                    for (int j = 0; j < B.Elements.GetLength(1); j++)
                    {
                        ans.Elements[i, j] = (dynamic)0;
                        for (int k = 0; k < A.Elements.GetLength(1); k++)
                        {
                            ans.Elements[i, j] += (dynamic)A.Elements[i, k] * (dynamic)B.Elements[k, j];
                        }
                    }
                }

                return ans;
            }
        }
    }
}
