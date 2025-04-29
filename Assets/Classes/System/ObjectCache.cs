using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Classes.System
{
    // TODO: Make generic
    public class ObjectCache
    {
        private readonly Dictionary<string, GameObject> _cache = new();

        public ICacheResult Entry(string key)
        {
            return new InternalCacheResult(this, key);
        }

        protected Dictionary<string, GameObject> GetCache()
        {
            return _cache;
        }

        protected class InternalCacheResult : ICacheResult
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

            public GameObject Result
            {
                get => _result;
                set => _result = value;
            }

            private string _key;
            private GameObject _result;
            private bool _isHit;
            private readonly ObjectCache _parent;

            public InternalCacheResult(ObjectCache parent, string key)
            {
                _parent = parent;
                _key = key;
                _isHit = _parent.GetCache().ContainsKey(_key);

                if (_isHit)
                {
                    _result = _parent.GetCache()[_key];
                }
            }

            public GameObject LoadOnMiss(GameObject value)
            {
                if (!_isHit && _key != null)
                {
                    _parent.GetCache().Add(_key, value);

                    _result = value;
                    _isHit = true;
                }

                return _result;
            }

            public GameObject LoadOnMiss(Func<GameObject> callback)
            {
                if (!_isHit && _key != null)
                {
                    GameObject result = callback.Invoke();

                    _parent.GetCache().Add(_key, result);
                    _result = result;
                    _isHit = true;
                }

                return _result;
            }
        }
    }

    public interface ICacheResult
    {
        string Key { get; set; }
        GameObject Result { get; set; }
        GameObject LoadOnMiss(GameObject value);
        GameObject LoadOnMiss(Func<GameObject> callback);
    }
}