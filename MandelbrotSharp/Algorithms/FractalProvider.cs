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
using MandelbrotSharp.Numerics;

namespace MandelbrotSharp.Algorithms
{
    public interface IFractalProvider<TNumber> : IAlgorithmProvider<Complex<TNumber>, PointData>
        where TNumber : struct
    {
        Rectangle<TNumber> GetOutputBounds(Number<TNumber> aspectRatio);
    }

    public abstract class FractalProvider<TNumber, TParam> : 
        AlgorithmProvider<Complex<TNumber>, PointData, TParam>, 
        IFractalProvider<TNumber>
        where TParam : FractalParams<TNumber>
        where TNumber : struct
    {
        public abstract Rectangle<TNumber> GetOutputBounds(Number<TNumber> aspectRatio);
    }
}
