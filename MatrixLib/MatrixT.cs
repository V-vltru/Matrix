namespace MatrixLib
{
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    public class MatrixT  <T>
    {
        public static bool Paral;
        public T[,] elements;
        public int rows, cols;
        public int Length;

        public T this[int i, int j] //индексатор
        {
            get
            {
                return elements[i, j];
            }
            set
            {
                elements[i, j] = value;
            }
        }

        public MatrixT(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            elements = new T[this.rows, this.cols];
            Length = rows * cols;
        }
        public MatrixT(T[,] _elements)
        {
            this.elements = _elements;

            this.rows = this.elements.GetLength(0);
            this.cols = this.elements.GetLength(1);
        }

        // Перегружаем бинарный оператор +
        public static MatrixT<T> operator +(MatrixT<T> A, MatrixT<T> B)
        {
            if (Paral)
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.elements.GetLength(0), A.elements.GetLength(1)]);
                Parallel.For(0, A.elements.GetLength(0), (i) =>
                {
                    for (int j = 0; j < A.elements.GetLength(1); j++)
                    {
                        ans.elements[i, j] = (dynamic)(A.elements[i, j]) + (dynamic)(B.elements[i, j]);
                    }
                });
                return ans;
            }
            else
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.elements.GetLength(0), A.elements.GetLength(1)]);
                for (int i = 0; i < A.elements.GetLength(0); i++)
                {
                    for (int j = 0; j < A.elements.GetLength(1); j++)
                    {
                        ans.elements[i, j] = (dynamic)(A.elements[i, j]) + (dynamic)(B.elements[i, j]);
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
                MatrixT<T> ans = new MatrixT<T>(new T[A.elements.GetLength(0), B.elements.GetLength(1)]);
                Parallel.For(0, A.elements.GetLength(0), (i) =>
                {
                    for (int j = 0; j < B.elements.GetLength(1); j++)
                    {
                        ans.elements[i, j] = (dynamic)0;
                        for (int k = 0; k < A.elements.GetLength(1); k++)
                        {
                            ans.elements[i, j] += (dynamic)A.elements[i, k] * (dynamic)B.elements[k, j];
                        }
                    }
                });
                return ans;
            }
            else
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.elements.GetLength(0), B.elements.GetLength(1)]);
                for (int i = 0; i < A.elements.GetLength(0); i++)
                {
                    for (int j = 0; j < B.elements.GetLength(1); j++)
                    {
                        ans.elements[i, j] = (dynamic)0;
                        for (int k = 0; k < A.elements.GetLength(1); k++)
                        {
                            ans.elements[i, j] += (dynamic)A.elements[i, k] * (dynamic)B.elements[k, j];
                        }
                    }
                }
                return ans;
            }
        }
    }
}
