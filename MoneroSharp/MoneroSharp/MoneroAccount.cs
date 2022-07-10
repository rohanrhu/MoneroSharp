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
using System.Numerics;
using System.Text;
using System.Linq;

using MoneroSharp.Utils;
using MoneroSharp.NaCl.Internal.Ed25519Ref10;

namespace MoneroSharp
{
    public class MoneroAccount
    {
        MoneroNetwork Network = MoneroNetwork.TESTNET;

        public byte[] PrivateSeed;
        
        public byte[] SecretSpendKey;
        public byte[] SecretViewKey;
        public byte[] PublicSpendKey;
        public byte[] PublicViewKey;
        public byte[] PublicAddress;
        
#nullable enable
        public string SecretSpendKeyHex;
        public string SecretViewKeyHex;
        public string PublicSpendKeyHex;
        public string PublicViewKeyHex;
        public string PublicAddressHex;
#nullable disable
        
        public MoneroAccount(byte[] private_seed, MoneroNetwork network = MoneroNetwork.TESTNET)
        {
            SetPrivateSeed(private_seed);
            SetNetwork(network);
            DeriveKeys();
        }

        public MoneroAccount(string private_key_hex, MoneroNetwork network = MoneroNetwork.TESTNET)
        {
            SetPrivateSeed(private_key_hex);
            SetNetwork(network);
            DeriveKeys();
        }
        
        public MoneroAccount(string[] words, WordList.Languages language = WordList.Languages.English, MoneroNetwork network = MoneroNetwork.TESTNET)
        {
            SetPrivateSeed(DecodeMnemonics(words, language));
            SetNetwork(network);
            DeriveKeys();
        }
        
        public MoneroAccount(string words, WordList.Languages language = WordList.Languages.English, MoneroNetwork network = MoneroNetwork.TESTNET)
        {
            SetPrivateSeed(DecodeMnemonics(words.Split(' '), language));
            SetNetwork(network);
            DeriveKeys();
        }

        public static string DecodeMnemonics(string[] words, WordList.Languages language)
        {
            if (language != WordList.Languages.English) {
                throw new NotImplementedException("Word list language is not implemented!");
            }
            
            string private_key_hex = "";

            int prefix_length = 0;
            string[] wordlist = null;
            string[] trunc_wordlist = null;

            if (language == WordList.Languages.English) {
                wordlist = WordList.English.Words;
                trunc_wordlist = WordList.English.TruncWords;
                prefix_length = WordList.English.PrefixLength;
            }

            string checksum_word = words.Last();
            words = words.Take(words.Length - 1).ToArray();

            for (int i = 0; i < words.Length; i += 3) {
                int w1 = Array.IndexOf(trunc_wordlist, words[i].Substring(0, prefix_length));
                if (w1 == -1)
                    throw new Exception("Invalid mnemonic!");
                int w2 = Array.IndexOf(trunc_wordlist, words[i + 1].Substring(0, prefix_length));
                if (w2 == -1)
                    throw new Exception("Invalid mnemonic!");
                int w3 = Array.IndexOf(trunc_wordlist, words[i + 2].Substring(0, prefix_length));
                if (w3 == -1)
                    throw new Exception("Invalid mnemonic!");

                ulong x = (ulong)w1 + (ulong)wordlist.Length * ((((ulong)wordlist.Length - (ulong)w1) + (ulong)w2) % (ulong)wordlist.Length) + (ulong)wordlist.Length * (ulong)wordlist.Length * ((((ulong)wordlist.Length - (ulong)w2) + (ulong)w3) % (ulong)wordlist.Length);
                if ((x % (ulong)wordlist.Length) != (ulong)w1)
                    throw new Exception("Mnemonic decoding failed!");
                
                string padded = "0000000" + string.Format("{0:X}", x).ToLower();
                padded = padded.Substring(padded.Length - 8);
                string hex = padded.Substring(6, 2) + padded.Substring(4, 2) + padded.Substring(2, 2) + padded.Substring(0, 2);

                private_key_hex += hex;
            }

            return private_key_hex;
        }

        public void SetPrivateSeed(byte[] private_seed)
        {
            PrivateSeed = private_seed;
        }
        
        public void SetPrivateSeed(string private_key_hex)
        {
            PrivateSeed = BigInteger.Parse(
                private_key_hex,
                System.Globalization.NumberStyles.AllowHexSpecifier
            ).ToByteArray().Reverse().ToArray();
        }

        public void SetNetwork(MoneroNetwork network)
        {
            if (Network == network) {
                return;
            }

            Network = network;
            DeriveKeys();
        }

        private void DeriveKeys()
        {
            var keccak256 = new Nethereum.Util.Sha3Keccack();
            var pkey_hash = keccak256.CalculateHash(PrivateSeed);

            byte[] pkey_padded = new byte[64];
            Buffer.BlockCopy(PrivateSeed, 0, pkey_padded, 0, PrivateSeed.Length);

            byte[] pkey_hash_padded = new byte[64];
            Buffer.BlockCopy(pkey_hash, 0, pkey_hash_padded, 0, pkey_hash.Length);

            SecretSpendKey = pkey_padded.ToArray();
            ScalarOperations.sc_reduce(SecretSpendKey);
            SecretSpendKey = SecretSpendKey.Take(32).ToArray();

            SecretViewKey = pkey_hash_padded.ToArray();
            ScalarOperations.sc_reduce(SecretViewKey);
            SecretViewKey = SecretViewKey.Take(32).ToArray();

            PublicSpendKey = new byte[32];
            GroupElementP3 ps_ge_p3;
            GroupOperations.ge_scalarmult_base(out ps_ge_p3, PrivateSeed, 0);
            GroupOperations.ge_p3_tobytes(PublicSpendKey, 0, ref ps_ge_p3);

            PublicViewKey = new byte[32];
            GroupElementP3 pv_ge_p3;
            GroupOperations.ge_scalarmult_base(out pv_ge_p3, SecretViewKey, 0);
            GroupOperations.ge_p3_tobytes(PublicViewKey, 0, ref pv_ge_p3);

            SecretSpendKeyHex = MoneroUtils.BytesToHex(SecretSpendKey);
            SecretViewKeyHex = MoneroUtils.BytesToHex(SecretViewKey);
            PublicSpendKeyHex = MoneroUtils.BytesToHex(PublicSpendKey);
            PublicViewKeyHex = MoneroUtils.BytesToHex(PublicViewKey);

            string pub_key = (int)Network + MoneroUtils.BytesToHex(PublicSpendKey) + MoneroUtils.BytesToHex(PublicViewKey);
            string checksum = MoneroUtils.BytesToHex(keccak256.CalculateHash(MoneroUtils.HexBytesToBinary(pub_key))).Substring(0, 8).ToString();
            string pub_address_hex = pub_key + checksum;
            byte[] pub_address_hex_bytes = MoneroUtils.HexBytesToBinary(pub_address_hex);
            PublicAddress = Base58.Encode(pub_address_hex_bytes);
            PublicAddressHex = Encoding.ASCII.GetString(PublicAddress);
        }
    }
}