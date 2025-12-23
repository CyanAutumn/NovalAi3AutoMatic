using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoNai3Tools.utils {
    internal static class FolderPicker {
        private const int ERROR_CANCELLED = unchecked((int)0x800704C7);
        private static readonly Guid IID_IShellItem = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");

        public static string PickFolder(string initialPath = null, IntPtr ownerHandle = default,
            bool preferLegacy = false) {
            if (preferLegacy)
                return PickWithLegacyDialog(initialPath, ownerHandle);

            if (Environment.OSVersion.Version.Major >= 6) {
                try {
                    return PickWithCommonItemDialog(initialPath, ownerHandle);
                }
                catch (COMException) {
                    return PickWithLegacyDialog(initialPath, ownerHandle);
                }
                catch (DllNotFoundException) {
                    return PickWithLegacyDialog(initialPath, ownerHandle);
                }
            }

            return PickWithLegacyDialog(initialPath, ownerHandle);
        }

        private static string PickWithCommonItemDialog(string initialPath, IntPtr ownerHandle) {
            IFileDialog dialog = null;
            IShellItem initialFolder = null;
            try {
                dialog = (IFileDialog)new FileOpenDialog();
                dialog.GetOptions(out FileDialogOptions options);
                options |= FileDialogOptions.FOS_PICKFOLDERS | FileDialogOptions.FOS_FORCEFILESYSTEM |
                           FileDialogOptions.FOS_PATHMUSTEXIST;
                dialog.SetOptions(options);

                if (!string.IsNullOrWhiteSpace(initialPath) && Directory.Exists(initialPath) &&
                    TryCreateItem(initialPath, out initialFolder)) {
                    dialog.SetFolder(initialFolder);
                    Marshal.ReleaseComObject(initialFolder);
                    initialFolder = null;
                }

                int hr = dialog.Show(ownerHandle);
                if (hr == ERROR_CANCELLED)
                    return null;
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                dialog.GetResult(out IShellItem resultItem);
                return GetPathFromShellItem(resultItem);
            }
            finally {
                if (initialFolder != null)
                    Marshal.ReleaseComObject(initialFolder);
                if (dialog != null)
                    Marshal.ReleaseComObject(dialog);
            }
        }

        private static bool TryCreateItem(string path, out IShellItem item) {
            Guid iid = IID_IShellItem;
            int hr = SHCreateItemFromParsingName(path, IntPtr.Zero, ref iid, out item);
            return hr >= 0 && item != null;
        }

        private static string GetPathFromShellItem(IShellItem item) {
            if (item == null)
                return null;

            try {
                item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out IntPtr pszString);
                if (pszString == IntPtr.Zero)
                    return null;

                string path = Marshal.PtrToStringUni(pszString);
                Marshal.FreeCoTaskMem(pszString);
                return path;
            }
            finally {
                Marshal.ReleaseComObject(item);
            }
        }

        private static string PickWithLegacyDialog(string initialPath, IntPtr ownerHandle) {
            using (var dialog = new FolderBrowserDialog()) {
                dialog.Description = "请选择文件夹";
                if (!string.IsNullOrEmpty(initialPath) && Directory.Exists(initialPath))
                    dialog.SelectedPath = initialPath;

                var ownerWindow = ownerHandle == IntPtr.Zero ? null : new WindowWrapper(ownerHandle);
                DialogResult result = ownerWindow == null ? dialog.ShowDialog() : dialog.ShowDialog(ownerWindow);
                return result == DialogResult.OK ? dialog.SelectedPath : null;
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        private static extern int SHCreateItemFromParsingName(string pszPath, IntPtr pbc, ref Guid riid,
            out IShellItem ppv);

        private class WindowWrapper : IWin32Window {
            public WindowWrapper(IntPtr handle) {
                Handle = handle;
            }

            public IntPtr Handle { get; }
        }

        [ComImport]
        [Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
        private class FileOpenDialog {
        }

        [Flags]
        private enum FileDialogOptions : uint {
            FOS_OVERWRITEPROMPT = 0x00000002,
            FOS_STRICTFILETYPES = 0x00000004,
            FOS_NOCHANGEDIR = 0x00000008,
            FOS_PICKFOLDERS = 0x00000020,
            FOS_FORCEFILESYSTEM = 0x00000040,
            FOS_ALLNONSTORAGEITEMS = 0x00000080,
            FOS_NOVALIDATE = 0x00000100,
            FOS_ALLOWMULTISELECT = 0x00000200,
            FOS_PATHMUSTEXIST = 0x00000800,
            FOS_FILEMUSTEXIST = 0x00001000,
            FOS_CREATEPROMPT = 0x00002000,
            FOS_SHAREAWARE = 0x00004000,
            FOS_NOREADONLYRETURN = 0x00008000,
            FOS_NOTESTFILECREATE = 0x00010000,
            FOS_HIDEMRUPLACES = 0x00020000,
            FOS_HIDEPINNEDPLACES = 0x00040000,
            FOS_NODEREFERENCELINKS = 0x00100000,
            FOS_DONTADDTORECENT = 0x02000000,
            FOS_FORCESHOWHIDDEN = 0x10000000,
            FOS_DEFAULTNOMINIMODE = 0x20000000,
            FOS_FORCEPREVIEWPANEON = 0x40000000
        }

        private enum FileDialogAddPlace {
            Bottom = 0,
            Top = 1
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct COMDLG_FILTERSPEC {
            [MarshalAs(UnmanagedType.LPWStr)] public string pszName;
            [MarshalAs(UnmanagedType.LPWStr)] public string pszSpec;
        }

        [ComImport]
        [Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileDialog {
            [PreserveSig] int Show(IntPtr parent);
            void SetFileTypes(uint cFileTypes, [MarshalAs(UnmanagedType.LPArray)] COMDLG_FILTERSPEC[] rgFilterSpec);
            void SetFileTypeIndex(uint iFileType);
            void GetFileTypeIndex(out uint piFileType);
            void Advise(IFileDialogEvents pfde, out uint pdwCookie);
            void Unadvise(uint dwCookie);
            void SetOptions(FileDialogOptions fos);
            void GetOptions(out FileDialogOptions pfos);
            void SetDefaultFolder(IShellItem psi);
            void SetFolder(IShellItem psi);
            void GetFolder(out IShellItem ppsi);
            void GetCurrentSelection(out IShellItem ppsi);
            void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
            void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
            void GetResult(out IShellItem ppsi);
            void AddPlace(IShellItem psi, FileDialogAddPlace fdap);
            void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
            void Close(int hr);
            void SetClientGuid(ref Guid guid);
            void ClearClientData();
            void SetFilter(IntPtr pFilter);
        }

        [ComImport]
        [Guid("973510DB-7D7F-452B-8975-74A85828D354")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileDialogEvents {
            [PreserveSig] int OnFileOk(IFileDialog pfd);
            [PreserveSig] int OnFolderChanging(IFileDialog pfd, IShellItem psiFolder);
            void OnFolderChange(IFileDialog pfd);
            void OnSelectionChange(IFileDialog pfd);
            [PreserveSig] int OnShareViolation(IFileDialog pfd, IShellItem psi, out uint pResponse);
            void OnTypeChange(IFileDialog pfd);
            void OnOverwrite(IFileDialog pfd, IShellItem psi, out uint pResponse);
        }

        private enum SIGDN : uint {
            SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
            SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
            SIGDN_FILESYSPATH = 0x80058000,
            SIGDN_NORMALDISPLAY = 0,
            SIGDN_PARENTRELATIVE = 0x80080001,
            SIGDN_PARENTRELATIVEEDITING = 0x80031001,
            SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
            SIGDN_PARENTRELATIVEPARSING = 0x80018001
        }

        [ComImport]
        [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem {
            void BindToHandler(IntPtr pbc, ref Guid bhid, ref Guid riid, out IntPtr ppv);
            void GetParent(out IShellItem ppsi);
            void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
            void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
            void Compare(IShellItem psi, uint hint, out int piOrder);
        }
    }
}
