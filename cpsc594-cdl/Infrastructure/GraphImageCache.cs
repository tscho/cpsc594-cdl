using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.IO;

namespace cpsc594_cdl.Infrastructure
{
    public class ChartImageCache
    {
        private const int MAX_AGE = 3;
        private static ChartImageCache instance;

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
            //cacheDir = System.Configuration.ConfigurationManager.AppSettings["ChartCacheDir"];
            //cleanTimer = new Timer(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["ChartCacheCleanIntervalSeconds"]) * 1000);
            cacheDir = @"Content\cache";
            cleanTimer = new Timer(30000);
            cleanTimer.Elapsed += this.CleanCache;
            cleanTimer.Start();
        }

        private void CleanCache(Object sender, ElapsedEventArgs args)
        {
            lock (cacheAccessLock)
            {
                foreach (var cacheItem in cachedFileAge.Keys)
                {
                    cachedFileAge[cacheItem] += 1;
                    if (cachedFileAge[cacheItem] >= MAX_AGE)
                        RemoveCacheItem(cacheItem);
                }
            }
        }

        private void RemoveCacheItem(string cacheItem)
        {
            cachedFileAge.Remove(cacheItem);
            File.Delete(Path.Combine(cacheDir, cacheItem));
        }

        public string SaveChartImage(string cacheKey, System.Web.UI.DataVisualization.Charting.Chart chart)
        {
            string relativepath_filename = Path.Combine(cacheDir, cacheKey + ".png");
            string fullpath_filename = Path.Combine(HttpRuntime.AppDomainAppPath, relativepath_filename);

            lock (cacheAccessLock)
            {
                if (cachedFileAge[cacheKey] == null)
                {
                    using (FileStream imageStream = new FileStream(fullpath_filename, FileMode.OpenOrCreate))
                    {
                        chart.SaveImage(imageStream, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                    }
                }

                cachedFileAge[cacheKey] = 0;
                return relativepath_filename;
            }
        }
    }
}