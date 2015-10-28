using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NewNameMP3Files.Properties;
using TagLib;
using File = TagLib.File;

namespace NewNameMP3Files.Code
{
    class StaticMethods
    {
        /// <summary>
        ///     Получение максимального и минимального битрейта в директории
        /// </summary>
        /// <param name="bitrates">Список битрейтов в директории</param>
        /// <returns>Строка с мин-макс битрейтами</returns>
        public static string GetMaxAndMinBitrate(IList<int> bitrates)
        {
            string bitrate;
            var min = bitrates[0];
            var max = bitrates[0];
            foreach (var i in bitrates)
            {
                if (i < min)
                    min = i;
                if (i > max)
                {
                    max = i;
                }
            }
            if (max != min)
            {
                bitrate = min + " - " + max;
            }
            else
                bitrate = max.ToString();
            return bitrate;
        }

        public static string StringToUp(IList<string> wordsInTag)
        {
            string newTag = null;
            for (var i = 0; i < wordsInTag.Count(); i++)
            {
                var firstSymbol = wordsInTag[i][0].ToString().ToUpper();
                firstSymbol += wordsInTag[i].Substring(1);
                newTag += firstSymbol;
                if (i < wordsInTag.Count() - 1)
                {
                    newTag += " ";
                }
            }

            return newTag;
        }

        /// <summary>
        ///     Изменяет слова в тегах к верхнему регистру
        /// </summary>
        /// <param name="mp3File">Изменяемый файл</param>
        public static void TagsToUpper(File mp3File)
        {
            var file = mp3File.Tag;

            if (file.FirstPerformer != null)
            {
                var wordsInTag = file.FirstPerformer.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                file.Performers[0] = StringToUp(wordsInTag);
            }

            if (file.Title != null)
            {
                var wordsInTag = file.Title.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                file.Title = StringToUp(wordsInTag);
            }

            if (file.Album != null)
            {
                var wordsInTag = file.Album.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                file.Album = StringToUp(wordsInTag);
            }

            if (file.Genres.Count() != 0)
            {
                var wordsInTag = file.Genres[0].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                file.Genres[0] = StringToUp(wordsInTag);
            }

            mp3File.Save();
        }

        /// <summary>
        ///     Удаляет запрещенные сиволы в имени файла
        /// </summary>
        /// <param name="tempName">Имя файла</param>
        /// <returns></returns>
        public static string DeleteBannedSymbols(string tempName)
        {
            if (tempName == null)
            {
                return "";
            }

            try
            {
                var invalidFileNameChars = Path.GetInvalidFileNameChars();

                for (var j = 0; j < invalidFileNameChars.Count(); j++)
                {
                    if (!tempName.Contains(invalidFileNameChars[j]))
                        continue;
                    if (tempName[tempName.Length - 1] == invalidFileNameChars[j])
                    {
                        tempName = tempName.Substring(0, tempName.Length - 1);
                    }

                    tempName = tempName.Replace(invalidFileNameChars[j].ToString(), invalidFileNameChars[j] == ':' ? " -" : " - ");
                }
                return tempName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + ex.StackTrace);
                return "";
            }
        }

        /// <summary>
        ///     Функция, выводящая сообщение о том что работа сделана
        /// </summary>
        public static void WorkDoneMessage()
        {
            MessageBox.Show(Resources.Work_Done_, Resources.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     Функция изменения имени файла по шаблону
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetNewNameByTemplate(string name, Tag file)
        {
            if (name.Contains("(a)"))
            {
                var a = file.Album;
                a = DeleteBannedSymbols(a);
                name = name.Replace("(a)", a);
            }
            if (name.Contains("(y)"))
            {
                name = name.Replace("(y)", file.Year.ToString());
            }
            if (name.Contains("(t)"))
            {
                var t = file.Title;
                t = DeleteBannedSymbols(t);
                name = name.Replace("(t)", t);
            }
            if (name.Contains("(n)"))
            {
                name = (int)file.Track < 10
                    ? name.Replace("(n)", "0" + file.Track)
                    : name.Replace("(n)", file.Track.ToString());
            }
            if (name.Contains("(p)"))
            {
                var p = file.FirstPerformer;
                p = DeleteBannedSymbols(p);
                name = name.Replace("(p)", p);
            }
            return name;
        }
    }
}
