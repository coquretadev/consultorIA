# Plan de Implementación: AI Consulting Business

## Visión General

Implementación incremental de la plataforma de consultoría de integración de IA siguiendo Clean Architecture con .NET 8+, ASP.NET Core, PostgreSQL, EF Core, Blazor WASM, xUnit y FsCheck. El diseño incluye 14 componentes, 19 entidades, 31 propiedades de corrección y soporte para multi-idioma, analytics, SEO, calendario de llamadas, webhooks y rate limiting. Cada tarea construye sobre las anteriores, integrando código progresivamente.

## Tareas

- [x] 1. Estructura del proyecto y capa de dominio
  - [x] 1.1 Crear la estructura de solución y proyectos
    - Crear la solución `AiConsulting.slnx` con los proyectos: `AiConsulting.Domain`, `AiConsulting.Application`, `AiConsulting.Infrastructure`, `AiConsulting.Api`, `AiConsulting.Web`
    - Crear los proyectos de test: `AiConsulting.Domain.Tests`, `AiConsulting.Application.Tests`, `AiConsulting.Api.Tests`, `AiConsulting.Properties.Tests`
    - Configurar referencias entre proyectos según Clean Architecture (Domain sin dependencias, Application referencia Domain, Infrastructure referencia Application, Api referencia todos)
    - Instalar paquetes NuGet: `Npgsql.EntityFrameworkCore.PostgreSQL`, `MediatR`, `FluentValidation`, `xUnit`, `FsCheck.Xunit`, `FluentAssertions`, `NSubstitute`, `Bogus`, `Microsoft.AspNetCore.Mvc.Testing`
    - _Requisitos: Todos (estructura base)_

  - [x] 1.2 Definir enums y value objects del dominio
    - Crear enums: `OpportunityPhase` (InitialContact, ProposalSent, Negotiation, ClosedWon, ClosedLost), `ProjectStatus` (Proposal, InProgress, Completed, Cancelled), `ExpenseCategory` (Tools, Training, Marketing, Infrastructure, Other), `NotificationChannel` (Slack, Telegram)
    - _Requisitos: 3.1, 2.4, 5.3, 1.2_

  - [x] 1.3 Implementar entidades del dominio
    - Crear entidades: `Service` (con campos Slug, MetaTitle, MetaDescription), `Client`, `Opportunity`, `PhaseTransition`, `Project`, `StatusChange`, `Deliverable`, `TimeEntry`, `ProjectTemplate`, `Invoice`, `Expense`, `TrainingChecklistItem`, `ContactRequest`, `NotificationConfig`, `ServiceTranslation`, `PageVisit`, `ConsultorAvailability`, `BookingSlot`
    - Incluir lógica de dominio: cálculo de `ProgressPercentage` en `Project` basado en entregables completados, cálculo de `DaysInPreviousPhase` en `PhaseTransition`, validación de `PriceRangeMin <= PriceRangeMax` en `Service`, validación de `StartTime < EndTime` en `ConsultorAvailability`
    - _Requisitos: 1.1, 1.2, 2.2, 2.3, 2.6, 3.1, 3.2, 4.2, 4.4, 5.1, 5.2, 5.3, 6.1, 6.2, 7.1, 8.1, 9.1, 10.5, 11.1, 11.4_

  - [x] 1.4 Definir interfaces de repositorio y servicios en el dominio
    - Crear interfaces de repositorio: `IServiceRepository`, `IClientRepository`, `IOpportunityRepository`, `IProjectRepository`, `IProjectTemplateRepository`, `IInvoiceRepository`, `IExpenseRepository`, `ITrainingRepository`, `IContactRequestRepository`, `INotificationConfigRepository`, `IServiceTranslationRepository`, `IPageVisitRepository`, `IConsultorAvailabilityRepository`, `IBookingSlotRepository`
    - _Requisitos: Todos (acceso a datos)_

  - [ ]* 1.5 Escribir tests unitarios para la lógica de dominio
    - Test de cálculo de `ProgressPercentage`: proyecto sin entregables = 0%, con todos completados = 100%, parcial = proporción correcta
    - Test de validación `PriceRangeMin <= PriceRangeMax` en `Service`
    - Test de cálculo de `DaysInPreviousPhase` en `PhaseTransition`
    - Test de validación `StartTime < EndTime` en `ConsultorAvailability`
    - _Requisitos: 4.4, 6.2, 3.2, 7.2, 11.4_

