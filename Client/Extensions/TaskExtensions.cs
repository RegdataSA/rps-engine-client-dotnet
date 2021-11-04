using System.Threading.Tasks;

namespace Regdata.RPS.Engine.Client.Extensions
{
    public static class TaskExtensions
    {
        public static T GetAwaitedResult<T>(this Task<T> task, bool continueOnCapturedContext = false)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();

        public static void GetAwaitedResult(this Task task)
            => task.ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
