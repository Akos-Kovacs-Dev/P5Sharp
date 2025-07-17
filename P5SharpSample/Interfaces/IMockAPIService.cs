using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P5SharpSample.Interfaces
{
    public interface IMockAPIService
    {
        public Task<List<string>> GetDataAsync();
    }
}