- [x] 2. Infraestructura y persistencia
  - [x] 2.1 Configurar DbContext y mapeo de entidades con EF Core
    - Crear `AiConsultingDbContext` con DbSets para las 19 entidades
    - Configurar mapeos con Fluent API: longitudes de campos, relaciones, índices, valores por defecto
    - Configurar columnas JSON para `DefaultDeliverables` y `DefaultMilestones` en `ProjectTemplate`
    - Configurar índice único en `Service.Slug`, índice compuesto en `ServiceTranslation(ServiceId, LanguageCode)`, índice en `PageVisit.VisitedAt`, índice en `BookingSlot(Date, StartTime)`
    - _Requisitos: Todos (persistencia)_

  - [x] 2.2 Implementar repositorios
    - Implementar todos los repositorios definidos en 1.4 usando EF Core
    - Incluir consultas optimizadas: filtrado paginado en clientes y proyectos, agrupación por fase en oportunidades, filtrado por mes/año en facturas y gastos, filtrado por rango de fechas en PageVisit, consulta de slots disponibles por fecha en BookingSlot
    - _Requisitos: Todos (acceso a datos)_

  - [x] 2.3 Crear migración inicial y datos semilla
    - Generar migración EF Core con el esquema completo (19 entidades)
    - Crear datos semilla: 5 servicios del catálogo con slugs SEO-friendly (Chatbot RAG, Automatización documental, Copiloto interno, Integración APIs IA, Auditoría IA), 5 plantillas de proyecto (una por servicio), checklist de formación con temas (LLMs, RAG, Function Calling, Agentes, Automatización documental), sectores objetivo (PYMEs .NET, SaaS B2B, Despachos legales, Agencias marketing), traducciones al inglés de los 5 servicios (ServiceTranslation), disponibilidad por defecto del consultor (lunes a viernes 9:00-18:00)
    - _Requisitos: 1.1, 1.4, 4.1, 6.1, 8.1, 10.5, 11.4_

- [x] 3. Checkpoint — Verificar estructura base
  - Asegurar que todos los tests pasan, preguntar al usuario si surgen dudas.

- [x] 4. Capa de aplicación — Portal público, contacto y webhooks
  - [x] 4.1 Implementar DTOs y validadores para el portal público
    - Crear DTOs: `ServiceSummaryDto`, `ServiceDetailDto`, `CaseStudyDto`, `SectorDto`, `ContactRequestDto`, `ContactRequestResultDto`, `NotificationConfigDto`, `UpdateNotificationConfigDto`
    - Crear validador FluentValidation para `ContactRequestDto`: Name, Email, Company y Message obligatorios, Email con formato válido
    - Crear validador para `UpdateNotificationConfigDto`: Channel y WebhookUrl obligatorios, WebhookUrl con formato URL válido
    - _Requisitos: 1.1, 1.2, 1.3, 1.4, 1.6_

  - [x] 4.2 Implementar `IPublicPortalService`
    - Implementar `GetActiveServicesAsync`: devolver solo servicios con `IsActive=true` ordenados por `SortOrder`
    - Implementar `SubmitContactRequestAsync`: persistir solicitud, crear oportunidad en fase "Contacto inicial" automáticamente, enviar notificación por email al consultor, disparar webhook si hay configuración activa
    - Implementar `GetCaseStudiesAsync`, `GetSectorsAsync`, `GetServiceByIdAsync`
    - _Requisitos: 1.1, 1.2, 1.3, 1.4, 3.3_

  - [x] 4.3 Implementar `INotificationService`
    - Implementar `SendWebhookAsync`: enviar HTTP POST al WebhookUrl configurado con payload del evento (contacto o reserva)
    - Implementar `GetConfigAsync` y `UpdateConfigAsync` para gestionar configuración de webhooks (Slack/Telegram)
    - Manejar fallos de webhook sin afectar la operación principal (log del error para reintento manual)
    - _Requisitos: 1.2, 11.3_

  - [ ]* 4.4 Escribir test de propiedad para solicitud de contacto con webhook y oportunidad (round-trip)
    - **Propiedad 1: Round-trip de solicitud de contacto con webhook y oportunidad**
    - **Valida: Requisitos 1.2, 3.3**

  - [ ]* 4.5 Escribir test de propiedad para validación de formulario de contacto
    - **Propiedad 2: Validación de formulario de contacto con campos vacíos**
    - **Valida: Requisito 1.6**

  - [ ]* 4.6 Escribir test de propiedad para servicios públicos activos
    - **Propiedad 3: Servicios públicos muestran solo activos con campos completos**
    - **Valida: Requisitos 1.1, 7.3, 7.4**

  - [ ]* 4.7 Escribir test de propiedad para webhook de notificación al crear contacto
    - **Propiedad 23: Webhook de notificación al crear contacto**
    - **Valida: Requisito 1.2**

