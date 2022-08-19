# DOPA

[![Build status](https://github.com/willhausman/dopa/actions/workflows/build.yml/badge.svg "Build status")](https://github.com/willhausman/dopa/actions/workflows/build.yml)
[![Latest version](https://img.shields.io/nuget/v/DOPA)](https://www.nuget.org/packages/DOPA)

A dotnet [OPA](https://www.openpolicyagent.org) client designed for applications to take advantage of its first-class [WebAssembly](https://webassembly.org) support.

DOPA is built with [Wasmtime](https://github.com/bytecodealliance/wasmtime), the same runtime used by OPA for WebAssembly.

## Usage

To start, you will need to compile your OPA policy for WebAssembly to get a `.wasm`. See OPA's documentation on [compiling policies](https://www.openpolicyagent.org/docs/latest/wasm/#compiling-policies) for more details. This document refers to [example.rego](./test/policies/example.rego).

Currently, the compiled wasm needs to be extracted from the output bundle.  This can be accomplished with `tar`.

```sh
$ opa build -t wasm -e example/hello example.rego
$ tar -xzf ./bundle.tar.gz /policy.wasm
```

With your compiled wasm available to your application, and named whatever you want, instantiate an `IOpaModule` from the file.

```csharp
using DOPA;

using IOpaModule module = WasmModule.FromFile("./example.wasm");
```

The module links the WebAssembly runtime to your application, and incurs an extra compilation cost that you don't want to repeat very often. It is also where you can configure things that don't change between your policy instances, such as a serializer.

Create an instance of `IOpaPolicy` to set any data, evaluate your policy, and start getting results.

```csharp
using IOpaPolicy policy = module.CreatePolicy();
policy.SetData(new { world = "hello" });
var allowed = policy.Evaluate<bool>(new { message = "hello" });
```

## Contributing

Pull requests and issues are appreciated and encouraged.

## License

DOPA is licensed under the [MIT License](./LICENSE).
