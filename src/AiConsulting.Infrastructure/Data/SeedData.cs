using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Data;

public static class SeedData
{
    // Fixed Guids for Services
    public static readonly Guid ChatbotRagServiceId = new("a1b2c3d4-0001-0001-0001-000000000001");
    public static readonly Guid AutoDocServiceId = new("a1b2c3d4-0001-0001-0001-000000000002");
    public static readonly Guid CopilotServiceId = new("a1b2c3d4-0001-0001-0001-000000000003");
    public static readonly Guid ApiIntegrationServiceId = new("a1b2c3d4-0001-0001-0001-000000000004");
    public static readonly Guid AuditServiceId = new("a1b2c3d4-0001-0001-0001-000000000005");

    // Fixed Guids for ProjectTemplates
    public static readonly Guid ChatbotRagTemplateId = new("b2c3d4e5-0002-0002-0002-000000000001");
    public static readonly Guid AutoDocTemplateId = new("b2c3d4e5-0002-0002-0002-000000000002");
    public static readonly Guid CopilotTemplateId = new("b2c3d4e5-0002-0002-0002-000000000003");
    public static readonly Guid ApiIntegrationTemplateId = new("b2c3d4e5-0002-0002-0002-000000000004");
    public static readonly Guid AuditTemplateId = new("b2c3d4e5-0002-0002-0002-000000000005");

    // Fixed Guids for TrainingWeeks
    public static readonly Guid Week01Id = new("c3d4e5f6-0003-0003-0003-000000000001");
    public static readonly Guid Week02Id = new("c3d4e5f6-0003-0003-0003-000000000002");
    public static readonly Guid Week03Id = new("c3d4e5f6-0003-0003-0003-000000000003");
    public static readonly Guid Week04Id = new("c3d4e5f6-0003-0003-0003-000000000004");
    public static readonly Guid Week05Id = new("c3d4e5f6-0003-0003-0003-000000000005");
    public static readonly Guid Week06Id = new("c3d4e5f6-0003-0003-0003-000000000006");
    public static readonly Guid Week07Id = new("c3d4e5f6-0003-0003-0003-000000000007");
    public static readonly Guid Week08Id = new("c3d4e5f6-0003-0003-0003-000000000008");
    public static readonly Guid Week09Id = new("c3d4e5f6-0003-0003-0003-000000000009");
    public static readonly Guid Week10Id = new("c3d4e5f6-0003-0003-0003-000000000010");
    public static readonly Guid Week11Id = new("c3d4e5f6-0003-0003-0003-000000000011");
    public static readonly Guid Week12Id = new("c3d4e5f6-0003-0003-0003-000000000012");

    // Fixed Guids for TrainingChecklistItems
    public static readonly Guid ChecklistNotificationConfigId = new("e5f6a7b8-0005-0005-0005-000000000001");