- [x] 5. Capa de aplicación — Autenticación y gestión de clientes
  - [x] 5.1 Configurar ASP.NET Identity y JWT
    - Configurar ASP.NET Identity con PostgreSQL
    - Implementar generación y validación de tokens JWT
    - Implementar `IAuthService` con `LoginAsync`, `RefreshTokenAsync`, `LogoutAsync`
    - _Requisitos: 2.1, 2.5_

  - [x] 5.2 Implementar `IClientService`
    - Implementar CRUD de clientes: crear, editar, archivar, consultar con paginación y filtros
    - _Requisitos: 2.2_

  - [ ]* 5.3 Escribir test de propiedad para round-trip de clientes
    - **Propiedad 4: Round-trip de clientes**
    - **Valida: Requisito 2.2**

  - [ ]* 5.4 Escribir test de propiedad para autenticación requerida
    - **Propiedad 7: Endpoints protegidos requieren autenticación**
    - **Valida: Requisito 2.5**

- [x] 6. Capa de aplicación — Pipeline de ventas
  - [x] 6.1 Implementar DTOs y validadores para oportunidades
    - Crear DTOs: `OpportunityDto`, `OpportunityGroupDto`, `CreateOpportunityDto`, `UpdateOpportunityDto`, `MoveToPhaseResultDto`
    - Crear validador: ContactName y ContactEmail obligatorios, EstimatedValue >= 0 si se proporciona
    - _Requisitos: 3.1, 3.6_

  - [x] 6.2 Implementar `IOpportunityService`
    - Implementar `GetOpportunitiesByPhaseAsync`: agrupar oportunidades por fase para vista kanban
    - Implementar `MoveToPhaseAsync`: registrar `PhaseTransition` con cálculo de días en fase anterior, si nueva fase es "ClosedWon" indicar en respuesta que se requiere crear proyecto
    - Implementar `GetStaleOpportunitiesAsync`: detectar oportunidades con más de 14 días en la misma fase
    - Implementar CRUD básico de oportunidades
    - _Requisitos: 3.1, 3.2, 3.4, 3.5, 3.6, 3.7_

  - [ ]* 6.3 Escribir test de propiedad para transiciones de fase
    - **Propiedad 8: Transiciones de fase registran tiempo**
    - **Valida: Requisito 3.2**

  - [ ]* 6.4 Escribir test de propiedad para agrupación kanban
    - **Propiedad 9: Oportunidades agrupadas por fase (kanban)**
    - **Valida: Requisitos 3.1, 3.4**

  - [ ]* 6.5 Escribir test de propiedad para cerrado ganado
    - **Propiedad 10: Cerrado ganado solicita creación de proyecto**
    - **Valida: Requisito 3.5**

  - [ ]* 6.6 Escribir test de propiedad para valor estimado
    - **Propiedad 11: Valor estimado de oportunidad persiste correctamente**
    - **Valida: Requisito 3.6**

  - [ ]* 6.7 Escribir test de propiedad para alerta de oportunidad estancada
    - **Propiedad 12: Alerta de oportunidad estancada**
    - **Valida: Requisito 3.7**

- [x] 7. Checkpoint — Verificar portal público, autenticación y pipeline
  - Asegurar que todos los tests pasan, preguntar al usuario si surgen dudas.

