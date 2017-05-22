using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Matrix<T> : IEnumerable<KeyValuePair<int, T[]>>
    {
        private T[,] _matrix;
        private Dictionary<int, int> _lookupA = new Dictionary<int, int>();
        private Dictionary<int, int> _lookupB = new Dictionary<int, int>();
        private int _currA = 0;
        private int _currB = 0;

        public int SizeX { get; }
        public int SizeY { get; }

        public Matrix(int sizeX, int sizeY)
        {
            _matrix = new T[sizeX, sizeY];
            SizeX = sizeX;
            SizeY = sizeY;
        }



        public void ReplaceDefault(T value)
        {
            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
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
                var row = new T[SizeY];
                for (var i = 0; i < SizeY; i++)
                {
                    row[i] = _matrix[_lookupA[a], i];
                }

                return row;
            }
        }

        public IEnumerator<KeyValuePair<int, T[]>> GetEnumerator()
        {
            foreach (var look in _lookupA)
            {
                yield return new KeyValuePair<int, T[]>(look.Key, this[look.Key]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}