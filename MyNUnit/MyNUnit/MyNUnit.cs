using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyNUnit
{
    public class MyNUnit
    {
        public void RunTests(string path)
        {
            var task = new Task<List<string>>[Directory.GetFiles(path, "*.dll").Length];
            for (int i = 0; i < task.Length; i++)
            {
                    
            }
        }
    }
}