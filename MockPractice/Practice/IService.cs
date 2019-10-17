using System;

namespace MockPractice
{
    public interface IService : IDisposable
    {
        string Name { get; }
        bool IsConnected { get; }
        IDisposable Connect();
        string GetContent(long identity);
    }
}