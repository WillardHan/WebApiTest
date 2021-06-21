using org.apache.zookeeper;
using org.apache.zookeeper.recipes.@lock;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.ZooKeepr
{
    public class ZooKeeprLock
    {
        private const int CONNECTION_TIMEOUT = 50000;
        private const string CONNECTION_STRING = "zookeeper:2181,zookeeper2:2181,zookeeper3:2181";

        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="key">加锁的节点名</param>
        /// <param name="lockAcquiredAction">加锁成功后需要执行的逻辑</param>
        /// <param name="lockReleasedAction">锁释放后需要执行的逻辑，可为空</param>
        /// <returns></returns>
        public async Task Lock(string key, Action lockAcquiredAction, Action lockReleasedAction = null)
        {
            // 获取 ZooKeeper Client
            ZooKeeper keeper = CreateClient();
            // 指定锁节点
            WriteLock writeLock = new WriteLock(keeper, $"/{key}", null);

            var lockCallback = new LockCallback(() =>
            {
                lockAcquiredAction.Invoke();
                writeLock.unlock();
            }, lockReleasedAction);
            // 绑定锁获取和释放的监听对象
            writeLock.setLockListener(lockCallback);
            // 获取锁（获取失败时会监听上一个临时节点）
            await writeLock.Lock();
        }

        private ZooKeeper CreateClient()
        {
            var zooKeeper = new ZooKeeper(CONNECTION_STRING, CONNECTION_TIMEOUT, NullWatcher.Instance);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < CONNECTION_TIMEOUT)
            {
                var state = zooKeeper.getState();
                if (state == ZooKeeper.States.CONNECTED || state == ZooKeeper.States.CONNECTING)
                {
                    break;
                }
            }
            sw.Stop();
            return zooKeeper;
        }

        class NullWatcher : Watcher
        {
            public static readonly NullWatcher Instance = new NullWatcher();
            private NullWatcher() { }
            public override Task process(WatchedEvent @event)
            {
                return Task.CompletedTask;
            }
        }

        class LockCallback : LockListener
        {
            private readonly Action _lockAcquiredAction;
            private readonly Action _lockReleasedAction;

            public LockCallback(Action lockAcquiredAction, Action lockReleasedAction)
            {
                _lockAcquiredAction = lockAcquiredAction;
                _lockReleasedAction = lockReleasedAction;
            }

            /// <summary>
            /// 获取锁成功回调
            /// </summary>
            /// <returns></returns>
            public Task lockAcquired()
            {
                _lockAcquiredAction?.Invoke();
                return Task.FromResult(0);
            }

            /// <summary>
            /// 释放锁成功回调
            /// </summary>
            /// <returns></returns>
            public Task lockReleased()
            {
                _lockReleasedAction?.Invoke();
                return Task.FromResult(0);
            }
        }

    }
}
