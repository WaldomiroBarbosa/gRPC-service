using System.IO;
using Grpc.Core;
using gRPCService;
using System.Threading.Tasks;

namespace gRPCService.Services
{
    public class UtilityServiceImpl : UtilityService.UtilityServiceBase
    {
        private readonly ILogger<UtilityServiceImpl> _logger;
        public UtilityServiceImpl(ILogger<UtilityServiceImpl> logger)
        {
            _logger = logger;
        }

        public override Task<OperationResult> Calculate(NumberOperands request, ServerCallContext context)
        {
            int result = 0;
            switch (request.Op_type)
            {
                case 1:
                    result = request.First_op + request.Second_op;
                    break;
                case 2:
                    result = request.First_op - request.Second_op;
                    break;
                case 3:
                    result = request.First_op * request.Second_op;
                    break;
                case 4:
                    if (request.Second_op != 0) 
                    {
                        result = request.First_op / request.Second_op;
                    }
                    else
                    {
                        result = 0; 
                    }
                    break;
                default:
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid operation type"));
            }

            return Task.FromResult(new OperationResult
            {
                Result = result;
            });
        }

        public override Task<StringResult> TransformString(StringMessage request, ServerCallContext context)
        {
            return Task.FromResult(new StringResult
            {
                Output = request.Input.ToUpper()
            });
        }


        public override Task<FileOperationResult> ModifyFile(FileOperationRequest request, ServerCallContext context)
        {
            bool operationDefinition;

            string content = request.Content;
            string relativePath = "../testfile.txt";
            string absolutePath = Path.GetFullPath(relativePath);

            try
            {
                File.WriteAllText(absolutePath, content);
                operationDefinition = true;
            }
            catch (System.Exception)
            {
                operationDefinition = false;
            }

            return Task.FromResult(new FileOperationResult
            {
                request.sucess = operationDefinition;
            });
        }
    }
}