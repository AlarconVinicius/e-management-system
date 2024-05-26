USE [EManagementSystemDBDev]
GO

-- Inserindo planos
INSERT [dbo].[Plans] ([Id], [Title], [Subtitle], [Price], [Benefits], [IsActive], [CreatedAt], [UpdatedAt]) 
VALUES 
    (N'7d67df76-2d4e-4a47-a19c-08eb80a9060b', N'Plano Básico', N'Ideal para profissionais liberais', 15.00, N'Clientes ilimitados,Agendamentos,Produtos Ilimitados,Notificação por e-mail,Controle Financeiro', 1, GETUTCDATE(), GETUTCDATE()),
    (N'78162be3-61c4-4959-89d7-5ebfb476427e', N'Plano Intermediário', N'Ideal para pequenas clinicas e Petshop', 35.00, N'Tudo do plano básico,+ Notificação por SMS (500/mes),+Gerencia dos colaboradores (até 10)', 1, GETUTCDATE(), GETUTCDATE()),
    (N'6ecaaa6b-ad9f-422c-b3bb-6013ec27b4bb', N'Plano Avançado', N'Ideal para clinicas, dentistas, petshop entre outros', 60.00, N'Todo do plano Intermediário,+ Notificação via Whatsapp (1000/mes),+Integração de pagamento com Stripe, MercadoPago e PagSeguro', 1, GETUTCDATE(), GETUTCDATE());
GO

-- Inserindo empresas
INSERT [dbo].[Companies] ([Id], [PlanId], [Name], [Brand], [Cpf], [IsActive], [CreatedAt], [UpdatedAt]) 
VALUES 
    (N'3eb1ed86-802c-4355-8045-482c274ac6ca', N'6ecaaa6b-ad9f-422c-b3bb-6013ec27b4bb', N'DSY', N'', N'64280963070', 1, GETUTCDATE(), GETUTCDATE());
GO

-- Inserindo usuários no AspNetUsers
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'ef65d0b9-a0df-45d1-a4af-66e95679b8b5', N'alarcon@gmail.com', N'ALARCON@GMAIL.COM', N'alarcon@gmail.com', N'ALARCON@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEKa4koXH8IQg9kR+nOBG09YVZBiNVU9yk0su7EXNXVvnk3MMjQPV/UawbyE/nzLF3w==', N'X53XMRXWWXFQC6MBQAWQDLJB2G7YPP65', N'3a155c4d-42d1-4476-9975-0dcde60f6d6f', NULL, 0, 0, NULL, 1, 0);
GO

-- Inserindo usuários customizados
INSERT [dbo].[Users] ([Id], [CompanyId], [Name], [LastName], [Email], [PhoneNumber], [Cpf], [Role], [IsActive], [CreatedAt], [UpdatedAt]) 
VALUES 
    (N'ef65d0b9-a0df-45d1-a4af-66e95679b8b5', N'3eb1ed86-802c-4355-8045-482c274ac6ca', N'Vinícius', N'Alarcon', N'alarcon@gmail.com', N'21991911558', N'64280963070', N'Admin', 1, GETUTCDATE(), GETUTCDATE());
GO

-- Inserindo roles
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
VALUES 
    (N'b9c5b644-de78-4417-8133-1fa96bba6228', N'Admin', N'ADMIN', NULL);
GO

-- Ativando IDENTITY_INSERT para AspNetUserClaims
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON;

-- Inserindo claims
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) 
VALUES 
    (1, N'ef65d0b9-a0df-45d1-a4af-66e95679b8b5', N'Tenant', N'3eb1ed86-802c-4355-8045-482c274ac6ca');

-- Desativando IDENTITY_INSERT para AspNetUserClaims
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF;
GO

-- Inserindo user roles
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) 
VALUES 
    (N'ef65d0b9-a0df-45d1-a4af-66e95679b8b5', N'b9c5b644-de78-4417-8133-1fa96bba6228');
GO

-- Inserindo funcionários
INSERT [dbo].[Employees] ([Id], [Salary]) 
VALUES 
    (N'ef65d0b9-a0df-45d1-a4af-66e95679b8b5', 10000.00);
GO
