using System;
using System.Collections.Generic;
using System.Linq;

namespace InTheHand.Net.Obex
{
    public sealed class ObexAppParameters
    {
        private readonly Dictionary<byte, byte[]> _params;

        public ObexAppParameters()
        {
            _params = new Dictionary<byte, byte[]>();
        }

        public ObexAppParameters(byte[] rawBytes)
        {
            _params = new Dictionary<byte, byte[]>();

            if (rawBytes == null)
                return;

            for (int i = 0; i < rawBytes.Length;)
            {
                // Check for the absence of additional values
                if (rawBytes.Length - i < 2)
                    break;

                byte tag = rawBytes[i++];
                byte length = rawBytes[i++];

                // Ensure the detected length is available
                if (rawBytes.Length - i - length < 0)
                    break;

                byte[] value = new byte[length];
                Array.Copy(rawBytes, i, value, 0, length);

                Add(tag, value);

                i += length;
            }
        }

        public byte[] GetHeaderBytes()
        {
            int totalLength = _params.Keys.Sum(paramTag => _params[paramTag].Length + 2);
            byte[] returnHeaderBytes = new byte[totalLength];

            int idx = 0;
            foreach (byte paramTag in _params.Keys)
            {
                int length = _params[paramTag].Length;
                returnHeaderBytes[idx++] = paramTag;
                returnHeaderBytes[idx++] = (byte)length;
                Array.Copy(_params[paramTag], 0, returnHeaderBytes, idx, length);
                idx += length;
            }

            return returnHeaderBytes;
        }

        public void Add(byte tag, byte value)
        {
            _params[tag] = new[] { value };
        }

        public void Add(byte tag, byte[] value)
        {
            _params[tag] = value;
        }
    }
}