/*
 *  Copyright 2018-2019 Chosen Few Software
 *  This file is part of MandelbrotSharp.
 *
 *  MandelbrotSharp is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  MandelbrotSharp is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with MandelbrotSharp.  If not, see <https://www.gnu.org/licenses/>.
 */
using System;

namespace MandelbrotSharp.Numerics
{
    public interface IComplex
    {
        INumber Real { get; }
        INumber Imag { get; }

        Complex<TOut> As<TOut>() where TOut : struct;
    }
    public struct Complex<T> : IComplex, IEquatable<Complex<T>> where T : struct
    {
        public static readonly Complex<T> Zero         = new Complex<T>(Number<T>.Zero, Number<T>.Zero);
        public static readonly Complex<T> One          = new Complex<T>(Number<T>.One,  Number<T>.Zero);
        public static readonly Complex<T> ImaginaryOne = new Complex<T>(Number<T>.Zero, Number<T>.One);

        public Number<T> Real { get; }
        public Number<T> Imag { get; }

        INumber IComplex.Real => Real;
        INumber IComplex.Imag => Imag;

        public Complex(Number<T> real, Number<T> imag)
        {
            Real = real;
            Imag = imag;
        }

        public static implicit operator Complex<T>(T n)
        {
            return new Complex<T>(n, Number<T>.Zero);
        }

        public static implicit operator Complex<T>(Number<T> n)
        {
            return new Complex<T>(n, Number<T>.Zero);
        }

        public static Complex<T> operator +(Complex<T> value)
        {
            return value;
        }

        public static Complex<T> operator -(Complex<T> value)
        {
            return new Complex<T>(-value.Real, -value.Imag);
        }

        public static Complex<T> operator +(Complex<T> left, Complex<T> right)
        {
            return new Complex<T>(left.Real + right.Real, left.Imag + right.Imag);
        }

        public static Complex<T> operator -(Complex<T> left, Complex<T> right)
        {
            return new Complex<T>(left.Real - right.Real, left.Imag - right.Imag);
        }

        public static Complex<T> operator *(Complex<T> left, Complex<T> right)
        {
            // (a + bi)(c + di) = a(c + di) + bi(c + di) = ac + adi + bci - bd
            return new Complex<T>(left.Real * right.Real - left.Imag * right.Imag, left.Real * right.Imag + left.Imag * right.Real);
        }

        public static Complex<T> operator /(Complex<T> left, Complex<T> right)
        {
            Complex<T> conjugate = new Complex<T>(right.Real, -right.Imag);
            Complex<T> numerator = left * conjugate;
            Number<T> denominator = (right * conjugate).Real;
            return new Complex<T>(numerator.Real / denominator, numerator.Imag / denominator);
        }

        public static bool operator ==(Complex<T> left, Complex<T> right)
        {
            return left.Real == right.Real && left.Imag == right.Imag;
        }

        public static bool operator !=(Complex<T> left, Complex<T> right)
        {
            return left.Real != right.Real || left.Imag != right.Imag;
        }

        public static Number<T> AbsSqu(Complex<T> value)
        {
            return value.Real * value.Real + value.Imag * value.Imag;
        }

        public Complex<TOut> As<TOut>() where TOut : struct
        {
            return new Complex<TOut>(Real.As<TOut>(), Imag.As<TOut>());
        }

        public bool Equals(Complex<T> other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            return obj is Complex<T> && Equals((Complex<T>)obj);
        }

        public override int GetHashCode()
        {
            return 10280812 + (Real.GetHashCode() - Imag.GetHashCode());
        }
    }
}
