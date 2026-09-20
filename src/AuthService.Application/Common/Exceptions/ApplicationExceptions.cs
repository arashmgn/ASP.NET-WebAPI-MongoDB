namespace AuthService.Application.Common.Exceptions;

public class NotFoundException(string message) : Exception(message);

public class ConflictException(string message) : Exception(message);

public class InvalidCredentialsException() : Exception("Invalid email or password");

public class ForbiddenException(string message) : Exception(message);