- [x] 8. Capa de aplicación — Gestión de proyectos
  - [x] 8.1 Implementar DTOs y validadores para proyectos
    - Crear DTOs: `ProjectSummaryDto`, `ProjectDetailDto`, `CreateProjectFromTemplateDto`, `UpdateProjectDto`, `DeliverableDto`, `TimeEntryDto`, `LogHoursDto`, `ProjectTemplateDto`, `ProjectTemplateDetailDto`
    - Crear validador: ClientId y ServiceId obligatorios, Name obligatorio
    - _Requisitos: 2.3, 4.6_

  - [x] 8.2 Implementar `IProjectService` e `IProjectTemplateService`
    - Implementar `CreateFromTemplateAsync`: generar entregables, hitos y horas estimadas desde la plantilla, permitir personalización antes de confirmar
    - Implementar `UpdateStatusAsync`: registrar `StatusChange` con fecha, estado anterior y nuevo
    - Implementar `CompleteDeliverableAsync`: marcar entregable como completado y recalcular `ProgressPercentage`
    - Implementar `LogHoursAsync`: registrar horas en un entregable
    - Implementar consultas con filtros y paginación
    - _Requisitos: 2.3, 2.4, 2.6, 4.2, 4.3, 4.4, 4.5_

  - [ ]* 8.3 Escribir test de propiedad para proyecto asociado a cliente
    - **Propiedad 5: Proyecto siempre asociado a cliente y servicio**
    - **Valida: Requisitos 2.3, 4.6**

  - [ ]* 8.4 Escribir test de propiedad para historial de estados
    - **Propiedad 6: Historial de estados del proyecto**
    - **Valida: Requisito 2.6**

  - [ ]* 8.5 Escribir test de propiedad para creación desde plantilla
    - **Propiedad 13: Creación de proyecto desde plantilla genera entregables**
    - **Valida: Requisito 4.2**

  - [ ]* 8.6 Escribir test de propiedad para cálculo de porcentaje de avance
    - **Propiedad 14: Cálculo de porcentaje de avance**
    - **Valida: Requisitos 4.4, 6.2**

  - [ ]* 8.7 Escribir test de propiedad para round-trip de registro de horas
    - **Propiedad 15: Round-trip de registro de horas**
    - **Valida: Requisito 4.5**

- [x] 9. Capa de aplicación — Seguimiento financiero
  - [x] 9.1 Implementar DTOs y validadores para finanzas
    - Crear DTOs: `MonthlySummaryDto`, `FinancialProjectionDto`, `MonthlyProjectionDto`, `InvoiceDto`, `CreateInvoiceDto`, `ExpenseDto`, `CreateExpenseDto`, `InvoiceFilterDto`, `ExpenseFilterDto`
    - Crear validadores: Amount > 0 obligatorio, ProjectId obligatorio en facturas, Category obligatoria en gastos
    - _Requisitos: 5.1, 5.2, 5.3_

  - [x] 9.2 Implementar `IFinanceService`
    - Implementar `GetMonthlySummaryAsync`: calcular ingresos (suma facturas), gastos (suma gastos), beneficio neto (ingresos - gastos), incluir alerta si gastos > ingresos
    - Implementar `GetProjectionAsync`: generar proyección a 12 meses basada en datos actuales
    - Implementar CRUD de facturas y gastos con filtros y paginación
    - _Requisitos: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6_

  - [ ]* 9.3 Escribir test de propiedad para beneficio neto
    - **Propiedad 16: Beneficio neto es ingresos menos gastos**
    - **Valida: Requisitos 5.1, 5.2**

  - [ ]* 9.4 Escribir test de propiedad para round-trip de gastos
    - **Propiedad 17: Round-trip de gastos**
    - **Valida: Requisito 5.3**

  - [ ]* 9.5 Escribir test de propiedad para proyección financiera
    - **Propiedad 18: Proyección financiera cubre 12 meses**
    - **Valida: Requisitos 5.4, 5.5**

  - [ ]* 9.6 Escribir test de propiedad para alerta de resultado negativo
    - **Propiedad 19: Alerta de resultado negativo mensual**
    - **Valida: Requisito 5.6**

- [x] 10. Checkpoint — Verificar proyectos y finanzas
  - Asegurar que todos los tests pasan, preguntar al usuario si surgen dudas.

