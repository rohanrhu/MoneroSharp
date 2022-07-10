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

using System;
using System.Text;
using System.Linq;
using System.Numerics;

namespace MoneroSharp.Utils {
     public static class MoneroUtils
     {
        public static byte[] HexBytesToBinary(String hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i=0; i < hex.Length; i += 2) {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static string BytesToHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte _byte in bytes) {
                hex.AppendFormat("{0:x2}", _byte);
            }
            return hex.ToString();
        }

        public static byte[] PrivateKeyToBytes(string private_key_hex)
        {
            var private_seed = BigInteger.Parse(
                private_key_hex,
                System.Globalization.NumberStyles.AllowHexSpecifier
            ).ToByteArray().Reverse().ToArray();
            return private_seed;
        }

        public static string PrivateSeedToHex(byte[] private_seed)
        {
            var private_key = new BigInteger(private_seed.Reverse().ToArray());
            return private_key.ToString("X").ToLower();
        }
     }
 }