    public static async Task SeedAsync(AiConsultingDbContext context)
    {
        if (await context.Services.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        var services = CreateServices(now);
        context.Services.AddRange(services);

        var templates = CreateProjectTemplates();
        
        context.ProjectTemplates.AddRange(templates);

        var (weeks, topics) = CreateTrainingRoadmap();
        context.TrainingWeeks.AddRange(weeks);
        context.Topics.AddRange(topics);

        var checklistItems = CreateTrainingChecklist();
        context.TrainingChecklistItems.AddRange(checklistItems);

        var notificationConfig = CreateNotificationConfig(now);
        context.NotificationConfigs.Add(notificationConfig);

        var availabilities = CreateConsultorAvailabilities();
        context.ConsultorAvailabilities.AddRange(availabilities);

        await context.SaveChangesAsync();
    }

    private static List<Service> CreateServices(DateTime now)
    {
        return
        [
            new Service
            {
                Id = ChatbotRagServiceId,
                Name = "Chatbot RAG",
                Description = "Implementación de chatbots con Retrieval-Augmented Generation para responder preguntas basándose en documentación interna, bases de conocimiento y datos empresariales.",
                Benefits = "Atención 24/7 a clientes y empleados, reducción de tickets de soporte en un 60%, respuestas precisas basadas en datos reales de la empresa.",
                PriceRangeMin = 3000m,
                PriceRangeMax = 8000m,
                EstimatedDeliveryDays = 30,
                TargetSector = "PYMEs .NET, SaaS B2B",
                Slug = "chatbot-rag",
                MetaTitle = "Chatbot RAG — Consultoría IA",
                MetaDescription = "Implementa un chatbot inteligente con RAG para responder preguntas basadas en tu documentación interna.",
                SortOrder = 1,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Service
            {
                Id = AutoDocServiceId,
                Name = "Automatización documental",
                Description = "Procesamiento inteligente de documentos con IA: extracción de datos, clasificación automática y generación de resúmenes a partir de facturas, contratos y formularios.",
                Benefits = "Reducción del 80% en tiempo de procesamiento manual, eliminación de errores de transcripción, trazabilidad completa del flujo documental.",
                PriceRangeMin = 2000m,
                PriceRangeMax = 5000m,
                EstimatedDeliveryDays = 20,
                TargetSector = "Despachos legales, PYMEs .NET",
                Slug = "automatizacion-documental",
                MetaTitle = "Automatización Documental con IA — Consultoría IA",
                MetaDescription = "Procesa facturas, contratos y formularios automáticamente con inteligencia artificial.",
                SortOrder = 2,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Service
            {
                Id = CopilotServiceId,
                Name = "Copiloto interno",
                Description = "Asistente IA personalizado para equipos internos que responde preguntas sobre procesos, políticas y documentación técnica de la organización.",
                Benefits = "Onboarding 3x más rápido, acceso instantáneo al conocimiento corporativo, reducción de interrupciones entre equipos.",
                PriceRangeMin = 5000m,
                PriceRangeMax = 15000m,
                EstimatedDeliveryDays = 45,
                TargetSector = "SaaS B2B, PYMEs .NET",
                Slug = "copiloto-interno",
                MetaTitle = "Copiloto Interno con IA — Consultoría IA",
                MetaDescription = "Asistente IA personalizado para tu equipo: acceso instantáneo al conocimiento corporativo.",
                SortOrder = 3,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Service
            {
                Id = ApiIntegrationServiceId,
                Name = "Integración APIs IA",
                Description = "Conexión de APIs de IA (OpenAI, Anthropic, Azure AI) con sistemas empresariales existentes: ERPs, CRMs, bases de datos y flujos de trabajo.",
                Benefits = "Automatización de procesos repetitivos, enriquecimiento de datos con IA, integración sin disrupciones en la operativa actual.",
                PriceRangeMin = 1500m,
                PriceRangeMax = 4000m,
                EstimatedDeliveryDays = 15,
                TargetSector = "SaaS B2B, Agencias marketing",
                Slug = "integracion-apis-ia",
                MetaTitle = "Integración de APIs de IA — Consultoría IA",
                MetaDescription = "Conecta OpenAI, Anthropic y Azure AI con tus sistemas empresariales existentes.",
                SortOrder = 4,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Service
            {
                Id = AuditServiceId,
                Name = "Auditoría IA",
                Description = "Evaluación y optimización de implementaciones de IA existentes: análisis de rendimiento, costes, calidad de respuestas y recomendaciones de mejora.",
                Benefits = "Reducción de costes de API hasta un 40%, mejora en la calidad de respuestas, identificación de riesgos y oportunidades de mejora.",
                PriceRangeMin = 2000m,
                PriceRangeMax = 6000m,
                EstimatedDeliveryDays = 10,
                TargetSector = "SaaS B2B, PYMEs .NET, Agencias marketing",
                Slug = "auditoria-ia",
                MetaTitle = "Auditoría de IA — Consultoría IA",
                MetaDescription = "Evalúa y optimiza tus implementaciones de IA: reduce costes y mejora la calidad.",
                SortOrder = 5,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            }
        ];
    }

    private static List<ProjectTemplate> CreateProjectTemplates()
    {
        return
        [
            new ProjectTemplate
            {
                Id = ChatbotRagTemplateId,
                ServiceId = ChatbotRagServiceId,
                Name = "Plantilla Chatbot RAG",
                EstimatedTotalHours = 120,
                DefaultDeliverables = """
                [
                  {"name": "Análisis de fuentes de datos", "description": "Identificación y evaluación de documentos, bases de datos y APIs disponibles para alimentar el RAG", "estimatedHours": 15},
                  {"name": "Pipeline de ingesta y embeddings", "description": "Desarrollo del pipeline de procesamiento, chunking y generación de embeddings vectoriales", "estimatedHours": 25},
                  {"name": "Configuración de vector database", "description": "Despliegue y configuración de la base de datos vectorial (Qdrant/Pinecone/pgvector)", "estimatedHours": 15},
                  {"name": "Desarrollo del chatbot", "description": "Implementación del chatbot con lógica RAG, prompts optimizados y manejo de contexto", "estimatedHours": 35},
                  {"name": "Integración y UI", "description": "Integración con la plataforma del cliente y desarrollo de la interfaz de chat", "estimatedHours": 20},
                  {"name": "Testing y optimización", "description": "Pruebas de calidad de respuestas, ajuste de parámetros y documentación", "estimatedHours": 10}
                ]
                """,
                DefaultMilestones = """
                [
                  {"name": "Kickoff y análisis", "description": "Reunión inicial, acceso a fuentes de datos y definición de alcance", "weekNumber": 1},
                  {"name": "MVP funcional", "description": "Chatbot básico funcionando con datos reales del cliente", "weekNumber": 2},
                  {"name": "Entrega final", "description": "Chatbot optimizado, integrado y documentado", "weekNumber": 4}
                ]
                """
            },
            new ProjectTemplate
            {
                Id = AutoDocTemplateId,
                ServiceId = AutoDocServiceId,
                Name = "Plantilla Automatización Documental",
                EstimatedTotalHours = 80,
                DefaultDeliverables = """
                [
                  {"name": "Análisis de tipos documentales", "description": "Catalogación de tipos de documentos, campos a extraer y reglas de clasificación", "estimatedHours": 10},
                  {"name": "Pipeline de OCR y extracción", "description": "Configuración de OCR y desarrollo de extractores de datos por tipo de documento", "estimatedHours": 20},
                  {"name": "Motor de clasificación", "description": "Implementación del clasificador automático de documentos con IA", "estimatedHours": 15},
                  {"name": "Integración con sistemas", "description": "Conexión con el ERP/CRM del cliente para volcado automático de datos extraídos", "estimatedHours": 20},
                  {"name": "Validación y entrega", "description": "Pruebas con documentos reales, ajustes y documentación de uso", "estimatedHours": 15}
                ]
                """,
                DefaultMilestones = """
                [
                  {"name": "Análisis completado", "description": "Tipos documentales catalogados y reglas definidas", "weekNumber": 1},
                  {"name": "Extracción funcionando", "description": "Pipeline de OCR y extracción validado con documentos reales", "weekNumber": 2},
                  {"name": "Entrega final", "description": "Sistema integrado, probado y documentado", "weekNumber": 3}
                ]
                """
            },
            new ProjectTemplate
            {
                Id = CopilotTemplateId,
                ServiceId = CopilotServiceId,
                Name = "Plantilla Copiloto Interno",
                EstimatedTotalHours = 180,
                DefaultDeliverables = """
                [
                  {"name": "Discovery y mapeo de conocimiento", "description": "Identificación de fuentes de conocimiento interno, procesos y preguntas frecuentes", "estimatedHours": 20},
                  {"name": "Ingesta de conocimiento", "description": "Procesamiento e indexación de documentación interna, wikis y procedimientos", "estimatedHours": 30},
                  {"name": "Desarrollo del copiloto", "description": "Implementación del asistente con RAG, function calling y personalización por rol", "estimatedHours": 50},
                  {"name": "Interfaz de usuario", "description": "Desarrollo de la UI integrada en las herramientas del equipo (Slack, Teams, web)", "estimatedHours": 30},
                  {"name": "Permisos y seguridad", "description": "Configuración de acceso por roles y auditoría de consultas", "estimatedHours": 25},
                  {"name": "Piloto y ajustes", "description": "Despliegue piloto con un equipo, recopilación de feedback y optimización", "estimatedHours": 25}
                ]
                """,
                DefaultMilestones = """
                [
                  {"name": "Discovery completado", "description": "Mapa de conocimiento y plan de ingesta definidos", "weekNumber": 1},
                  {"name": "Copiloto MVP", "description": "Asistente básico funcionando con conocimiento indexado", "weekNumber": 3},
                  {"name": "Piloto con equipo", "description": "Despliegue piloto con un equipo real", "weekNumber": 5},
                  {"name": "Entrega final", "description": "Copiloto optimizado, documentado y desplegado", "weekNumber": 6}
                ]
                """
            },
            new ProjectTemplate
            {
                Id = ApiIntegrationTemplateId,
                ServiceId = ApiIntegrationServiceId,
                Name = "Plantilla Integración APIs IA",
                EstimatedTotalHours = 60,
                DefaultDeliverables = """
                [
                  {"name": "Análisis de integración", "description": "Mapeo de sistemas existentes, APIs disponibles y flujos de datos a automatizar", "estimatedHours": 8},
                  {"name": "Capa de abstracción de APIs IA", "description": "Desarrollo de wrapper unificado para OpenAI, Anthropic y Azure AI con fallback", "estimatedHours": 15},
                  {"name": "Conectores empresariales", "description": "Desarrollo de conectores con ERP, CRM o bases de datos del cliente", "estimatedHours": 15},
                  {"name": "Orquestación de flujos", "description": "Implementación de los flujos automatizados con manejo de errores y reintentos", "estimatedHours": 12},
                  {"name": "Testing y documentación", "description": "Pruebas end-to-end, documentación técnica y guía de operación", "estimatedHours": 10}
                ]
                """,
                DefaultMilestones = """
                [
                  {"name": "Análisis completado", "description": "Sistemas mapeados y plan de integración definido", "weekNumber": 1},
                  {"name": "Entrega final", "description": "Integración funcionando, probada y documentada", "weekNumber": 2}
                ]
                """
            },
            new ProjectTemplate
            {
                Id = AuditTemplateId,
                ServiceId = AuditServiceId,
                Name = "Plantilla Auditoría IA",
                EstimatedTotalHours = 40,
                DefaultDeliverables = """
                [
                  {"name": "Inventario de implementaciones IA", "description": "Catalogación de todas las implementaciones de IA en uso: modelos, APIs, costes y métricas", "estimatedHours": 8},
                  {"name": "Análisis de rendimiento", "description": "Evaluación de latencia, calidad de respuestas, tasa de errores y costes por consulta", "estimatedHours": 10},
                  {"name": "Análisis de seguridad y compliance", "description": "Revisión de manejo de datos sensibles, políticas de retención y cumplimiento normativo", "estimatedHours": 8},
                  {"name": "Informe de recomendaciones", "description": "Documento con hallazgos, riesgos identificados y plan de mejora priorizado", "estimatedHours": 10},
                  {"name": "Presentación ejecutiva", "description": "Sesión de presentación de resultados al equipo directivo", "estimatedHours": 4}
                ]
                """,
                DefaultMilestones = """
                [
                  {"name": "Inventario completado", "description": "Todas las implementaciones IA catalogadas", "weekNumber": 1},
                  {"name": "Entrega del informe", "description": "Informe de auditoría entregado con recomendaciones", "weekNumber": 2}
                ]
                """
            }
        ];
    }

    private static (List<TrainingWeek> Weeks, List<Topic> Topics) CreateTrainingRoadmap()
    {
        var weeks = new List<TrainingWeek>
        {
            new() { Id = Week01Id, WeekNumber = 1, Title = "LLMs — Fundamentos", Description = "Introducción a Large Language Models: arquitectura transformer, tokenización, ventanas de contexto y principales proveedores (OpenAI, Anthropic, Azure)." },
            new() { Id = Week02Id, WeekNumber = 2, Title = "LLMs — Prompting y Fine-tuning", Description = "Técnicas avanzadas de prompting (few-shot, chain-of-thought, system prompts) y conceptos de fine-tuning para casos de uso específicos." },
            new() { Id = Week03Id, WeekNumber = 3, Title = "RAG — Embeddings y Vector Databases", Description = "Generación de embeddings, modelos de embedding, bases de datos vectoriales (pgvector, Qdrant, Pinecone) y estrategias de indexación." },
            new() { Id = Week04Id, WeekNumber = 4, Title = "RAG — Estrategias de Retrieval", Description = "Chunking strategies, hybrid search, re-ranking, evaluación de calidad de retrieval y optimización de pipelines RAG." },
            new() { Id = Week05Id, WeekNumber = 5, Title = "Function Calling — Tool Use", Description = "Implementación de function calling con OpenAI y Anthropic, definición de herramientas, validación de parámetros y manejo de errores." },
            new() { Id = Week06Id, WeekNumber = 6, Title = "Function Calling — Structured Outputs", Description = "Generación de salidas estructuradas (JSON mode, response format), validación de esquemas y integración con sistemas tipados." },
            new() { Id = Week07Id, WeekNumber = 7, Title = "Agentes — Arquitecturas de Agentes", Description = "Patrones de diseño de agentes IA: ReAct, plan-and-execute, reflexión. Frameworks: Semantic Kernel, LangChain, AutoGen." },
            new() { Id = Week08Id, WeekNumber = 8, Title = "Agentes — Sistemas Multi-Agente", Description = "Orquestación de múltiples agentes, comunicación inter-agente, delegación de tareas y patrones de supervisión." },
            new() { Id = Week09Id, WeekNumber = 9, Title = "Automatización Documental — OCR y Document Parsing", Description = "Tecnologías de OCR (Azure Document Intelligence, Tesseract), parsing de PDFs, extracción de tablas y datos estructurados." },
            new() { Id = Week10Id, WeekNumber = 10, Title = "Automatización Documental — Clasificación y Extracción", Description = "Clasificación automática de documentos con IA, extracción de entidades, validación de datos y flujos de procesamiento." },
            new() { Id = Week11Id, WeekNumber = 11, Title = "Integración y Despliegue — APIs y Monitoring", Description = "Diseño de APIs para servicios IA, rate limiting, caching de respuestas, observabilidad y monitorización de costes." },
            new() { Id = Week12Id, WeekNumber = 12, Title = "Integración y Despliegue — Producción", Description = "Despliegue en producción: containerización, CI/CD para modelos IA, testing A/B, rollback strategies y gestión de versiones de prompts." }
        };

        var topicId = 1;
        Guid MakeTopicId(int id) => new($"d4e5f6a7-0004-0004-0004-{id:D12}");

        var topics = new List<Topic>
        {
            // Week 1: LLMs — Fundamentos
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week01Id, Name = "Arquitectura Transformer", Description = "Estudio de la arquitectura transformer: attention mechanism, self-attention y multi-head attention.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week01Id, Name = "Tokenización y contexto", Description = "Tokenizadores (BPE, SentencePiece), ventanas de contexto y gestión de tokens.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week01Id, Name = "Proveedores de LLMs", Description = "Comparativa de OpenAI, Anthropic, Azure OpenAI, Google Gemini: modelos, precios y casos de uso.", SortOrder = 3 },

            // Week 2: LLMs — Prompting y Fine-tuning
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week02Id, Name = "Técnicas de prompting", Description = "Zero-shot, few-shot, chain-of-thought, tree-of-thought y system prompts efectivos.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week02Id, Name = "Prompt engineering avanzado", Description = "Técnicas de meta-prompting, prompt chaining y optimización iterativa de prompts.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week02Id, Name = "Fine-tuning de modelos", Description = "Cuándo y cómo hacer fine-tuning: preparación de datasets, entrenamiento y evaluación.", SortOrder = 3 },

            // Week 3: RAG — Embeddings y Vector Databases
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week03Id, Name = "Modelos de embedding", Description = "text-embedding-ada-002, Cohere Embed, modelos open source. Dimensionalidad y similitud coseno.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week03Id, Name = "Bases de datos vectoriales", Description = "pgvector, Qdrant, Pinecone, Weaviate: instalación, configuración e indexación.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week03Id, Name = "Estrategias de indexación", Description = "HNSW, IVF, PQ: trade-offs entre velocidad, memoria y precisión.", SortOrder = 3 },

            // Week 4: RAG — Estrategias de Retrieval
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week04Id, Name = "Chunking strategies", Description = "Estrategias de segmentación: fixed-size, semantic, recursive y document-aware chunking.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week04Id, Name = "Hybrid search y re-ranking", Description = "Combinación de búsqueda vectorial y keyword search, modelos de re-ranking (Cohere, cross-encoders).", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week04Id, Name = "Evaluación de RAG", Description = "Métricas de calidad: faithfulness, relevance, answer correctness. Frameworks de evaluación (RAGAS).", SortOrder = 3 },

            // Week 5: Function Calling — Tool Use
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week05Id, Name = "Function calling con OpenAI", Description = "Definición de funciones, esquemas JSON, invocación y manejo de respuestas con la API de OpenAI.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week05Id, Name = "Tool use con Anthropic", Description = "Implementación de tool use con Claude: definición de herramientas, flujo de ejecución y best practices.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week05Id, Name = "Validación y error handling", Description = "Validación de parámetros de funciones, manejo de errores, reintentos y fallbacks.", SortOrder = 3 },

            // Week 6: Function Calling — Structured Outputs
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week06Id, Name = "JSON mode y response format", Description = "Configuración de JSON mode en OpenAI y Anthropic, definición de esquemas de respuesta.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week06Id, Name = "Validación de esquemas", Description = "Validación de salidas estructuradas con JSON Schema, Pydantic y System.Text.Json.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week06Id, Name = "Integración con sistemas tipados", Description = "Mapeo de salidas IA a DTOs de C#, serialización/deserialización y manejo de tipos.", SortOrder = 3 },

            // Week 7: Agentes — Arquitecturas
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week07Id, Name = "Patrón ReAct", Description = "Implementación del patrón Reasoning + Acting: ciclo de pensamiento, acción y observación.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week07Id, Name = "Plan-and-Execute", Description = "Agentes que planifican antes de ejecutar: descomposición de tareas y ejecución secuencial.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week07Id, Name = "Frameworks de agentes", Description = "Semantic Kernel, LangChain, AutoGen: comparativa, setup y primeros agentes.", SortOrder = 3 },

            // Week 8: Agentes — Sistemas Multi-Agente
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week08Id, Name = "Orquestación multi-agente", Description = "Patrones de comunicación entre agentes: secuencial, paralelo, jerárquico y basado en eventos.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week08Id, Name = "Delegación de tareas", Description = "Diseño de agentes especializados, routing de tareas y agregación de resultados.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week08Id, Name = "Supervisión y guardrails", Description = "Patrones de supervisión humana, guardrails de seguridad y límites de autonomía.", SortOrder = 3 },