- [x] 11. Capa de aplicación — Checklist de formación y catálogo configurable
  - [x] 11.1 Implementar `ITrainingService`
    - Implementar `GetRoadmapAsync`: devolver checklist plano de temas de formación ordenados por `SortOrder`
    - Implementar `CompleteItemAsync`: marcar item como completado con fecha, recalcular progreso global
    - Implementar `GetProgressAsync`: calcular porcentaje de avance global (items completados / total × 100)
    - _Requisitos: 6.1, 6.2, 6.3_

  - [x] 11.2 Implementar `ICatalogService`
    - Implementar CRUD de servicios: crear, editar, activar/desactivar, reordenar
    - Implementar `ToggleServiceAsync`: cambiar `IsActive`, afecta visibilidad en portal público
    - Implementar `ReorderServicesAsync`: actualizar `SortOrder` de los servicios
    - Implementar `DeleteServiceAsync`: verificar si tiene proyectos asociados, rechazar sin confirmación, proceder con confirmación
    - _Requisitos: 7.1, 7.2, 7.3, 7.4, 7.5_

  - [ ]* 11.3 Escribir test de propiedad para round-trip de servicios del catálogo
    - **Propiedad 20: Round-trip de servicios del catálogo**
    - **Valida: Requisitos 7.1, 7.2**

  - [ ]* 11.4 Escribir test de propiedad para eliminación con proyectos
    - **Propiedad 21: Eliminación de servicio con proyectos requiere confirmación**
    - **Valida: Requisito 7.5**

- [x] 12. Capa de aplicación — Multi-idioma (i18n)
  - [x] 12.1 Implementar `ILocalizationService`
    - Implementar `GetLocalizedServiceAsync`: devolver servicio con contenido de la traducción correspondiente al idioma solicitado, fallback a español si no existe traducción
    - Implementar `GetLocalizedServicesAsync`: devolver lista de servicios activos con contenido localizado
    - Crear DTOs de traducción: `ServiceTranslationDto`, `CreateServiceTranslationDto`
    - _Requisitos: 8.1, 8.3_

  - [x] 12.2 Integrar localización en endpoints públicos
    - Modificar `GET /api/public/services` y `GET /api/public/services/{id}` para aceptar parámetro `lang` en query string
    - Implementar lógica de detección de idioma: parámetro `lang` > localStorage > header `Accept-Language` > fallback español
    - Crear archivos `.resx` base para textos de interfaz estáticos en español e inglés (`Resources/Localization/`)
    - _Requisitos: 8.1, 8.2, 8.3, 8.4_

  - [x] 12.3 Implementar gestión de traducciones en el panel
    - Modificar formulario de edición de servicio para solicitar contenido en cada idioma soportado
    - Implementar CRUD de `ServiceTranslation` asociado al servicio
    - _Requisitos: 8.5_

  - [ ]* 12.4 Escribir test de propiedad para contenido localizado por idioma
    - **Propiedad 24: Contenido localizado por idioma**
    - **Valida: Requisitos 8.1, 8.3**

- [x] 13. Capa de aplicación — Analytics del portal
  - [x] 13.1 Implementar `IAnalyticsService`
    - Implementar `RecordVisitAsync`: registrar visita con página, referrer, user-agent, tipo de dispositivo (Desktop/Mobile/Tablet) y hash SHA-256 de IP
    - Implementar `GetSummaryAsync`: calcular visitas totales, visitantes únicos (por IpHash), páginas más vistas y origen del tráfico en un rango de fechas
    - Implementar `GetTopServicesAsync`: ranking de servicios más consultados ordenado de mayor a menor
    - Implementar `GetTrafficSourcesAsync`: agrupación de visitas por referrer
    - Crear DTOs: `PageVisitDto`, `AnalyticsSummaryDto`, `TopServiceDto`, `TrafficSourceDto`
    - _Requisitos: 9.1, 9.2, 9.3, 9.4, 9.5_

  - [x] 13.2 Implementar middleware de tracking de analytics
    - Registrar automáticamente cada request a rutas públicas: página visitada, referrer, user-agent, tipo de dispositivo, generar hash SHA-256 de IP
    - No registrar requests a la API del Panel Consultor (rutas autenticadas)
  - [x] 14.1 Configurar proyecto Blazor WASM y servicios HTTP
  - [ ]* 13.3 Escribir test de propiedad para registro de visitas
    - **Propiedad 25: Registro de visitas incrementa contadores**
    - **Valida: Requisitos 9.1, 9.3**

  - [ ]* 13.4 Escribir test de propiedad para métricas filtradas por periodo
    - **Propiedad 26: Métricas de analytics filtradas por periodo**
    - **Valida: Requisitos 9.2, 9.5**

  - [ ]* 13.5 Escribir test de propiedad para ranking de servicios
    - **Propiedad 27: Ranking de servicios más consultados**
    - **Valida: Requisito 9.4**

