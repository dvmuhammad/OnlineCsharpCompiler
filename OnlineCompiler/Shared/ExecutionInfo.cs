using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCompiler.Shared
{
    public class ExecutionInfo
    {
        public enum ExecutionStatus
        {
            Preparing,
            Compiling,
            CompilationError,
            Running,
            Finished
        }

        public ExecutionStatus Status { get; set; }
        public string Output { get; set; }

        /// <summary>
        /// Конструктор для десериализации
        /// </summary>
        [System.Text.Json.Serialization.JsonConstructor]
        public ExecutionInfo(){ this.Output = String.Empty; }
        public ExecutionInfo(ExecutionStatus resultStatus, string Output = "")
        {
            this.Status = resultStatus;
            this.Output = Output;
        }
    }
}
