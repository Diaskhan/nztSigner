using nztSigner.Utils;
using Org.BouncyCastle.Cms;
using System.Diagnostics;

namespace nztSigner
{
    public partial class Form1 : Form
    {
        private string filename = "C:\\Dev\\nztSigner\\Data\\Иманбердиев Д_приказ_отпуск_август_2025.pdf.cms";

        public Form1()
        {
            InitializeComponent();
            SetFormFilename();
        }

        private void SetFormFilename()
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                Text = "nztSigner - Файл не выбран";
                return;
            }

            Text = $"{Path.GetFileName(filename)}";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1]) && args[1].EndsWith(".cms", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    filename = args[1];
                    SetFormFilename();
                    toolStripButton2_Click(this, EventArgs.Empty); // Automatically show CMS info
                }
                catch (Exception ex)
                {
                    Hide();
                    MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CMS files (*.cms)|*.cms";
                openFileDialog.Title = "Выберите CMS-файл";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filename = openFileDialog.FileName;
                    SetFormFilename();
                }
                else
                {
                    filename = string.Empty;
                    SetFormFilename();
                    treeView1.Nodes.Clear();

                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                MessageBox.Show("Файл не выбран.");
                return;
            }

            var outputFilename = filename;
            if (outputFilename.EndsWith(".cms", StringComparison.OrdinalIgnoreCase))
            {
                outputFilename = Path.ChangeExtension(outputFilename, null);
            }


            using var stream = File.OpenRead(filename);
            var signedData = new CmsSignedData(stream);

            // Получаем содержимое (Detached или Embedded)
            var sc = signedData.SignedContent;
            if (sc == null)
            {
                MessageBox.Show("В CMS-файле отсутствует вложенное содержимое.");
                return;
            }

            // Извлекаем байты исходного файла
            using (var memoryStream = new MemoryStream())
            {
                sc.Write(memoryStream);
                var contentBytes = memoryStream.ToArray();

                // Сохраняем извлечённый файл
                try
                {
                    File.WriteAllBytes(outputFilename, contentBytes);
                }
                catch
                {
                    MessageBox.Show("Ошибка при сохранении файла." + Environment.NewLine + outputFilename + Environment.NewLine + "Возможно файл открыть в другой программе.");
                    return;
                }

                string[] blockedExtensions = { ".exe", ".bat", ".cmd", ".msi", ".vbs", ".js", ".scr", ".ps1" };
                string ext = Path.GetExtension(outputFilename).ToLowerInvariant();

                if (blockedExtensions.Contains(ext))
                {
                    MessageBox.Show($"Запуск файлов с расширением {blockedExtensions} запрещён! Но файл извлечен.");
                    return;
                }

                // Проверяем существование файла
                if (!File.Exists(outputFilename))
                {
                    MessageBox.Show("Файл не найден: " + outputFilename);
                    return;
                }

                try
                {
                    // Открываем файл стандартным приложением
                    Process.Start(new ProcessStartInfo(outputFilename) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при открытии файла: " + ex.Message);
                }
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                MessageBox.Show("Файл не выбран.");
                return;
            }

            treeView1.Nodes.Clear(); // Очистить дерево

            using var stream = File.OpenRead(filename);
            var signedData = new CmsSignedData(stream);

            var signers = signedData.GetSignerInfos();
            var certs = signedData.GetCertificates();

            var signersList = signers.GetSigners().Cast<SignerInformation>().ToList();

            signersList.Sort(SignerUtils.CompareSignersBySigningTime);

            foreach (SignerInformation signer in signersList)
            {
                var signingTimeAttr = signer.SignedAttributes[Org.BouncyCastle.Asn1.Cms.CmsAttributes.SigningTime];
                DateTime? signingTime = null;
                if (signingTimeAttr != null)
                {
                    var attrValue = signingTimeAttr.AttrValues.SingleOrDefault()?.ToString();
                    if (!string.IsNullOrWhiteSpace(attrValue))
                        signingTime = SignerUtils.ParseAsn1UtcTimeString(attrValue);
                }

                var selector = signer.SignerID;
                var matchingCerts = certs.EnumerateMatches(selector).Cast<Org.BouncyCastle.X509.X509Certificate>();
                var cert = matchingCerts.FirstOrDefault();

                if (cert != null)
                {
                    string subjectDN = cert.SubjectDN.ToString();
                    string issuerDN = cert.IssuerDN.ToString();
                    string serialNumber = cert.SerialNumber.ToString();

                    var dnFields = SignerUtils.ParseSubjectDN(subjectDN);

                    string orgName = dnFields.TryGetValue("O", out var o) ? o : string.Empty;
                    string bin = dnFields.TryGetValue("OU", out var ou) ? ou : string.Empty;
                    string commonName = dnFields.TryGetValue("CN", out var cn) ? cn : string.Empty;
                    string givenName = dnFields.TryGetValue("GIVENNAME", out var gn) ? gn : string.Empty;
                    string fio = commonName + " " + givenName; // Обычно ФИО в CN
                    string iin = dnFields.TryGetValue("SERIALNUMBER", out var sn) ? sn : string.Empty;

                    DateTime startDate = cert.CertificateStructure.StartDate.ToDateTime();
                    DateTime endDate = cert.CertificateStructure.EndDate.ToDateTime();


                    var parentNode = new TreeNode(fio);

                    if (!string.IsNullOrEmpty(iin))
                        parentNode.Nodes.Add(new TreeNode($"ИИН: {iin}"));
                    if (!string.IsNullOrEmpty(fio))
                        parentNode.Nodes.Add(new TreeNode($"ФИО: {fio}"));
                    if (!string.IsNullOrEmpty(bin))
                        parentNode.Nodes.Add(new TreeNode($"БИН: {bin}"));

                    if (!string.IsNullOrEmpty(orgName))
                        parentNode.Nodes.Add(new TreeNode($"Организация: {orgName}"));

                    parentNode.Nodes.Add(new TreeNode($"Издатель сертификата: {issuerDN}"));
                    parentNode.Nodes.Add(new TreeNode($"Срок действия сертификата: {startDate:dd/MM/yyyy HH:mm:ss} - {endDate:dd/MM/yyyy HH:mm:ss}"));

                    if (signingTime != null)
                        parentNode.Nodes.Add(new TreeNode($"Дата подписания: {signingTime:dd/MM/yyyy HH:mm:ss} (UTC)"));

                    // Проверка сертификата
                    bool isCertValid = false;
                    try
                    {
                        cert.CheckValidity();
                        isCertValid = true;
                    }
                    catch { isCertValid = false; }

                    string validationInfo = $"Результат проверки сертификата: {(isCertValid ? "Успешно" : "Неуспешно")}";
                    parentNode.Nodes.Add(new TreeNode(validationInfo));

                    // Timestamp validation
                    string timeStampInfo = SignerUtils.VerifyTimeStamp(signer);
                    parentNode.Nodes.Add(new TreeNode(timeStampInfo));

                    treeView1.Nodes.Add(parentNode);
                }
            }
            treeView1.ExpandAll();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
            aboutForm.Dispose();

        }
    }
}
