namespace MainTainSenseAPI
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string traceId = DateTimeOffset.Now.ToString("yyyyMMddHHmmssffff") + "-" + Guid.NewGuid().ToString("N");
            context.TraceIdentifier = traceId;

            await _next(context); // Call the next middleware or controller
        }
    }
}
