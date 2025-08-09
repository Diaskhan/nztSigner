using Microsoft.Win32;

namespace nztSigner.Utils
{
    public static class FileAssociations
    {
        public static void RegisterFileAssociation()
        {
            try
            {
                string executablePath = Application.ExecutablePath;
                string fileType = ".cms";
                string applicationName = "nztSigner";
                string description = "CMS Signed Document";

                // Register file extension
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{fileType}"))
                {
                    key.SetValue("", $"{applicationName}File");
                }

                // Register application
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{applicationName}File"))
                {
                    key.SetValue("", description);
                    
                    using (RegistryKey iconKey = key.CreateSubKey("DefaultIcon"))
                    {
                        iconKey.SetValue("", $"{executablePath},0");
                    }

                    using (RegistryKey shellKey = key.CreateSubKey(@"shell\open\command"))
                    {
                        shellKey.SetValue("", $"\"{executablePath}\" \"%1\"");
                    }
                }

                // Notify the system about the change
                SHChangeNotify(0x08000000, 0x0000, nint.Zero, nint.Zero);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering file association: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        private static extern void SHChangeNotify(int eventId, int flags, nint item1, nint item2);
    }
}