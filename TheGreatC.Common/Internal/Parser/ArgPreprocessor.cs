using System;

namespace TheGreatC.Common.Internal.Parser
{
    public static class ArgPreprocessor
    {
        /// <summary>
        /// Implicitly converts the datatype of the arguments passed during the function call to match the datatype in the definition of the function
        /// </summary>
        /// <param name="requiredType"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object CoerceArgument(Type requiredType, string inputValue)
        {
            var requiredTypeCode = Type.GetTypeCode(requiredType);
            var exceptionMessage =
                $"Cannnot Coerce The Input Argument {inputValue} To Required Type {requiredType.Name}";

            object result;
            switch (requiredTypeCode)
            {
                case TypeCode.String:
                    result = inputValue;
                    break;

                case TypeCode.Int16:
                    if (short.TryParse(inputValue, out var number16))
                    {
                        result = number16;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.Int32:
                    if (int.TryParse(inputValue, out var number32))
                    {
                        result = number32;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.Int64:
                    if (long.TryParse(inputValue, out var number64))
                    {
                        result = number64;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.Boolean:
                    if (bool.TryParse(inputValue, out var trueFalse))
                    {
                        result = trueFalse;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.Byte:
                    if (byte.TryParse(inputValue, out var byteValue))
                    {
                        result = byteValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.Char:
                    if (char.TryParse(inputValue, out var charValue))
                    {
                        result = charValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.DateTime:
                    if (DateTime.TryParse(inputValue, out var dateValue))
                    {
                        result = dateValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.Decimal:
                    if (decimal.TryParse(inputValue, out var decimalValue))
                    {
                        result = decimalValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.Double:
                    if (double.TryParse(inputValue, out var doubleValue))
                    {
                        result = doubleValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.Single:
                    if (float.TryParse(inputValue, out var singleValue))
                    {
                        result = singleValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.UInt16:
                    if (ushort.TryParse(inputValue, out var uInt16Value))
                    {
                        result = uInt16Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.UInt32:
                    if (uint.TryParse(inputValue, out var uInt32Value))
                    {
                        result = uInt32Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                case TypeCode.UInt64:
                    if (ulong.TryParse(inputValue, out var uInt64Value))
                    {
                        result = uInt64Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }

                    break;

                default:
                    throw new ArgumentException(exceptionMessage);
            }

            return result;
        }
    }
}
