using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    public interface IStorageContentReader
    {
        string GetContent(string fileName);
    }
}
