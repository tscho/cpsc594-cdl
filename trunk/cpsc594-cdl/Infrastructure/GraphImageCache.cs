using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.IO;
using System.Configuration;
using System.Drawing;

namespace cpsc594_cdl.Infrastructure
{
    public class ChartImageCache
    {
        private static ChartImageCache instance;

        private int MAX_AGE;
        private string cacheDir;
        private Timer cleanTimer;
        private Dictionary<string, int> cachedFileAge;
        private object cacheAccessLock;

        public static ChartImageCache GetImageCache()
        {
            if(instance == null)
                instance = new ChartImageCache();

            return instance;
        }

        private ChartImageCache()
        {
            cacheDir = ConfigurationManager.AppSettings["ChartCacheDir"];

            cachedFileAge = new Dictionary<string, int>();

            cacheAccessLock = new object();

            ClearCache();

            MAX_AGE = Convert.ToInt32(ConfigurationManager.AppSettings["ChartCacheItemMaxAge"]);

            cleanTimer = new Timer(Convert.ToDouble(ConfigurationManager.AppSettings["ChartCacheCleanIntervalMinutes"]) * 1000 * 60);
            cleanTimer.Elapsed += this.CleanCache;
            cleanTimer.Start();
        }

        private void ClearCache()
        {
            foreach (var file in Directory.EnumerateFiles(Path.Combine(HttpRuntime.AppDomainAppPath, cacheDir)))
            {
                File.Delete(file);
            }
        }

        private void CleanCache(Object sender, ElapsedEventArgs args)
        {
            cleanTimer.Stop();
            lock (cacheAccessLock)
            {
                string[] keys = cachedFileAge.Keys.ToArray();
                foreach (var cacheItemKey in keys)
                {
                    cachedFileAge[cacheItemKey] += 1;
                    if (cachedFileAge[cacheItemKey] >= MAX_AGE)
                        RemoveCacheItem(cacheItemKey);
                }
            }
            cleanTimer.Start();
        }

        private void RemoveCacheItem(string cacheItem)
        {
            cachedFileAge.Remove(cacheItem);
            File.Delete(Path.Combine(HttpRuntime.AppDomainAppPath, cacheDir, cacheItem + ".png"));
        }

        public string SaveChartImage(string cacheKey, System.Web.UI.DataVisualization.Charting.Chart chart)
        {
            string[] colors = new string[10] { "#4572A7", "#AA4643", "#CCE8CF", "#71588F", "#4198AF", "#DB843D", "#93A9CF", "#D19392", "#B9CD96", "#A99BBD" };

            int max = (chart.Series.Count<10) ? chart.Series.Count : 10;
            for (int i=0; i<max; i++) {
                chart.Series[i].Color = ColorTranslator.FromHtml(colors[i]);
            }

            string relativepath_filename = Path.Combine(cacheDir, cacheKey + ".png");
            string fullpath_filename = Path.Combine(HttpRuntime.AppDomainAppPath, relativepath_filename);

            lock (cacheAccessLock)
            {
                if (!cachedFileAge.ContainsKey(cacheKey))
                {
                    using (FileStream imageStream = new FileStream(fullpath_filename, FileMode.OpenOrCreate))
                    {
                        chart.SaveImage(imageStream, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                    }
                }

                cachedFileAge[cacheKey] = 0;
                return relativepath_filename.Replace('\\', '/');
            }
        }
    }
}