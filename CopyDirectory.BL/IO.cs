using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CopyDirectory.BL
{
    public class IO_Functions
    {
        public event EventHandler<IOFunctionsEventArgs> SuccesfullCopy;

        public void Copy(string source, string target)
        {
            if (!Directory.Exists(source) || !Directory.Exists(target))
                throw new System.IO.DirectoryNotFoundException(string.Format("Source folder: {0} or Target folder: {1} does not exists.", source, target));

            try
            {
                CopyDirectories(source, target);
            }
            catch (IOException ioException)
            {
                throw new IOException("An error has occurred copying source folder. Please try again.", ioException);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CopyDirectories(string source, string target)
        {
            string[] directories = Directory.GetDirectories(source);

            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory);
                Directory.CreateDirectory(string.Format("{0}\\{1}", target, directoryName));

                CopyDirectories(directory, string.Format("{0}\\{1}", target, directoryName));
            }

            string[] files = Directory.GetFiles(source);

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                File.Copy(file, string.Format("{0}\\{1}", target, filename),true);

                IOFunctionsEventArgs args = new IOFunctionsEventArgs
                {
                    FileNameCreated = filename
                };

                OnSuccesfullCopy(args);
            }
        }

        protected virtual void OnSuccesfullCopy(IOFunctionsEventArgs e)
        {
            EventHandler<IOFunctionsEventArgs> handler = SuccesfullCopy;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class IOFunctionsEventArgs : EventArgs
    {
        public string FileNameCreated { get; set; }
    }
}
