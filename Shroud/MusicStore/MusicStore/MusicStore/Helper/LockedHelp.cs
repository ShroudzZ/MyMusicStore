﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MusicStore.Helper
{
    /// <summary>
    /// 防止进程互斥
    /// </summary>
    public class LockedHelp
    {
        public static Mutex _Mutex = new Mutex();
        private static readonly Dictionary<Guid, SemaphoreSlim> _Slim = new Dictionary<Guid, SemaphoreSlim>();
        private static readonly Dictionary<Guid, int> _Count = new Dictionary<Guid, int>();
        //加锁
        public static void ThreadLocked(Guid id)
        {
            _Mutex.WaitOne();
            SemaphoreSlim slim;
            if (!_Slim.TryGetValue(id,out slim))
            {
                slim = new SemaphoreSlim(1);
                _Slim.Add(id,slim);
                _Count.Add(id,0);
            }

            _Count[id]++;
            _Mutex.ReleaseMutex();
            slim.Wait();
        }
        //解锁
        public static void ThreadUnLocked(Guid id)
        {
            var slim = _Slim[id];
            if (_Count[id] == 1)
            {
                _Count.Remove(id);
                _Slim.Remove(id);
            }
            else
                _Count[id]--;

            slim.Release();
        }
    }
}