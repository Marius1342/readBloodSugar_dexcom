using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesReader
{
    internal class Helper
    {
        /// <summary>
        /// Invokes into the main thread
        /// </summary>
        /// <param name="action">The function</param>
        /// <returns></returns>
        public static async Task Invoke(Action action)
        {
            await MainThread.InvokeOnMainThreadAsync(action);
        }



    }
}
