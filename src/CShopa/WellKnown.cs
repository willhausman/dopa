namespace CShopa;

// https://github.com/open-policy-agent/opa/blob/main/docs/content/wasm.md
public static class WellKnown
{
    public static class Global
    {
        public const string opa_wasm_abi_version = "opa_wasm_abi_version";

        public const string opa_wasm_abi_minor_version = "opa_wasm_abi_minor_version";
    }

    public static class Export
    {
        public const string eval = "eval";

        public const string builtins = "builtins";

        public const string entrypoints = "entrypoints";

        public const string opa_eval_ctx_new = "opa_eval_ctx_new";

        public const string opa_eval_ctx_set_input = "opa_eval_ctx_set_input";

        public const string opa_eval_ctx_set_data = "opa_eval_ctx_set_data";

        public const string opa_eval_ctx_set_entrypoint = "opa_eval_ctx_set_entrypoint";

        public const string opa_eval_ctx_get_result = "opa_eval_ctx_get_result";

        public const string opa_malloc = "opa_malloc";

        public const string opa_free = "opa_free";

        public const string opa_json_parse = "opa_json_parse";

        public const string opa_value_parse = "opa_value_parse";

        public const string opa_json_dump = "opa_json_dump";

        public const string opa_value_dump = "opa_value_dump";

        public const string opa_heap_ptr_set = "opa_heap_ptr_set";

        public const string opa_heap_ptr_get = "opa_heap_ptr_get";

        public const string opa_value_add_path = "opa_value_add_path";

        public const string opa_value_remove_path = "opa_value_remove_path";

        public const string opa_eval = "opa_eval";
    }

    public static class Imports
    {
        public const string Namespace = "env"; // The namespace for all imports

        public const string memory = "memory";

        public const string opa_abort = "opa_abort";

        public const string opa_println = "opa_println";

        public const string opa_builtin0 = "opa_builtin0";

        public const string opa_builtin1 = "opa_builtin1";

        public const string opa_builtin2 = "opa_builtin2";

        public const string opa_builtin3 = "opa_builtin3";

        public const string opa_builtin4 = "opa_builtin4";
    }

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
}
