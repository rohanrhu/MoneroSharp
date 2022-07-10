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
using System.Linq;
using MoneroSharp;
using MoneroSharp.Utils;

class Program {
    public static void Main(string[] args)
    {   
        // Recover account from words
        
        string words = "worry irritate mural vocal bogeys peeled nudged muddy uphill rewind python pairing bubble cottage hotel boil teeming dented demonstrate moment lamb love pride rudely worry";

        var account = new MoneroAccount(
            words,
            MoneroSharp.WordList.Languages.English,
            MoneroSharp.MoneroNetwork.MAINNET
        );

        Console.WriteLine("Words:");
        Console.WriteLine(string.Join(" ", account.Words));
        Console.WriteLine("Sec Spend Key:");
        Console.WriteLine(account.SecretSpendKeyHex);
        Console.WriteLine("Sec View Key:");
        Console.WriteLine(account.SecretViewKeyHex);
        Console.WriteLine("Pub Spend Key:");
        Console.WriteLine(account.PublicSpendKeyHex);
        Console.WriteLine("Pub View Key:");
        Console.WriteLine(account.PublicViewKeyHex);
        Console.WriteLine("Pub Address:");
        Console.WriteLine(account.PublicAddressHex);
        
        // Recover account from hex private key
        
        account = new MoneroAccount("4e25d92060638d875517575c5bd285f2208c86390fa29f597c31f5ee3bccae0e", MoneroSharp.MoneroNetwork.MAINNET);

        Console.WriteLine("Words:");
        Console.WriteLine(string.Join(" ", account.Words));
        Console.WriteLine("Sec Spend Key:");
        Console.WriteLine(account.SecretSpendKeyHex);
        Console.WriteLine("Sec View Key:");
        Console.WriteLine(account.SecretViewKeyHex);
        Console.WriteLine("Pub Spend Key:");
        Console.WriteLine(account.PublicSpendKeyHex);
        Console.WriteLine("Pub View Key:");
        Console.WriteLine(account.PublicViewKeyHex);
        Console.WriteLine("Pub Address:");
        Console.WriteLine(account.PublicAddressHex);

        // Decode Monero mnemonics to private seed

        byte[] private_seed = MoneroAccount.DecodeMnemonics(
            words,
            MoneroSharp.WordList.Languages.English
        );

        Console.WriteLine("Private key from words:");
        Console.WriteLine(MoneroUtils.PrivateSeedToHex(private_seed));

        // Recover account from private seed

        account = new MoneroAccount(private_seed, MoneroNetwork.MAINNET);

        Console.WriteLine("Words:");
        Console.WriteLine(string.Join(" ", account.Words));
        Console.WriteLine("Sec Spend Key:");
        Console.WriteLine(account.SecretSpendKeyHex);
        Console.WriteLine("Sec View Key:");
        Console.WriteLine(account.SecretViewKeyHex);
        Console.WriteLine("Pub Spend Key:");
        Console.WriteLine(account.PublicSpendKeyHex);
        Console.WriteLine("Pub View Key:");
        Console.WriteLine(account.PublicViewKeyHex);
        Console.WriteLine("Pub Address:");
        Console.WriteLine(account.PublicAddressHex);

        // Encode private seed or private key to mnemonics

        private_seed = MoneroUtils.PrivateKeyToBytes("4e25d92060638d875517575c5bd285f2208c86390fa29f597c31f5ee3bccae0e");
        words = string.Join(
            " ",
            MoneroAccount.EncodeMnemonics(private_seed, MoneroSharp.WordList.Languages.English)
        );

        Console.WriteLine("Encoded words:");
        Console.WriteLine(words);

        Console.WriteLine("Meooowwww!!!!");
    }
}