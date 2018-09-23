﻿using ManagedCuda;
using ManagedCuda.VectorTypes;
using Mandelbrot.Imaging;
using Mandelbrot.Mathematics;
using Mandelbrot.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot.Algorithms
{
    class TraditionalAlgorithmProvider<T> : IAlgorithmProvider<T>
    {
        private IGenericMath<T> TMath;

        private CudaKernel gpuKernel;

        private T Zero;
        private T Two;
        private T Four;

        private T offsetX;
        private T offsetY;

        private int MaxIterations;

        public void Init(IGenericMath<T> TMath, decimal offsetX, decimal offsetY, int maxIterations)
        {
            this.TMath = TMath;
            MaxIterations = maxIterations;

            this.offsetX = TMath.fromDecimal(offsetX);
            this.offsetY = TMath.fromDecimal(offsetY);

            Zero = TMath.fromInt32(0);
            Two = TMath.fromInt32(2);
            Four = TMath.fromInt32(4);
        }

        public PixelData<T> Run(T px, T py)
        {
            T x0 = TMath.Add(px, offsetX);
            T y0 = TMath.Add(py, offsetY);

            // Initialize some variables..
            T x = Zero;
            T y = Zero;

            // Define x squared and y squared as their own variables
            // To avoid unnecisarry multiplication.
            T xx = Zero;
            T yy = Zero;

            // Initialize our iteration count.
            int iter = 0;

            // Mandelbrot algorithm
            while (TMath.LessThan(TMath.Add(xx, yy), Four) && iter < MaxIterations)
            {
                // xtemp = xx - yy + x0
                T xtemp = TMath.Add(TMath.Subtract(xx, yy), x0);
                // ytemp = 2 * x * y + y0
                T ytemp = TMath.Add(TMath.Multiply(Two, TMath.Multiply(x, y)), y0);

                if (TMath.EqualTo(x, xtemp) && TMath.EqualTo(y, ytemp))
                {
                    iter = MaxIterations;
                    break;
                }

                x = xtemp;
                y = ytemp;
                xx = TMath.Multiply(x, x);
                yy = TMath.Multiply(y, y);

                iter++;
            }
            return new PixelData<T>(TMath.Add(xx, yy), iter, iter < MaxIterations);
        }

        public void GPUInit(CudaContext ctx, byte[] ptxImage, dim3 gridDim, dim3 blockDim)
        {
            gpuKernel = ctx.LoadKernelPTX(ptxImage, "traditional");

            gpuKernel.GridDimensions = gridDim;
            gpuKernel.BlockDimensions = blockDim;
        }

        public void GPUPreFrame() { return; }

        public void GPUPostFrame() { return; }

        public void GPUCell(
            CudaDeviceVariable<int> dev_image,
            CudaDeviceVariable<int> dev_palette,
            int cell_x, int cell_y,
            int cellWidth, int cellHeight,
            int totalCells_x, int totalCells_y,
            double xMax, double yMax,
            int chunkSize, int maxChunkSize)
        {
            gpuKernel.Run(
                dev_image.DevicePointer,
                dev_palette.DevicePointer,
                dev_palette.Size,
                cell_x, cell_y,
                cellWidth, cellHeight,
                totalCells_x, totalCells_y,
                xMax, yMax,
                offsetX, offsetY,
                MaxIterations, 
                chunkSize, maxChunkSize);
        }
    }
}