using System.Collections.Generic;
using System;

namespace Assets.Hypercrops.System
{
    public class ObjectCache<T>
    {
        private readonly Dictionary<string, T> _cache = new();

        public ICacheResult<T> Entry(string key)
        {
            return new InternalCacheResult(this, key);
        }

        protected Dictionary<string, T> GetCache()
        {
            return _cache;
        }

        protected class InternalCacheResult : ICacheResult<T>
        {
            public string Key
            {
                get { return _key; }
                set
                {
                    _key = value;

                    if (_key != null && _parent != null)
                    {
                        _isHit = _parent.GetCache().ContainsKey(_key);

                        if (_isHit)
                        {
                            _result = _parent.GetCache()[_key];
                        }
                    }
                }
            }

            public T Result
            {
                get => _result;
            }

            public bool IsHit
            {
                get => _isHit;
            }

            private string _key;
            private T _result;
            private bool _isHit;
            private readonly ObjectCache<T> _parent;

            public InternalCacheResult(ObjectCache<T> parent, string key)
            {
                _parent = parent;
                _key = key;
                _isHit = _parent.GetCache().ContainsKey(_key);

                if (_isHit)
                {
                    _result = _parent.GetCache()[_key];
                }
            }

            public T LoadOnMiss(T value)
            {
                if (!_isHit && _key != null)
                {
                    _parent.GetCache().Add(_key, value);

                    _result = value;
                    _isHit = true;
                }

                return _result;
            }

            public T LoadOnMiss(Func<T> callback)
            {
                if (!_isHit && _key != null)
                {
                    T result = callback.Invoke();

                    _parent.GetCache().Add(_key, result);
                    _result = result;
                    _isHit = true;
                }

                return _result;
            }

            public T Delete()
            {
                if (_isHit)
                {
                    _parent.GetCache().Remove(_key);
                }

                return _result;
            }
        }
    }

    public interface ICacheResult<T>
    {
        string Key { get; set; }
        T Result { get; }
        bool IsHit { get; }
        T LoadOnMiss(T value);
        T LoadOnMiss(Func<T> callback);
        T Delete();
    }
}