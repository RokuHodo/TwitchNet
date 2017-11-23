// project namespaces
using System;

namespace
TwitchNet.Extensions
{
    internal static class
    MathExtensions
    {
        /// <summary>
        /// Clamps a comparable value to a minimum value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="minimum">The smallest allowable value.</param>
        /// <returns>
        /// Returns the minimum value when the clamped value is less than the minimum.
        /// Returns the original value otherwise.
        /// </returns>
        public static type
        ClampMin<type>(this type value, type minimum)
        where type : IComparable<type>
        {
            if (value.CompareTo(minimum) < 0)
            {
                value = minimum;
            }

            return value;
        }

        /// <summary>
        /// Clamps a comparable value to a minimum value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="minimum">The smallest allowable value.</param>
        /// <param name="default_value">The returned value if the original value is less than the minimum.</param>
        /// <returns>
        /// Returns the default value when the original value is less than the minimum.
        /// Returns the original value otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the default value is less than the minimum.</exception>
        public static type
        ClampMin<type>(this type value, type minimum, type default_value)
        where type : IComparable<type>
        {
            if (default_value.IsLessThan(minimum))
            {
                throw new ArgumentOutOfRangeException(nameof(default_value), default_value, nameof(default_value) + " must be greater or equal to the " + nameof(minimum));
            }

            if (value.CompareTo(minimum) < 0)
            {
                value = default_value.IsNull() ? minimum : default_value;
            }

            return value;
        }

        /// <summary>
        /// Clamps a comparable value to a maximum value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="maximum">The largest allowable value.</param>
        /// <returns>
        /// Returns the maximum value when the clamped value is greater than the minimum.
        /// Returns the original value otherwise.
        /// </returns>
        public static type
        ClampMax<type>(this type value, type maximum)
        where type : IComparable<type>
        {
            if (value.CompareTo(maximum) > 0)
            {
                value = maximum;
            }

            return value;
        }

        /// <summary>
        /// Clamps a comparable value to a maximum value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="maximum">The largest allowable value.</param>
        /// <param name="default_value">The returned value if the original value is greater than the maximum.</param>
        /// <returns>
        /// Returns the default value when the original value is greater than the maximum.
        /// Returns the original value otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the default value is greater than the maximum.</exception>
        public static type
        ClampMax<type>(this type value, type maximum, type default_value)
        where type : IComparable<type>
        {
            if (default_value.IsGreaterThan(maximum))
            {
                throw new ArgumentOutOfRangeException(nameof(default_value), default_value, nameof(default_value) + " must be less than or equal to the " + nameof(maximum));
            }

            if (value.CompareTo(maximum) > 0)
            {
                value = default_value.IsNull() ? maximum : default_value;
            }

            return value;
        }

        /// <summary>
        /// Clamps a comparable value between a minimum and maximum.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="minimum">The smallest allowable value.</param>
        /// <param name="maximum">The largest allowable value.</param>
        /// <returns>
        /// Returns the minimum value when the original value is less than the maximum.
        /// Returns the maximum value when the original value is greater than the maximum.
        /// Returns the original value otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the maximum value is less than the minimum.</exception>
        public static type
        Clamp<type>(this type value, type minimum, type maximum)
        where type : IComparable<type>
        {
            if (maximum.IsLessThan(minimum))
            {
                throw new ArgumentOutOfRangeException(nameof(maximum), maximum, nameof(maximum) + " must be greater than or equal to the " + nameof(minimum));
            }

            value = value.ClampMin(minimum);
            value = value.ClampMax(maximum);

            return value;
        }

        /// <summary>
        /// Clamps a comparable value between a minimum and maximum.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="minimum">The smallest allowable value.</param>
        /// <param name="maximum">The largest allowable value.</param>
        /// <param name="default_value">The returned value if the original value is less than the minimum or greater than the maximum.</param>
        /// <returns>
        /// Returns the default value when the original value is less than the maximum.
        /// Returns the default value when the original value is greater than the maximum.
        /// Returns the original value otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the default value is less than the minimum or greater than the maximum.</exception>
        public static type
        Clamp<type>(this type value, type minimum, type maximum, type default_value)
        where type : IComparable<type>
        {
            if (maximum.IsLessThan(minimum))
            {
                throw new ArgumentOutOfRangeException(nameof(maximum), maximum, nameof(maximum) + " must be greater than or equal to the " + nameof(minimum));
            }

            value = value.ClampMin(minimum, default_value);
            value = value.ClampMax(maximum, default_value);

            return value;
        }

        /// <summary>
        /// Checks if a comparable value is between two values.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="minimum">The smallest allowable value.</param>
        /// <param name="maximum">The largest allowable value.</param>
        /// <returns>
        /// Returns true is the value is between or equal to the minimum and maximum.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the maximum value is less than the minimum.</exception>
        public static bool
        IsInRange<type>(this type value, type minimum, type maximum)
        where type : IComparable<type>
        {
            bool result = false;

            if (maximum.IsLessThan(minimum))
            {
                throw new ArgumentOutOfRangeException(nameof(maximum), maximum, nameof(maximum) + " must be greater than or equal to the " + nameof(minimum));
            }

            if (value.IsLessThan(minimum))
            {
                return result;
            }

            if (value.IsGreaterThan(maximum))
            {
                return result;
            }

            result = true;

            return result;

        }

        #region Comparable operations

        /// <summary>
        /// Checks if a comparable value is equal to another value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="compare_to">The value to compare against.</param>
        /// <returns>
        /// Returns true if the value is equal to the other value.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsEqualTo<type>(this type value, type compare_to)
        where type : IComparable<type>
        {
            bool result = false;

            result = value.CompareTo(compare_to) == 0;

            return result;
        }

        /// <summary>
        /// Checks if a comparable value is less than another value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="compare_to">The value to compare against.</param>
        /// <returns>
        /// Returns true if the value is less than the other value.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsLessThan<type>(this type value, type compare_to)
        where type : IComparable<type>
        {
            bool result = false;

            result = value.CompareTo(compare_to) < 0;

            return result;
        }

        /// <summary>
        /// Checks if a comparable value is less than or equal to another value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="compare_to">The value to compare against.</param>
        /// <returns>
        /// Returns true if the value is less than or equal to the other value.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsLessOrEqualTo<type>(this type value, type compare_to)
        where type : IComparable<type>
        {
            bool result = false;

            result = value.IsLessThan(compare_to) || value.IsEqualTo(compare_to);

            return result;
        }

        /// <summary>
        /// Checks if a comparable value is greater than another value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="compare_to">The value to compare against.</param>
        /// <returns>
        /// Returns true if the value is greater than the other value.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsGreaterThan<type>(this type value, type compare_to)
        where type : IComparable<type>
        {
            bool result = false;

            result = value.CompareTo(compare_to) > 0;

            return result;
        }

        /// <summary>
        /// Checks if a comparable value is greater than or equal to another value.
        /// </summary>
        /// <typeparam name="type">The value's implicit type.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="compare_to">The value to compare against.</param>
        /// <returns>
        /// Returns true if the value is greater than or equal to the other value.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsGreaterOrEqualTo<type>(this type value, type compare_to)
        where type : IComparable<type>
        {
            bool result = false;

            result = value.IsGreaterThan(compare_to) || value.IsEqualTo(compare_to);

            return result;
        }

        #endregion
    }
}
