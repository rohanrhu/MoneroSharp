/*
 * MoneroSharp, .Net library for Monero
 * 
 * Github: https://github.com/rohanrhu/MoneroSharp
 * 
 * Copyright (C) 2022, Tabby Labs Inc.
 * Author: Oğuzhan Eroğlu <rohanrhu2@gmail.com> (https://oguzhaneroglu.com)
 *
 * Licensed under MIT (See LICENSE file)
 */

/*
 * Monero-specific Base58 Encoding
 * Encodes data in 8-byte blocks, except the last block which is the remaining 8 (or less) bytes.
 * 8-byte blocks convert to 11 (or less) Base58 characters which is the ASCII range between [49-90].
 */

using System;
using System.Linq;

namespace MoneroSharp {
    public static class Base58
    {
        public static readonly char[] Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz".ToCharArray();
        public static readonly int[] EncodedBlockSizes = {0, 2, 3, 5, 6, 7, 9, 10, 11};

        public static void EncodeBlock(byte[] data, int index, byte[] encoded)
        {
            byte[] raw = data.Skip(index * 8).Take(8).ToArray();
            raw = raw.Reverse().ToArray();
            int block_len = raw.Length;
            if ((block_len < 1) || (block_len > 11)) {
                throw new Exception("Invalid block size!");
            }
            byte[] block = new byte[8];
            for (int i=0; i < raw.Length; i++) {
                block[i] = raw[i];
            }
            for (int i=raw.Length; i < block_len; i++) {
                block[i] = 0;
            }

            ulong scalar = BitConverter.ToUInt64(block, 0);
            int j = EncodedBlockSizes[block_len] - 1;
            while (scalar > 0) {
                int remainder = (int)(scalar % 58);
                scalar = scalar / 58;
                encoded[index * 11 + j] = (byte)(Alphabet[remainder]);
                j--;
            }
        }

        public static byte[] Encode(byte[] data)
        {
            int blocks_length = data.Length / 8;
            int remaining_length = data.Length % 8;

            int encoded_length = (data.Length / 8) * 11 + EncodedBlockSizes[remaining_length];
            byte[] encoded = new byte[encoded_length];
            
            for (int i=0; i < encoded.Length; i++) {
                encoded[i] = (byte)(Alphabet[0]);
            }
            for (int i=0; i < blocks_length; i++) {
                EncodeBlock(data, i, encoded);
            }

            if (remaining_length > 0) {
                EncodeBlock(data, blocks_length, encoded);
            }
            
            return encoded;
        }
        
        public static void DecodeBlock(byte[] data, int index, byte[] decoded)
        {
            byte[] raw = data.Skip(index * 11).Take(11).ToArray();
            int block_length = Array.IndexOf(EncodedBlockSizes, raw.Length);

            ulong scalar = 0;
            ulong order = 1;
            for (int i=raw.Length-1; i >= 0; i--) {
                int digit = Array.IndexOf(Alphabet, (char)raw[i]);
                if (digit < 0) {
                    throw new Exception("Invalid digit!");
                }
                
                ulong product = order * (ulong)digit + scalar;
                if (product == UInt64.MaxValue) {
                    throw new Exception("Overflow!");
                }

                scalar = product;
                order = order * (ulong)Alphabet.Length;
            }

            byte[] decoded_block = BitConverter.GetBytes(scalar).Reverse().ToArray();
            
            int j = 0;
            for (int i=0; i < decoded_block.Length; i++) {
                if (decoded_block[i] == 0) {
                    continue;
                }
                decoded[index * 8 + j++] = decoded_block[i];
            }
        }

        public static byte[] Decode(byte[] data)
        {
            int blocks_length = (int)Math.Floor(data.Length / 11.0);
            int remaining_length = data.Length % 11;
            int last_block_decoded_length = Array.IndexOf(EncodedBlockSizes, remaining_length);
            if (last_block_decoded_length < 0) {
                throw new Exception("Invalid encoded size!");
            }

            int decoded_length = blocks_length * 8 + last_block_decoded_length;
            byte[] decoded = new byte[decoded_length];
            for (int i=0; i < blocks_length; i++) {
                DecodeBlock(data, i, decoded);
            }
            if (remaining_length > 0) {
                DecodeBlock(data, blocks_length, decoded);
            }
            
            return decoded;
        }
    }
}