- [x] 14. Capa de aplicación — SEO del portal
  - [x] 14.1 Implementar `ISeoService`
    - Implementar `GenerateSitemapAsync`: generar sitemap.xml dinámico con URLs de todos los servicios activos y casos de éxito usando sus slugs
    - Implementar `GetMetaForServiceAsync`: generar meta tags (title, description, keywords) y Open Graph tags (og:title, og:description, og:image, og:url) para un servicio, con soporte de idioma
    - Implementar `GetMetaForCaseStudyAsync`: generar meta tags y Open Graph tags para un caso de éxito
    - Crear DTOs: `SeoMetaDto`
    - _Requisitos: 10.1, 10.2, 10.3, 10.4, 10.5_

  - [ ]* 14.2 Escribir test de propiedad para sitemap
    - **Propiedad 28: Sitemap contiene todas las páginas públicas**
    - **Valida: Requisitos 10.2, 10.5**

  - [ ]* 14.3 Escribir test de propiedad para meta tags
    - **Propiedad 29: Meta tags generados para servicios**
    - **Valida: Requisitos 10.1, 10.3**

- [x] 15. Capa de aplicación — Calendario de llamadas
  - [x] 15.1 Implementar `ICalendarService`
    - Implementar `GetAvailableSlotsAsync`: consultar disponibilidad del consultor para una fecha, excluir slots ya reservados, devolver lista de horarios disponibles
    - Implementar `BookSlotAsync`: validar que el slot esté disponible, crear `BookingSlot` con `IsConfirmed=true`, crear oportunidad automática en fase "Contacto inicial", enviar email de confirmación al visitante, disparar webhook al consultor
    - Implementar `GetAvailabilityConfigAsync` y `UpdateAvailabilityConfigAsync`: CRUD de disponibilidad semanal del consultor
    - Crear DTOs: `AvailableSlotDto`, `BookSlotDto`, `BookingResultDto`, `ConsultorAvailabilityDto`, `UpdateAvailabilityDto`
    - Crear validador: VisitorName, VisitorEmail y VisitorCompany obligatorios en `BookSlotDto`
    - _Requisitos: 11.1, 11.2, 11.3, 11.4, 11.5, 11.6_

  - [ ]* 15.2 Escribir test de propiedad para reserva de llamada
    - **Propiedad 30: Reserva de llamada crea oportunidad y dispara webhook**
    - **Valida: Requisitos 11.2, 11.3, 11.5**

  - [ ]* 15.3 Escribir test de propiedad para round-trip de disponibilidad
    - **Propiedad 31: Round-trip de disponibilidad del consultor**
    - **Valida: Requisito 11.4**

- [x] 16. Checkpoint — Verificar i18n, analytics, SEO y calendario
  - Asegurar que todos los tests pasan, preguntar al usuario si surgen dudas.

