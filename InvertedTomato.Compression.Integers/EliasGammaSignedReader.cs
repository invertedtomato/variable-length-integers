﻿using System;
using System.IO;

namespace InvertedTomato.Compression.Integers {
    /// <summary>
    /// Reader for Elias Gamma universal coding adapted for signed values.
    /// </summary>
    public class EliasGammaSignedReader : ISignedReader {
        /// <summary>
        /// Read first value from a byte array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long ReadOneDefault(byte[] input) {
            if (null == input) {
                throw new ArgumentNullException("input");
            }

            using (var stream = new MemoryStream(input)) {
                using (var reader = new EliasGammaSignedReader(stream)) {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        /// If it's disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// The underlying unsigned reader.
        /// </summary>
        private readonly EliasGammaUnsignedReader Underlying;

        /// <summary>
        /// Standard instantiation.
        /// </summary>
        /// <param name="input"></param>
        public EliasGammaSignedReader(Stream input) {
            Underlying = new EliasGammaUnsignedReader(input);
        }

        /// <summary>
        /// Read the next value. 
        /// </summary>
        /// <returns></returns>
        public long Read() {
            return ZigZag.Decode(Underlying.Read());
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (IsDisposed) {
                return;
            }
            IsDisposed = true;

            Underlying.Dispose();

            if (disposing) {
                // Dispose managed state (managed objects)
                Underlying.DisposeIfNotNull();
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }
    }
}