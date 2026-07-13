using System;

namespace IdleGame.Core.Identifiers
{
    public static class StableId
    {
        public static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value[0] < 'a' || value[0] > 'z')
            {
                return false;
            }

            for (var index = 1; index < value.Length; index++)
            {
                var character = value[index];
                var isLower = character >= 'a' && character <= 'z';
                var isDigit = character >= '0' && character <= '9';
                var isSeparator = character == '_';

                if (!isLower && !isDigit && !isSeparator)
                {
                    return false;
                }
            }

            return true;
        }

        public static void ThrowIfInvalid(string value, string parameterName)
        {
            if (!IsValid(value))
            {
                throw new ArgumentException($"Stable IDs must use lower_snake_case and start with a letter. Received '{value}'.", parameterName);
            }
        }
    }
}
