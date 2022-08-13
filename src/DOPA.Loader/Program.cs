using DOPA.Loader;

var e = new OpaExecutable();

var opaBundle = await e.Build("example.rego", "example/hello");
