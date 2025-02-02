namespace OnlineShop.Application.Common
{
	public class ServiceResult<T>
	{
		public T? Data { get; private set; }
		public bool IsSuccess { get; private set; }
		public string Message { get; private set; }

		private ServiceResult(T? data, bool isSuccess, string message)
		{
			Data = data;
			IsSuccess = isSuccess;
			Message = message;
		}

		public static ServiceResult<T> Success(T data, string message = "Success")
		{
			return new ServiceResult<T>(data, true, message);
		}

		public static ServiceResult<T> Failure(string message)
		{
			return new ServiceResult<T>(default, false, message);
		}
	}
}
