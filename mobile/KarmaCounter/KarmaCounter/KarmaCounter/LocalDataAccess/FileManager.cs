using System;
using System.IO;
using System.Threading.Tasks;

namespace KarmaCounter.LocalDataAccess
{
    public class FileManager
    {
        private string GetFullPath(Environment.SpecialFolder folder, string filename) => $"{Environment.GetFolderPath(folder)}/{filename}";

        public bool Exists(Environment.SpecialFolder folder, string filename) =>
            File.Exists(GetFullPath(folder, filename));

        public void CreateFile(Environment.SpecialFolder folder, string filename)
        {
            using (Stream S = File.Create(GetFullPath(folder, filename))) { }
        }

        public void CreateFileIfNotExists(Environment.SpecialFolder folder, string filename)
        {
            if (!Exists(folder, filename))
                CreateFile(folder, filename);
        }

        public async Task<string> ReadFile(Environment.SpecialFolder folder, string filename)
        {
            using (StreamReader str = new StreamReader(GetFullPath(folder, filename)))
                return await str.ReadToEndAsync();
        }

        public async Task WriteFile(Environment.SpecialFolder folder, string filename, string data, bool append)
        {
            using (StreamWriter strw = new StreamWriter(GetFullPath(folder, filename), append))
                await strw.WriteAsync(data);
        }
    }
}