- [x] 17. Capa API — Controllers y middleware
  - [x] 17.1 Implementar controllers del portal público
    - Crear/actualizar `PublicController` con endpoints: `GET /api/public/services` (con parámetro `lang`), `GET /api/public/services/{id}` (con parámetro `lang`), `GET /api/public/case-studies`, `GET /api/public/sectors`, `POST /api/public/contact`, `GET /api/public/availability?date={date}`, `POST /api/public/book`, `GET /api/public/sitemap.xml`
    - Configurar respuestas Problem Details (RFC 7807) para errores de validación
    - _Requisitos: 1.1, 1.2, 1.3, 1.4, 1.6, 8.1, 10.2, 11.1, 11.2_

  - [x] 17.2 Configurar Rate Limiting en endpoint de contacto
    - Configurar Rate Limiter nativo de .NET 8 con `AddFixedWindowLimiter("ContactEndpoint")`: 5 peticiones por IP cada 15 minutos
    - Aplicar `[EnableRateLimiting("ContactEndpoint")]` al endpoint `POST /api/public/contact`
    - Configurar respuesta HTTP 429 Too Many Requests con header `Retry-After`
    - _Requisitos: 1.7, 1.8_

  - [ ]* 17.3 Escribir test de propiedad para rate limiting
    - **Propiedad 22: Rate limiting en endpoint de contacto**
    - **Valida: Requisitos 1.7, 1.8**

  - [x] 17.4 Implementar controllers de autenticación y clientes
    - Crear/actualizar `AuthController` con endpoints: `POST /api/auth/login`, `POST /api/auth/refresh`, `POST /api/auth/logout`
    - Crear/actualizar `ClientsController` con endpoints CRUD protegidos con `[Authorize]`
    - _Requisitos: 2.1, 2.2, 2.5_

  - [x] 17.5 Implementar controllers de oportunidades y proyectos
    - Crear/actualizar `OpportunitiesController` con endpoints CRUD y `PATCH /api/opportunities/{id}/move` protegidos
    - Crear/actualizar `ProjectsController` con endpoints CRUD, cambio de estado, completar entregable y registrar horas protegidos
    - Crear/actualizar `ProjectTemplatesController` con `GET /api/project-templates`
    - _Requisitos: 3.1-3.7, 2.3, 2.4, 2.6, 4.1-4.6_

  - [x] 17.6 Implementar controllers de finanzas, formación y catálogo
    - Crear/actualizar `FinanceController` con endpoints de resumen, proyección, facturas y gastos protegidos
    - Crear/actualizar `TrainingController` con endpoints de checklist: `GET /api/training/roadmap`, `PATCH /api/training/items/{id}/complete`, `GET /api/training/progress` protegidos
    - Crear/actualizar `ServicesController` (admin) con endpoints CRUD, toggle, reorder y delete protegidos
    - _Requisitos: 5.1-5.6, 6.1-6.3, 7.1-7.5_

  - [x] 17.7 Implementar controllers de notificaciones, analytics y calendario
    - Crear `NotificationsController` con endpoints: `GET /api/notifications/config`, `PUT /api/notifications/config` protegidos
    - Crear `AnalyticsController` con endpoints: `GET /api/analytics/summary`, `GET /api/analytics/top-services`, `GET /api/analytics/traffic-sources` protegidos
    - Crear `CalendarController` con endpoints: `GET /api/calendar/availability`, `PUT /api/calendar/availability` protegidos
    - _Requisitos: 1.2, 9.2, 9.4, 9.5, 11.4_

  - [x] 17.8 Configurar middleware global y Program.cs
    - Configurar middleware de manejo de excepciones global con Problem Details
    - Registrar `AnalyticsTrackingMiddleware` en el pipeline para rutas públicas
    - Configurar Rate Limiter nativo de .NET 8
    - Configurar CORS para permitir Blazor WASM
    - Configurar inyección de dependencias de todos los servicios y repositorios (incluyendo: INotificationService, ILocalizationService, IAnalyticsService, ISeoService, ICalendarService)
    - Configurar pipeline de autenticación JWT
    - Configurar Swagger/OpenAPI
    - _Requisitos: 2.5, 1.7, 9.1_

- [x] 18. Checkpoint — Verificar API completa
  - Asegurar que todos los tests pasan, preguntar al usuario si surgen dudas.

- [x] 19. Frontend Blazor WASM — Portal público
  - [x] 19.1 Configurar proyecto Blazor WASM, servicios HTTP e i18n
    - Configurar `HttpClient` con base URL de la API
    - Crear servicios de cliente HTTP para consumir la API pública
    - Configurar enrutamiento y layout principal responsive
    - Configurar sistema de localización con archivos `.resx` (español e inglés)
    - Implementar selector de idioma accesible desde cualquier página
    - Implementar detección de idioma del navegador (Accept-Language) con fallback a español
    - _Requisitos: 1.5, 8.1, 8.2, 8.4_

  - [x] 19.2 Implementar páginas del portal público con SEO
    - Crear página de inicio con catálogo de servicios localizado (nombre, descripción, beneficios, rango de precio)
    - Crear sección de casos de éxito (problema, solución, resultados)
    - Crear sección de sectores objetivo con descripción de beneficios por sector
    - Crear formulario de contacto con validación client-side (campos obligatorios, mensajes específicos por campo)
    - Integrar meta tags dinámicos y Open Graph tags en cada página usando `ISeoService`
    - Generar URLs amigables con slugs para servicios y casos de éxito
    - Asegurar diseño responsive para dispositivos móviles
    - _Requisitos: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 10.1, 10.3, 10.5_

  - [x] 19.3 Implementar widget de calendario de llamadas
    - Crear componente de selección de fecha con visualización de horarios disponibles
    - Implementar formulario de reserva (nombre, email, empresa) con validación
    - Mostrar confirmación de reserva y manejo de slot ya ocupado con alternativas
    - _Requisitos: 11.1, 11.2, 11.6_

