using AiConsulting.Application.DTOs.Public;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Enums;
using AiConsulting.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiConsulting.Infrastructure.Services;

public class PublicPortalService : IPublicPortalService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IContactRequestRepository _contactRequestRepository;
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly WebhookQueue _webhookQueue;
    private readonly ILogger<PublicPortalService> _logger;

    public PublicPortalService(
        IServiceRepository serviceRepository,
        IContactRequestRepository contactRequestRepository,
        IOpportunityRepository opportunityRepository,
        WebhookQueue webhookQueue,
        ILogger<PublicPortalService> logger)
    {
        _serviceRepository = serviceRepository;
        _contactRequestRepository = contactRequestRepository;
        _opportunityRepository = opportunityRepository;
        _webhookQueue = webhookQueue;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ServiceSummaryDto>> GetActiveServicesAsync()
    {
        var services = await _serviceRepository.GetActiveOrderedAsync();
        return services.Select(MapToSummaryDto).ToList();
    }

    public async Task<ServiceDetailDto?> GetServiceByIdAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service is null) return null;

        return new ServiceDetailDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Benefits = service.Benefits,
            PriceRangeMin = service.PriceRangeMin,
            PriceRangeMax = service.PriceRangeMax,
            EstimatedDeliveryDays = service.EstimatedDeliveryDays,
            TargetSector = service.TargetSector,
            SortOrder = service.SortOrder,
            IsActive = service.IsActive
        };
    }

    public Task<IReadOnlyList<CaseStudyDto>> GetCaseStudiesAsync()
    {
        var caseStudies = new List<CaseStudyDto>
        {
            new()
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                ServiceId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ServiceName = "Chatbot RAG para atención al cliente",
                Problem = "Una firma de abogados con 40 empleados recibía más de 200 consultas diarias repetitivas sobre honorarios, plazos y documentación requerida, saturando al equipo administrativo.",
                Solution = "Implementación de un chatbot basado en RAG (Retrieval-Augmented Generation) entrenado sobre la base de conocimiento interna de la firma, integrado en su web y portal de clientes.",
                Results = "Reducción del 70% en consultas administrativas repetitivas, tiempo de respuesta de segundos frente a horas, y satisfacción del cliente mejorada en un 35% según encuestas post-implementación."
            },
            new()
            {
                Id = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                ServiceId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ServiceName = "Automatización documental con IA",
                Problem = "Una agencia de marketing procesaba manualmente más de 500 informes de campaña al mes, con un equipo de 3 personas dedicando el 60% de su tiempo a tareas de extracción y resumen de datos.",
                Solution = "Pipeline de automatización documental con modelos de lenguaje para extracción de KPIs, generación de resúmenes ejecutivos y clasificación automática de documentos en el sistema de gestión.",
                Results = "Reducción del tiempo de procesamiento de 4 horas a 15 minutos por informe, liberando al equipo para tareas de análisis estratégico y aumentando la capacidad de gestión de cuentas en un 40%."
            },
            new()
            {
                Id = Guid.Parse("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                ServiceId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ServiceName = "Copiloto interno para equipo de desarrollo",
                Problem = "Una empresa SaaS B2B con 15 desarrolladores .NET perdía productividad por la búsqueda de documentación interna, revisiones de código manuales y onboarding lento de nuevos miembros.",
                Solution = "Despliegue de un copiloto interno basado en el codebase y documentación de la empresa, con integración en VS Code y acceso vía API REST para consultas desde Slack y Jira.",
                Results = "Reducción del 50% en tiempo de onboarding, aumento del 25% en velocidad de desarrollo medida en story points por sprint, y disminución del 30% en bugs detectados en revisiones de código."
            }
        };

        return Task.FromResult<IReadOnlyList<CaseStudyDto>>(caseStudies);
    }

    public Task<IReadOnlyList<SectorDto>> GetSectorsAsync()
    {
        var sectors = new List<SectorDto>
        {
            new()
            {
                Id = Guid.Parse("d4e5f6a7-b8c9-0123-defa-234567890123"),
                Name = "PYMEs .NET",
                Description = "Pequeñas y medianas empresas con stack tecnológico basado en .NET que buscan modernizar sus procesos internos con IA sin reemplazar su infraestructura existente.",
                Benefits = "Integración nativa con ecosistema Microsoft, reducción de costes operativos, automatización de procesos repetitivos y mejora de la productividad del equipo técnico."
            },
            new()
            {
                Id = Guid.Parse("e5f6a7b8-c9d0-1234-efab-345678901234"),
                Name = "SaaS B2B",
                Description = "Empresas de software como servicio orientadas a clientes empresariales que quieren diferenciar su producto incorporando capacidades de IA generativa y análisis avanzado.",
                Benefits = "Diferenciación competitiva, nuevas fuentes de ingresos mediante funcionalidades premium de IA, mayor retención de clientes y reducción del churn gracias a mayor valor percibido."
            },
            new()
            {
                Id = Guid.Parse("f6a7b8c9-d0e1-2345-fabc-456789012345"),
                Name = "Despachos legales",
                Description = "Firmas de abogados y despachos jurídicos que necesitan gestionar grandes volúmenes de documentación, automatizar la búsqueda de jurisprudencia y mejorar la atención al cliente.",
                Benefits = "Reducción drástica del tiempo en revisión documental, búsqueda semántica en bases de conocimiento jurídico, chatbots de atención al cliente 24/7 y generación asistida de contratos."
            },
            new()
            {
                Id = Guid.Parse("a7b8c9d0-e1f2-3456-abcd-567890123456"),
                Name = "Agencias de marketing",
                Description = "Agencias creativas y de performance marketing que buscan escalar la producción de contenido, automatizar reportes y ofrecer análisis predictivo a sus clientes.",
                Benefits = "Generación de contenido a escala, automatización de informes de campaña, análisis de sentimiento en redes sociales, personalización masiva de mensajes y optimización de presupuestos publicitarios."
            }
        };

        return Task.FromResult<IReadOnlyList<SectorDto>>(sectors);
    }

    public async Task<ContactRequestResultDto> SubmitContactRequestAsync(ContactRequestDto request)
    {
        var contactRequest = new ContactRequest
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Company = request.Company,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow,
            IsProcessed = false
        };

        await _contactRequestRepository.AddAsync(contactRequest);

        var opportunity = new Opportunity
        {
            Id = Guid.NewGuid(),
            ContactName = request.Name,
            ContactEmail = request.Email,
            Company = request.Company,
            Message = request.Message,
            CurrentPhase = OpportunityPhase.InitialContact,
            PhaseEnteredAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _opportunityRepository.AddAsync(opportunity);

        // Encolar webhook: el BackgroundService lo envía sin bloquear ni arriesgar la operación principal
        _webhookQueue.TryEnqueue("contact.created", new
        {
            contactId = contactRequest.Id,
            name = request.Name,
            email = request.Email,
            company = request.Company,
            opportunityId = opportunity.Id
        });

        _logger.LogInformation("Solicitud de contacto {Id} registrada", contactRequest.Id);

        return new ContactRequestResultDto
        {
            Id = contactRequest.Id,
            Message = "Tu solicitud ha sido recibida. Nos pondremos en contacto contigo en breve.",
            OpportunityId = opportunity.Id
        };
    }

    private static ServiceSummaryDto MapToSummaryDto(Service service) => new()
    {
        Id = service.Id,
        Name = service.Name,
        Description = service.Description,
        Benefits = service.Benefits,
        PriceRangeMin = service.PriceRangeMin,
        PriceRangeMax = service.PriceRangeMax,
        EstimatedDeliveryDays = service.EstimatedDeliveryDays,
        TargetSector = service.TargetSector,
        SortOrder = service.SortOrder
    };
}
