using System.Globalization;
using System.Text;

namespace SecureFlow.Shared.Extensions
{
    public static class StringExtension
    {
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                }

                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        public static string ToBase64String(this Stream stream)
        {
            byte[] bytes;
            using (var st = stream)
            {
                bytes = new byte[st.Length];
                st.Read(bytes, 0, bytes.Length);
                st.Position = 0;
            }

            return Convert.ToBase64String(bytes);
        }

        public static DateTime GetExactDateTimeFromString(this string value)
        {
            DateTime parsedDate = DateTime.MinValue;
            bool isParse = false;

            isParse = DateTime.TryParse(value, out parsedDate);
            if (isParse) return parsedDate;

            string[] dateTimeFormats = new string[]
            {
                "yyyy-MM-dd",
                "MM/dd/yyyy",
                "dd/MM/yyyy",
                "yyyy-MM-dd HH:mm:ss",
                "MM/dd/yyyy HH:mm:ss",
                "dd/MM/yyyy HH:mm:ss",
                "dd-MMM-yy",
                "d-MMM-yy",
                "dd/MMM/yy",
                "d/MMM/yy",
                "dd/MMM/yyyy",
                "d/MMM/yyyy",
                "dd-MMM-yyyy",
                "d-MMM-yyyy"
            };

            isParse = DateTime.TryParseExact(value, dateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
            if (isParse) return parsedDate;

            return parsedDate;
        }

        public static double StringTodouble(this string? text)
        {
            if (string.IsNullOrEmpty(text)) return 0.0;
            return Convert.ToDouble(text);
        }

        public static decimal StringToDecimal(this string? text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            return Convert.ToDecimal(text);
        }

        public static decimal ConvertPercentageStringToDecimal(this string? text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            return Convert.ToDecimal(text.Replace("%", string.Empty));
        }
    }
}
