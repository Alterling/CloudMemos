using System;
using System.Text;

namespace CloudMemos.Logic.BusinessLogic
{
    public static class ByteConverter
    {
        public static string ToBase32String(byte[] source)
        {
            if (source.Length % 8 != 0)
            {
                throw new ArgumentException($"The length of source must be aligned to a multiple of 8 amount of bytes.");
            }

            const string base32AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var idStringBuilder = new StringBuilder();
            for (var i = 0; i < source.Length / 8; i++)
            {
                idStringBuilder.AppendFormat("{0,8:x8}", BitConverter.ToUInt64(source, i * 8));
            }

            var idString = idStringBuilder.ToString();
            var byteArray = new byte[idString.Length];
            for (var index = 0; index < idString.Length; index++)
            {
                var idChar = idString[index];
                byteArray[index] = idChar >= '0' && idChar <= '9' ? (byte) (idChar - '0') : (byte) (idChar - 'a' + 10);
            }

            var chars = new char[byteArray.Length / 2];
            for (var i = 0; i < chars.Length; i++)
            {
                var index = byteArray[i * 2] + byteArray[i * 2 + 1];
                chars[i] = base32AllowedCharacters[index];
            }

            return new string(chars);
        }
    }
}