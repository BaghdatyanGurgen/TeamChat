namespace TeamChat.Domain.Models.Exceptions;

public class InvalidEmailException : ArgumentException { }
public class InvalidPasswordException : Exception { }
public class InvalidTokenException : Exception { }
public class UserNotFoundException : ArgumentException { }