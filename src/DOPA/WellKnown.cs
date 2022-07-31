namespace DOPA;

/// <summary>
/// https://github.com/open-policy-agent/opa/blob/main/docs/content/wasm.md
/// </summary>
public static class WellKnown
{
    /// <summary>
    /// Global variables in the wasm module.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// The ABI major version.
        /// </summary>
        public const string opa_wasm_abi_version = "opa_wasm_abi_version";

        /// <summary>
        /// The ABI minor version.
        /// </summary>
        public const string opa_wasm_abi_minor_version = "opa_wasm_abi_minor_version";
    }

    /// <summary>
    /// Exports from the wasm module.
    /// </summary>
    public static class Export
    {
        /// <summary>
        /// The eval func.
        /// </summary>
        public const string eval = "eval";

        /// <summary>
        /// The builtins map.
        /// </summary>
        public const string builtins = "builtins";

        /// <summary>
        /// The entrypoints map.
        /// </summary>
        public const string entrypoints = "entrypoints";

        /// <summary>
        /// Start a new eval context.
        /// </summary>
        public const string opa_eval_ctx_new = "opa_eval_ctx_new";

        /// <summary>
        /// Set input on an eval context.
        /// </summary>
        public const string opa_eval_ctx_set_input = "opa_eval_ctx_set_input";

        /// <summary>
        /// Set data on an eval context.
        /// </summary>
        public const string opa_eval_ctx_set_data = "opa_eval_ctx_set_data";

        /// <summary>
        /// Choose an entrypoint on an eval context.
        /// </summary>
        public const string opa_eval_ctx_set_entrypoint = "opa_eval_ctx_set_entrypoint";

        /// <summary>
        /// Read result from eval.
        /// </summary>
        public const string opa_eval_ctx_get_result = "opa_eval_ctx_get_result";

        /// <summary>
        /// Reserve memory in the shared buffer.
        /// </summary>
        public const string opa_malloc = "opa_malloc";

        /// <summary>
        /// Release memory in the shared buffer.
        /// </summary>
        public const string opa_free = "opa_free";

        /// <summary>
        /// After writing to the shared buffer, tell the module to load the json into its process.
        /// </summary>
        public const string opa_json_parse = "opa_json_parse";

        /// <summary>
        /// Export json to the shared buffer for reading.
        /// </summary>
        public const string opa_json_dump = "opa_json_dump";

        /// <summary>
        /// Set the heap pointer to a specific address.
        /// </summary>
        public const string opa_heap_ptr_set = "opa_heap_ptr_set";

        /// <summary>
        /// Get the current heap pointer.
        /// </summary>
        public const string opa_heap_ptr_get = "opa_heap_ptr_get";

        /// <summary>
        /// The fast-eval func.
        /// </summary>
        public const string opa_eval = "opa_eval";
    }

    /// <summary>
    /// Imports to the wasm module.
    /// </summary>
    public static class Imports
    {
        /// <summary>
        /// The namespace for all imports.
        /// </summary>
        public const string Namespace = "env";

        /// <summary>
        /// The name of the shared buffer.
        /// </summary>
        public const string memory = "memory";

        /// <summary>
        /// A function to call on abort.
        /// </summary>
        public const string opa_abort = "opa_abort";

        /// <summary>
        /// A function to call on println.
        /// </summary>
        public const string opa_println = "opa_println";

        /// <summary>
        /// A custom function with 0 parameters.
        /// </summary>
        public const string opa_builtin0 = "opa_builtin0";

        /// <summary>
        /// A custom function with 1 parameter.
        /// </summary>
        public const string opa_builtin1 = "opa_builtin1";

        /// <summary>
        /// A custom function with 2 parameters.
        /// </summary>
        public const string opa_builtin2 = "opa_builtin2";

        /// <summary>
        /// A custom function with 3 parameters.
        /// </summary>
        public const string opa_builtin3 = "opa_builtin3";

        /// <summary>
        /// A custom function with 4 parameters.
        /// </summary>
        public const string opa_builtin4 = "opa_builtin4";
    }

    /// <summary>
    /// Error codes from the OPA module.
    /// </summary>
    public enum ErrorCode : int
    {
        /// <summary>No error.</summary>
        OPA_ERR_OK = 0,
        
        /// <summary>Unrecoverable internal error.</summary>
        OPA_ERR_INTERNAL = 1,
        
        /// <summary>Invalid value type was encountered.</summary>
        OPA_ERR_INVALID_TYPE = 2,

        /// <summary>Invalid object path reference.</summary>
        OPA_ERR_INVALID_PATh = 3,
    }

    /// <summary>
    /// Requirements to use the wasm module.
    /// </summary>
    public static class Requirements
    {
        /// <summary>
        /// The minimum size of the shared buffer.
        /// </summary>
        public const int MinimumMemorySize = 2;
    }

    /// <summary>
    /// Values to use in the wasm module.
    /// </summary>
    public static class Values
    {
        /// <summary>
        /// The default entrypoint.
        /// </summary>
        public const int DefaultEntrypointId = 0;
    }
}
