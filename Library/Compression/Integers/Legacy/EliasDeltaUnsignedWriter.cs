﻿using InvertedTomato.IO;
using InvertedTomato.IO.Bits;
using System;
using System.IO;

namespace InvertedTomato.Compression.Integers {
    /// <summary>
    /// Reader for Elias Delta universal coding for unsigned values.
    /// </summary>
    
    public class EliasDeltaUnsignedWriter : IUnsignedWriter, IDisposable {
        /// <summary>
        /// Write a given value.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static byte[] WriteOneDefault(ulong value) {
            using (var stream = new MemoryStream()) {
                using (var writer = new EliasDeltaUnsignedWriter(stream)) {
                    writer.Write(value);
                }
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Calculate the length of an encoded value in bits.
        /// </summary>
        /// <param name="allowZeros">(non-standard) Support zeros by automatically offsetting all values by one.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? CalculateBitLength(ulong value) {
            var result = 0;

            // Offset for zero
            value++;

            // #1 Separate X into the highest power of 2 it contains (2N) and the remaining N binary digits.
            byte n = 0;
            while (Math.Pow(2, n + 1) <= value) {
                n++;
            }
            var r = value - (ulong)Math.Pow(2, n);

            // #2 Encode N+1 with Elias gamma coding.
            var np = (byte)(n + 1);
            var len = BitOperation.CountUsed(np);
            result += len - 1;
            result += len;

            // #3 Append the remaining N binary digits to this representation of N+1.
            result += n;

            return result;
        }

        /// <summary>
        /// If disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Underlying stream to be writing encoded values to.
        /// </summary>
        private readonly BitWriter Output;

        /// <summary>
        /// Standard instantiation.
        /// </summary>
        /// <param name="output"></param>
        public EliasDeltaUnsignedWriter(Stream output) {
            if (null == output) {
                throw new ArgumentNullException("output");
            }

            Output = new BitWriter(output);
        }

        /// <summary>
        /// Append value to stream.
        /// </summary>
        /// <param name="value"></param>
        public void Write(ulong value) {
            if (IsDisposed) {
                throw new ObjectDisposedException("this");
            }

            // Offset value to allow zeros
            value++;

            // #1 Separate X into the highest power of 2 it contains (2N) and the remaining N binary digits.
            int n = 0;
            while (Math.Pow(2, n + 1) <= value) {
                n++;
            }
            var r = value - (ulong)Math.Pow(2, n);

            // #2 Encode N+1 with Elias gamma coding.
            var np = (ulong)n + 1;
            var len = BitOperation.CountUsed(np);
            Output.Write(0, len - 1);
            Output.Write(np, len);

            // #3 Append the remaining N binary digits to this representation of N+1.
            Output.Write(r, n);
        }

        /// <summary>
        /// Flush any unwritten bits and dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (IsDisposed) {
                return;
            }
            IsDisposed = true;

            if (disposing) {
                // Dispose managed state (managed objects)
                Output?.Dispose();
            }
        }

        /// <summary>
        /// Flush any unwritten bits and dispose.
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose() {
            Dispose(true);
        }
    }
}
