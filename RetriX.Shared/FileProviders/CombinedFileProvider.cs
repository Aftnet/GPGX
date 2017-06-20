﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RetriX.Shared.FileProviders
{
    public class CombinedFileProvider : IFileProvider
    {
        private readonly ISet<IFileProvider> Providers;

        public CombinedFileProvider(ISet<IFileProvider> providers)
        {
            Providers = providers;
        }

        public void Dispose()
        {
            foreach (var i in Providers)
            {
                i.Dispose();
            }
        }

        public async Task<Stream> GetFileStreamAsync(string path, FileAccess accessType)
        {
            foreach(var i in Providers)
            {
                var stream = await i.GetFileStreamAsync(path, accessType);
                if (stream != null)
                {
                    return stream;
                }
            }

            return null;
        }
    }
}
