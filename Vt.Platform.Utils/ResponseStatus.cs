namespace Vt.Platform.Utils
{
    public enum ResponseStatus
    {
        /// <summary>
        /// Operation completed successfully
        /// </summary>
        Successful = 0,
        /// <summary>
        /// Operation completed successfully and resource was created
        /// </summary>
        Created = 10,
        /// <summary>
        /// Operation completed successfully and previously created resource was returned
        /// </summary>
        FulfilledByExisting = 20,
        /// <summary>
        /// Operation completed and was queued for further processing
        /// </summary>
        Queued = 30,
        /// <summary>
        /// Operation failed with validation errors the originator can inspect the errors
        /// meta data and try again
        /// </summary>
        ValidationError = 40,

        /// <summary>
        /// Operation failed as the user was not authenticated to perform the specified action
        /// </summary>
        AuthenticationError = 45,
        /// <summary>
        /// Operation failed due to upstream service that timed out. Operation should be
        /// attempted again with the request model
        /// </summary>
        UpstreamTimeoutError = 50,
        /// <summary>
        /// Operation failed with a transient error and should be
        /// attempted again with the request model
        /// </summary>
        TransientError = 60,
        /// <summary>
        /// Operation failed with a permanent error and should NOT
        /// be attempted against with provided request model
        /// </summary>
        PermanentError = 70,
        /// <summary>
        /// Operation failed as the resource requested was not found.
        /// </summary>
        ResourceNotFound = 80
    }
}