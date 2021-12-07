using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Test2
{
    public class MD5
    {
        public static string CheckSum(string path)
        {
            var dir = new DirectoryInfo(path);
            var otherdir = dir.GetDirectories();
            var files = dir.GetFiles();
            var md5 = new MD5CryptoServiceProvider();
            string hash = BitConverter.ToString(md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(dir.Name)));
            hash += MD5Single(otherdir);
            return hash;
        }
        
        public static string CheckSumMulti(string path)
        {
            var dir = new DirectoryInfo(path);
            var otherdir = dir.GetDirectories();
            var files = dir.GetFiles();
            var md5 = new MD5CryptoServiceProvider();
            string hash = BitConverter.ToString(md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(dir.Name)));
            hash += MD5Multi(otherdir);
            return hash;
        }

        private static string MD5Single(DirectoryInfo[] dirs)
        {
            var md5 = new MD5CryptoServiceProvider();
            string hash = null;
            foreach (var d in dirs)
            {
                var byteDirHash = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(d.Name));
                hash += BitConverter.ToString(byteDirHash);
                var files = d.GetFiles();
                foreach (var f in files)
                {
                    hash += FileHash(f, md5);
                }
                MD5Single(d.GetDirectories());
            }
            return hash;
        }

        private static string FileHash(FileInfo file, MD5CryptoServiceProvider md5)
        {
            var fileStream = File.OpenRead(file.FullName);
            byte[] hashByte = md5.ComputeHash(fileStream);
            return BitConverter.ToString(hashByte);
        }

        private static string FindHash(DirectoryInfo dir)
        {
            var size = dir.GetFiles().Length < Environment.ProcessorCount
                ? dir.GetFiles().Length
                : Environment.ProcessorCount;
            var threads = new Thread[size];
            var chunkSize = dir.GetFiles().Length / threads.Length + 1;
            var md5 = new MD5CryptoServiceProvider();
            var bytedir = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(dir.Name));
            var hash = BitConverter.ToString(bytedir);
            var files = dir.GetFiles();
            for (int i = 0; i < dir.GetFiles().Length; i++)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    for (var l = localI * chunkSize; l < (localI + 1) * chunkSize && l < files.Length; ++l)
                    {
                        hash += FileHash(files[l], md5);
                    }
                });
            }
            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return hash;
        }
        
        
        private static string MD5Multi(DirectoryInfo[] dirs)
        {
            string hash = null;
            for (int i = 0; i < dirs.Length; ++i)
            {
                hash += FindHash(dirs[i]);
            }

            return hash;
        }
    }
}