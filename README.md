# ContractDigitalSignature

Hệ thống ký số hợp đồng điện tử. Backend là ASP.NET Core Web API (.NET 10) theo kiến trúc phân lớp. Frontend là ứng dụng Next.js (React, Tailwind CSS).

Hiện tại đây là khung dự án ban đầu: API có endpoint mẫu `GET /weatherforecast` và tài liệu OpenAPI khi chạy môi trường Development. Giao diện frontend là trang khởi tạo của Next.js.

## Yêu cầu

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/) và npm

## Chạy dự án

Mở hai terminal, một cho API và một cho giao diện.

### Backend

```bash
cd backend/ContractDigitalSignature.API
dotnet run
```

API lắng nghe tại:

- HTTP: http://localhost:5235
- HTTPS: https://localhost:7106

Kiểm tra nhanh:

- http://localhost:5235/weatherforecast
- http://localhost:5235/openapi/v1.json (chỉ khi `ASPNETCORE_ENVIRONMENT=Development`)

Chạy test:

```bash
dotnet test
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

Giao diện: http://localhost:3000

Các lệnh khác:

| Lệnh | Tác dụng |
| --- | --- |
| `npm run build` | Build bản production |
| `npm run start` | Chạy bản đã build |
| `npm run lint` | Kiểm tra ESLint |

## Cấu trúc thư mục

```text
ContractDigitalSignature/
├── ContractDigitalSignature.slnx          # Solution .NET
├── backend/
│   ├── ContractDigitalSignature.API/              # HTTP API, cấu hình, endpoint
│   ├── ContractDigitalSignature.Application/      # Use case, phụ thuộc Domain
│   ├── ContractDigitalSignature.Domain/           # Thực thể và quy tắc nghiệp vụ
│   ├── ContractDigitalSignature.Infrastructure/   # Truy cập dữ liệu, dịch vụ bên ngoài
│   └── ContractDigitalSignature.Tests/            # Unit test (xUnit)
└── frontend/
    ├── app/           # App Router: layout, trang, CSS toàn cục
    ├── public/        # File tĩnh
    ├── package.json
    └── next.config.ts
```

Luồng phụ thuộc backend: `API` → `Application` và `Infrastructure`; `Infrastructure` → `Application` và `Domain`; `Application` → `Domain`.
