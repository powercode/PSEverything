using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PSEverything
{
    internal class NativeMethods32
    {
        const string Everything32Dll = "Everything32.dll";
        [DllImport(Everything32Dll, CharSet = CharSet.Unicode)]
        public static extern int Everything_SetSearchW(string lpSearchString);

        [DllImport(Everything32Dll)]
        public static extern void Everything_SetMatchPath(bool bEnable);

        [DllImport(Everything32Dll)]
        public static extern void Everything_SetMatchCase(bool bEnable);

        [DllImport(Everything32Dll)]
        public static extern void Everything_SetMatchWholeWord(bool bEnable);

        [DllImport(Everything32Dll)]
        public static extern void Everything_SetRegex(bool bEnable);

        [DllImport(Everything32Dll)]
        public static extern void Everything_SetMax(int dwMax);

        [DllImport(Everything32Dll)]
        public static extern void Everything_SetOffset(int dwOffset);

        [DllImport(Everything32Dll)]
        public static extern bool Everything_GetMatchPath();

        [DllImport(Everything32Dll)]
        public static extern bool Everything_GetMatchCase();

        [DllImport(Everything32Dll)]
        public static extern bool Everything_GetMatchWholeWord();

        [DllImport(Everything32Dll)]
        public static extern bool Everything_GetRegex();

        [DllImport(Everything32Dll)]
        public static extern UInt32 Everything_GetMax();

        [DllImport(Everything32Dll)]
        public static extern UInt32 Everything_GetOffset();

        [DllImport(Everything32Dll)]
        public static extern string Everything_GetSearchW();

        [DllImport(Everything32Dll)]
        public static extern int Everything_GetLastError();

        [DllImport(Everything32Dll)]
        public static extern bool Everything_QueryW(bool bWait);

        [DllImport(Everything32Dll)]
        public static extern void Everything_SortResultsByPath();

        [DllImport(Everything32Dll)]
        public static extern int Everything_GetNumFileResults();

        [DllImport(Everything32Dll)]
        public static extern int Everything_GetNumFolderResults();

        [DllImport(Everything32Dll)]
        public static extern int Everything_GetNumResults();

        [DllImport(Everything32Dll)]
        public static extern int Everything_GetTotFileResults();

        [DllImport(Everything32Dll)]
        public static extern int Everything_GetTotFolderResults();

        [DllImport(Everything32Dll)]
        public static extern int Everything_GetTotResults();

        [DllImport(Everything32Dll)]
        public static extern bool Everything_IsVolumeResult(int nIndex);

        [DllImport(Everything32Dll)]
        public static extern bool Everything_IsFolderResult(int nIndex);

        [DllImport(Everything32Dll)]
        public static extern bool Everything_IsFileResult(int nIndex);

        [DllImport(Everything32Dll, CharSet = CharSet.Unicode)]
        public static extern void Everything_GetResultFullPathNameW(int nIndex, StringBuilder lpString, int nMaxCount);

        [DllImport(Everything32Dll)]
        public static extern void Everything_Reset();

        [DllImport(Everything32Dll)]
        public static extern void Everything_CleanUp();
    }
}