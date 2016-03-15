using System;

namespace FashionStore.Service.Interfaces.Results
{
    public class PurchaseResult
    {
        private bool _success;
        public bool Success
        {
            get { return _success; }
        }
        private bool _fail;
        public bool Fail
        {
            get { return _fail; }
        }
        public string Error { get; protected set; }

        private Exception _exception;
        public Exception Exception
        {
            get { return _exception; }
        }

        public PurchaseResult(string errors)
        {
            _success = false;
            _fail = true;
            Error = errors;
        }
        public PurchaseResult(string error, Exception exc)
            : this(error)
        {
            _exception = exc;
        }
        public PurchaseResult()
        {
            _fail = false;
            _success = true;
        }
    }
}