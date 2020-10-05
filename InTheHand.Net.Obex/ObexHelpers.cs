using System.IO;

namespace InTheHand.Net.Obex
{
    public static class ObexHelpers
    {
        /// <summary>
        /// A wrapper for Stream.Read that blocks until the requested number of bytes
        /// have been read, and throw an exception if the stream is closed before that occurs.
        /// </summary>
        public static void StreamReadBlockMust(Stream stream, byte[] buffer, int offset, int size)
        {
            int numRead = StreamReadBlock(stream, buffer, offset, size);
            System.Diagnostics.Debug.Assert(numRead <= size);
            if (numRead < size) {
                throw new EndOfStreamException("Connection closed whilst reading an OBEX packet.");
            }
        }

        /// <summary>
        /// A wrapper for Stream.Read that blocks until the requested number of bytes
        /// have been read or the end of the Stream has been reached.
        /// Returns the number of bytes read.
        /// </summary>
        private static int StreamReadBlock(Stream stream, byte[] buffer, int offset, int size)
        {
            int numRead = 0;
            while (size - numRead > 0) {
                int curCount = stream.Read(buffer, offset + numRead, size - numRead);
                if (curCount == 0) { // EoF
                    break;
                }
                numRead += curCount;
            }
            System.Diagnostics.Debug.Assert(numRead <= size);
            return numRead;
        }
    }
}
