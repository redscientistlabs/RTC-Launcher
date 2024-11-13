namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Runtime.ExceptionServices;
    using System.Windows.Forms;

    public static class FormSync
    {
        public static Form SyncObject { get; set; }
        public delegate void ActionDelegate(Action a);
        public delegate void ActionDelegateT<T>(Action<T> a, T b);
        public delegate void GenericDelegate();
        public static ActionDelegate EmuInvokeDelegate { get; set; }
        public static bool UseQueue { get; set; } = false;
        public static bool EmuThreadIsMainThread { get; set; } = false;


        public static void FormExecute(Action a)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (SyncObject.InvokeRequired)
            {
                SyncObject.InvokeCorrectly(new MethodInvoker(a.Invoke));
            }
            else
            {
                a.Invoke();
            }
        }

        public static void FormExecute<T>(Action<T> a, T b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (SyncObject.InvokeRequired)
            {
                SyncObject.InvokeCorrectly(new MethodInvoker(() => { a.Invoke(b); }));
            }
            else
            {
                a.Invoke(b);
            }
        }

        public static void FormExecute(Delegate a)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (SyncObject.InvokeRequired)
            {
                SyncObject.InvokeCorrectly(a);
            }
            else
            {
                a.DynamicInvoke();
            }
        }

        //https://stackoverflow.com/a/56931457
        private static object InvokeCorrectly(this Control control, Delegate method, params object[] args)
        {
            Exception failure = null;
            var result = control.Invoke(new Func<object>(() =>
            {
                try
                {
                    return method.DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    failure = ex.InnerException;
                    return failure;
                }
            }));
            if (failure != null)
            {
                ExceptionDispatchInfo.Capture(failure).Throw();
            }
            return result;
        }
    }
}
