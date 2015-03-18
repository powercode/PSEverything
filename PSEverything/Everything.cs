using System;
using System.Collections.Generic;
using System.Text;

namespace PSEverything
{
    public static class Everything
    {
        public const int EVERYTHING_OK = 0;
        public const int EVERYTHING_ERROR_MEMORY = 1;
        public const int EVERYTHING_ERROR_IPC = 2;
        public const int EVERYTHING_ERROR_REGISTERCLASSEX = 3;
        public const int EVERYTHING_ERROR_CREATEWINDOW = 4;
        public const int EVERYTHING_ERROR_CREATETHREAD = 5;
        public const int EVERYTHING_ERROR_INVALIDINDEX = 6;
        public const int EVERYTHING_ERROR_INVALIDCALL = 7;
        static bool Is64Bit {
            get { return Environment.Is64BitProcess; }
        }

        public static void SetSearch(string text)
        {
            int res = Is64Bit ? NativeMethods64.Everything_SetSearchW(text) : NativeMethods32.Everything_SetSearchW(text);
            if (res != EVERYTHING_OK)
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

        public static IEnumerable<string> GetAllResults()
        {
            var buf = new StringBuilder(260);
            var resCount = GetNumberOfResults();
            for (int i = 0; i < resCount; ++i)
            {
                yield return GetFullPathName(i, buf);
                buf.Clear();
            }
        }

        static void Throw(int errorCode)
        {
            switch (errorCode)
            {
                case EVERYTHING_ERROR_MEMORY:
                    throw new Exception("Memory Error");
                case EVERYTHING_ERROR_REGISTERCLASSEX:
                    throw new Exception("Class registration error");
                case EVERYTHING_ERROR_IPC:
                    throw new Exception("IPC error");
                case EVERYTHING_ERROR_CREATEWINDOW:
                    throw new Exception("create window error");
                case EVERYTHING_ERROR_CREATETHREAD:
                    throw new Exception("Thread creation error");
                case EVERYTHING_ERROR_INVALIDINDEX:
                    throw new Exception("Invalid index error");
                case EVERYTHING_ERROR_INVALIDCALL:
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