- [x] 20. Frontend Blazor WASM — Panel del consultor
  - [x] 20.1 Implementar autenticación y layout del panel
    - Crear página de login con formulario de credenciales
    - Implementar `AuthenticationStateProvider` personalizado con JWT
    - Configurar redirección a login si no autenticado
    - Crear layout del panel con navegación lateral
    - _Requisitos: 2.1, 2.5_

  - [x] 20.2 Implementar dashboard principal
    - Crear componente dashboard con resumen de proyectos activos, ingresos del mes y tareas pendientes
    - Incluir barra de progreso del checklist de formación
    - _Requisitos: 2.1, 6.3_

  - [x] 20.3 Implementar gestión de clientes
    - Crear página de lista de clientes con filtros
    - Crear formulario de crear/editar cliente (nombre, empresa, sector, email, teléfono, notas)
    - Implementar acción de archivar cliente
    - _Requisitos: 2.2_

  - [x] 20.4 Implementar pipeline de ventas (kanban)
    - Crear vista kanban con columnas por fase (Contacto inicial, Propuesta enviada, Negociación, Cerrado ganado, Cerrado perdido)
    - Implementar drag-and-drop o botones para mover oportunidades entre fases
    - Mostrar alerta visual en oportunidades estancadas (>14 días en misma fase)
    - Mostrar valor estimado en cada oportunidad
    - Al mover a "Cerrado ganado", solicitar creación de proyecto
    - _Requisitos: 3.1, 3.2, 3.4, 3.5, 3.6, 3.7_

  - [x] 20.5 Implementar gestión de proyectos
    - Crear página de lista de proyectos con estado, cliente y fechas
    - Crear formulario de creación desde plantilla con personalización de entregables
    - Implementar vista de detalle con entregables, barra de progreso, registro de horas y cambio de estado
    - _Requisitos: 2.3, 2.4, 2.6, 4.1, 4.2, 4.3, 4.4, 4.5, 4.6_

  - [x] 20.6 Implementar seguimiento financiero
    - Crear página de resumen financiero mensual (ingresos, gastos, beneficio neto) con alerta si resultado negativo
    - Crear formularios de registro de factura (asociada a proyecto) y gasto (con categoría)
    - Crear vista de proyección financiera a 12 meses con gráfico de evolución mensual
    - _Requisitos: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6_

  - [x] 20.7 Implementar checklist de formación
    - Crear vista del checklist plano de temas de formación
    - Implementar marcar item como completado con actualización de barra de progreso global
    - _Requisitos: 6.1, 6.2, 6.3_

  - [x] 20.8 Implementar gestión del catálogo de servicios con traducciones
    - Crear página de lista de servicios con orden configurable
    - Crear formulario de crear/editar servicio con campos en cada idioma soportado (nombre, descripción, beneficios, meta title, meta description)
    - Implementar activar/desactivar servicio y reordenar
    - Implementar eliminación con aviso de confirmación si tiene proyectos asociados
    - _Requisitos: 7.1, 7.2, 7.3, 7.4, 7.5, 8.5, 10.4_

  - [x] 20.9 Implementar dashboard de analytics
    - Crear página de métricas con selector de periodo de fechas
    - Mostrar visitas totales, visitantes únicos, páginas más vistas y origen del tráfico
    - Mostrar ranking de servicios más consultados
    - _Requisitos: 9.2, 9.4, 9.5_

  - [x] 20.10 Implementar configuración de notificaciones y calendario
    - Crear página de configuración de webhooks (Slack/Telegram): canal, URL, activar/desactivar
    - Crear página de configuración de disponibilidad semanal del consultor (día, hora inicio, hora fin, activo)
    - _Requisitos: 1.2, 11.4_

- [x] 21. Checkpoint final — Verificar integración completa
  - Asegurar que todos los tests pasan, preguntar al usuario si surgen dudas.

## Notas

- Las tareas marcadas con `*` son opcionales y pueden omitirse para un MVP más rápido
- Cada tarea referencia los requisitos específicos para trazabilidad
- Los checkpoints aseguran validación incremental
- Los tests de propiedad validan las 31 propiedades universales de corrección con FsCheck
- Los tests unitarios complementan cubriendo ejemplos específicos y edge cases
- El diseño incluye 14 componentes y 19 entidades de dominio
- Nuevas funcionalidades: webhooks (Slack/Telegram), rate limiting, multi-idioma (es/en), analytics del portal, SEO (meta tags, sitemap, Open Graph), calendario de llamadas
