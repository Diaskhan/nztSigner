using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Tsp;

namespace nztSigner.Utils
{
    public class SignerUtils
    {
        public static int CompareSignersBySigningTime(SignerInformation a, SignerInformation b)
        {
            DateTime? dateA = null, dateB = null;

            var attrA = a.SignedAttributes?[Org.BouncyCastle.Asn1.Cms.CmsAttributes.SigningTime];
            if (attrA != null)
                dateA = ParseAsn1UtcTimeString(attrA.AttrValues[0]?.ToString());

            var attrB = b.SignedAttributes?[Org.BouncyCastle.Asn1.Cms.CmsAttributes.SigningTime];
            if (attrB != null)
                dateB = ParseAsn1UtcTimeString(attrB.AttrValues[0]?.ToString());

            // Сортировка: сначала без даты, потом по возрастанию даты
            if (dateA == null && dateB == null) return 0;
            if (dateA == null) return 1;
            if (dateB == null) return -1;
            return dateA.Value.CompareTo(dateB.Value);
        }


        public static DateTime ParseAsn1UtcTimeString(string? utcTime)
        {
            // Формат: YYMMDDhhmmssZ
            if (string.IsNullOrWhiteSpace(utcTime) || utcTime.Length != 13 || utcTime[12] != 'Z')
                throw new FormatException("Некорректный формат времени ASN.1 UTC");

            int year = int.Parse(utcTime.Substring(0, 2));
            int month = int.Parse(utcTime.Substring(2, 2));
            int day = int.Parse(utcTime.Substring(4, 2));
            int hour = int.Parse(utcTime.Substring(6, 2));
            int minute = int.Parse(utcTime.Substring(8, 2));
            int second = int.Parse(utcTime.Substring(10, 2));

            // ASN.1 UTC Time: 1950-2049
            year += (year < 50) ? 2000 : 1900;

            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }

        public static string VerifyTimeStamp(SignerInformation signer)
        {
            try
            {
                var unsignedAttrs = signer.UnsignedAttributes;
                if (unsignedAttrs == null)
                    return "Метка времени отсутствует";

                var timeStampAttr = unsignedAttrs.FirstOrDefault();
                if (timeStampAttr == null)
                    return "Метка времени отсутствует";

                // The value is an ASN.1 encoded TimeStampToken (as per RFC 3161)
                var asn1 = timeStampAttr.AttrValues[0].ToAsn1Object();
                var encoded = asn1.GetEncoded();
                var timeStampToken = new TimeStampToken(new CmsSignedData(encoded));

                // Get timestamp time
                var tsaTime = timeStampToken.TimeStampInfo.GenTime;

                //Verify TSP certificate
                var tsaCerts = timeStampToken.GetCertificates();
                var tsaSigner = timeStampToken.SignerID;
                var tsaCert = tsaCerts.EnumerateMatches(tsaSigner).OfType<Org.BouncyCastle.X509.X509Certificate>().FirstOrDefault();
                if (tsaCert == null)
                    return "Сертификат TSP не найден";

                try
                {
                    tsaCert.CheckValidity(tsaTime);
                    return $"Результат проверки TSP:Успешно";
                }
                catch (Exception)
                {
                    return "Сертификат TSP недействителен на момент подписи";
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка проверки метки времени: {ex.Message}";
            }
        }


        public static Dictionary<string, string> ParseSubjectDN(string subjectDN)
        {
            // Разбиваем по запятым, затем по знаку равенства
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var part in subjectDN.Split(','))
            {
                var trimmed = part.Trim();
                var idx = trimmed.IndexOf('=');
                if (idx > 0)
                {
                    var key = trimmed.Substring(0, idx).Trim();
                    var value = trimmed.Substring(idx + 1).Trim();
                    dict[key] = value;
                }
            }
            return dict;
        }
    }
}
