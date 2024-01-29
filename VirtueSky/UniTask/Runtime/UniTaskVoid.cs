#pragma warning disable CS1591
#pragma warning disable CS0436

using System.Runtime.CompilerServices;
using VirtueSky.Threading.Tasks.CompilerServices;


namespace VirtueSky.Threading.Tasks
{
    [AsyncMethodBuilder(typeof(AsyncUniTaskVoidMethodBuilder))]
    public readonly struct UniTaskVoid
    {
        public void Forget()
        {
        }
    }
}

