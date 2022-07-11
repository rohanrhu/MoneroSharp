# MoneroSharp

MoneroSharp is a .Net library for Monero.

## What can MoneroSharp do?

MoneroSharp is currently able to derive Monero keys (public/secret spend keys, public/secret view keys and public address)

## Recovering Monero account from mnemonics/words

```cs
var words = "howls cedar simplest enigma punch megabyte autumn family spiders utensils hazard wrap language toilet muppet jaded debut touchy roster speedy icon adhesive items kickoff utensils";
var account = new MoneroSharp.MoneroAccount(words, MoneroSharp.WordList.Languages.English, MoneroSharp.MoneroNetwork.MAINNET);
```

## Recovering Monero account from hexadecmial private key

```cs
var account = new MoneroSharp.MoneroAccount("c3b2836e92f540111f594fd2675d82205f0c534d4e80489be3b8b306db522d04", MoneroSharp.MoneroNetwork.MAINNET);
```

## Recovering Monero account from private seed bytes

```cs
using System;

var private_seed = new byte[64];
var random = new Random();
random.NextBytes(private_seed);

var account = new MoneroSharp.MoneroAccount(private_seed, MoneroSharp.MoneroNetwork.MAINNET);
```

## Retrieving Monero keys

```cs
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
```

## Encoding Private Seed to Mnemonics

To encode private seed into Monero words you can use `MoneroSharp.MoneroAccount.EncodeMnemonics()`.

```cs
using MoneroSharp;
using MoneroSharp.Utils;

byte[] private_seed = MoneroUtils.PrivateKeyToBytes("4e25d92060638d875517575c5bd285f2208c86390fa29f597c31f5ee3bccae0e");
string[] words = MoneroAccount.EncodeMnemonics(private_seed, MoneroSharp.WordList.Languages.English);

Console.WriteLine(string.Join(" ", words));
```

## Decoding Mnemonics to Private Seed

To decode private seed into Monero words you can use `MoneroSharp.MoneroAccount.DecodeMnemonics()`.

```cs
using MoneroSharp;
using MoneroSharp.Utils;

byte[] private_seed = MoneroAccount.DecodeMnemonics(words, MoneroSharp.WordList.Languages.English);

Console.WriteLine("Private key from words:");
Console.WriteLine(MoneroUtils.PrivateSeedToHex(private_seed));
```

## Supported .Net Targets

MoneroSharp supports most of .Net targets.

* .Net 6
* .Net 5
* .Net Standard 2.0
* .Net Standard 2.1
* .Net Framework 4.7.2
* .Net Framework 4.8

> **Warning**:
> You should specify `<LangVersion>8.0</LangVersion>` for .Net Framework 4.x and .Net Standard 2.0!
> This still doesn't mean you can use all newer .Net features that comes with C# 8.0 in .Net Framework 4.x/Standard 2.0 but it will work for syntax-specific new features in C# 8.0 which MoneroSharp just uses.

## Using in .Net Framework 4.x (Unity or Godot Engine)

.Net Framework 4.x is still being used in Godot Engine and Unity. You can use MoneroSharp in Godot or Unity, just need to specify C# version to `8.0` in `.csproj`.

```xml
<TargetFramework>net472</TargetFramework>
<LangVersion>8.0</LangVersion>
```

## License

Copyright (c) 2022 Tabby Labs Inc.

Author, 2022 Oğuzhan Eroğlu &lt;rohanrhu2@gmail.com&gt;

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Thirdparty Licenses:

* MoneroSharp/NaCl/LICENSE
