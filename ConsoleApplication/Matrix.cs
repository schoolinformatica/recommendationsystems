using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Matrix<T>
    {
        private T[,] _matrix;
        private Dictionary<int, int> _lookupA = new Dictionary<int, int>();
        private Dictionary<int, int> _lookupB = new Dictionary<int, int>();
        private int _currA = 0;
        private int _currB = 0;
        private int _sizeX;
        private int _sizeY;

        public Matrix(int sizeX, int sizeY)
        {
            _matrix = new T[sizeX, sizeY];
            _sizeX = sizeX;
            _sizeY = sizeY;
        }

        public void ReplaceDefault(T value)
        {
            for (var i = 0; i < _sizeX; i++)
            {
                for (var j = 0; j < _sizeY; j++)
                {
                    _matrix[i, j] = value;
                }
            }
        }

        public T this[int a, int b]
        {
            get { return _matrix[_lookupA[a], _lookupB[b]]; }

            set
            {
                if (!_lookupA.ContainsKey(a))
                {
                    _lookupA.Add(a, _currA);
                    _currA++;
                }


                if (!_lookupB.ContainsKey(b))
                {
                    _lookupB.Add(b, _currB);
                    _currB++;
                }
                _matrix[_lookupA[a], _lookupB[b]] = value;
            }
        }

        public T[] this[int a]
        {
            get
            {
                var row = new T[_sizeY];
                for (var i = 0; i < _sizeY; i++)
                {
                    row[i] = _matrix[_lookupA[a], i];
                }

                return row;
            }
        }
    }
}