using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_DevConsole.DevConsole
{
    class LimitedMessageQueue
    {
        private List<string> dataList;
        private int dataLimit;
        public int DataLimit { get { return dataLimit; } set { dataLimit = value; } }
        public LimitedMessageQueue(int limit)
        {
            dataList = new List<string>();
            dataLimit = limit;
        }

        public void Enqueue(string data)
        {
            dataList.Add(data);
            if (dataList.Count > dataLimit)
            {
                dataList.RemoveAt(0);
            }
        }

        public void Clear()
        {
            dataList.Clear();
        }

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