            // Week 9: Automatización Documental — OCR
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week09Id, Name = "Azure Document Intelligence", Description = "Configuración y uso de Azure Document Intelligence para extracción de datos de documentos.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week09Id, Name = "OCR con Tesseract", Description = "Instalación, configuración y optimización de Tesseract OCR para documentos empresariales.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week09Id, Name = "Extracción de tablas", Description = "Técnicas para extraer datos tabulares de PDFs e imágenes: Camelot, Tabula y modelos IA.", SortOrder = 3 },

            // Week 10: Automatización Documental — Clasificación
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week10Id, Name = "Clasificación de documentos", Description = "Clasificación automática usando embeddings y LLMs: facturas, contratos, formularios.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week10Id, Name = "Extracción de entidades", Description = "Named Entity Recognition (NER) aplicado a documentos empresariales con modelos IA.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week10Id, Name = "Flujos de procesamiento", Description = "Diseño de pipelines de procesamiento documental: ingesta, clasificación, extracción y validación.", SortOrder = 3 },

            // Week 11: Integración y Despliegue — APIs
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week11Id, Name = "Diseño de APIs para IA", Description = "Patrones de diseño de APIs que exponen capacidades IA: streaming, async, rate limiting.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week11Id, Name = "Caching y optimización", Description = "Estrategias de caching de respuestas IA: semantic cache, TTL y invalidación.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week11Id, Name = "Observabilidad y costes", Description = "Monitorización de latencia, tokens consumidos, costes por request y alertas.", SortOrder = 3 },

            // Week 12: Integración y Despliegue — Producción
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week12Id, Name = "Containerización de servicios IA", Description = "Docker y Kubernetes para servicios IA: imágenes optimizadas, health checks y scaling.", SortOrder = 1 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week12Id, Name = "CI/CD para modelos IA", Description = "Pipelines de CI/CD que incluyen testing de prompts, evaluación de calidad y despliegue automatizado.", SortOrder = 2 },
            new() { Id = MakeTopicId(topicId++), TrainingWeekId = Week12Id, Name = "Gestión de versiones de prompts", Description = "Versionado de prompts, testing A/B, rollback y gestión de configuración en producción.", SortOrder = 3 }
        };

        return (weeks, topics);
    }

    private static List<TrainingChecklistItem> CreateTrainingChecklist()
    {
        var itemId = 1;
        Guid MakeId(int id) => new($"f6a7b8c9-0006-0006-0006-{id:D12}");

        return
        [
            new() { Id = MakeId(itemId++), Name = "Arquitectura Transformer", Description = "Estudio de la arquitectura transformer: attention mechanism, self-attention y multi-head attention.", SortOrder = 1 },
            new() { Id = MakeId(itemId++), Name = "Tokenización y contexto", Description = "Tokenizadores (BPE, SentencePiece), ventanas de contexto y gestión de tokens.", SortOrder = 2 },
            new() { Id = MakeId(itemId++), Name = "Proveedores de LLMs", Description = "Comparativa de OpenAI, Anthropic, Azure OpenAI, Google Gemini: modelos, precios y casos de uso.", SortOrder = 3 },
            new() { Id = MakeId(itemId++), Name = "Técnicas de prompting", Description = "Zero-shot, few-shot, chain-of-thought, tree-of-thought y system prompts efectivos.", SortOrder = 4 },
            new() { Id = MakeId(itemId++), Name = "Prompt engineering avanzado", Description = "Técnicas de meta-prompting, prompt chaining y optimización iterativa de prompts.", SortOrder = 5 },
            new() { Id = MakeId(itemId++), Name = "Fine-tuning de modelos", Description = "Cuándo y cómo hacer fine-tuning: preparación de datasets, entrenamiento y evaluación.", SortOrder = 6 },
            new() { Id = MakeId(itemId++), Name = "Modelos de embedding", Description = "text-embedding-ada-002, Cohere Embed, modelos open source. Dimensionalidad y similitud coseno.", SortOrder = 7 },
            new() { Id = MakeId(itemId++), Name = "Bases de datos vectoriales", Description = "pgvector, Qdrant, Pinecone, Weaviate: instalación, configuración e indexación.", SortOrder = 8 },
            new() { Id = MakeId(itemId++), Name = "Estrategias de indexación", Description = "HNSW, IVF, PQ: trade-offs entre velocidad, memoria y precisión.", SortOrder = 9 },
            new() { Id = MakeId(itemId++), Name = "Chunking strategies", Description = "Estrategias de segmentación: fixed-size, semantic, recursive y document-aware chunking.", SortOrder = 10 },
            new() { Id = MakeId(itemId++), Name = "Hybrid search y re-ranking", Description = "Combinación de búsqueda vectorial y keyword search, modelos de re-ranking.", SortOrder = 11 },
            new() { Id = MakeId(itemId++), Name = "Evaluación de RAG", Description = "Métricas de calidad: faithfulness, relevance, answer correctness. Frameworks de evaluación (RAGAS).", SortOrder = 12 },
            new() { Id = MakeId(itemId++), Name = "Function calling con OpenAI", Description = "Definición de funciones, esquemas JSON, invocación y manejo de respuestas con la API de OpenAI.", SortOrder = 13 },
            new() { Id = MakeId(itemId++), Name = "Tool use con Anthropic", Description = "Implementación de tool use con Claude: definición de herramientas, flujo de ejecución y best practices.", SortOrder = 14 },
            new() { Id = MakeId(itemId++), Name = "Validación y error handling", Description = "Validación de parámetros de funciones, manejo de errores, reintentos y fallbacks.", SortOrder = 15 },
            new() { Id = MakeId(itemId++), Name = "JSON mode y response format", Description = "Configuración de JSON mode en OpenAI y Anthropic, definición de esquemas de respuesta.", SortOrder = 16 },
            new() { Id = MakeId(itemId++), Name = "Validación de esquemas", Description = "Validación de salidas estructuradas con JSON Schema, Pydantic y System.Text.Json.", SortOrder = 17 },
            new() { Id = MakeId(itemId++), Name = "Integración con sistemas tipados", Description = "Mapeo de salidas IA a DTOs de C#, serialización/deserialización y manejo de tipos.", SortOrder = 18 },
            new() { Id = MakeId(itemId++), Name = "Patrón ReAct", Description = "Implementación del patrón Reasoning + Acting: ciclo de pensamiento, acción y observación.", SortOrder = 19 },
            new() { Id = MakeId(itemId++), Name = "Plan-and-Execute", Description = "Agentes que planifican antes de ejecutar: descomposición de tareas y ejecución secuencial.", SortOrder = 20 },
            new() { Id = MakeId(itemId++), Name = "Frameworks de agentes", Description = "Semantic Kernel, LangChain, AutoGen: comparativa, setup y primeros agentes.", SortOrder = 21 },
            new() { Id = MakeId(itemId++), Name = "Orquestación multi-agente", Description = "Patrones de comunicación entre agentes: secuencial, paralelo, jerárquico y basado en eventos.", SortOrder = 22 },
            new() { Id = MakeId(itemId++), Name = "Delegación de tareas", Description = "Diseño de agentes especializados, routing de tareas y agregación de resultados.", SortOrder = 23 },
            new() { Id = MakeId(itemId++), Name = "Supervisión y guardrails", Description = "Patrones de supervisión humana, guardrails de seguridad y límites de autonomía.", SortOrder = 24 },
            new() { Id = MakeId(itemId++), Name = "Azure Document Intelligence", Description = "Configuración y uso de Azure Document Intelligence para extracción de datos de documentos.", SortOrder = 25 },
            new() { Id = MakeId(itemId++), Name = "OCR con Tesseract", Description = "Instalación, configuración y optimización de Tesseract OCR para documentos empresariales.", SortOrder = 26 },
            new() { Id = MakeId(itemId++), Name = "Extracción de tablas", Description = "Técnicas para extraer datos tabulares de PDFs e imágenes: Camelot, Tabula y modelos IA.", SortOrder = 27 },
            new() { Id = MakeId(itemId++), Name = "Clasificación de documentos", Description = "Clasificación automática usando embeddings y LLMs: facturas, contratos, formularios.", SortOrder = 28 },
            new() { Id = MakeId(itemId++), Name = "Extracción de entidades", Description = "Named Entity Recognition (NER) aplicado a documentos empresariales con modelos IA.", SortOrder = 29 },
            new() { Id = MakeId(itemId++), Name = "Flujos de procesamiento", Description = "Diseño de pipelines de procesamiento documental: ingesta, clasificación, extracción y validación.", SortOrder = 30 },
            new() { Id = MakeId(itemId++), Name = "Diseño de APIs para IA", Description = "Patrones de diseño de APIs que exponen capacidades IA: streaming, async, rate limiting.", SortOrder = 31 },
            new() { Id = MakeId(itemId++), Name = "Caching y optimización", Description = "Estrategias de caching de respuestas IA: semantic cache, TTL y invalidación.", SortOrder = 32 },
            new() { Id = MakeId(itemId++), Name = "Observabilidad y costes", Description = "Monitorización de latencia, tokens consumidos, costes por request y alertas.", SortOrder = 33 },
            new() { Id = MakeId(itemId++), Name = "Containerización de servicios IA", Description = "Docker y Kubernetes para servicios IA: imágenes optimizadas, health checks y scaling.", SortOrder = 34 },
            new() { Id = MakeId(itemId++), Name = "CI/CD para modelos IA", Description = "Pipelines de CI/CD que incluyen testing de prompts, evaluación de calidad y despliegue automatizado.", SortOrder = 35 },
            new() { Id = MakeId(itemId++), Name = "Gestión de versiones de prompts", Description = "Versionado de prompts, testing A/B, rollback y gestión de configuración en producción.", SortOrder = 36 },
        ];
    }

    private static NotificationConfig CreateNotificationConfig(DateTime now)
    {
        return new NotificationConfig
        {
            Id = ChecklistNotificationConfigId,
            Channel = Domain.Enums.NotificationChannel.Slack,
            WebhookUrl = "https://hooks.slack.com/services/placeholder",
            IsActive = false,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    private static List<ConsultorAvailability> CreateConsultorAvailabilities()
    {
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(18, 0);

        var items = new List<ConsultorAvailability>
        {
            new() { Id = new Guid("a8b9c0d1-0007-0007-0007-000000000001"), DayOfWeek = 1, IsActive = true },
            new() { Id = new Guid("a8b9c0d1-0007-0007-0007-000000000002"), DayOfWeek = 2, IsActive = true },
            new() { Id = new Guid("a8b9c0d1-0007-0007-0007-000000000003"), DayOfWeek = 3, IsActive = true },
            new() { Id = new Guid("a8b9c0d1-0007-0007-0007-000000000004"), DayOfWeek = 4, IsActive = true },
            new() { Id = new Guid("a8b9c0d1-0007-0007-0007-000000000005"), DayOfWeek = 5, IsActive = true },
        };

        foreach (var item in items)
            item.SetTimeRange(start, end);

        return items;
    }
}
