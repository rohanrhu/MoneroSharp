/*
 * MoneroSharp, .Net library for Monero
 * 
 * Github: https://github.com/rohanrhu/MoneroSharp
 * 
 * Copyright (C) 2022, Tabby Labs Inc.
 * Copyright (C) 2022, Oğuzhan Eroğlu <rohanrhu2@gmail.com> (https://oguzhaneroglu.com)
 *
 * Licensed under MIT (See LICENSE file)
 */

using System;
using System.Text;

namespace MoneroSharp.Utils {
     static class MoneroUtils
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
     }
 }