using System;
using System.Text;
using static PSEverything.Status;

namespace PSEverything
{

    public enum Status
    {
        Ok = 0,
        ErrorMemory = 1,
        ErrorIpc = 2,
        ErrorRegisterclassex = 3,
        ErrorCreatewindow = 4,
        ErrorCreatethread = 5,
        ErrorInvalidindex = 6,
        ErrorInvalidcall = 7,
    }

    public static class Everything
    {

        static bool Is64Bit => Environment.Is64BitProcess;

        public static void SetSearch(string text)
        {
            int res = Is64Bit ? NativeMethods64.Everything_SetSearchW(text) : NativeMethods32.Everything_SetSearchW(text);
            if (res != (int) Ok)
            {
                Throw(res);
            }
        }

        public static void SetRegEx(bool value)
        {
            if (Is64Bit)
                NativeMethods64.Everything_SetRegex(value);
            else
                NativeMethods32.Everything_SetRegex(value);            
            
        }

        public static void SetMatchCase(bool value)
        {
            if (Is64Bit)
                NativeMethods64.Everything_SetMatchCase(value);
            else
                NativeMethods32.Everything_SetMatchCase(value);
        }

        public static void SetMatchWholeWord(bool value)
        {
            if (Is64Bit)
                NativeMethods64.Everything_SetMatchWholeWord(value);
            else
                NativeMethods32.Everything_SetMatchWholeWord(value);
        }

        public static void SetMatchPath(bool value)
        {
            if (Is64Bit)
                NativeMethods64.Everything_SetMatchPath(value);
            else
                NativeMethods32.Everything_SetMatchPath(value);
        }

        public static void SetMax(int count)
        {
            if (Is64Bit)
                NativeMethods64.Everything_SetMax(count);
            else
                NativeMethods32.Everything_SetMax(count);
        }

        public static void SetOffset(int index)
        {
            if (Is64Bit)
                NativeMethods64.Everything_SetOffset(index);
            else
                NativeMethods32.Everything_SetOffset(index);
        }

        public static void SortResultsByPath()
        {
            if (Is64Bit)
                NativeMethods64.Everything_SortResultsByPath();
            else
                NativeMethods32.Everything_SortResultsByPath();
        }


        public static bool Query(bool wait)
        {
            return Is64Bit ? NativeMethods64.Everything_QueryW(wait) : NativeMethods32.Everything_QueryW(wait);            
        }

        public static int GetNumberOfResults()
        {
            return Is64Bit ? NativeMethods64.Everything_GetNumResults() : NativeMethods32.Everything_GetNumResults();
        }

        public static int GetNumberOfFileResults()
        {
            return Is64Bit ? NativeMethods64.Everything_GetNumFileResults() : NativeMethods32.Everything_GetNumFileResults();
        }

        public static int GetNumberOfFolderResults()
        {
            return Is64Bit ? NativeMethods64.Everything_GetNumFolderResults() : NativeMethods32.Everything_GetNumFolderResults();
        }

        public static int GetTotalNumberOfResults()
        {
            return Is64Bit ? NativeMethods64.Everything_GetTotResults() : NativeMethods32.Everything_GetTotResults();
        }

        public static string GetFullPathName(int index, StringBuilder buf)
        {
            if (Is64Bit)
                NativeMethods64.Everything_GetResultFullPathNameW(index, buf, buf.Capacity);
            else
                NativeMethods32.Everything_GetResultFullPathNameW(index, buf, buf.Capacity);
            
            return buf.ToString();
        }

        public static string[] GetAllResults(int count)
        {
            var retVal = new string[count];
            var buf = new StringBuilder(260);
            
            for (int i = 0; i < count ; ++i)
            {
                var path = GetFullPathName(i, buf);
                retVal[i] = path;
                buf.Clear();
            }
            return retVal;
        }

        static void Throw(int errorCode)
        {
            switch ((Status)errorCode)
            {
                case ErrorMemory:
                    throw new Exception("Memory Error");
                case ErrorRegisterclassex:
                    throw new Exception("Class registration error");
                case ErrorIpc:
                    throw new Exception("IPC error");
                case ErrorCreatewindow:
                    throw new Exception("create window error");
                case ErrorCreatethread:
                    throw new Exception("Thread creation error");
                case ErrorInvalidindex:
                    throw new Exception("Invalid index error");
                case ErrorInvalidcall:
                    throw new Exception("Invalid call error");
            }
        }


        public static void Reset()
        {
            if (Is64Bit)
                NativeMethods64.Everything_Reset();
            else
                NativeMethods32.Everything_Reset();
        }
    }
}
