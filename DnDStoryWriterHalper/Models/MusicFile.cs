using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DnDStoryWriterHalper.Models
{
    public class MusicFile
    {
        public static string BaseFilePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "/Musics/";
        public string FileName { get; set; }
        public ObservableCollection<string> Genres { get; set; }   

        public MusicFile(string FileName) 
        {
            var genresFile = BaseFilePath + FileName + ".txt";
            if (File.Exists(genresFile))
            {
                Genres = new ObservableCollection<string>();
                foreach(var genre in File.ReadAllLines(genresFile))
                {
                    Genres.Add(genre.Trim());
                }
            }
        }
    }
}
