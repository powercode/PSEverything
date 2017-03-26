using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PSEverything
{
    internal class NativeMethods64
    {
        const string Everything64Dll = "Everything64.dll";
        [DllImport(Everything64Dll, CharSet = CharSet.Unicode)]
        public static extern int Everything_SetSearchW(string lpSearchString);

        [DllImport(Everything64Dll)]
        public static extern void Everything_SetMatchPath(bool bEnable);

        [DllImport(Everything64Dll)]
        public static extern void Everything_SetMatchCase(bool bEnable);

        [DllImport(Everything64Dll)]
        public static extern void Everything_SetMatchWholeWord(bool bEnable);

        [DllImport(Everything64Dll)]
        public static extern void Everything_SetRegex(bool bEnable);

        [DllImport(Everything64Dll)]
        public static extern void Everything_SetMax(int dwMax);

        [DllImport(Everything64Dll)]
        public static extern void Everything_SetOffset(int dwOffset);

        [DllImport(Everything64Dll)]
        public static extern bool Everything_GetMatchPath();

        [DllImport(Everything64Dll)]
        public static extern bool Everything_GetMatchCase();

        [DllImport(Everything64Dll)]
        public static extern bool Everything_GetMatchWholeWord();

        [DllImport(Everything64Dll)]
        public static extern bool Everything_GetRegex();

        [DllImport(Everything64Dll)]
        public static extern UInt32 Everything_GetMax();

        [DllImport(Everything64Dll)]
        public static extern UInt32 Everything_GetOffset();

        [DllImport(Everything64Dll)]
        public static extern string Everything_GetSearchW();
       
        [DllImport(Everything64Dll)]
        public static extern int Everything_GetLastError();

        [DllImport(Everything64Dll)]
        public static extern bool Everything_QueryW(bool bWait);

        [DllImport(Everything64Dll)]
        public static extern void Everything_SortResultsByPath();

        [DllImport(Everything64Dll)]
        public static extern int Everything_GetNumFileResults();

        [DllImport(Everything64Dll)]
        public static extern int Everything_GetNumFolderResults();

        [DllImport(Everything64Dll)]
        public static extern int Everything_GetNumResults();

        [DllImport(Everything64Dll)]
        public static extern int Everything_GetTotFileResults();

        [DllImport(Everything64Dll)]
        public static extern int Everything_GetTotFolderResults();

        [DllImport(Everything64Dll)]
        public static extern int Everything_GetTotResults();

        [DllImport(Everything64Dll)]
        public static extern bool Everything_IsVolumeResult(int nIndex);

        [DllImport(Everything64Dll)]
        public static extern bool Everything_IsFolderResult(int nIndex);

        [DllImport(Everything64Dll)]
        public static extern bool Everything_IsFileResult(int nIndex);

        [DllImport(Everything64Dll, CharSet = CharSet.Unicode)]
        public static extern void Everything_GetResultFullPathNameW(int nIndex, StringBuilder lpString, int nMaxCount);

        [DllImport(Everything64Dll)]
        public static extern void Everything_Reset();

        [DllImport(Everything64Dll)]
        public static extern void Everything_Cleanup ();
    }
}