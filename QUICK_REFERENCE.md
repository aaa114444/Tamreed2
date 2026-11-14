# Quick Reference - Authentication Phase 0

## ?? Documentation Files

1. **PHASE_0_AUTHENTICATION.md** - Complete technical documentation
2. **PHASE_0_COMPLETION_SUMMARY.md** - Executive summary and checklist
3. **This file** - Quick reference guide

---

## ?? Quick Start

### Apply Database Migration
```bash
cd E:\Tamreed2
dotnet ef database update --project src/Tamreed2.Infrastructure --startup-project src/Tamreed2.Api
```

### Run Application
```bash
cd E:\Tamreed2\src\Tamreed2.Api
dotnet run
```

### Run Tests
```bash
cd E:\Tamreed2
dotnet test tests/Tamreed2.UnitTests
```

### Access Swagger UI
```
https://localhost:5001/swagger/index.html
```

---

## ?? API Quick Reference

### Register New User
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePass123!",
    "firstName": "John",
    "lastName": "Doe"
  }'
```

### Login
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePass123!"
  }'
```

### Use Token
```bash
curl -X GET https://localhost:5001/api/services \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN_HERE"
```

---

## ?? Password Requirements

- **Minimum 8 characters**
- **At least 1 uppercase letter** (A-Z)
- **At least 1 lowercase letter** (a-z)
- **At least 1 digit** (0-9)
- **At least 1 special character** (!@#$%^&*...)

? Invalid: `pass123` (no uppercase, no special)
? Valid: `SecurePass123!`

---

## ?? JWT Token Claims

Every access token contains:

```json
{
  "sub": "user-id-guid",
  "email": "user@example.com",
  "role": "Customer|Nurse|Admin",
  "iat": 1234567890,
  "exp": 1234571490
}
```

### Access Current User in Handlers

```csharp
// Inject ICurrentUserContext
public class MyHandler
{
    private readonly ICurrentUserContext _currentUser;
    
    public async Task Handle(Command cmd, CancellationToken ct)
    {
        var userId = _currentUser.UserId;// Guid
        var email = _currentUser.Email;  // string
        var role = _currentUser.Role;    // "Customer" | "Nurse" | "Admin"
        var isAuth = _currentUser.IsAuthenticated; // bool
    }
}
```

---

## ?? Key Project Locations

| Location | Purpose |
|----------|---------|
| `src/Tamreed2.Domain/Users/` | User aggregate, UserRole enum |
| `src/Tamreed2.Application/Auth/` | Register/Login commands & validators |
| `src/Tamreed2.Infrastructure/Authentication/` | JWT, BCrypt, CurrentUserContext |
| `src/Tamreed2.Api/Controllers/AuthController.cs` | /api/auth/* endpoints |
| `appsettings.json` | JWT configuration |
| `tests/Tamreed2.UnitTests/` | All authentication tests |

---

## ?? Unit Test Coverage

```
User Domain Tests (16):
  ? Create with valid data
  ? Email validation (format)
  ? Email normalization
  ? Password operations
  ? Profile updates
  ? Login recording
  ? Active/Inactive state

Register Handler Tests (6):
  ? Successful registration
  ? Email already exists
  ? Password hashing
  ? Token generation
  ? Cancellation token handling

Login Handler Tests (6):
  ? Successful login
  ? Invalid email
  ? Inactive user
  ? Wrong password
  ? Login timestamp recording
  ? Cancellation token handling

TOTAL: 28 tests ?
```

Run tests:
```bash
dotnet test
```

---

## ?? Configuration

### JWT Settings (appsettings.json)

```json
{
  "Jwt": {
    "SecretKey": "your-secret-here-min-32-chars",
    "Issuer": "Tamreed2",
    "Audience": "Tamreed2Users",
    "ExpirationMinutes": 60
  }
}
```

### Database Settings (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=Tamreed2Db;..."
  }
}
```

---

## ?? User Lifecycle

```
1. REGISTER
   ?? Create new user (Customer role by default)
   ?? Hash password
   ?? Return JWT tokens

2. LOGIN
   ?? Find user by email
   ?? Verify password
   ?? Record login time
   ?? Return JWT tokens

3. USE TOKENS
   ?? Include in Authorization: Bearer header
   ?? Token validated on each request
   ?? Claims extracted for current user context

4. TOKEN EXPIRATION
   ?? After 60 minutes, token expires
   ?? User must login again
   ?? (Refresh token endpoint coming in Phase 1)

5. DEACTIVATE (Admin function)
   ?? User cannot login
   ?? Existing tokens still valid until expiration
   ?? Can be reactivated
```

---

## ??? Security Features

| Feature | How It Works |
|---------|-------------|
| **Password Hashing** | BCrypt with salt, workFactor 12 |
| **JWT Signature** | HS256 algorithm with secret key |
| **Token Expiration** | 60 minutes (configurable) |
| **Email Uniqueness** | Database unique constraint + code check |
| **Input Validation** | FluentValidation on all commands |
| **Active User Check** | Only active users can login |
| **Username Enumeration Prevention** | Same error for invalid email & wrong password |
| **Claim Extraction** | Current user accessible in handlers |

---

## ? Pre-Flight Checklist

Before using authentication:

- [ ] Secret key changed to production value
- [ ] Database migration applied
- [ ] HTTPS enabled
- [ ] appsettings.json configured
- [ ] BCrypt.Net-Next package restored
- [ ] JWT packages restored
- [ ] Build successful
- [ ] All tests passing
- [ ] Swagger UI accessible

---

## ?? Troubleshooting

### Build Fails
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

### JWT Errors
```
"Invalid token" or "Unauthorized"
? Check JWT configuration in appsettings.json
? Verify token is in request header as: Authorization: Bearer <token>
? Check token hasn't expired (60 min)
```

### Database Connection Issues
```
"Connection string not found"
? Check ConnectionStrings in appsettings.json
? Verify SQL Server is running
? Verify database exists
```

### Password Validation Fails
```
"Password must contain..."
? Check requirements: 8+ chars, uppercase, lowercase, digit, special
? Example: SecurePass123!
```

### Email Already Registered
```
Status 409 Conflict
? Use different email
? Or reset user in database if needed
```

---

## ?? Database Schema

### Users Table
```sql
Id (GUID, PK)
Email (nvarchar(255), UNIQUE)
PasswordHash (nvarchar(255))
FirstName (nvarchar(100))
LastName (nvarchar(100))
Role (nvarchar(20)) -- Customer | Nurse | Admin
IsActive (bit)
CreatedAtUtc (datetimeoffset)
LastLoginAtUtc (datetimeoffset, nullable)
```

### Query Examples

```sql
-- Find user by email
SELECT * FROM Users WHERE Email = 'john@example.com'

-- Count registrations per day
SELECT 
    CAST(CreatedAtUtc AS DATE) as Date,
    COUNT(*) as Count
FROM Users
GROUP BY CAST(CreatedAtUtc AS DATE)

-- Find inactive users
SELECT * FROM Users WHERE IsActive = 0

-- Find users who never logged in
SELECT * FROM Users WHERE LastLoginAtUtc IS NULL
```

---

## ?? Learning Path

### Understand the Flow
1. Read `PHASE_0_AUTHENTICATION.md` section "Architecture"
2. Look at `RegisterUserCommand.cs` and handler
3. Look at `LoginUserCommand.cs` and handler
4. Check `AuthController.cs` endpoints

### Run the Tests
1. `dotnet test` to see all 28 tests pass
2. Open test files to see usage examples
3. Notice AAA pattern (Arrange-Act-Assert)

### Try It Yourself
1. Register a new user via API
2. Get the access token
3. Decode the JWT at jwt.io
4. Use token in Authorization header
5. Check current user claims in handler

---

## ?? Next: Phase 1

The next phase will add:
- Booking API endpoints
- Authorization checks (`[Authorize]` attributes)
- Role-based access control
- Resource-based authorization
- Booking queries and commands

**Estimated Duration:** 2 days

---

## ?? Support

### Code Comments
Every class and method has XML documentation explaining purpose and usage.

### Test Examples
Each test case demonstrates correct usage of the authentication system.

### Configuration
All configurable values explained with examples in `appsettings.json`.

---

## Version Info

- **.NET Version:** 8.0
- **C# Version:** 12.0
- **Project Status:** Phase 0 Complete ?
- **Last Updated:** 2026-11-09

---

**Ready to proceed to Phase 1?** 
? Implement Booking API endpoints with authorization
