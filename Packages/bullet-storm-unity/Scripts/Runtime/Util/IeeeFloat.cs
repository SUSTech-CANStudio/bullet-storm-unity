using System;

namespace CANStudio.BulletStorm.Util
{
    /// <summary>
    ///     This class contains a float value that you can parse it in IEEE form.
    /// </summary>
    public struct IeeeFloat : IComparable<float>, IEquatable<float>, IComparable<IeeeFloat>, IEquatable<IeeeFloat>
    {
        private uint bits;

        private const uint MaskSign = 0x80_00_00_00;
        private const uint MaskExponent = 0x7F_80_00_00;
        private const uint MaskFraction = 0x00_7F_FF_FF;

        public static readonly IeeeFloat Zero = 0f;

        /// <summary>
        ///     The float value.
        /// </summary>
        public float Value
        {
            get => BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
            set => bits = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
        }

        /// <summary>
        ///     The highest 1 bit (31).
        /// </summary>
        public bool Sign
        {
            get => bits >> 31 == 1;
            set => bits = value ? bits | MaskSign : bits & ~MaskSign;
        }

        /// <summary>
        ///     8 bits (30 ~ 23).
        ///     Value from 1 to 254, 0 and 255 are reserved.
        ///     This value minus 127 is the real exponent in math.
        /// </summary>
        public byte Exponent
        {
            get => (byte) ((bits & MaskExponent) >> 23);
            set => bits = ((uint) value << 23) | (bits & ~MaskExponent);
        }

        /// <summary>
        ///     23 bits (22 ~ 0).
        ///     Fraction of float value, should not exceed <see cref="MaskFraction" />
        /// </summary>
        /// <exception cref="OverflowException"></exception>
        public uint Fraction
        {
            get => bits & MaskFraction;
            set
            {
                if (value > MaskFraction)
                    throw new OverflowException($"Value {value} exceeded max value {MaskFraction}");
                bits = value | (bits & ~MaskFraction);
            }
        }

        public IeeeFloat(float value) : this()
        {
            Value = value;
        }

        /// <summary>
        /// </summary>
        /// <param name="negative"></param>
        /// <param name="exponent"></param>
        /// <param name="fraction"></param>
        /// <exception cref="OverflowException"></exception>
        public IeeeFloat(bool negative, byte exponent, uint fraction)
        {
            if (fraction > MaskFraction)
                throw new OverflowException($"Value {fraction} exceeded max value {MaskFraction}");
            bits = ((negative ? 1U : 0U) << 31) | ((uint) exponent << 23) | fraction;
        }

        public bool Equals(float other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(float other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(IeeeFloat other)
        {
            return Value.Equals(other.Value);
        }

        public int CompareTo(IeeeFloat other)
        {
            return Value.CompareTo(other.Value);
        }

        public static implicit operator float(IeeeFloat t)
        {
            return t.Value;
        }

        public static implicit operator IeeeFloat(float f)
        {
            return new IeeeFloat(f);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"IeeeFloat{{exp: {Exponent}, frac: {Fraction}, value: {Value}}}";
        }
    }
}