using System;

namespace DryRun
{
    public class UnknownService : IUnknownService
    {
        private int count = 0;

        public void AddRef()
        {
            count++;
        }

        public string QueryInterface(string key)
        {
            return $"Do you really? WTF! Anyway, the current count is {count}";
        }

        public void Release()
        {
            count--;
        }
    }
}