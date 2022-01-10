using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.IO;

namespace Nmkoder.Media
{
    class GetMediaResolutionCached
    {
        public static Dictionary<QueryInfo, Size> cache = new Dictionary<QueryInfo, Size>();

        public static async Task<Size> GetSizeAsync(string path)
        {
            long filesize = IoUtils.GetFilesize(path);
            QueryInfo hash = new QueryInfo(path, filesize);

            if (filesize > 0 && CacheContains(hash))
            {
                Size s = GetFromCache(hash);
                Logger.Log($"GetMediaResolutionCached: Returned cached resolution ({s}).", true);
                return s;
            }
            else
            {
                Logger.Log($"GetMediaResolutionCached: Resolution not cached, reading resolution.", true);
            }

            Size size;
            size = await IoUtils.GetVideoOrFramesRes(path);

            cache.Add(hash, size);

            return size;
        }

        private static bool CacheContains(QueryInfo hash)
        {
            foreach (KeyValuePair<QueryInfo, Size> entry in cache)
                if (entry.Key.path == hash.path && entry.Key.filesize == hash.filesize)
                    return true;

            return false;
        }

        private static Size GetFromCache(QueryInfo hash)
        {
            foreach (KeyValuePair<QueryInfo, Size> entry in cache)
                if (entry.Key.path == hash.path && entry.Key.filesize == hash.filesize)
                    return entry.Value;

            return new Size();
        }

        public static void ClearCache ()
        {
            cache.Clear();
        }
    }
}
