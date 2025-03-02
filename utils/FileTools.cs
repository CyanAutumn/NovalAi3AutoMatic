using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.utils {
    internal class FileTools {
        public static void CheckFolderAlreadExist(string path) {
            if (!Directory.Exists(path)) {
                try {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e) { }
            }
        }
    }
}
