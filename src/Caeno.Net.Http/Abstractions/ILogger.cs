using System;
namespace Caeno.Net.Http.Abstractions
{
    public interface ILogger
    {

        void Trace(string message, params object[] args);

    }
}
