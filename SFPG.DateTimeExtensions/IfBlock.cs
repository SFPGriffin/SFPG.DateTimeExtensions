// Copyright S. F. P. Griffin 2018, License: GNU LESSER GENERAL PUBLIC LICENSE, Version 3, 29 June 2007.

using System.ComponentModel;

namespace SFPG.DateTimeExtensions
{
    using System;

    public class IfBlock<TOriginalObject, TResult> 
        where TOriginalObject : class
    {
        private readonly bool _isTrue;
        private TResult _result;

        public IfBlock(bool isTrue, TOriginalObject originalObject)
        {
            _isTrue = isTrue;
            OriginalObject = originalObject;
        }

        private TOriginalObject OriginalObject { get; }

        public IfBlock<TOriginalObject, TResult> Then(Func<TOriginalObject, TResult> thenDo)
        {
            if (_isTrue)
            {
                _result = thenDo(OriginalObject);
            }

            return this;
        }

        public IfBlock<TOriginalObject, TResult> Else(Func<TOriginalObject, TResult> elseDo)
        {
            if (!_isTrue)
            {
               _result = elseDo(OriginalObject);
            }

            return this;
        }

        public  IfResult<TResult> Result => new IfResult<TResult>(_isTrue, _result);
    }
}