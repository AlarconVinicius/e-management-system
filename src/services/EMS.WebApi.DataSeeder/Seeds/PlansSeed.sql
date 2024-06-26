USE [EManagementSystemDBDev]
GO
INSERT [dbo].Plans ([Id], [Title], [Subtitle], [Price], [Benefits], [IsActive], [CreatedAt], [UpdatedAt]) 
VALUES 
    (N'7d67df76-2d4e-4a47-a19c-08eb80a9060b', N'Plano Básico', N'Ideal para profissionais liberais', 15.00, N'Clientes ilimitados,Agendamentos,Produtos Ilimitados,Notificação por e-mail,Controle Financeiro', 1, GETUTCDATE(), GETUTCDATE()),
    (N'78162be3-61c4-4959-89d7-5ebfb476427e', N'Plano Intermediário', N'Ideal para pequenas clinicas e Petshop', 35.00, N'Tudo do plano básico,+ Notificação por SMS (500/mes),+Gerencia dos colaboradores (até 10)', 1, GETUTCDATE(), GETUTCDATE()),
    (N'6ecaaa6b-ad9f-422c-b3bb-6013ec27b4bb', N'Plano Avançado', N'Ideal para clinicas, dentistas, petshop entre outros', 60.00, N'Todo do plano Intermediário,+ Notificação via Whatsapp (1000/mes),+Integração de pagamento com Stripe, MercadoPago e PagSeguro', 1, GETUTCDATE(), GETUTCDATE());