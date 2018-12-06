// Copyright S. F. P. Griffin 2018, License: GNU GENERAL PUBLIC LICENSE, Version 3, 29 June 2007.

namespace SFPG.DateTimeExtensions
{
    public class IfResult<TResult>
    {
        public bool Success { get; }

        public TResult Value { get; }

        internal IfResult(bool success, TResult value)
        {
            Success = success;
            Value = value;
        }
    }
}