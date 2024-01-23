using System;

namespace Kit
{
    internal class Callback : IAction
    {
        private static readonly SimpleObjectPool<Callback> mSimpleObjectPool = new(() => new Callback(), null, 10);

        private Action mCallback;

        private Callback()
        {
        }

        public ulong ActionID { get; set; }
        public ActionStatus Status { get; set; }
        public bool Deinited { get; set; }
        public bool Paused { get; set; }


        public void OnStart()
        {
            mCallback?.Invoke();
            this.Finish();
        }

        public void OnExecute(float deltaTime)
        {
        }

        public void OnFinish()
        {
        }

        public void Deinit()
        {
            if (!Deinited)
            {
                Deinited = true;
                mCallback = null;
                mSimpleObjectPool.Recycle(this);
            }
        }

        public void Reset()
        {
            Paused = false;
            Status = ActionStatus.NotStart;
        }

        public static Callback Allocate(Action callback)
        {
            var callbackAction = mSimpleObjectPool.Allocate();
            callbackAction.ActionID = ActionKit.ID_GENERATOR++;
            callbackAction.Reset();
            callbackAction.Deinited = false;
            callbackAction.mCallback = callback;
            return callbackAction;
        }
    }

    public static class CallbackExtension
    {
        public static ISequence Callback(this ISequence self, Action callback)
        {
            return self.Append(Kit.Callback.Allocate(callback));
        }
    }
}