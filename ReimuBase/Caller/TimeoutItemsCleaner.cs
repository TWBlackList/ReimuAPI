using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace ReimuAPI.ReimuBase.Caller
{
    public class TimeoutItemsCleaner
    {
        private readonly bool Lock = false;

        public void StartCleanDaemon()
        {
            if (Lock) return;
            Thread.Sleep(900);
            Log.i("Calling plugins to clean items.");
            callMemberJoinReceiver(TempData.pluginsList);
        }

        private void callMemberJoinReceiver(List<PluginObject> plugins)
        {
            TempData.tempAdminList = null;
            foreach (var pl in plugins)
                try
                {
                    pl.callClear();
                }
                catch (NotImplementedException)
                {
                }
                catch (StopProcessException)
                {
                    return;
                }
                catch (TargetInvocationException e)
                {
                    if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                    throw e;
                }
        }
    }
}