using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication
{
    public class CsvReader<T>
    {
        public static List<T> MapFromFile(string file, char delimitter, Func<string[], T> map, bool skipFirst = false)
        {
            var objects = new List<T>();

            foreach (var line in File.ReadLines(file))
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }

                var columns = line.Split(delimitter);
                objects.Add(map(columns));
            }

            return objects;
        }
    }
}