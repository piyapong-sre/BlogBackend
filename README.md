# Blog Backend API

ASP.NET Core Web API สำหรับระบบ Blog พร้อม JWT Authentication, Real-time Comments (SignalR) และเชื่อมต่อกับ PostgreSQL

## 📋 สารบัญ

- [คุณสมบัติ](#คุณสมบัติ)
- [เทคโนโลยีที่ใช้](#เทคโนโลยีที่ใช้)
- [โครงสร้างโปรเจค](#โครงสร้างโปรเจค)
- [การติดตั้ง](#การติดตั้ง)
- [การใช้งาน](#การใช้งาน)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)
- [Authentication](#authentication)
- [Real-time Features](#real-time-features)

## ✨ คุณสมบัติ

- 🔐 **JWT Authentication** - ระบบ Login/Register ที่ปลอดภัย
- 📝 **Blog Post Management** - CRUD สำหรับจัดการบทความ
- 💬 **Comments System** - ระบบคอมเมนต์แบบ Real-time ด้วย SignalR
- 👤 **User Management** - จัดการข้อมูลผู้ใช้
- 🔒 **Authorization** - ป้องกัน API endpoints ด้วย JWT Bearer Token
- 🗃️ **PostgreSQL Database** - ใช้ Entity Framework Core
- 📚 **Swagger UI** - API Documentation แบบ Interactive
- 🏗️ **Clean Architecture** - แยก layer อย่างชัดเจน (Controllers, Services, Repositories)
- 🐳 **Docker Support** - พร้อม docker-compose configuration

## 🛠 เทคโนโลยีที่ใช้

- **.NET 10.0** - Latest .NET framework
- **Entity Framework Core 9.0** - ORM สำหรับ database operations
- **PostgreSQL** - Relational database (Npgsql 9.0)
- **JWT Bearer Authentication** - Token-based authentication
- **BCrypt.Net** - Password hashing
- **SignalR** - Real-time communication
- **Swagger/OpenAPI** - API documentation
- **Docker** - Containerization

## 📁 โครงสร้างโปรเจค

```
BlogBackend/
├── Controllers/          # API Controllers
│   ├── AuthController.cs         # Authentication endpoints
│   ├── BlogPostsController.cs    # Blog post CRUD
│   └── CommentsController.cs     # Comment management
├── Services/            # Business Logic Layer
│   ├── IAuthService.cs / AuthService.cs
│   ├── IBlogPostService.cs / BlogPostService.cs
│   └── ICommentService.cs / CommentService.cs
├── Repositories/        # Data Access Layer
│   ├── IUserRepository.cs / UserRepository.cs
│   ├── IBlogPostRepository.cs / BlogPostRepository.cs
│   └── ICommentRepository.cs / CommentRepository.cs
├── Models/              # Domain Entities
│   ├── User.cs
│   ├── BlogPost.cs
│   └── Comment.cs
├── DTOs/                # Data Transfer Objects
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   ├── AuthResponseDto.cs
│   ├── BlogPostDto.cs
│   └── CommentDto.cs
├── Data/                # Database Context
│   ├── ApplicationDbContext.cs
│   └── DbSeeder.cs
├── Hubs/                # SignalR Hubs
│   └── CommentHub.cs
├── Extensions/          # Extension Methods
│   └── ClaimsPrincipalExtensions.cs
├── Migrations/          # EF Core Migrations
├── Program.cs           # Application Entry Point
├── appsettings.json     # Configuration
├── Dockerfile
└── docker-compose.yml
```

## 🚀 การติดตั้ง

### ข้อกำหนดเบื้องต้น

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (หรือใช้ Docker)
- [Git](https://git-scm.com/)

### 1. Clone โปรเจค

```bash
git clone <repository-url>
cd BlogBackend
```

### 2. ติดตั้ง Dependencies

```bash
dotnet restore
```

### 3. ตั้งค่า Database Connection

แก้ไขไฟล์ `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=blogdb;Username=postgres;Password=your_password"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyForJWTAuthenticationThatIsAtLeast32CharactersLong",
    "Issuer": "BlogBackendAPI",
    "Audience": "BlogBackendClient"
  }
}
```

### 4. สร้าง Database และ Apply Migrations

```bash
# สร้าง migration (ถ้ายังไม่มี)
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### 5. Seed ข้อมูลตัวอย่าง (Optional)

โปรเจคจะทำการ seed ข้อมูลตัวอย่างอัตโนมัติเมื่อรันครั้งแรก ข้อมูลที่จะถูก seed ได้แก่:

#### 👥 ผู้ใช้ตัวอย่าง (Users)

| Username | Email | Password | Display Name |
|----------|-------|----------|--------------|
| admin | admin@blog.com | Password123! | Admin User |
| user01 | change@example.com | Password123! | Change Can |
| user02 | blend@example.com | Password123! | Blend 285 |

#### 📝 Blog Post ตัวอย่าง

- **Title**: "IT 08-1"
- **Author**: user01 (Change Can)
- **Status**: Published
- **Image**: Sample Unsplash image

> **หมายเหตุ**: ข้อมูลตัวอย่างจะถูกสร้างเฉพาะเมื่อ database ว่างเปล่า ถ้ามีข้อมูลอยู่แล้วจะข้ามขั้นตอนการ seed

หากต้องการ seed ข้อมูลใหม่ ให้ลบ database และสร้างใหม่:

```bash
# ลบ database
dotnet ef database drop

# สร้าง database ใหม่
dotnet ef database update

# รันโปรเจคเพื่อ seed ข้อมูล
dotnet run
```

### 6. รันโปรเจค

```bash
dotnet run
```

API จะรันที่:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`

> **💡 Tip**: เมื่อรันครั้งแรก ระบบจะทำการ seed ข้อมูลตัวอย่างให้อัตโนมัติ คุณสามารถใช้ข้อมูลผู้ใช้ที่ seed ไว้เพื่อทดสอบ API ได้ทันที

### ใช้ Docker (ทางเลือก)

```bash
# รัน PostgreSQL และ API พร้อมกัน
docker-compose up -d

# หยุด containers
docker-compose down
```

## 💻 การใช้งาน

### 1. Register ผู้ใช้ใหม่

```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "email": "john@example.com",
    "password": "Password123!",
    "displayName": "John Doe"
  }'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "john_doe",
  "email": "john@example.com",
  "displayName": "John Doe"
}
```

### 2. Login

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "Password123!"
  }'
```

### 3. สร้าง Blog Post (ต้อง Login ก่อน)

```bash
curl -X POST https://localhost:5001/api/blogposts \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "title": "My First Blog Post",
    "content": "This is the content of my first blog post...",
    "contentImage": "https://example.com/image.jpg",
    "isPublished": true
  }'
```

### 4. ดึงข้อมูล Blog Posts ทั้งหมด

```bash
curl -X GET https://localhost:5001/api/blogposts \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 5. เพิ่ม Comment

```bash
curl -X POST https://localhost:5001/api/comments \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "blogPostId": 1,
    "content": "Great article!"
  }'
```

## 📡 API Endpoints

### 🔐 Authentication (`/api/auth`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | ลงทะเบียนผู้ใช้ใหม่ | ❌ |
| POST | `/api/auth/login` | เข้าสู่ระบบ | ❌ |

### 📝 Blog Posts (`/api/blogposts`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/blogposts` | ดึงข้อมูล blog posts ทั้งหมด | ✅ |
| GET | `/api/blogposts/published` | ดึงข้อมูล posts ที่เผยแพร่แล้ว | ✅ |
| GET | `/api/blogposts/{id}` | ดึงข้อมูล post ตาม ID | ✅ |
| POST | `/api/blogposts` | สร้าง blog post ใหม่ | ✅ |
| PUT | `/api/blogposts/{id}` | แก้ไข blog post | ✅ |
| DELETE | `/api/blogposts/{id}` | ลบ blog post | ✅ |

### 💬 Comments (`/api/comments`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/comments/blogpost/{blogPostId}` | ดึง comments ของ blog post | ✅ |
| GET | `/api/comments/{id}` | ดึง comment ตาม ID | ✅ |
| POST | `/api/comments` | สร้าง comment ใหม่ | ✅ |
| DELETE | `/api/comments/{id}` | ลบ comment | ✅ |

## 🗄 Database Schema

### User Table

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary Key (Identity) |
| Username | string | ชื่อผู้ใช้ (Unique) |
| Email | string | อีเมล (Unique) |
| PasswordHash | string | รหัสผ่านที่เข้ารหัสด้วย BCrypt |
| DisplayName | string | ชื่อแสดงผล |
| CreatedAt | DateTime | วันที่สร้างบัญชี |
| UpdatedAt | DateTime | วันที่แก้ไขล่าสุด |
| IsActive | bool | สถานะการใช้งาน |

### BlogPost Table

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary Key (Identity) |
| Title | string | หัวข้อบทความ |
| Content | text | เนื้อหาบทความ |
| ContentImage | string | URL รูปภาพประกอบ |
| UserId | int | Foreign Key → User.Id |
| CreatedAt | DateTime | วันที่สร้าง |
| UpdatedAt | DateTime | วันที่แก้ไขล่าสุด |
| IsPublished | bool | สถานะการเผยแพร่ |

### Comment Table

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary Key (Identity) |
| BlogPostId | int | Foreign Key → BlogPost.Id |
| UserId | int | Foreign Key → User.Id |
| Content | text | เนื้อหาคอมเมนต์ |
| CreatedAt | DateTime | วันที่สร้าง |
| UpdatedAt | DateTime | วันที่แก้ไขล่าสุด |

### Relationships

- User ↔ BlogPost (One-to-Many)
- User ↔ Comment (One-to-Many)
- BlogPost ↔ Comment (One-to-Many)

## 🔒 Authentication

โปรเจคนี้ใช้ **JWT (JSON Web Token)** สำหรับการยืนยันตัวตน

### การใช้งาน JWT Token

1. Register หรือ Login เพื่อรับ JWT Token
2. ใส่ Token ใน Header ของทุก Request ที่ต้องการ Authentication:

```
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

3. Token จะมีอายุตามที่กำหนดใน JWT Settings
4. เมื่อ Token หมดอายุ ต้อง Login ใหม่

### JWT Configuration

แก้ไขค่าใน `appsettings.json`:

```json
"JwtSettings": {
  "SecretKey": "Your-Secret-Key-At-Least-32-Characters",
  "Issuer": "BlogBackendAPI",
  "Audience": "BlogBackendClient"
}
```

⚠️ **สำคัญ**: ใน Production ต้องเปลี่ยน `SecretKey` เป็นค่าที่ปลอดภัยและเก็บใน Environment Variables

## 🔴 Real-time Features

โปรเจคใช้ **SignalR** สำหรับ Real-time Communication ในส่วนของ Comments

### SignalR Hub Endpoint

```
wss://localhost:5001/commentHub
```

### การเชื่อมต่อ SignalR (JavaScript Example)

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:5001/commentHub", {
        accessTokenFactory: () => yourJwtToken
    })
    .build();

// รับ Comment ใหม่แบบ Real-time
connection.on("ReceiveComment", (comment) => {
    console.log("New comment:", comment);
    // Update UI
});

// เริ่มการเชื่อมต่อ
connection.start()
    .then(() => console.log("Connected to CommentHub"))
    .catch(err => console.error(err));

// Join กลุ่มของ Blog Post เฉพาะ (Optional)
connection.invoke("JoinBlogPostGroup", blogPostId);
```

### Events ที่รองรับ

- `ReceiveComment` - รับ Comment ใหม่แบบ Real-time
- `JoinBlogPostGroup` - เข้าร่วมกลุ่มของ Blog Post เฉพาะ

## 🏗 Architecture Pattern

โปรเจคนี้ใช้ **Clean Architecture** แบ่งเป็น 3 Layers หลัก:

### 1. Presentation Layer (Controllers)
- จัดการ HTTP Requests/Responses
- Validation พื้นฐาน
- Authorization ด้วย `[Authorize]` attribute

### 2. Business Logic Layer (Services)
- Business rules และ validation ที่ซับซ้อน
- แปลง DTO ↔ Domain Models
- Orchestrate การทำงานของ Repositories

### 3. Data Access Layer (Repositories)
- เข้าถึง Database ผ่าน Entity Framework Core
- CRUD operations
- Query optimization

## 🔧 Configuration

### CORS Settings

โปรเจคมี CORS configuration สำหรับ Frontend frameworks:

```csharp
// Program.cs
options.AddPolicy("AllowAll", policy =>
{
    policy.WithOrigins(
        "http://localhost:3000",  // React
        "http://localhost:5173",  // Vite
        "http://localhost:4200"   // Angular
    )
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
});
```

สามารถแก้ไข origins ได้ตามต้องการ

## 📦 Dependencies

```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.3.1" />
```

## 🐛 Troubleshooting

### Database Connection Error
```
Check PostgreSQL service is running
Verify connection string in appsettings.json
Ensure database 'blogdb' exists
```

### JWT Token Invalid
```
Check token expiration
Verify SecretKey matches between token generation and validation
Ensure Bearer prefix in Authorization header
```

### Migration Issues
```bash
# ลบ migrations ทั้งหมด
dotnet ef migrations remove

# สร้างใหม่
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 📝 License

This project is licensed under the MIT License

---
