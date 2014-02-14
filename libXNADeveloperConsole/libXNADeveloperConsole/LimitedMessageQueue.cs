using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libXNADeveloperConsole
{
    /// <summary>
    /// A limited message queue (LMQ) meant to store a limited number of strings in a queue format.
    /// </summary>
    public sealed class LimitedMessageQueue
    {
        private List<string> dataList;
        private int dataLimit;

        /// <summary>
        /// The maximum number of elements in the queue at once.
        /// </summary>
        public int Capacity { get { return dataLimit; } set { dataLimit = value; } }
        
        /// <summary>
        /// Creates a new instance of the limited message queue
        /// </summary>
        /// <param name="limit">The maximum number of elements in the queue at once.</param>
        public LimitedMessageQueue(int limit)
        {
            dataList = new List<string>();
            dataLimit = limit;
        }

        /// <summary>
        /// Enqueue a new string into the LMQ
        /// </summary>
        /// <param name="data">The string to enqueue</param>
        public void Enqueue(string data)
        {
            dataList.Add(data);
            if (dataList.Count > dataLimit)
            {
                dataList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Clears the limited message queue
        /// </summary>
        public void Clear()
        {
            dataList.Clear();
        }

        /// <summary>
        /// A ToString for the LMQ
        /// </summary>
        /// <returns>A string containing every element in the LMQ</returns>
        public override string ToString()
        {
            string returnString = "";

            for (int i = dataList.Count - 1; i >= 0; i--)
            {
                returnString += dataList[i] + "\n";
            }
            return returnString;
        }

    }
}
