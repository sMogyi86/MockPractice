using System;

namespace MockPractice
{
    public interface INowProvider
    {
        DateTime GetNow();
    }

    internal class NowProvider : INowProvider
    {
        public DateTime GetNow()
            => DateTime.Now;
    }
}
